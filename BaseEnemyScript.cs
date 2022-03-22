using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[System.Serializable]
public class BaseEnemyScript : MonoBehaviour
{
    #region Variables

    [Header("bools for referencing")]
    [Tooltip("The bool for if the enemy has been spawned")]
    protected bool IsEnemySpawned;

    [Tooltip("The bool for if the enemy is dead or not")]
    protected bool IsEnemyDead;

    [Tooltip("This is the bool for if the enemy was hit")]
    [SerializeField]
    protected bool IsEnemyHit;

    [Tooltip("This is the bool for if the player is within distance of the enemy and the enemy has been alerted")]
    protected bool IsEnemyAlerted;

    [Tooltip("The bool for if the enemy is moving")]
    protected bool IsEnemyMoving;

    [Tooltip("This is the bool for if the enemy is melee attacking the player")]
    protected bool IsEnemyMeleeAttacking;

    [Tooltip("This is the bool for if the enemy is Range Attacking the Player")]

    protected bool IsEnemyRangeAttacking;

    [Tooltip("This is the bool for if the enemy is in the Idle Activate State")]
    protected bool IsIdleActivate;

    [Tooltip("This is the bool for if the enemy is activating")]
    protected bool IsActivating;

    [Tooltip("This is the Idle bool for if the enemy is Idling")]
    protected bool IsIdle;

    [Tooltip("This is the Praying bool for if the enemy is praying")]
    protected bool IsPraying;

    [Tooltip("This is the bool for if the enemy is picking from nav points")]
    [SerializeField]
    protected bool pickingFromPoints;

    [Tooltip("This is the bool for if the enemy is counting down")]
    [SerializeField]
    protected bool countingDown;

    [Header("Components")]
    [Tooltip("Put the enemy slider health bar here")]
    protected Slider EnemySlider;

    [Tooltip("Put the enemy prefab here")]
    [SerializeField]
    protected GameObject enemyPrefab;

    [Tooltip("Put the nav mesh agent component here")]
    [SerializeField]
    public NavMeshAgent enemyNavAgent;

    [Tooltip("Put the enemy transform here")]
    [SerializeField]
    protected Transform enemyTransform;

    [Header("Floats")]
    [Tooltip("Set the enemy health here")]
    [SerializeField]
    protected float maxEnemyHealth;

    [Tooltip("Set the distance that the enemy needs to be in before it interacts with the player")]
    [SerializeField]
    protected float safeDistance;

    [Tooltip("Enter a number here for how close the player has to be before the enemy will fire at them when the enemy is alerted because they were fired on")]
    [SerializeField]
    protected float rangeAttackDistance;

    //[SerializeField]
    //protected float runAwayDistance;

    [Tooltip("Enter the number here for when the enemy will melee attack the player")]
    [SerializeField]
    protected float attackRange;

    [Tooltip("Enter the number for when the enemy is to chase the player")]
    [SerializeField]
    protected float chaseRange;

    [Tooltip("Put the amount of time between shots here")]
    [SerializeField]
    protected float startTimeBetweenShots;

    [SerializeField]
    [Tooltip("This is the time the coroutine delays setting the enemyHit back to false in order to play that animation")]
    protected float damageTime;

    [SerializeField]
    [Tooltip("This is the walking radius of the enemy")]
    protected float walkRadius;

    [SerializeField]
    [Tooltip("This is how long before the game object will destroy itself so the animation can play")]
    protected float deathTime;

    [SerializeField]
    [Tooltip("This is setting how fast the enemy will move when its running to be able to time with its animation")]
    public float chaseSpeed;

    [SerializeField]
    [Tooltip("This is setting how fast the enemy will move when it is walking around to time with its animation")]
    public float walkSpeed;

    [SerializeField]
    protected float startTimeBetweenHits;

    [SerializeField]
    protected float timeBetweenHits;

    [Header("Strings for the base enemy animation names")]
    [SerializeField]
    [Tooltip("The animation is a bool")]
    protected string runAnimationName;

    [SerializeField]
    [Tooltip("The animation is a bool")]
    protected string walkAnimationName;

    [SerializeField]
    [Tooltip("The animation is a trigger")]
    protected string dieAnimationName;

    [SerializeField]
    [Tooltip("The animation is a trigger")]
    protected string takeDamageAnimationName;

