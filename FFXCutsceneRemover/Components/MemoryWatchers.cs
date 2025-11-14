using System;
using System.Diagnostics;
using System.Reflection;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Factories;
using FFXCutsceneRemover.Logging;
using FFXCutsceneRemover.Resources;

namespace FFXCutsceneRemover;

/* This class contains all the memory watchers used in the program. All watchers should be added here. */
public static class MemoryWatchers
{
    private const string MODULE = "FFX.exe";

    private static int processBaseAddress;

    public static Process Process;
    public static MemoryWatcherList Watchers = new MemoryWatcherList();

    public static MemoryWatcher<byte> Language;

    public static MemoryWatcher<short> RoomNumber;
    public static MemoryWatcher<short> Storyline;
    public static MemoryWatcher<byte> ForceLoad;
    public static MemoryWatcher<byte> SpawnPoint;
    public static MemoryWatcher<short> BattleState;
    public static MemoryWatcher<short> BattleState2;
    public static MemoryWatcher<short> Input;
    public static MemoryWatcher<byte> Menu;
    public static MemoryWatcher<byte> MenuLock;
    public static MemoryWatcher<short> Intro;
    public static MemoryWatcher<sbyte> State;
    public static MemoryWatcher<float> XCoordinate;
    public static MemoryWatcher<float> YCoordinate;
    public static MemoryWatcher<byte> Camera;
    public static MemoryWatcher<float> Camera_x;
    public static MemoryWatcher<float> Camera_y;
    public static MemoryWatcher<float> Camera_z;
    public static MemoryWatcher<float> CameraRotation;
    public static MemoryWatcher<byte> EncounterStatus;
    public static MemoryWatcher<byte> MovementLock;
    public static MemoryWatcher<byte> ActiveMusicId;
    public static MemoryWatcher<byte> MusicId;
    public static MemoryWatcher<short> RoomNumberAlt;
    public static MemoryWatcher<short> CutsceneAlt;
    public static MemoryWatcher<short> AirshipDestinations;
    public static MemoryWatcher<byte> AuronOverdrives;
    public static MemoryWatcher<int> Gil;
    public static MemoryWatcher<int> TargetFramerate;
    public static MemoryWatcher<int> Dialogue1;
    public static MemoryWatcher<byte> DialogueBoxStructs;
    public static MemoryWatcher<byte> PlayerTurn;
    public static MemoryWatcher<int> FrameCounterFromLoad;

    // Event File
    public static MemoryWatcher<int> EventFileStart;

