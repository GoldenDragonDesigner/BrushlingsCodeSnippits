using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FireBossManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveSpawner
    {
        #region WaveSpawnerVariables

        [Tooltip("Name of Wave to Spawn")]
        public string nameOfWave;

        [Tooltip("Put the enemy prefab that will be spawning here")]
        public GameObject fireEnemyToSpawn;

        [Tooltip("The number of enemies that will be spawned")]
        public int numberOfFireEnemiesThatSpawn;

        #endregion WaveSpawnerVariables
    }

    #region Variables

    #region SpawnerVariables

    public WaveSpawner[] wavesOfEnemies = new WaveSpawner[7];

    public bool pickingAMushroom;

    private GlobalVariables.WaveStates _waveStates;

    public int _sevenMushrooms;

    [HideInInspector]
    public int enemiesSpawnedMushroomOne;
    [HideInInspector]
    public int enemiesSpawnedMushroomTwo;
    [HideInInspector]
    public int enemiesSpawnedMushroomThree;
    [HideInInspector]
    public int enemiesSpawnedMushroomFour;
    [HideInInspector]
    public int enemiesSpawnedMushroomFive;
    [HideInInspector]
    public int enemiesSpawnedMushroomSix;
    [HideInInspector]
    public int enemiesSpawnedMushroomSeven;

    private int[] _enemiesSpawnedFromMushroomsArray;

    public int minNumberOfEnemiesToSpawn;
    public int maxNumberOfEnemiesToSpawn;

    #endregion SpawnerVariables

    #region FireBossManagerVariables

    [Tooltip("This is the reference to the fire boss and its prefab should go here")]
    public GameObject fireBoss;

    [Tooltip("This is the array of the fire boss mushrooms")]
    public GameObject[] fireBossMushrooms = new GameObject[7];

    [Tooltip("this is the array of the mushrooms that have been triggered")]
    public bool[] fireBossMushroomsTriggered = new bool[7];

    private FireBossMushroomBase[] _mushroomScriptArray;
    private FireBossMushroom_1 _fireBossMushroom1;
    private FireBossMushroom_2 _fireBossMushroom2;
    private FireBossMushroom_3 _fireBossMushroom3;
    private FireBossMushroom_4 _fireBossMushroom4;
    private FireBossMushroom_5 _fireBossMushroom5;
    private FireBossMushroom_6 _fireBossMushroom6;
    private FireBossMushroom_7 _fireBossMushroom7;

    public int fireBossMushroomCount;

    public bool fireEnemiesSpawned;

    public List<GameObject> fireEnemiesTest;

    private GlobalVariables.FireBossManagerState _fireBossManagerState;

    public bool stageOne;
    public bool stageTwo;
    public bool stageThree;
    public bool fireBossUnlocking;

    private FireBoss _fireBoss;

    public GameObject[] firesToDestroy;

    private UIManager _uiManager;
    private PlayerAttack _playerAttack;
    private PlayAnimFireAttackHeart _playAnimFireAttackHeart;

    public GameObject fireTriggerVolume;

    private Player _player;
    #endregion FireBossManagerVariables

    #endregion Variables

    #region Awake, Start, and Update

    private void Awake()
    {
        GlobalVariables.FireBossManager = this;
        _player = GlobalVariables.Player;
    }

    private void Start()
    {
        _fireBoss = GlobalVariables.FireBoss;

        _fireBossManagerState = GlobalVariables.FireBossManagerState.StageOne;

        fireEnemiesSpawned = false;

        stageOne = true;
        stageTwo = false;
        stageThree = false;
        fireBossUnlocking = false;

        _fireBossMushroom1 = fireBossMushrooms[0].GetComponent<FireBossMushroom_1>();
        _fireBossMushroom2 = fireBossMushrooms[1].GetComponent<FireBossMushroom_2>();
        _fireBossMushroom3 = fireBossMushrooms[2].GetComponent<FireBossMushroom_3>();
        _fireBossMushroom4 = fireBossMushrooms[3].GetComponent<FireBossMushroom_4>();
        _fireBossMushroom5 = fireBossMushrooms[4].GetComponent<FireBossMushroom_5>();
        _fireBossMushroom6 = fireBossMushrooms[5].GetComponent<FireBossMushroom_6>();
        _fireBossMushroom7 = fireBossMushrooms[6].GetComponent<FireBossMushroom_7>();
        _mushroomScriptArray = new FireBossMushroomBase[]
        {
            _fireBossMushroom1, _fireBossMushroom2, _fireBossMushroom3, _fireBossMushroom4,
            _fireBossMushroom5, _fireBossMushroom6, _fireBossMushroom7
        };

        _enemiesSpawnedFromMushroomsArray = new[]
        {
            enemiesSpawnedMushroomOne, enemiesSpawnedMushroomTwo, enemiesSpawnedMushroomThree,
            enemiesSpawnedMushroomFour, enemiesSpawnedMushroomFive, enemiesSpawnedMushroomSix,
            enemiesSpawnedMushroomSeven
        };

        fireBossMushroomsTriggered[0] = false;
        fireBossMushroomsTriggered[1] = false;
        fireBossMushroomsTriggered[2] = false;
        fireBossMushroomsTriggered[3] = false;
        fireBossMushroomsTriggered[4] = false;
        fireBossMushroomsTriggered[5] = false;
        fireBossMushroomsTriggered[6] = false;

        _uiManager = GlobalVariables.UiManager.GetComponent<UIManager>();
        _playerAttack = GlobalVariables.PlayerAttack.GetComponent<PlayerAttack>();
        _playAnimFireAttackHeart = GlobalVariables.PlayAnimFireAttackHeart;
    }

    private void Update()
    {
        FireManagerState();
        FireBossMushroomTriggered(true);
        PickingAMushroomToSpawnFrom();
        SpawningWaves();
        FireBossManagerReset();
    }

    #endregion Awake, Start, and Update

    #region FireBossManagerEnum

    private void FireManagerState()
    {
        switch (_fireBossManagerState)
        {
            case GlobalVariables.FireBossManagerState.StageOne:
                StageOne();
                break;
            case GlobalVariables.FireBossManagerState.StageTwo:
                StageTwo();
                break;
            case GlobalVariables.FireBossManagerState.StageThree:
                StageThree();
                break;
            case GlobalVariables.FireBossManagerState.FireBossUnlocking:
                FireBossUnlocking();
                break;
        }
    }

    #endregion FireBossManagerEnum

    #region FireBossManagerStageOne

    private void StageOne()
    {
        stageOne = true;
        MushroomAndEnemyCheck();
    }

    #endregion FireBossManagerStageOne

    #region FireBossManagerStageTwo

    private void StageTwo()
    {
        stageTwo = true;
        if (_fireBoss.curEnemyHealth <= _fireBoss.halfLife)
        {
            stageTwo = false;
            _fireBossManagerState = GlobalVariables.FireBossManagerState.StageThree;
        }
    }

    #endregion FireBossManagerStageTwo

    #region FireBossManagerStageThree

    private void StageThree()
    {
        stageThree = true;
        MushroomAndEnemyCheck();
    }

    #endregion FireBossManagerStageThree

    #region FireBossManagerUnlocking

    private void FireBossUnlocking()
    {
        if (!fireBossUnlocking)
        {
            return;
        }

        foreach (GameObject fires in firesToDestroy)
        {
            Destroy(fires);
        }

        _playAnimFireAttackHeart.PlayAnimation();
        _uiManager.GivePlayerHeart();

        GlobalVariables.PlayerAttack.canUseFireAttack = true;
        _playerAttack.ActivateFire();

        fireBossUnlocking = false;
    }

    public void GivingFire()
    {
        _uiManager.GivePlayerHeart();
        GlobalVariables.PlayerAttack.canUseFireAttack = true;
        _playerAttack.ActivateFire();
    }

    #endregion FireBossManagerUnlocking

    #region FireBossSummoningEnemies

    #region TheMushrooms

    #region SpawningWavesEnum

    public void SpawningWaves()
    {
        if (_fireBoss == null || !_fireBoss.summoning)
        {
            return;
        }

        switch (_waveStates)
        {
            case GlobalVariables.WaveStates.MushroomOne:
                SpawnEnemiesFromMushroom(0);
                break;
            case GlobalVariables.WaveStates.MushroomTwo:
                SpawnEnemiesFromMushroom(1);
                break;
            case GlobalVariables.WaveStates.MushroomThree:
                SpawnEnemiesFromMushroom(2);
                break;
            case GlobalVariables.WaveStates.MushroomFour:
                SpawnEnemiesFromMushroom(3);
                break;
            case GlobalVariables.WaveStates.MushroomFive:
                SpawnEnemiesFromMushroom(4);
                break;
            case GlobalVariables.WaveStates.MushroomSix:
                SpawnEnemiesFromMushroom(5);
                break;
            case GlobalVariables.WaveStates.MushroomSeven:
                SpawnEnemiesFromMushroom(6);
                break;
        }
    }

    #endregion SpawningWavesEnum

    #region PickingAMushroomEnum

    private void PickingAMushroomToSpawnFrom()
    {
        if (!pickingAMushroom) { return; }

        _sevenMushrooms = Random.Range(0, 6);
        switch (_sevenMushrooms)
        {
            case 1:
                _waveStates = GlobalVariables.WaveStates.MushroomOne;
                pickingAMushroom = false;
                break;
            case 2:
                _waveStates = GlobalVariables.WaveStates.MushroomTwo;
                pickingAMushroom = false;
                break;
            case 3:
                _waveStates = GlobalVariables.WaveStates.MushroomThree;
                pickingAMushroom = false;
                break;
            case 4:
                _waveStates = GlobalVariables.WaveStates.MushroomFour;
                pickingAMushroom = false;
                break;
            case 5:
                _waveStates = GlobalVariables.WaveStates.MushroomFive;
                pickingAMushroom = false;
                break;
            case 6:
                _waveStates = GlobalVariables.WaveStates.MushroomSix;
                pickingAMushroom = false;
                break;
            case 7:
                _waveStates = GlobalVariables.WaveStates.MushroomSeven;
                pickingAMushroom = false;
                break;
        }

    }

    #endregion PickingAMushroomEnum

    #region SpawningFromMushroom

    private void SpawnEnemiesFromMushroom(int mushroomNumber)
    {
        if (_mushroomScriptArray[mushroomNumber].fireBossMushroom || !fireEnemiesSpawned)
        {
            return;
        }

        wavesOfEnemies[mushroomNumber].numberOfFireEnemiesThatSpawn = Random.Range(minNumberOfEnemiesToSpawn, maxNumberOfEnemiesToSpawn);
        for (int i = 0; i < wavesOfEnemies[mushroomNumber].numberOfFireEnemiesThatSpawn; i++)
        {
            if (_enemiesSpawnedFromMushroomsArray[mushroomNumber] >=
                wavesOfEnemies[mushroomNumber].numberOfFireEnemiesThatSpawn)
            {
                fireEnemiesSpawned = false;
            }
            else
            {
                Transform mushroomTransform = _mushroomScriptArray[mushroomNumber].transform;
                GameObject tempFireEnemy = Instantiate(wavesOfEnemies[mushroomNumber].fireEnemyToSpawn, mushroomTransform.position, mushroomTransform.rotation);
                tempFireEnemy.name = "FireEnemy";
                _enemiesSpawnedFromMushroomsArray[mushroomNumber]++;
            }
        }
    }

    #endregion SpawningFromMushroom

    #endregion TheMushrooms

    #endregion FireBossSummoningEnemies

    #region FireBossMushroomControls

    private int FireBossMushroomTriggered(bool amount)
    {
        fireBossMushroomCount = 0;
        for(int i = 0; i < fireBossMushroomsTriggered.Length; i++)
        {
            if (fireBossMushroomsTriggered[i])
            {
                fireBossMushroomCount++;
            }
        }
        return fireBossMushroomCount;
    }

    public void MushroomAndEnemyCheck()
    {
        if (stageOne)
        {
            if (fireBossMushroomCount >= fireBossMushroomsTriggered.Length && fireEnemiesTest.Count <= 0)
            {
                stageOne = false;
                _fireBossManagerState = GlobalVariables.FireBossManagerState.StageTwo;
            }
            else
            {
                if (fireEnemiesTest.Count <= 0)
                {
                    fireEnemiesSpawned = true;
                    _fireBossManagerState = GlobalVariables.FireBossManagerState.StageOne;
                }
            }
        }
        else if (stageThree)
        {
            if (fireBossMushroomCount >= fireBossMushroomsTriggered.Length && fireEnemiesTest.Count <= 0)
            {
                    _fireBoss.FireBossRoaring();
                    ResettingTheMushrooms();
                    fireEnemiesSpawned = true;
                    _fireBoss.Summoning();
            }
            else
            {
                if (fireEnemiesTest.Count <= 0)
                {
                    fireEnemiesSpawned = true;
                }
            }

            if (fireBoss == null)
            {
                foreach(GameObject bossBugs in fireEnemiesTest)
                {
                    Destroy(bossBugs.gameObject);
                }
                fireEnemiesTest.Clear();
                stageThree = false;
                fireBossUnlocking = true;
                _fireBossManagerState = GlobalVariables.FireBossManagerState.FireBossUnlocking;
            }
        }
    }

    public void ResettingTheMushrooms()
    {
        if (_fireBossMushroom1.fireBossMushroom)
        {
            fireBossMushrooms[0].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
        if (_fireBossMushroom2.fireBossMushroom)
        {
            fireBossMushrooms[1].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
        if (_fireBossMushroom3.fireBossMushroom)
        {
            fireBossMushrooms[2].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
        if (_fireBossMushroom4.fireBossMushroom)
        {
            fireBossMushrooms[3].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
        if (_fireBossMushroom5.fireBossMushroom)
        {
            fireBossMushrooms[4].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
        if (_fireBossMushroom6.fireBossMushroom)
        {
            fireBossMushrooms[5].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
        if (_fireBossMushroom7.fireBossMushroom)
        {
            fireBossMushrooms[6].GetComponent<PlantControl>().BossReSettingTheMushrooms();
        }
    }

    #endregion FireBossMushroomControls

    #region FireBossManagerReset

    private void FireBossManagerReset()
    {
        if (_player.Health > 0.0f) return;

        fireEnemiesSpawned = false;

        stageOne = true;
        stageTwo = false;
        stageThree = false;
        fireBossUnlocking = false;

        _fireBossMushroom1 = fireBossMushrooms[0].GetComponent<FireBossMushroom_1>();
        _fireBossMushroom2 = fireBossMushrooms[1].GetComponent<FireBossMushroom_2>();
        _fireBossMushroom3 = fireBossMushrooms[2].GetComponent<FireBossMushroom_3>();
        _fireBossMushroom4 = fireBossMushrooms[3].GetComponent<FireBossMushroom_4>();
        _fireBossMushroom5 = fireBossMushrooms[4].GetComponent<FireBossMushroom_5>();
        _fireBossMushroom6 = fireBossMushrooms[5].GetComponent<FireBossMushroom_6>();
        _fireBossMushroom7 = fireBossMushrooms[6].GetComponent<FireBossMushroom_7>();
        _mushroomScriptArray = new FireBossMushroomBase[]
        {
            _fireBossMushroom1, _fireBossMushroom2, _fireBossMushroom3, _fireBossMushroom4,
            _fireBossMushroom5, _fireBossMushroom6, _fireBossMushroom7
        };

        _enemiesSpawnedFromMushroomsArray = new[]
        {
            enemiesSpawnedMushroomOne, enemiesSpawnedMushroomTwo, enemiesSpawnedMushroomThree,
            enemiesSpawnedMushroomFour, enemiesSpawnedMushroomFive, enemiesSpawnedMushroomSix,
            enemiesSpawnedMushroomSeven
        };

        fireBossMushroomsTriggered[0] = false;
        fireBossMushroomsTriggered[1] = false;
        fireBossMushroomsTriggered[2] = false;
        fireBossMushroomsTriggered[3] = false;
        fireBossMushroomsTriggered[4] = false;
        fireBossMushroomsTriggered[5] = false;
        fireBossMushroomsTriggered[6] = false;

        if(fireEnemiesTest.Count <= 0)
        {
            return;
        }
        else
        {
            foreach (GameObject bossBugs in fireEnemiesTest)
            {
                Destroy(bossBugs.gameObject);
            }
            fireEnemiesTest.Clear();
        }

        fireTriggerVolume.SetActive(true);
    }

    #endregion FireBossManagerReset
}