    [SerializeField]
    [Tooltip("The animation is a trigger")]
    protected string meleeAttackAnimationName;

    [SerializeField]
    [Tooltip("The animation is a bool")]
    protected string preMorphIdle;

    [SerializeField]
    [Tooltip("The animation is a bool")]
    protected string idle00;

    [Header("Sound Effect(Will add those here eventually)")]
    //these are the private variables
    protected GameObject PlayerPrefab;
    protected GlobalVariables.AIStates EnemyStates;
    public float curEnemyHealth;
    //This is just showing the position of the enemy in vector 3
    protected Vector3 Destination;
    //This shows the distance that the player is away from the enemy
    [SerializeField]
    protected float distanceFromPlayer;

    //this shows the distance that the enemy is from the player for shooting
    [SerializeField]
    protected float rangeAttackDistanceFromPlayer;
    //this is the time between shots
    [SerializeField]
    protected float timeBetweenShots;

    //[SerializeField]
    //protected float runAwayTime;

    //[SerializeField]
    //protected float runAwayStartTime;

    protected Animator anim;

    protected EnemyCounter EnemyCounter;
    protected UIManager UIManager;
    protected int _animationVelocity;

    [HideInInspector]
    public PlantControl mushroomSpawnedFrom;

    private bool isDead;

    //[SerializeField]
    //protected string enemyTagName;

    #endregion Variables