    // Deep Pointers
    public static MemoryWatcher<int> HpEnemyA;
    public static MemoryWatcher<byte> GuadoCount;
    public static MemoryWatcher<short> NPCLastInteraction;
    public static MemoryWatcher<byte> TidusActionCount;
    public static MemoryWatcher<float> TidusXCoordinate;
    public static MemoryWatcher<float> TidusYCoordinate;
    public static MemoryWatcher<float> TidusZCoordinate;
    public static MemoryWatcher<float> TidusRotation;
    public static MemoryWatcher<byte> DialogueFile;
    public static MemoryWatcher<byte> CutsceneTiming;
    public static MemoryWatcher<byte> IsLoading;
    public static MemoryWatcher<int> CurrentMagicID;
    public static MemoryWatcher<int> ToBeDeletedMagicID;
    public static MemoryWatcher<int> CurrentMagicHandle;
    public static MemoryWatcher<int> ToBeDeletedMagicHandle;
    public static MemoryWatcher<int> EffectPointer;
    public static MemoryWatcher<byte> EffectStatusFlag;
    public static MemoryWatcher<int> AuronTransition;
    public static MemoryWatcher<int> AmmesTransition;
    public static MemoryWatcher<int> TankerTransition;
    public static MemoryWatcher<int> InsideSinTransition;
    public static MemoryWatcher<int> DiveTransition;
    public static MemoryWatcher<int> DiveTransition2;
    public static MemoryWatcher<int> DiveTransition3;
    public static MemoryWatcher<int> GeosTransition;
    public static MemoryWatcher<int> KlikkTransition;
    public static MemoryWatcher<int> AlBhedBoatTransition;
    public static MemoryWatcher<int> UnderwaterRuinsTransition;
    public static MemoryWatcher<int> UnderwaterRuinsTransition2;
    public static MemoryWatcher<int> UnderwaterRuinsOutsideTransition;
    public static MemoryWatcher<int> BeachTransition;
    public static MemoryWatcher<int> LagoonTransition1;
    public static MemoryWatcher<int> LagoonTransition2;
    public static MemoryWatcher<int> ValeforTransition;
    public static MemoryWatcher<int> BesaidNightTransition1;
    public static MemoryWatcher<int> BesaidNightTransition2;
    public static MemoryWatcher<int> KimahriTransition;
    public static MemoryWatcher<int> YunaBoatTransition;
    public static MemoryWatcher<int> SinFinTransition;
    public static MemoryWatcher<int> EchuillesTransition;
    public static MemoryWatcher<int> GeneauxTransition;
    public static MemoryWatcher<int> KilikaElevatorTransition;
    public static MemoryWatcher<int> KilikaTrialsTransition;
    public static MemoryWatcher<int> KilikaAntechamberTransition;
    public static MemoryWatcher<int> IfritTransition;
    public static MemoryWatcher<int> IfritTransition2;
    public static MemoryWatcher<int> JechtShotTransition;
    public static MemoryWatcher<int> OblitzeratorTransition;
    public static MemoryWatcher<int> BlitzballTransition;
    public static MemoryWatcher<int> SahaginTransition;
    public static MemoryWatcher<int> GarudaTransition;
    public static MemoryWatcher<int> RinTransition;
    public static MemoryWatcher<int> ChocoboEaterTransition;
    public static MemoryWatcher<int> GuiTransition;
    public static MemoryWatcher<int> Gui2Transition;
    public static MemoryWatcher<int> DjoseTransition;
    public static MemoryWatcher<int> IxionTransition;
    public static MemoryWatcher<int> ExtractorTransition;
    public static MemoryWatcher<int> SeymoursHouseTransition1;
    public static MemoryWatcher<int> SeymoursHouseTransition2;
    public static MemoryWatcher<int> FarplaneTransition1;
    public static MemoryWatcher<int> FarplaneTransition2;
    public static MemoryWatcher<int> TromellTransition;
    public static MemoryWatcher<int> CrawlerTransition;
    public static MemoryWatcher<int> SeymourTransition;
    public static MemoryWatcher<int> SeymourTransition2;
    public static MemoryWatcher<int> WendigoTransition;
    public static MemoryWatcher<int> SpherimorphTransition;
    public static MemoryWatcher<int> UnderLakeTransition;
    public static MemoryWatcher<int> BikanelTransition;
    public static MemoryWatcher<int> HomeTransition;
    public static MemoryWatcher<int> EvraeTransition;
    public static MemoryWatcher<int> EvraeAirshipTransition;
    public static MemoryWatcher<int> GuardsTransition;
    public static MemoryWatcher<int> BahamutTransition;
    public static MemoryWatcher<int> IsaaruTransition;
    public static MemoryWatcher<int> AltanaTransition;
    public static MemoryWatcher<int> NatusTransition;
    public static MemoryWatcher<int> DefenderXTransition;
    public static MemoryWatcher<int> RonsoTransition;
    public static MemoryWatcher<int> FluxTransition;
    public static MemoryWatcher<int> SanctuaryTransition;
    public static MemoryWatcher<int> SpectralKeeperTransition;
    public static MemoryWatcher<int> SpectralKeeperTransition2;
    public static MemoryWatcher<int> YunalescaTransition;
    public static MemoryWatcher<int> FinsTransition;
    public static MemoryWatcher<int> FinsAirshipTransition;
    public static MemoryWatcher<int> SinCoreTransition;
    public static MemoryWatcher<int> OverdriveSinTransition;
    public static MemoryWatcher<int> OmnisTransition;
    public static MemoryWatcher<int> BFATransition;
    public static MemoryWatcher<int> AeonTransition;
    public static MemoryWatcher<int> YuYevonTransition;
    public static MemoryWatcher<int> YojimboFaythTransition;
    public static MemoryWatcher<int> CutsceneProgress_Max;
    public static MemoryWatcher<int> CutsceneProgress_uVar1;
    public static MemoryWatcher<int> CutsceneProgress_uVar2;
    public static MemoryWatcher<int> CutsceneProgress_uVar3;

