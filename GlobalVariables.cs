

public static class GlobalVariables
{
    #region Player

    public static Player Player;
    public static PlayerAttack PlayerAttack;
    public static bool FirstPlayerDeath = false;
    public static int TotalPlayerDeaths = 0;
    public static FOVUpdater FOVUpdater;

    #endregion Player

    #region Managers

    public static UIManager UiManager;
    public static EnemySpawningManager EnemySpawningManager;
    public static GameManager GameManager;
    public static ColorChnageManager ColorChangeManager;
    public static MainGameAudioManagement MainGameAudioManagement;
    public static DemoAudioManagement DemoAudioManagement;
    public static AudioManager AudioManager;
    public static DemoAudioManager DemoAudioManager;
    public static GameMenus GameMenus;
    public static GrayscaleResourceManager GrayscaleResourceManager;
    public static ColorResourceManager ColorResourceManager;
    public static AudioResourceManager AudioResourceManager;

    #endregion Managers

    #region WorldControl

    public static WorldControl WorldControl;

    public enum ZoneIds : int
    {
        FireZone,
        MoltenZone,
        WaterZone,
        GroundingZone,
        WindZone,
        FrozenZone,
        FinalBossZone,
        WaterBossFight
    }
    #endregion WorldControl

    #region SavingStuff

    public static MenuSave MenuSave;
    public static bool IsGameLoading;

    #endregion SavingStuff

    #region PlantStuff

    public static PlantControl PlantControl;
    public static AreaControl AreaControl;
    public static ObjectiveSystem ObjectiveSystem;
    public static GeyserPuzzle GeyserPuzzle;
    public static Outline Outline;
    public static OrbColoring OrbColoring;
    public static bool AreMaterialsLoaded;

    #endregion PlantStuff

    #region RegularEnemies

    //public static BasicEnemy BASICENEMY;
    //public static FireEnemy FIREENEMY;
    //public static MoltenEnemy MOLTENENEMY;
    //public static WaterEnemy WATERENEMY;
    //public static GroundingEnemy GROUNDINGENEMY;
    //public static WindEnemy WINDENEMY;
    //public static FrozenEnemy FROZENENEMY;
    //public static SlowEnemy SLOWENEMY;

    public static EnemyCounter EnemyCounter;
    public static EnemyObjectiveSystem EnemyObjectiveSystem;
    public static Carver CARVER;

    #endregion RegularEnemies

    #region EnemyEnumerations

    public enum AIStates { Idle, FindingNextLocation, Moving, Attacking, AttackingTwo, Chasing, Searching, RangeAttack, Activate, IdleActivate, Roll, Whirl, Pray, Slam, Morph, CountingDown, Evade, Summon }

    #endregion EnemyEnumerations

    #region FireBoss

    public static FireBoss FireBoss;
    public static FireBossManager FireBossManager;
    public static FireBossAttack FireBossAttack;
    public static FireBossAttackPoint FireBossAttackPoint;
    public static PlayAnimFireAttackHeart PlayAnimFireAttackHeart;
    public enum FireBossState { Idle, Summon, Birth, Moving }
    public enum FireBossManagerState { StageOne, StageTwo, StageThree, FireBossUnlocking }
    public enum FireBossPickingAPoint { Point1, Point2, Point3, Point4, Point5 }
    public enum WaveStates { MushroomOne, MushroomTwo, MushroomThree, MushroomFour, MushroomFive, MushroomSix, MushroomSeven }

    #endregion FireBoss

    #region MoltenBoss
    public static MoltenBoss MoltenBoss;
    public static MoltenBossManager MoltenBossManager;
    public static MoltenBossAttack MoltenBossAttack;
    public static MoltenBossAttackPointL MoltenBossAttackPointL;
    public static MoltenBossAttackPointR MoltenBossAttackPointR;

    public enum MoltenBossManagerState { StageOne, StageTwo, StageThree, StageFour, MoltenBosUnlocking, RandomizingMushrooms, SwitchingToNewState }
    public enum MoltenBossMushroomWaveStates { MoltenMushroomOne, MoltenMushroomTwo, MoltenMushroomThree, MoltenMushroomFour, MoltenMushroomFive, MoltenMushroomSix, MoltenMushroomSeven }
    public enum MoltenBossStates { Waiting, Activating, Attacking }