    #region Awake, Start, and Update

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        curEnemyHealth = maxEnemyHealth;
        EnemyStates = GlobalVariables.AIStates.Idle;
        IsEnemyDead = false;
        IsEnemyAlerted = false;
        PlayerPrefab = GlobalVariables.Player.gameObject;
        IsEnemyHit = false;
        IsEnemyMoving = false;
        anim = GetComponent<Animator>();
        ResetAttackRange();
        EnemyCounter = GlobalVariables.EnemyCounter;
        UIManager = GlobalVariables.UiManager;
        _animationVelocity = Animator.StringToHash("Velocity");
    }

    protected virtual void Update()
    {
        EnemyState();
        CalculatingEnemyHealth();
        distanceFromPlayer = Vector3.Distance(enemyTransform.position, PlayerPrefab.transform.position);
    }

    #endregion Awake, Start, and Update

    #region EnemyBehaviour

    #region EnemyEnum
    //The state that the enemy is in
    protected virtual void EnemyState()
    {
        switch (EnemyStates)
        {
            case GlobalVariables.AIStates.Moving:
                Moving();
                break;
            case GlobalVariables.AIStates.Idle:
                Idle();
                break;
            case GlobalVariables.AIStates.Chasing:
                Chase();
                break;
            case GlobalVariables.AIStates.RangeAttack:
                RangeAttack();
                break;
            case GlobalVariables.AIStates.Attacking:
                Attacking();
                break;
            //case GlobalVariables.AIStates.Evade:
            //    Evade(enemyTagName);
            //    break;
        }
    }

    #endregion EnemyEnum

    #region EnemyShooting

    protected virtual void EnemyShooting()//this function is empty due to be used in other script that inherit it.
    {

    }

    #endregion EnemyShooting

    #region MeleeAttack

    protected virtual void EnemyMeleeAttacking()
    {
        if (IsEnemyMeleeAttacking)
        {
            IsEnemyMeleeAttacking = true;
            anim.SetTrigger(meleeAttackAnimationName);
            enemyNavAgent.isStopped = true;
            FacingTarget(PlayerPrefab.transform.position);
        }
        else
        {
            IsEnemyMeleeAttacking = false;
        }
    }

    #endregion MeleeAttack

    #region RangeAttack

    //the function for when the enemy has been hit by the player to fire back at the player and is used in child scripts
    protected virtual void RangeAttack()
    {

    }

    public void ResetAttackRange()
    {
        StartCoroutine(ResetWait());
    }

    IEnumerator ResetWait()
    {
        float ar = attackRange;
        attackRange = 0;
        yield return new WaitForSeconds(2f);
        attackRange = ar;
        StopCoroutine(ResetWait());
    }

    #endregion RangeAttack

    #region Chase

    //The chase state for when the player is within range of the enemy also goes back to the moving state when the player is out of range of the enemy
    protected virtual void Chase()
    {
        if (enemyNavAgent == null)
        {
            return;
        }

        if (distanceFromPlayer < safeDistance)
        {
            IsEnemyAlerted = true;
            distanceFromPlayer = Vector3.Distance(enemyTransform.position, PlayerPrefab.transform.position);
            if (IsEnemyAlerted)
            {
                if (distanceFromPlayer < safeDistance && distanceFromPlayer > attackRange)
                {
                    Destination = PlayerPrefab.transform.position;
                    enemyNavAgent.destination = Destination;
                    anim.SetBool(runAnimationName, true);
                    enemyNavAgent.speed = chaseSpeed;
                    anim.SetFloat(_animationVelocity, enemyNavAgent.velocity.magnitude);
                    FacingTarget(Destination);
                }

                if (distanceFromPlayer < attackRange)
                {
                    anim.SetBool(runAnimationName, false);
                    IsEnemyMeleeAttacking = true;
                    enemyNavAgent.isStopped = true;
                    EnemyStates = GlobalVariables.AIStates.Attacking;
                }

                if (distanceFromPlayer >= safeDistance)
                {
                    anim.SetBool(runAnimationName, false);
                    IsEnemyAlerted = false;
                    EnemyStates = GlobalVariables.AIStates.Moving;
                }
            }
            if (!IsEnemyAlerted)
            {
                IsEnemyAlerted = false;
                EnemyStates = GlobalVariables.AIStates.Moving;
                anim.SetBool(runAnimationName, false);
            }
        }
        else if (distanceFromPlayer >= safeDistance)
        {
            anim.SetBool(runAnimationName, false);
            IsEnemyAlerted = false;
            EnemyStates = GlobalVariables.AIStates.Moving;
        }

    }

    #endregion Chase

    #region Idle

    //The Idle state the enemy is in
    protected virtual void Idle()
    {
        IsEnemySpawned = true;
        if (enemyNavAgent != null)
        {
            float timer = 0;
            if (timer != 0)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                EnemyStates = GlobalVariables.AIStates.Moving;
            }
        }
    }

    #endregion Idle

    #region Moving

    //the moving state the enemy is in
    protected virtual void Moving()
    {
        IsEnemyMoving = true;
        distanceFromPlayer = Vector3.Distance(enemyTransform.position, PlayerPrefab.transform.position);

        if (IsEnemySpawned && !enemyNavAgent.pathPending && enemyNavAgent.remainingDistance < .5f && enemyPrefab != null && distanceFromPlayer >= safeDistance)
        {
            enemyNavAgent.SetDestination(PickNextLocation(walkRadius));
            IsEnemyMoving = true;
            IsEnemyAlerted = false;
            anim.SetBool(walkAnimationName, true);
            enemyNavAgent.speed = walkSpeed;
            anim.SetFloat(_animationVelocity, enemyNavAgent.velocity.magnitude);
        }

        if (distanceFromPlayer <= safeDistance)
        {
            IsEnemyMoving = false;
            anim.SetBool(walkAnimationName, false);
            EnemyStates = GlobalVariables.AIStates.Chasing;
        }
        //else if (distanceFromPlayer >= safeDistance)
        //{
        //    //IsEnemyAlerted = false;
        //    //IsEnemyMoving = true;
        //    //anim.SetBool(walkAnimationName, true);
        //    //anim.SetFloat(_animationVelocity, enemyNavAgent.velocity.magnitude);
        //    //EnemyStates = GlobalVariables.AIStates.Moving;
        //}
    }

    #endregion Moving


    #region Attacking

    protected virtual void Attacking()
    {
        IsEnemyMeleeAttacking = true;
        distanceFromPlayer = Vector3.Distance(enemyTransform.position, PlayerPrefab.transform.position);

        if (distanceFromPlayer < attackRange)
        {
            IsEnemyMeleeAttacking = true;

            anim.SetTrigger(meleeAttackAnimationName);
            enemyNavAgent.isStopped = true;
            FacingTarget(PlayerPrefab.transform.position);
            StartCoroutine(SettingTheAttackBackToFalse());

        }
        else if (distanceFromPlayer > attackRange && distanceFromPlayer <= safeDistance)
        {
            enemyNavAgent.isStopped = false;
            IsEnemyMeleeAttacking = false;
            EnemyStates = GlobalVariables.AIStates.Chasing;
        }
        else if (distanceFromPlayer >= safeDistance)
        {
            enemyNavAgent.isStopped = false;
            IsEnemyMeleeAttacking = false;
            EnemyStates = GlobalVariables.AIStates.Moving;
        }
    }

    protected virtual IEnumerator SettingTheAttackBackToFalse()
    {
        yield return new WaitForSeconds(5f);
        IsEnemyMeleeAttacking = false;
        enemyNavAgent.isStopped = false;
        StopCoroutine(SettingTheAttackBackToFalse());
    }

    #endregion Attacking

    //#region Evade

    ////TODO if there are problems with the way the enemies move we may need to adjust this function
    //protected virtual void Evade(string tagName)
    //{
    //    Debug.Log("In Evade");
    //    if (runAwayTime <= 0 && distanceFromPlayer >= runAwayDistance)
    //    {
    //        Debug.Log("Far enough from the player to begin moving again");
    //        runAwayTime = runAwayStartTime;
    //        //enemyPrefab.gameObject.tag = tagName;
    //        EnemyStates = GlobalVariables.AIStates.Moving;
    //    }
    //    else
    //    {
    //        Debug.Log("Nope Nope running away");
    //        runAwayTime -= Time.deltaTime;
    //        if (distanceFromPlayer <= runAwayDistance)
    //        {
    //            Destination = PlayerPrefab.transform.position;
    //            enemyNavAgent.destination = -Destination + new Vector3(transform.position.x + 70f, transform.position.y, transform.position.z + 70f);
    //            enemyNavAgent.speed = chaseSpeed * 2;
    //            enemyPrefab.gameObject.tag = "Enemy";
    //        }
    //    }
    //}

    //#endregion Evade

    #region Death

    //the function for the death of the enemy 
    protected virtual void Death()
    {
        if (curEnemyHealth > 0)
        {
            return;
        }

        if (!isDead)
        {
            KillEnemy();
        }
       
    }

    private void KillEnemy()
    {
        isDead = true;
        EnemyCounter.numOfEnemiesDead++;
        EnemyCounter.currentEnemiesInScene--;
        enemyNavAgent.isStopped = true;
        IsEnemyDead = true;
        anim.SetTrigger(dieAnimationName);
        if (mushroomSpawnedFrom != null)
        {
            mushroomSpawnedFrom.EnemyKilled();
        }
        Destroy(gameObject, deathTime);
        enemyPrefab = null;
    }

    #endregion Death

    #endregion EnemyBehaviour

    #region HealthAndDamage

    //calculating the enemy health
    protected virtual float CalculatingEnemyHealth()
    {
        return curEnemyHealth / maxEnemyHealth;
    }

    //the function for if the player is damaging the enemy on collision with projectiles
    public void DoDamage(float amount)
    {
        //curEnemyHealth -= amount;
        //anim.SetTrigger(takeDamageAnimationName);
        IsEnemyHit = true;
        curEnemyHealth -= amount;
        anim.SetTrigger(takeDamageAnimationName);

        StartCoroutine(SettingTheEnemyHItToFalse());

        if (curEnemyHealth <= 0)
        {
            Death();
            IsEnemyHit = false;
            enemyPrefab = null;
        }
    }

    protected IEnumerator SettingTheEnemyHItToFalse()
    {
        yield return new WaitForSeconds(damageTime);
        IsEnemyHit = false;
        StopCoroutine(SettingTheEnemyHItToFalse());
    }

    #endregion HealthAndDamage

    #region OnTriggerEnterFunctions

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GroundingAttack") || other.CompareTag("FrozenAttack"))
        {
            enemyNavAgent.isStopped = true;
        }

        if(other.CompareTag("Molten"))
        {
            DoDamage(FindObjectOfType<MoltenEnemyDamage>().initialDamage);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GroundingAttack") || other.CompareTag("FrozenAttack"))
        {
            enemyNavAgent.isStopped = false;
        }
    }

    #endregion OnTriggerEnterFunctions

    #region UtilityFunctions

    //the function for the enemy to face the player when the player is within a certain distance of the enemy
    protected virtual void FacingTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, .5f);
    }

    protected virtual Vector3 PickNextLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    #endregion UtilityFunctions
}