    // Encounters
    public static MemoryWatcher<byte> EncounterMapID;
    public static MemoryWatcher<byte> EncounterFormationID1;
    public static MemoryWatcher<byte> EncounterFormationID2;
    public static MemoryWatcher<byte> ScriptedBattleFlag1;
    public static MemoryWatcher<byte> ScriptedBattleFlag2;
    public static MemoryWatcher<int> ScriptedBattleVar1;
    public static MemoryWatcher<int> ScriptedBattleVar3;
    public static MemoryWatcher<int> ScriptedBattleVar4;
    public static MemoryWatcher<byte> EncounterTrigger;

    // Party Configuration
    public static MemoryWatcher<byte> Formation;
    public static MemoryWatcher<byte> RikkuName;
    public static MemoryWatcher<byte> EnableTidus;
    public static MemoryWatcher<byte> EnableYuna;
    public static MemoryWatcher<byte> EnableAuron;
    public static MemoryWatcher<byte> EnableKimahri;
    public static MemoryWatcher<byte> EnableWakka;
    public static MemoryWatcher<byte> EnableLulu;
    public static MemoryWatcher<byte> EnableRikku;
    public static MemoryWatcher<byte> EnableSeymour;
    public static MemoryWatcher<byte> EnableValefor;
    public static MemoryWatcher<byte> EnableIfrit;
    public static MemoryWatcher<byte> EnableIxion;
    public static MemoryWatcher<byte> EnableShiva;
    public static MemoryWatcher<byte> EnableBahamut;
    public static MemoryWatcher<byte> EnableAnima;
    public static MemoryWatcher<byte> EnableYojimbo;
    public static MemoryWatcher<byte> EnableMagus;

    // Encounter Rate
    public static MemoryWatcher<byte> EncountersActiveFlag;
    public static MemoryWatcher<float> TotalDistance;
    public static MemoryWatcher<float> CycleDistance;

    // HP/MP
    public static MemoryWatcher<int> TidusHP;
    public static MemoryWatcher<short> TidusMP;
    public static MemoryWatcher<int> TidusMaxHP;
    public static MemoryWatcher<short> TidusMaxMP;
    public static MemoryWatcher<int> YunaHP;
    public static MemoryWatcher<short> YunaMP;
    public static MemoryWatcher<int> YunaMaxHP;
    public static MemoryWatcher<short> YunaMaxMP;
    public static MemoryWatcher<int> AuronHP;
    public static MemoryWatcher<short> AuronMP;
    public static MemoryWatcher<int> AuronMaxHP;
    public static MemoryWatcher<short> AuronMaxMP;
    public static MemoryWatcher<int> WakkaHP;
    public static MemoryWatcher<short> WakkaMP;
    public static MemoryWatcher<int> WakkaMaxHP;
    public static MemoryWatcher<short> WakkaMaxMP;
    public static MemoryWatcher<int> KimahriHP;
    public static MemoryWatcher<short> KimahriMP;
    public static MemoryWatcher<int> KimahriMaxHP;
    public static MemoryWatcher<short> KimahriMaxMP;
    public static MemoryWatcher<int> LuluHP;
    public static MemoryWatcher<short> LuluMP;
    public static MemoryWatcher<int> LuluMaxHP;
    public static MemoryWatcher<short> LuluMaxMP;
    public static MemoryWatcher<int> RikkuHP;
    public static MemoryWatcher<short> RikkuMP;
    public static MemoryWatcher<int> RikkuMaxHP;
    public static MemoryWatcher<short> RikkuMaxMP;
    public static MemoryWatcher<int> ValeforHP;
    public static MemoryWatcher<short> ValeforMP;
    public static MemoryWatcher<int> ValeforMaxHP;
    public static MemoryWatcher<short> ValeforMaxMP;