    #endregion MoltenBoss

    #region WaterBoss

    public static WaterBossManager WaterBossManager;
    public static WaterBossOne WaterBossOne;
    public static WaterBossTwo WaterBossTwo;
    public static WaterBossAttackPoint WaterBossAttackPoint;
    //public static WaterBossAttackPoint2 WaterBossAttackPoint2;
    public static WaterBossProjectileAttack WaterBossProjectileAttack;
    //public static WaterBossProjectileAttack2 WaterBossProjectileAttack2;
    public static TeleportToCharacter TeleportToCharacter;
    //public static TeleportToCharacter2 TeleportToCharacter2;
    public static WhirlpoolAttack WhirlpoolAttack;
    //public static WhirlpoolAttack2 WhirlpoolAttack2;
    public static StartWaves StartWaves;
    public static WaterBossMushroomPuzzle WaterBossMushroomPuzzle;

    public enum WaterBossManagerStates { StageOne, StageTwo, StageThree, WaterBossPowersUnlocking }
    public enum WaterBossStates { IdlePremorph, Morph, Idle, Attack, Whirl, Slam }
    public enum WaterBossPuzzleStates { FirstLayer, SecondLayer, ThirdLayer }
    #endregion WaterBoss

    #region GroundingBoss

    public static GroundingBossBase GroundingBossBase;
    public static GroundingBossManager GroundingBossManager;
    public enum CanDrawRound { RoundOne, RoundTwo, RoundThree, RoundFour, RoundFive, RoundSix, RoundSeven, RoundEight, RoundNine, RoundTen, RoundEleven, RoundTwelve, RoundThirteen }
    public enum CanDrawRoundTwo { RoundOneStageTwo, RoundTwoStageTwo, RoundThreeStageTwo }
    public enum BossState { StageOne, StageTwo, StageThree, Rolling, Activate, Run, Death, GroundingBossUnlock, CountingDown, CountingDownStageTwo, BossOneConverging, BossTwoConverging, BossThreeConverging, BossFourConverging, BossFiveConverging }
    //public enum GroundingBossState { EnemyStateNotFunctioning, UsingNavPoints, Chase, Idle, HuddleIdle, Activating, Moving, Rolling, Attack, }

    #endregion GroundingBoss

    #region WindBoss

    public static ChaseTrigger ChaseTrigger;
    public static WindBossSnipe WindBossSnipe;
    public static WindBossManager WindBossManager;
    public static WBAttack WbAttack;
    public static WindBossAttackPoint WindBossAttackPoint;
    public enum ManagerStates { Pursuing, Fighting, WindBossUnlocking }
    public static WindBossMovement WindBossMovement;
    public enum PickingAPoint { Point1, Point2, Point3, Point4, Point5, Point6, Point7, Point8, Point9, Point10, Point11, Point12, Point13 }

    #endregion WindBoss

    #region FrozenBoss

    public static FrozenBoss FrozenBoss;
    public static FrozenBossManager FrozenBossManager;
    public enum FrozenWaveStates { WaveOne, WaveTwo, WaveThree, WaveFour, WaveFive }
    public enum FrozenBossStates { IdleStates, AttackStates, MagicStates, DamageOrHealingStates, Walking, RuningState, FlyingState, FlyingDownState, SummonState, EvadeState }

    public enum FrozenBossManagerStages { StageOne, StageTwo, StageThree }

    #endregion FrozenBoss

    #region FinalBoss

    public static FinalBossController FinalBossController;
    public static FBBasic FbBasic;
    public static FBFire FbFire;
    public static FBFrozen FbFrozen;
    public static FBEarth FbEarth;
    public static FBWater FbWater;
    public static FBMolten FbMolten;
    public static FBAttackPoint FbAttackPoint;
    public static FinalBossCloning FinalBossCloning;
    public static FinalBossTeleport FinalBossTeleport;

    #endregion FinalBoss
}