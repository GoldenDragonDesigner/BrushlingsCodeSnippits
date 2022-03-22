using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FireBoss : BaseEnemyScript
{
    #region FireBossVariables

    [Header("Fire Boss Bools")]
    public bool birthing;
    public bool summoning;

    [Header("Boss Animations")]
    [Tooltip("This is a bool animation and it is called at the beginning of the fight")]
    public string birth;
    [Tooltip("This is a trigger animation and causes the enemies to spawn")]
    public string spellA;
    [Tooltip("This is a bool animation and is done just before the boss is going to fire a projectile or spawn enemies")]
    public string spellReady;
    [Tooltip("This is a bool animation and is for the boss firing projectiles")]
    public string spellB;
    [Tooltip("This is a trigger and is used when the player completes a stage in the fight and makes the boss mad")]
    public string roar;

    [Header("Boss animation delay times")]
    [Tooltip("This is the time set for delaying the animation for the boss spawning the enemies")]
    public float spawnTime;
    [Tooltip("this is the health of the boss halved")]
    public float halfLife;
    private float _results;

    private FireBossManager _fireBossManager;

    private GlobalVariables.FireBossState _fireBossStates;
    private GlobalVariables.FireBossPickingAPoint _fireBossPickingAPoint;
    [Tooltip("This is the number the boss chose to move to")]
    public int fiveNumbers;

    [HideInInspector]
    public bool canUseAttack = true;
    [Tooltip("This is the delay time for shooting at the player")]
    public float animTime;
    private FireBossAttack _fireBossAttack;

    [Header("These are the nav points the boss moves to")]
    [FormerlySerializedAs("navPoint_01")]
    public Transform navPoint01;
    [FormerlySerializedAs("navPoint_02")] public Transform navPoint02;
    [FormerlySerializedAs("navPoint_03")] public Transform navPoint03;
    [FormerlySerializedAs("navPoint_04")] public Transform navPoint04;
    [FormerlySerializedAs("navPoint_05")] public Transform navPoint05;
    public Transform curNavPoint;

    //[Tooltip("Add the BossHealthBar_All game object here")]
    public GameObject healthBar;
    [SerializeField]
    private Image _healthBarImage;

    [Header("This is the amount of time the boss is at a nav point")]
    private float _fireBossTime;
    public float fireBossStartTime;

    private AudioSource _audioSource;

    public AudioClip fireBossAttack, meleeAttack;
    [FormerlySerializedAs("damaged_01")] public AudioClip damaged01;
    [FormerlySerializedAs("damaged_02")] public AudioClip damaged02;
    [FormerlySerializedAs("damaged_03")] public AudioClip damaged03;
    [FormerlySerializedAs("deathSound_01")] public AudioClip deathSound01;
    [FormerlySerializedAs("deathSound_02")] public AudioClip deathSound02;
    [FormerlySerializedAs("deathSound_03")] public AudioClip deathSound03;
    public AudioClip rawrSound, birthSound, spawningEnemies;

    [Tooltip("Add the fire boss fiery force field here")]
    public GameObject fireBossForceField;

    private Player _player;
    //public Transform bossBeforeBirth;


    #endregion FireBossVariables

    #region Awake, Start, and Update

    protected override void Awake()
    {
        GlobalVariables.FireBoss = this;
        _player = GlobalVariables.Player;
    }

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        UIManager = GlobalVariables.UiManager;
        _fireBossTime = fireBossStartTime;
        curEnemyHealth = maxEnemyHealth;
        UIManager.bossCurrentHealth = curEnemyHealth;
        UIManager.bossMaxHealth = maxEnemyHealth;
        _results = maxEnemyHealth - maxEnemyHealth * 50f / 100f;
        halfLife = _results;
        _fireBossManager = GlobalVariables.FireBossManager;
        _fireBossAttack = GlobalVariables.FireBossAttack;
        _fireBossStates = GlobalVariables.FireBossState.Birth;
        IsEnemyDead = false;
        IsEnemyAlerted = false;
        PlayerPrefab = GlobalVariables.Player.gameObject;
        IsEnemyHit = false;
        IsEnemyMoving = false;
        healthBar = UIManager.bossHealthBar.gameObject;
        healthBar.SetActive(true);
        _audioSource = GetComponent<AudioSource>();
        _healthBarImage = healthBar.GetComponentInChildren<HealthSlider>().GetComponent<Image>();
    }

    protected override void Update()
    {
        EnemyState();
        PickingANavPoint();
        UIManager.bossCurrentHealth = curEnemyHealth;
        _healthBarImage.fillAmount = CalculatingEnemyHealth();
        //_healthBarImage.fillAmount = CalculatingFireBossHealth();
        distanceFromPlayer = Vector3.Distance(enemyTransform.position, PlayerPrefab.transform.position);
        FireBossReset();
    }

    #endregion Awake, Start, and Update

    #region FireBossBehaviour

    #region FireBossEnum

    protected override void EnemyState()
    {
        switch (_fireBossStates)
        {
            case GlobalVariables.FireBossState.Birth:
                Birthing();
                break;
            case GlobalVariables.FireBossState.Idle:
                Idle();
                break;
            case GlobalVariables.FireBossState.Summon:
                Summoning();
                break;
            case GlobalVariables.FireBossState.Moving:
                FireBossMoving();
                break;
        }
    }

    #endregion FireBossEnum

    #region FireBossBirthing

    private void Birthing()
    {
        birthing = true;
        if (enemyNavAgent == null)
        {
            return;
        }
        _audioSource.PlayOneShot(birthSound);
        anim.SetTrigger(birth);
        UIManager.BossHealthBarActive();
        StartCoroutine(DelayingSwitchingToIdle());
    }

    IEnumerator DelayingSwitchingToIdle()
    {
        yield return new WaitForSeconds(.1f);
        _fireBossStates = GlobalVariables.FireBossState.Idle;
        birthing = false;
        StopCoroutine(DelayingSwitchingToIdle());
    }

    #endregion FireBossBirthing

    #region FireBossIdling

    protected override void Idle()
    {
        IsEnemySpawned = true;
        IsIdle = true;
        if (enemyNavAgent == null)
        {
            return;
        }

        if (_fireBossManager.stageOne)
        {
            anim.SetBool(idle00, true);
            FacingTarget(PlayerPrefab.transform.position);
            enemyNavAgent.destination = navPoint01.position;
            enemyNavAgent.speed = chaseSpeed;
            enemyNavAgent.isStopped = false;
            gameObject.tag = "Enemy";
            fireBossForceField.SetActive(true);
            if (_fireBossManager.fireEnemiesSpawned)
            {
                StartCoroutine(DelayingSwitchingToSummoning());
            }
        }

        else if (_fireBossManager.stageTwo)
        {
            anim.SetBool(idle00, true);
            FacingTarget(PlayerPrefab.transform.position);
            pickingFromPoints = true;
            StartCoroutine(DelayingSwitchingToMoving());
        }

        else if (_fireBossManager.stageThree)
        {
            anim.SetBool(idle00, true);
            FacingTarget(PlayerPrefab.transform.position);
            pickingFromPoints = true;
            gameObject.tag = "Enemy";
            fireBossForceField.SetActive(true);
            StartCoroutine(DelayingSwitchingToMoving());
        }
    }

    IEnumerator DelayingSwitchingToSummoning()
    {
        yield return new WaitForSeconds(1.5f);
        _fireBossStates = GlobalVariables.FireBossState.Summon;
        IsIdle = false;
        StopCoroutine(DelayingSwitchingToSummoning());
    }

    IEnumerator DelayingSwitchingToMoving()
    {
        yield return new WaitForSeconds(.1f);
        _fireBossStates = GlobalVariables.FireBossState.Moving;
        IsIdle = false;
        StopCoroutine(DelayingSwitchingToMoving());
    }

    #endregion FireBossIdling

    #region FireBossSummoning

    public void Summoning()
    {
        summoning = true;
        if (enemyNavAgent == null) { return; }

        if (_fireBossManager.stageOne)
        {
            FacingTarget(PlayerPrefab.transform.position);
            StartCoroutine(SpawningTheEnemiesDelay());
        }

        else if (_fireBossManager.stageThree)
        {
            FacingTarget(PlayerPrefab.transform.position);
            enemyNavAgent.isStopped = true;
            gameObject.tag = "FireBoss";
            fireBossForceField.SetActive(false);
            StartCoroutine(SpawningTheEnemiesDelay());
        }
    }

    IEnumerator SpawningTheEnemiesDelay()
    {
        _audioSource.PlayOneShot(spawningEnemies);
        anim.SetBool(spellA, true);
        yield return new WaitForSeconds(spawnTime);
        _fireBossManager.pickingAMushroom = true;
        anim.SetBool(spellA, false);
        summoning = false;
        _fireBossStates = GlobalVariables.FireBossState.Idle;
        StopCoroutine(SpawningTheEnemiesDelay());
    }

    #endregion FireBossSummoning

    #region FireBossMoving

    public void FireBossMoving()
    {
        IsEnemyMoving = true;
        if (enemyNavAgent == null) { return; }

        switch (_fireBossPickingAPoint)
        {
            case GlobalVariables.FireBossPickingAPoint.Point1:
                PointOne();
                break;
            case GlobalVariables.FireBossPickingAPoint.Point2:
                PointTwo();
                break;
            case GlobalVariables.FireBossPickingAPoint.Point3:
                PointThree();
                break;
            case GlobalVariables.FireBossPickingAPoint.Point4:
                PointFour();
                break;
            case GlobalVariables.FireBossPickingAPoint.Point5:
                PointFive();
                break;
        }
    }

    private void PointOne()
    {
        if (fiveNumbers == 1)
        {
            MoveProcessing(navPoint01);
        }
    }

    private void PointTwo()
    {
        if (fiveNumbers == 2)
        {
            MoveProcessing(navPoint02);
        }
    }

    private void PointThree()
    {
        if (fiveNumbers == 3)
        {
            MoveProcessing(navPoint03);
        }
    }

    private void PointFour()
    {
        if (fiveNumbers == 4)
        {
            MoveProcessing(navPoint04);
        }
    }

    private void PointFive()
    {
        if (fiveNumbers == 5)
        {
            MoveProcessing(navPoint05);
        }
    }

    private void MoveProcessing(Transform navPoint)
    {
        curNavPoint = navPoint;
        if (_fireBossManager.stageOne)
        {
            return;
        }

        if (enemyNavAgent.destination.x != navPoint.position.x || enemyNavAgent.destination.y != navPoint.position.y || enemyNavAgent.destination.z != navPoint.position.z)
        {
            MovingToNavPoint(navPoint);
        }

        if (enemyNavAgent.remainingDistance <= enemyNavAgent.stoppingDistance)
        {
            ArrivedAtNavPoint();
        }
    }

    private void MovingToNavPoint(Transform navPoint)
    {
        gameObject.tag = "Enemy";
        fireBossForceField.SetActive(true);
        enemyNavAgent.isStopped = false;
        enemyNavAgent.destination = navPoint.position;
        anim.SetBool(runAnimationName, true);
    }

    private void ArrivedAtNavPoint()
    {
        if (_fireBossTime <= 0)
        {
            _fireBossTime = fireBossStartTime;
            StartCoroutine(DelayingSwitchingBackToIdle());
            return;
        }

        _fireBossTime -= Time.deltaTime;

        gameObject.tag = "FireBoss";
        fireBossForceField.SetActive(false);
        enemyNavAgent.isStopped = true;
        anim.SetBool(runAnimationName, false);
        FacingTarget(PlayerPrefab.transform.position);
        EnemyShooting();
    }

    IEnumerator DelayingSwitchingBackToIdle()
    {
        yield return new WaitForSeconds(.1f);
        _fireBossStates = GlobalVariables.FireBossState.Idle;
        IsEnemyMoving = false;
        StopCoroutine(DelayingSwitchingBackToIdle());
    }

    private void PickingANavPoint()
    {
        if (!pickingFromPoints)
        {
            return;
        }

        fiveNumbers = Random.Range(0, 4);
        switch (fiveNumbers)
        {
            case 1:
                _fireBossPickingAPoint = GlobalVariables.FireBossPickingAPoint.Point1;
                pickingFromPoints = false;
                break;
            case 2:
                _fireBossPickingAPoint = GlobalVariables.FireBossPickingAPoint.Point2;
                pickingFromPoints = false;
                break;
            case 3:
                _fireBossPickingAPoint = GlobalVariables.FireBossPickingAPoint.Point3;
                pickingFromPoints = false;
                break;
            case 4:
                _fireBossPickingAPoint = GlobalVariables.FireBossPickingAPoint.Point4;
                pickingFromPoints = false;
                break;
            case 5:
                _fireBossPickingAPoint = GlobalVariables.FireBossPickingAPoint.Point5;
                pickingFromPoints = false;
                break;
        }
    }

    #endregion FireBossMoving

    #region FireBossRangedAttack

    protected override void EnemyShooting()
    {
        if (timeBetweenShots <= 0)
        {
            FireBossAttack();
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void FireBossAttack()
    {
        if (canUseAttack)
        {
            StartCoroutine(FireBossAttackCo());
        }
    }

    IEnumerator FireBossAttackCo()
    {
        canUseAttack = false;
        _audioSource.PlayOneShot(fireBossAttack);
        anim.SetBool(spellB, true);
        yield return new WaitForSeconds(animTime);
        _fireBossAttack.Projectile();
        anim.SetBool(spellB, false);
        yield return new WaitForSeconds(0.2f);
        canUseAttack = true;
        StopCoroutine(FireBossAttackCo());
    }

    #endregion FireBossRangedAttack

    #region BossRoaring

    public void FireBossRoaring()
    {
        StartCoroutine(BossRoaring());
    }

    IEnumerator BossRoaring()
    {
        _audioSource.PlayOneShot(rawrSound);
        anim.SetBool(roar, true);
        yield return new WaitForSeconds(.5f);
        anim.SetBool(roar, false);
        StopCoroutine(BossRoaring());
    }

    #endregion BossRoaring

    #region FireBossDamage, Death, and Health

    public void DoDamageToFireBoss(float amount)
    {
        if (IsEnemyDead)
        {
            return;
        }

        curEnemyHealth -= amount;
        anim.SetTrigger(takeDamageAnimationName);
        IsEnemyHit = true;
        StartCoroutine(SettingTheBossHitToFalse());
        CheckIfBossIsDead();
        IsEnemyDead = false;
    }

    private void CheckIfBossIsDead()
    {
        if (curEnemyHealth <= 0)
        {
            Death();
        }
    }

    IEnumerator SettingTheBossHitToFalse()
    {
        yield return new WaitForSeconds(damageTime);
        IsEnemyHit = false;
        StopCoroutine(SettingTheBossHitToFalse());
    }

    private float CalculatingFireBossHealth()
    {
        return UIManager.bossCurrentHealth / UIManager.bossMaxHealth;
    }

    protected override void Death()
    {
        if (curEnemyHealth > 0)
        {
            return;
        }

        healthBar.SetActive(false);
        enemyNavAgent.isStopped = true;
        IsEnemyDead = true;
        anim.SetTrigger(dieAnimationName);
        _audioSource.PlayOneShot(deathSound01);
        _audioSource.PlayOneShot(deathSound02);
        _audioSource.PlayOneShot(deathSound03);
        Destroy(gameObject, deathTime);
        enemyPrefab = null;
    }

    #endregion FireBossDamage, Death, and Health

    #endregion FireBossBehaviour

    #region FireBossReset

    private void FireBossReset()
    {
        if (_player.Health > 0.0f) return;

        _fireBossTime = fireBossStartTime;
        curEnemyHealth = maxEnemyHealth;
        UIManager.bossCurrentHealth = curEnemyHealth;
        UIManager.bossMaxHealth = maxEnemyHealth;
        _results = maxEnemyHealth - maxEnemyHealth * 50f / 100f;
        halfLife = _results;
        _fireBossStates = GlobalVariables.FireBossState.Birth;
        IsEnemyDead = false;
        IsEnemyAlerted = false;
        IsEnemyHit = false;
        IsEnemyMoving = false;
        healthBar.SetActive(true);
        //Transform bossTransform = transform;
        //bossTransform.position = bossBeforeBirth.position;
        //bossTransform.rotation = bossBeforeBirth.rotation;
    }

    #endregion FireBossReset
}