    // HP/MP Aeons


    // Special Flags
    public static MemoryWatcher<short> FangirlsOrKidsSkip;
    public static MemoryWatcher<byte> BaajFlag1;
    public static MemoryWatcher<byte> BesaidFlag1;
    public static MemoryWatcher<byte> SSWinnoFlag1;
    public static MemoryWatcher<byte> KilikaMapFlag;
    public static MemoryWatcher<byte> SSWinnoFlag2;
    public static MemoryWatcher<byte> LucaFlag;
    public static MemoryWatcher<byte> LucaFlag2;
    public static MemoryWatcher<byte> BlitzballFlag;
    public static MemoryWatcher<byte> MiihenFlag1;
    public static MemoryWatcher<byte> MiihenFlag2;
    public static MemoryWatcher<byte> MiihenFlag3;
    public static MemoryWatcher<byte> MiihenFlag4;
    public static MemoryWatcher<byte> MRRFlag1;
    public static MemoryWatcher<byte> MRRFlag2;
    public static MemoryWatcher<byte> MoonflowFlag;
    public static MemoryWatcher<byte> MoonflowFlag2;
    public static MemoryWatcher<byte> RikkuOutfit;
    public static MemoryWatcher<byte> TidusWeaponDamageBoost;
    public static MemoryWatcher<byte> GuadosalamShopFlag;
    public static MemoryWatcher<byte> ThunderPlainsFlag;
    public static MemoryWatcher<byte> MacalaniaFlag;
    public static MemoryWatcher<byte> BikanelFlag;
    public static MemoryWatcher<byte> Sandragoras;
    public static MemoryWatcher<byte> ViaPurificoPlatform;
    public static MemoryWatcher<byte> NatusFlag;
    public static MemoryWatcher<ushort> CalmLandsFlag;
    public static MemoryWatcher<byte> WantzFlag;
    public static MemoryWatcher<short> GagazetCaveFlag;
    public static MemoryWatcher<byte> OmegaRuinsFlag;
    public static MemoryWatcher<byte> WantzMacalaniaFlag;

    // Blitzball Abilities
    public static MemoryWatcher<byte> AurochsTeamBytes;
    public static MemoryWatcher<byte> BlitzballBytes;
    public static MemoryWatcher<byte> AurochsPlayer1;

    // Battle Rewards
    public static MemoryWatcher<int> GilBattleRewards;
    public static MemoryWatcher<int> GilRewardCounter;
    public static MemoryWatcher<byte> BattleRewardItemCount;
    public static MemoryWatcher<short> BattleRewardItem1;
    public static MemoryWatcher<byte> BattleRewardItemQty1;
    public static MemoryWatcher<byte> BattleRewardEquipCount;
    public static MemoryWatcher<byte> BattleRewardEquip1;

    // Items
    public static MemoryWatcher<byte> ItemsStart;
    public static MemoryWatcher<byte> ItemsQtyStart;

    // AP
    public static MemoryWatcher<int> CharacterAPRewards;
    public static MemoryWatcher<byte> CharacterAPFlags;

    // Menu Values
    public static MemoryWatcher<int> MenuTriggerValue;

    public static MemoryWatcher<int> MenuValue1;
    public static MemoryWatcher<int> MenuValue2;

    public static MemoryWatcher<int> MenuValue3;
    public static MemoryWatcher<int> MenuValue4;
    public static MemoryWatcher<byte> MenuValue5;
    public static MemoryWatcher<int> MenuValue6;
    public static MemoryWatcher<byte> MenuValue7;

    public static MemoryWatcher<int> SpeedBoostAmount;
    public static MemoryWatcher<int> SpeedBoostVar1;

    public static MemoryWatcher<int> ActorArrayLength;

    public static MemoryWatcher<byte> AutosaveTrigger;
    public static MemoryWatcher<byte> SupressAutosaveOnForceLoad;
    public static MemoryWatcher<byte> SupressAutosaveCounter;

    public static MemoryWatcher<byte> LucaMusicSpheresUnlocked;

    public static MemoryWatcher<byte> RNGArrayOpBytes;

    public static void Initialize(Process process)
    {
        Process = process;
        processBaseAddress = process.Modules[0].BaseAddress.ToInt32();
        DiagnosticLog.Information($"Process base address: {processBaseAddress:X8}");

        // Basic game state watchers
        Language = MemoryWatcherFactory.Create<byte>(nameof(Language));
        RoomNumber = MemoryWatcherFactory.Create<short>(nameof(RoomNumber));
        Storyline = MemoryWatcherFactory.Create<short>(nameof(Storyline));
        ForceLoad = MemoryWatcherFactory.Create<byte>(nameof(ForceLoad));
        SpawnPoint = MemoryWatcherFactory.Create<byte>(nameof(SpawnPoint));
        BattleState = MemoryWatcherFactory.Create<short>(nameof(BattleState));
        BattleState2 = MemoryWatcherFactory.Create<short>(nameof(BattleState2));
        Input = MemoryWatcherFactory.Create<short>(nameof(Input));
        Menu = MemoryWatcherFactory.Create<byte>(nameof(Menu));
        MenuLock = MemoryWatcherFactory.Create<byte>(nameof(MenuLock));
        Intro = MemoryWatcherFactory.Create<short>(nameof(Intro));
        State = MemoryWatcherFactory.Create<sbyte>(nameof(State));
        XCoordinate = MemoryWatcherFactory.Create<float>(nameof(XCoordinate));
        YCoordinate = MemoryWatcherFactory.Create<float>(nameof(YCoordinate));
        Camera = MemoryWatcherFactory.Create<byte>(nameof(Camera));
        Camera_x = MemoryWatcherFactory.Create<float>(nameof(Camera_x));
        Camera_y = MemoryWatcherFactory.Create<float>(nameof(Camera_y));
        Camera_z = MemoryWatcherFactory.Create<float>(nameof(Camera_z));
        CameraRotation = MemoryWatcherFactory.Create<float>(nameof(CameraRotation));
        EncounterStatus = MemoryWatcherFactory.Create<byte>(nameof(EncounterStatus));
        MovementLock = MemoryWatcherFactory.Create<byte>(nameof(MovementLock));
        ActiveMusicId = MemoryWatcherFactory.Create<byte>(nameof(ActiveMusicId));
        MusicId = MemoryWatcherFactory.Create<byte>(nameof(MusicId));
        RoomNumberAlt = MemoryWatcherFactory.Create<short>(nameof(RoomNumberAlt));
        CutsceneAlt = MemoryWatcherFactory.Create<short>(nameof(CutsceneAlt));
        AirshipDestinations = MemoryWatcherFactory.Create<short>(nameof(AirshipDestinations));
        AuronOverdrives = MemoryWatcherFactory.Create<byte>(nameof(AuronOverdrives));
        Gil = MemoryWatcherFactory.Create<int>(nameof(Gil));
        TargetFramerate = MemoryWatcherFactory.Create<int>(nameof(TargetFramerate));
        Dialogue1 = MemoryWatcherFactory.Create<int>(nameof(Dialogue1));
        DialogueBoxStructs = MemoryWatcherFactory.Create<byte>(nameof(DialogueBoxStructs));
        PlayerTurn = MemoryWatcherFactory.Create<byte>(nameof(PlayerTurn));
        FrameCounterFromLoad = MemoryWatcherFactory.Create<int>(nameof(FrameCounterFromLoad));

        // Event File
        EventFileStart = MemoryWatcherFactory.Create<int>(nameof(EventFileStart));

        // Deep Pointers - Misc
        HpEnemyA = MemoryWatcherFactory.Create<int>(nameof(HpEnemyA));
        GuadoCount = MemoryWatcherFactory.Create<byte>(nameof(GuadoCount));
        NPCLastInteraction = MemoryWatcherFactory.Create<short>(nameof(NPCLastInteraction));
        TidusActionCount = MemoryWatcherFactory.Create<byte>(nameof(TidusActionCount));
        TidusXCoordinate = MemoryWatcherFactory.Create<float>(nameof(TidusXCoordinate));
        TidusYCoordinate = MemoryWatcherFactory.Create<float>(nameof(TidusYCoordinate));
        TidusZCoordinate = MemoryWatcherFactory.Create<float>(nameof(TidusZCoordinate));
        TidusRotation = MemoryWatcherFactory.Create<float>(nameof(TidusRotation));
        DialogueFile = MemoryWatcherFactory.Create<byte>(nameof(DialogueFile));
        CutsceneTiming = MemoryWatcherFactory.Create<byte>(nameof(CutsceneTiming));
        IsLoading = MemoryWatcherFactory.Create<byte>(nameof(IsLoading));
        CurrentMagicID = MemoryWatcherFactory.Create<int>(nameof(CurrentMagicID));
        ToBeDeletedMagicID = MemoryWatcherFactory.Create<int>(nameof(ToBeDeletedMagicID));
        CurrentMagicHandle = MemoryWatcherFactory.Create<int>(nameof(CurrentMagicHandle));
        ToBeDeletedMagicHandle = MemoryWatcherFactory.Create<int>(nameof(ToBeDeletedMagicHandle));
        EffectPointer = MemoryWatcherFactory.Create<int>(nameof(EffectPointer));
        EffectStatusFlag = MemoryWatcherFactory.Create<byte>(nameof(EffectStatusFlag));

        // Boss Transition Watchers (all int type)
        MemoryWatcherFactory.CreateBatch<int>(new[]
        {
            nameof(AuronTransition), nameof(AmmesTransition), nameof(TankerTransition),
            nameof(InsideSinTransition), nameof(DiveTransition), nameof(DiveTransition2), nameof(DiveTransition3),
            nameof(GeosTransition), nameof(KlikkTransition), nameof(AlBhedBoatTransition),
            nameof(UnderwaterRuinsTransition), nameof(UnderwaterRuinsTransition2), nameof(UnderwaterRuinsOutsideTransition),
            nameof(BeachTransition), nameof(LagoonTransition1), nameof(LagoonTransition2),
            nameof(ValeforTransition), nameof(BesaidNightTransition1), nameof(BesaidNightTransition2),
            nameof(KimahriTransition), nameof(YunaBoatTransition), nameof(SinFinTransition),
            nameof(EchuillesTransition), nameof(GeneauxTransition), nameof(KilikaElevatorTransition),
            nameof(KilikaTrialsTransition), nameof(KilikaAntechamberTransition), nameof(IfritTransition),
            nameof(IfritTransition2), nameof(JechtShotTransition), nameof(OblitzeratorTransition),
            nameof(BlitzballTransition), nameof(SahaginTransition), nameof(GarudaTransition),
            nameof(RinTransition), nameof(ChocoboEaterTransition), nameof(GuiTransition),
            nameof(Gui2Transition), nameof(DjoseTransition), nameof(IxionTransition),
            nameof(ExtractorTransition), nameof(SeymoursHouseTransition1), nameof(SeymoursHouseTransition2),
            nameof(FarplaneTransition1), nameof(FarplaneTransition2), nameof(TromellTransition),
            nameof(CrawlerTransition), nameof(SeymourTransition), nameof(SeymourTransition2),
            nameof(WendigoTransition), nameof(SpherimorphTransition), nameof(UnderLakeTransition),
            nameof(BikanelTransition), nameof(HomeTransition), nameof(EvraeTransition),
            nameof(EvraeAirshipTransition), nameof(GuardsTransition), nameof(BahamutTransition),
            nameof(IsaaruTransition), nameof(AltanaTransition), nameof(NatusTransition),
            nameof(DefenderXTransition), nameof(RonsoTransition), nameof(FluxTransition),
            nameof(SanctuaryTransition), nameof(SpectralKeeperTransition), nameof(SpectralKeeperTransition2),
            nameof(YunalescaTransition), nameof(FinsTransition), nameof(FinsAirshipTransition),
            nameof(SinCoreTransition), nameof(OverdriveSinTransition), nameof(OmnisTransition),
            nameof(BFATransition), nameof(AeonTransition), nameof(YuYevonTransition),
            nameof(YojimboFaythTransition), nameof(CutsceneProgress_Max), nameof(CutsceneProgress_uVar1),
            nameof(CutsceneProgress_uVar2), nameof(CutsceneProgress_uVar3)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        // Encounters
        MemoryWatcherFactory.CreateBatch<byte>(new[]
        {
            nameof(EncounterMapID), nameof(EncounterFormationID1), nameof(EncounterFormationID2),
            nameof(ScriptedBattleFlag1), nameof(ScriptedBattleFlag2), nameof(EncounterTrigger)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        MemoryWatcherFactory.CreateBatch<int>(new[]
        {
            nameof(ScriptedBattleVar1), nameof(ScriptedBattleVar3), nameof(ScriptedBattleVar4)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        // Party Configuration
        MemoryWatcherFactory.CreateBatch<byte>(new[]
        {
            nameof(Formation), nameof(RikkuName), nameof(EnableTidus), nameof(EnableYuna),
            nameof(EnableAuron), nameof(EnableKimahri), nameof(EnableWakka), nameof(EnableLulu),
            nameof(EnableRikku), nameof(EnableSeymour), nameof(EnableValefor), nameof(EnableIfrit),
            nameof(EnableIxion), nameof(EnableShiva), nameof(EnableBahamut), nameof(EnableAnima),
            nameof(EnableYojimbo), nameof(EnableMagus)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });
        // Encounter Rate
        EncountersActiveFlag = MemoryWatcherFactory.Create<byte>(nameof(EncountersActiveFlag));
        TotalDistance = MemoryWatcherFactory.Create<float>(nameof(TotalDistance));
        CycleDistance = MemoryWatcherFactory.Create<float>(nameof(CycleDistance));

        // HP/MP - Batch create for characters
        MemoryWatcherFactory.CreateBatch<int>(new[]
        {
            nameof(TidusHP), nameof(TidusMaxHP), nameof(YunaHP), nameof(YunaMaxHP),
            nameof(AuronHP), nameof(AuronMaxHP), nameof(WakkaHP), nameof(WakkaMaxHP),
            nameof(KimahriHP), nameof(KimahriMaxHP), nameof(LuluHP), nameof(LuluMaxHP),
            nameof(RikkuHP), nameof(RikkuMaxHP), nameof(ValeforHP), nameof(ValeforMaxHP)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        MemoryWatcherFactory.CreateBatch<short>(new[]
        {
            nameof(TidusMP), nameof(TidusMaxMP), nameof(YunaMP), nameof(YunaMaxMP),
            nameof(AuronMP), nameof(AuronMaxMP), nameof(WakkaMP), nameof(WakkaMaxMP),
            nameof(KimahriMP), nameof(KimahriMaxMP), nameof(LuluMP), nameof(LuluMaxMP),
            nameof(RikkuMP), nameof(RikkuMaxMP), nameof(ValeforMP), nameof(ValeforMaxMP)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        // Special Flags
        FangirlsOrKidsSkip = MemoryWatcherFactory.Create<short>(nameof(FangirlsOrKidsSkip));
        CalmLandsFlag = MemoryWatcherFactory.Create<ushort>(nameof(CalmLandsFlag));
        GagazetCaveFlag = MemoryWatcherFactory.Create<short>(nameof(GagazetCaveFlag));

        MemoryWatcherFactory.CreateBatch<byte>(new[]
        {
            nameof(BaajFlag1), nameof(BesaidFlag1), nameof(SSWinnoFlag1), nameof(KilikaMapFlag),
            nameof(SSWinnoFlag2), nameof(LucaFlag), nameof(LucaFlag2), nameof(BlitzballFlag),
            nameof(MiihenFlag1), nameof(MiihenFlag2), nameof(MiihenFlag3), nameof(MiihenFlag4),
            nameof(MRRFlag1), nameof(MRRFlag2), nameof(MoonflowFlag), nameof(MoonflowFlag2),
            nameof(RikkuOutfit), nameof(TidusWeaponDamageBoost), nameof(GuadosalamShopFlag),
            nameof(ThunderPlainsFlag), nameof(MacalaniaFlag), nameof(BikanelFlag),
            nameof(Sandragoras), nameof(ViaPurificoPlatform), nameof(NatusFlag),
            nameof(WantzFlag), nameof(OmegaRuinsFlag), nameof(WantzMacalaniaFlag)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        // Blitzball Abilities
        MemoryWatcherFactory.CreateBatch<byte>(new[]
        {
            nameof(AurochsTeamBytes), nameof(BlitzballBytes), nameof(AurochsPlayer1)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        // Battle Rewards
        GilBattleRewards = MemoryWatcherFactory.Create<int>(nameof(GilBattleRewards));
        GilRewardCounter = MemoryWatcherFactory.Create<int>(nameof(GilRewardCounter));
        BattleRewardItemCount = MemoryWatcherFactory.Create<byte>(nameof(BattleRewardItemCount));
        BattleRewardItem1 = MemoryWatcherFactory.Create<short>(nameof(BattleRewardItem1));
        BattleRewardItemQty1 = MemoryWatcherFactory.Create<byte>(nameof(BattleRewardItemQty1));
        BattleRewardEquipCount = MemoryWatcherFactory.Create<byte>(nameof(BattleRewardEquipCount));
        BattleRewardEquip1 = MemoryWatcherFactory.Create<byte>(nameof(BattleRewardEquip1));

        //Items
        ItemsStart = MemoryWatcherFactory.Create<byte>(nameof(ItemsStart));
        ItemsQtyStart = MemoryWatcherFactory.Create<byte>(nameof(ItemsQtyStart));

        // AP
        CharacterAPRewards = MemoryWatcherFactory.Create<int>(nameof(CharacterAPRewards));
        CharacterAPFlags = MemoryWatcherFactory.Create<byte>(nameof(CharacterAPFlags));

        // Menu Values
        MemoryWatcherFactory.CreateBatch<int>(new[]
        {
            nameof(MenuTriggerValue), nameof(MenuValue1), nameof(MenuValue2), nameof(MenuValue3),
            nameof(MenuValue4), nameof(MenuValue6), nameof(SpeedBoostAmount), nameof(SpeedBoostVar1),
            nameof(ActorArrayLength)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        MemoryWatcherFactory.CreateBatch<byte>(new[]
        {
            nameof(MenuValue5), nameof(MenuValue7), nameof(AutosaveTrigger),
            nameof(SupressAutosaveOnForceLoad), nameof(SupressAutosaveCounter)
        }, (name, watcher) =>
        {
            typeof(MemoryWatchers).GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, watcher);
        });

        // Break Specific Values
        LucaMusicSpheresUnlocked = MemoryWatcherFactory.Create<byte>(nameof(LucaMusicSpheresUnlocked));

        // RNGMod
        RNGArrayOpBytes = MemoryWatcherFactory.Create<byte>(nameof(RNGArrayOpBytes));

        HpEnemyA.FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull;

        Watchers.Clear();

        foreach (FieldInfo field in typeof(MemoryWatchers).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(MemoryWatcher<>))
            {
                Watchers.Add(field.GetValue(null) as MemoryWatcher);
            }
        }
    }

    private static MemoryWatcher<T> GetMemoryWatcher<T>(MemoryLocation data) where T : struct
    {
        MemoryWatcher<T> watcher = data.Offsets.Length == 0
            ? new MemoryWatcher<T>(GetPointerAddress(data.BaseAddress))
            : new MemoryWatcher<T>(GetDeepPointer(data.BaseAddress, data.Offsets));

        watcher.Name = data.Name;
        return watcher;
    }

    private static IntPtr GetPointerAddress(int offset)
    {
        return new IntPtr(processBaseAddress + offset);
    }

    private static DeepPointer GetDeepPointer(int baseAddress, int[] offsets)
    {
        return new DeepPointer(MODULE, baseAddress, offsets);
    }

    public static int GetBaseAddress()
    {
        return processBaseAddress;
    }
}
