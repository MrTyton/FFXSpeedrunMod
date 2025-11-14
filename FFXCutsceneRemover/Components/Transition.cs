using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Logging;
using FFXCutsceneRemover.Resources;

namespace FFXCutsceneRemover;

/* Represents a change in current state of the game's memory. Create one of these objects
 * with the values you care about, and Execute() will set the game's state to match this object. */
public class Transition
{
    private static string DEFAULT_DESCRIPTION = "Executing Transition - No Description";
    private static FieldInfo[] publicFields = typeof(Transition).GetFields(BindingFlags.Public | BindingFlags.Instance);

    private Process process;

    // General transition behavior flags
    public bool ConsoleOutput = true, ForceLoad = true, FullHeal = false, FixMenu = false,
                MenuCleanup = false, AddRewardItems = false, AddSinLocation = false,
                RemoveSinLocation = false, PositionPartyOffScreen = false, PositionTidusAfterLoad = false,
                KeepEncounterThreatAfterLoad = false, Repeatable = false, Suspendable = true;

    // General transition parameters
    public int AddOverdrive = 0, BaseCutsceneValue = 0, BaseCutsceneValue2 = 0, Stage = 0;

    public string Description;
    public (byte itemref, byte itemqty)[] AddItemsToInventory;

    // Actor targeting and positioning
    public int? ActorArrayLength;
    public short[] TargetActorIDs;
    public float? Target_x, Target_y, Target_z, Target_rot,
                  PartyTarget_x, PartyTarget_y, PartyTarget_z;
    public short? Target_var1;
    public byte? MoveFrame = 8; // Default to 8 Frames as this seems to work for most transitions

    // Party formation to set
    public Formations? FormationSwitch;

    /* Only add members here for memory addresses that we want to write the value to.
     * If we only ever read the value then there is no need to add it here. */

    // Game state - Room/Story progression
    public short? RoomNumber, Storyline, BattleState, BattleState2, Intro,
                  FangirlsOrKidsSkip, RoomNumberAlt, CutsceneAlt,
                  AirshipDestinations, AirshipDestinationChange, EncounterMapID;

    // Game state - Menu, camera, encounters
    public byte? SpawnPoint, Menu, MenuLock, Camera, EncounterStatus,
                 MovementLock, ActiveMusicId, MusicId, AuronOverdrives,
                 PartyMembers, Sandragoras, EncounterFormationID1, EncounterFormationID2,
                 ScriptedBattleFlag1, ScriptedBattleFlag2, EncounterTrigger,
                 GuadoCount, CutsceneTiming, IsLoading;

    public sbyte? State;

    // Game state - Position and camera coordinates
    public float? XCoordinate, YCoordinate, Camera_x, Camera_y, Camera_z,
                  CameraRotation, TidusXCoordinate, TidusYCoordinate,
                  TidusZCoordinate, TidusRotation;

    // Game state - Framerate and battle variables
    public int? TargetFramerate, ScriptedBattleVar1, ScriptedBattleVar3,
                ScriptedBattleVar4, HpEnemyA;

    public byte[] DialogueFile;

    // Magic File
    public int? CurrentMagicID, ToBeDeletedMagicID, CurrentMagicHandle,
                ToBeDeletedMagicHandle, EffectPointer;
    public byte? EffectStatusFlag;

    // Bespoke Transitions
    public int? AuronTransition, AmmesTransition, TankerTransition,
                InsideSinTransition, DiveTransition, DiveTransition2, DiveTransition3,
                GeosTransition, KlikkTransition, AlBhedBoatTransition,
                UnderwaterRuinsTransition, UnderwaterRuinsTransition2, UnderwaterRuinsOutsideTransition,
                BeachTransition, LagoonTransition1, LagoonTransition2,
                ValeforTransition, BesaidNightTransition1, BesaidNightTransition2,
                KimahriTransition, YunaBoatTransition, SinFinTransition,
                EchuillesTransition, GeneauxTransition, KilikaElevatorTransition,
                KilikaTrialsTransition, KilikaAntechamberTransition, IfritTransition, IfritTransition2,
                JechtShotTransition, OblitzeratorTransition, BlitzballTransition,
                SahaginTransition, GarudaTransition, RinTransition,
                ChocoboEaterTransition, GuiTransition, Gui2Transition,
                DjoseTransition, IxionTransition, ExtractorTransition,
                SeymoursHouseTransition1, SeymoursHouseTransition2, FarplaneTransition1, FarplaneTransition2,
                TromellTransition, CrawlerTransition, SeymourTransition, SeymourTransition2,
                WendigoTransition, SpherimorphTransition, UnderLakeTransition,
                BikanelTransition, HomeTransition, EvraeTransition, EvraeAirshipTransition,
                GuardsTransition, BahamutTransition, IsaaruTransition,
                AltanaTransition, NatusTransition, DefenderXTransition,
                RonsoTransition, FluxTransition, SanctuaryTransition,
                SpectralKeeperTransition, SpectralKeeperTransition2, YunalescaTransition,
                FinsTransition, FinsAirshipTransition, SinCoreTransition,
                OverdriveSinTransition, OmnisTransition, BFATransition,
                AeonTransition, YuYevonTransition, YojimboFaythTransition;

    // Character and Aeon enable flags
    public byte? EnableTidus, EnableYuna, EnableAuron, EnableKimahri,
                 EnableWakka, EnableLulu, EnableRikku, EnableSeymour,
                 EnableValefor, EnableIfrit, EnableIxion, EnableShiva,
                 EnableBahamut, EnableAnima, EnableYojimbo, EnableMagus;

    // Encounter mechanics
    public byte? EncountersActiveFlag;
    public float? TotalDistance, CycleDistance;

    // Location-specific story flags - Baaj Temple
    public byte? BaajFlag1;

    // Location-specific story flags - S.S. Winno / Kilika
    public byte? SSWinnoFlag1, KilikaMapFlag, SSWinnoFlag2;

    // Location-specific story flags - Luca
    public byte? LucaFlag, LucaFlag2, BlitzballFlag;

    // Location-specific story flags - Mi'ihen Highroad
    public byte? MiihenFlag1, MiihenFlag2, MiihenFlag3, MiihenFlag4;

    // Location-specific story flags - Mushroom Rock Road
    public byte? MRRFlag1, MRRFlag2;

    // Location-specific story flags - Moonflow through Bikanel
    public byte? MoonflowFlag, MoonflowFlag2, RikkuOutfit,
                 TidusWeaponDamageBoost, GuadosalamShopFlag,
                 ThunderPlainsFlag, MacalaniaFlag, BikanelFlag;

    // Party formation and character name
    public byte[] Formation, RikkuName;

    // Location-specific story flags - Via Purifico through Omega Ruins
    public byte? ViaPurificoPlatform, NatusFlag, WantzFlag, OmegaRuinsFlag, WantzMacalaniaFlag, AurochsPlayer1;
    public ushort? CalmLandsFlag;
    public short? GagazetCaveFlag;

    // Blitzball team data
    public byte[] AurochsTeamBytes, BlitzballBytes;

    // Battle rewards
    public int? GilBattleRewards, GilRewardCounter;
    public byte? BattleRewardItemCount, BattleRewardItemQty1, BattleRewardEquipCount;
    public short? BattleRewardItem1;
    public byte[] BattleRewardEquip1;

    // Inventory management
    public byte[] ItemsStart, ItemsQtyStart;

    // Character progression
    public int[] CharacterAPRewards;
    public byte[] CharacterAPFlags;

    // Menu system
    public int? MenuValue1, MenuValue2, MenuTriggerValue;

    // Autosave control
    public byte? AutosaveTrigger, SupressAutosaveOnForceLoad, SupressAutosaveCounter;

    // Music sphere collection
    public byte? LucaMusicSpheresUnlocked;

    // RNG manipulation
    public byte[] RNGArrayOpBytes;

    public bool SetSeed = false;
    public int? SetSeedValue;

    // Bitmask Addition
    public int? AddCalmLandsBitmask;

    // Stored Values - Encounter distances
    public float TotalDistanceBeforeLoad = 0.0f, CycleDistanceBeforeLoad = 0.0f;

    public virtual void Execute(string defaultDescription = "")
    {
        if (ConsoleOutput)
        {
            DiagnosticLog.Information(
                !string.IsNullOrEmpty(Description) ? Description :
                !string.IsNullOrEmpty(defaultDescription) ? defaultDescription :
                DEFAULT_DESCRIPTION);
        }
        // Always update to get the latest process
        process = MemoryWatchers.Process;

        // Write all properties to their corresponding MemoryWatchers
        WriteValuesFromProperties();

        // Update Bitmasks
        WriteValue(MemoryWatchers.CalmLandsFlag, MemoryWatchers.CalmLandsFlag.Current | AddCalmLandsBitmask);

        if (ForceLoad)
        {
            ForceGameLoad();
            FixMenuBug();
            FixSpeedBoosterBug();
        }

        if (FullHeal)
        {
            FullPartyHeal();
        }

        if (MenuCleanup)
        {
            CleanMenuValues();
            ClearAllBattleRewards();
        }

        if (AddSinLocation)
        {
            AddSin();
        }

        if (RemoveSinLocation)
        {
            RemoveSin();
        }

        if (PositionPartyOffScreen)
        {
            PartyOffScreen();
        }

        if (!(AddItemsToInventory is null))
        {
            AddItems(AddItemsToInventory);
        }

        if (AddOverdrive > 0)
        {
            WriteValue<byte>(MemoryWatchers.AuronOverdrives, (byte)(MemoryWatchers.AuronOverdrives.Current | AddOverdrive));
        }

        UpdateFormation(Formation);

        if (PositionTidusAfterLoad)
        {
            process.Resume();
            MemoryWatchers.ForceLoad.Update(process);
            MemoryWatchers.State.Update(process);
            while (MemoryWatchers.ForceLoad.Current == 1 || MemoryWatchers.State.Current == -1) // Wait for loading to finish and black screen to end
            {
                MemoryWatchers.ForceLoad.Update(process);
                MemoryWatchers.State.Update(process);
            }
            MemoryWatchers.FrameCounterFromLoad.Update(process);
            while (MemoryWatchers.FrameCounterFromLoad.Current < MoveFrame)
            {
                MemoryWatchers.FrameCounterFromLoad.Update(process);
            }
            process.Suspend();
            byte originalEncountersActiveFlag = MemoryWatchers.EncountersActiveFlag.Current;
            WriteValue<byte>(MemoryWatchers.EncountersActiveFlag, 0);
            SetActorPosition(1, Target_x, Target_y, Target_z, Target_rot, Target_var1);
            SetActorPosition(101, Target_x, Target_y, Target_z, Target_rot, Target_var1); // In Besaid Temple Tidus is ID 101 for some reason, also some other locations.
            process.Resume();
            while (MemoryWatchers.FrameCounterFromLoad.Current < MoveFrame + 3)
            {
                MemoryWatchers.FrameCounterFromLoad.Update(process);
            }
            process.Suspend();
            if (KeepEncounterThreatAfterLoad)
            {
                WriteValue<float>(MemoryWatchers.TotalDistance, TotalDistanceBeforeLoad);
                WriteValue<float>(MemoryWatchers.CycleDistance, CycleDistanceBeforeLoad);
            }
            else
            {
                WriteValue<float>(MemoryWatchers.TotalDistance, 0.0f);
                WriteValue<float>(MemoryWatchers.CycleDistance, 0.0f);
            }
            WriteValue<byte>(MemoryWatchers.EncountersActiveFlag, originalEncountersActiveFlag);
            process.Resume();
        }
        else
        {
            if (TargetActorIDs != null)
            {
                foreach (short TargetActorID in TargetActorIDs)
                {
                    SetActorPosition(TargetActorID, Target_x, Target_y, Target_z, Target_rot, Target_var1);
                }
            }
        }

        if (SetSeed)
        {
            SetRngValues();
        }

    }

    /* Set the force load bit. Will immediately cause a fade and load. */
    private void ForceGameLoad()
    {
        // Store distances for random encounter chance before reloading map
        TotalDistanceBeforeLoad = MemoryWatchers.TotalDistance.Current;
        CycleDistanceBeforeLoad = MemoryWatchers.CycleDistance.Current;

        // Trigger map reload
        WriteValue<byte>(MemoryWatchers.ForceLoad, 1);
        MemoryWatchers.ForceLoad.Update(process);
    }

    protected void WriteValue<T>(MemoryWatcher watcher, T? value) where T : struct
    {
        if (value.HasValue)
        {
            var dbgAddr = watcher.Address - MemoryWatchers.GetBaseAddress();

            if (watcher.AddrType == MemoryWatcher.AddressType.Absolute)
            {
                DiagnosticLog.Debug($"w {watcher.Name}: write {value.Value} to addr {dbgAddr:X8}.");
                process.WriteValue(watcher.Address, value.Value);
                return;
            }
            else
            {
                // To write to a deep pointer we need to dereference its pointer path.
                // Then we write to the final pointer.
                if (!watcher.DeepPtr.DerefOffsets(process, out IntPtr finalPointer))
                {
                    DiagnosticLog.Information("Couldn't read the pointer path for: " + watcher.Name);
                }

                DiagnosticLog.Debug($"w {watcher.Name}: write {value.Value} to addr {finalPointer:X8}.");
                process.WriteValue(finalPointer, value.Value);
            }
        }
    }

    protected void WriteBytes(MemoryWatcher watcher, byte[] bytes)
    {
        if (bytes != null)
        {
            var hexstring = Convert.ToHexString(bytes);
            var dbgAddr = watcher.Address - MemoryWatchers.GetBaseAddress();

            if (watcher.AddrType == MemoryWatcher.AddressType.Absolute)
            {
                DiagnosticLog.Debug($"w {watcher.Name}: write {hexstring} to addr {dbgAddr:X8}.");
                process.WriteBytes(watcher.Address, bytes);
                return;
            }
            else
            {
                // To write to a deep pointer we need to dereference its pointer path.
                // Then we write to the final pointer.
                if (!watcher.DeepPtr.DerefOffsets(process, out IntPtr finalPointer))
                {
                    DiagnosticLog.Information("Couldn't read the pointer path for: " + watcher.Name);
                }

                DiagnosticLog.Debug($"w {watcher.Name}: write {hexstring} to addr {finalPointer:X8}.");
                process.WriteBytes(finalPointer, bytes);
            }
        }
    }

    /// <summary>
    /// Automatically writes all non-null properties to their corresponding MemoryWatchers.
    /// Uses reflection to match property names with MemoryWatcher fields.
    /// </summary>
    protected void WriteValuesFromProperties()
    {
        var transitionProperties = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        var memoryWatcherFields = typeof(MemoryWatchers).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var property in transitionProperties)
        {
            var propertyValue = property.GetValue(this);
            if (propertyValue == null)
                continue;

            // Find matching MemoryWatcher field by name
            var matchingWatcher = memoryWatcherFields.FirstOrDefault(f => f.Name == property.Name);
            if (matchingWatcher == null)
                continue;

            var watcher = matchingWatcher.GetValue(null) as MemoryWatcher;
            if (watcher == null)
                continue;

            // Handle byte arrays specially
            if (property.FieldType == typeof(byte[]))
            {
                WriteBytes(watcher, propertyValue as byte[]);
                continue;
            }

            // Handle nullable value types
            var propertyType = property.FieldType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(propertyType);

                // Use reflection to call the generic WriteValue method
                var writeValueMethod = this.GetType().GetMethod("WriteValue", BindingFlags.NonPublic | BindingFlags.Instance);
                var genericMethod = writeValueMethod.MakeGenericMethod(underlyingType);
                genericMethod.Invoke(this, new object[] { watcher, propertyValue });
            }
        }
    }

    public override bool Equals(object obj)
    {
        // If they're the exact same object then we're good.
        if (Equals(this, obj))
        {
            return true;
        }

        // Otherwise check all of the public fields to verify if they are the same transition
        return this == (Transition)obj;
    }

    public static bool operator ==(Transition first, Transition second)
    {
        foreach (var property in publicFields)
        {
            var thisValue = first is null ? null : property.GetValue(first);
            var otherValue = second is null ? null : property.GetValue(second);
            if (!Equals(thisValue, otherValue))
            {
                return false;
            }
        }
        return true;
    }

    public static bool operator !=(Transition first, Transition second)
    {
        return !(first == second);
    }

    public override int GetHashCode()
    {
        int hashCode = 3;
        foreach (var property in publicFields)
        {
            // This line *should* be getting the hashcode of the actual property value
            // NOT the property type.
            hashCode *= property.GetValue(this).GetHashCode();
        }
        return hashCode;
    }

    private void FullPartyHeal()
    {
        //Process process = MemoryWatchers.Process;

        int baseAddress = MemoryWatchers.GetBaseAddress();

        for (int i = 0; i < 18; i++)
        {
            MemoryWatcher<int> CharacterHP_Current = new MemoryWatcher<int>(new IntPtr(baseAddress + 0xD32078 + 0x94 * i));
            MemoryWatcher<int> CharacterHP_Max = new MemoryWatcher<int>(new IntPtr(baseAddress + 0xD32080 + 0x94 * i));
            MemoryWatcher<int> CharacterMP_Current = new MemoryWatcher<int>(new IntPtr(baseAddress + 0xD3207C + 0x94 * i));
            MemoryWatcher<int> CharacterMP_Max = new MemoryWatcher<int>(new IntPtr(baseAddress + 0xD32084 + 0x94 * i));
            MemoryWatcher<byte> BattlesUntilReady = new MemoryWatcher<byte>(new IntPtr(baseAddress + 0xD32099 + 0x94 * i)); // Reset battles until ready counter for aeons

            CharacterHP_Current.Update(process);
            CharacterHP_Max.Update(process);
            CharacterMP_Current.Update(process);
            CharacterMP_Max.Update(process);
            BattlesUntilReady.Update(process);

            WriteValue<int>(CharacterHP_Current, CharacterHP_Max.Current);
            WriteValue<int>(CharacterMP_Current, CharacterMP_Max.Current);
            WriteValue<byte>(BattlesUntilReady, 0);
        }
    }

    private void CleanMenuValues()
    {
        WriteValue<int>(MemoryWatchers.MenuValue1, 0);
        WriteValue<int>(MemoryWatchers.MenuValue2, 0);
    }

    private void FixMenuBug()
    {
        WriteValue<int>(MemoryWatchers.MenuValue3, unchecked((int)0xFFFFFFFF));
        WriteValue<int>(MemoryWatchers.MenuValue4, 0x00000000);
        WriteValue<byte>(MemoryWatchers.MenuValue5, 0x00);
        WriteValue<int>(MemoryWatchers.MenuValue6, 0x00000001);
        WriteValue<byte>(MemoryWatchers.MenuValue7, 0x00);
    }

    private void FixSpeedBoosterBug()
    {
        WriteValue<int>(MemoryWatchers.SpeedBoostVar1, 1);
    }

    private void ClearAllBattleRewards()
    {
        // Clear Gil
        WriteValue<int>(MemoryWatchers.GilBattleRewards, 0);

        if (AddRewardItems)
        {
            byte[] items = process.ReadBytes(MemoryWatchers.ItemsStart.Address, 224);
            byte[] itemsQty = process.ReadBytes(MemoryWatchers.ItemsQtyStart.Address, 112);
            byte[] itemRewards = process.ReadBytes(MemoryWatchers.BattleRewardItem1.Address, 16);
            byte[] itemRewardsQty = process.ReadBytes(MemoryWatchers.BattleRewardItemQty1.Address, 8);

            int rewardCount = MemoryWatchers.BattleRewardItemCount.Current;

            bool alreadyExists;

            for (int i = 0; i < rewardCount; i++)
            {
                alreadyExists = false;

                for (int j = 0; j < 112; j++)
                {
                    if (items[2 * j] == itemRewards[2 * i] && itemsQty[j] > 0)
                    {
                        alreadyExists = true;
                        itemsQty[j] += itemRewardsQty[i];

                        break;
                    }
                }

                if (alreadyExists == false)
                {
                    for (int j = 0; j < 112; j++)
                    {
                        if (items[2 * j] == 0xFF && itemsQty[j] == 0)
                        {
                            alreadyExists = true;
                            items[2 * j] = itemRewards[2 * i];
                            items[2 * j + 1] = itemRewards[2 * i + 1];
                            itemsQty[j] = itemRewardsQty[i];

                            break;
                        }
                    }
                }
            }

            WriteBytes(MemoryWatchers.ItemsStart, items);
            WriteBytes(MemoryWatchers.ItemsQtyStart, itemsQty);
        }

        // Clear Battle Reward Items
        WriteValue<byte>(MemoryWatchers.BattleRewardItemCount, 0);
        process.WriteBytes(MemoryWatchers.BattleRewardItem1.Address, Enumerable.Repeat((byte)0x00, 16).ToArray<byte>());
        process.WriteBytes(MemoryWatchers.BattleRewardItemQty1.Address, Enumerable.Repeat((byte)0x00, 8).ToArray<byte>());

        // Clear Battle Reward Equipment -- Equipment Arrays are 22 bytes long
        WriteValue<byte>(MemoryWatchers.BattleRewardEquipCount, 0);
        process.WriteBytes(MemoryWatchers.BattleRewardEquip1.Address, Enumerable.Repeat((byte)0x00, 22 * 8).ToArray<byte>());

        // Clear AP Flags
        WriteBytes(MemoryWatchers.CharacterAPFlags, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
    }

    private void AddItems((byte itemref, byte itemqty)[] AddItemsToInventory)
    {
        byte[] items = process.ReadBytes(MemoryWatchers.ItemsStart.Address, 224);
        byte[] itemsQty = process.ReadBytes(MemoryWatchers.ItemsQtyStart.Address, 112);

        bool alreadyExists;

        for (int i = 0; i < AddItemsToInventory.Length; i++)
        {
            alreadyExists = false;

            for (int j = 0; j < 112; j++)
            {
                if (items[2 * j] == AddItemsToInventory[i].itemref)
                {
                    alreadyExists = true;
                    itemsQty[j] += AddItemsToInventory[i].itemqty;

                    break;
                }
            }

            if (alreadyExists == false)
            {
                for (int j = 0; j < 112; j++)
                {
                    if (items[2 * j] == 0xFF && itemsQty[j] == 0)
                    {
                        DiagnosticLog.Information($"Adding Item: {AddItemsToInventory[i].itemref}");
                        items[2 * j] = AddItemsToInventory[i].itemref;
                        items[2 * j + 1] = 0x20;
                        itemsQty[j] = AddItemsToInventory[i].itemqty;

                        break;
                    }
                }
            }
        }

        WriteBytes(MemoryWatchers.ItemsStart, items);
        WriteBytes(MemoryWatchers.ItemsQtyStart, itemsQty);
    }

    private void AddSin()
    {
        WriteValue<short>(MemoryWatchers.AirshipDestinations, (short)(MemoryWatchers.AirshipDestinations.Current + 512));
    }

    private void RemoveSin()
    {
        WriteValue<short>(MemoryWatchers.AirshipDestinations, (short)(MemoryWatchers.AirshipDestinations.Current - 512));
    }

    private void PartyOffScreen()
    {
        if (MemoryWatchers.EnableTidus.Current == 17)
        {
            SetActorPosition(1, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableYuna.Current == 17)
        {
            SetActorPosition(2, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableAuron.Current == 17)
        {
            SetActorPosition(3, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableKimahri.Current == 17)
        {
            SetActorPosition(4, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableWakka.Current == 17)
        {
            SetActorPosition(5, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableLulu.Current == 17)
        {
            SetActorPosition(6, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableRikku.Current == 17)
        {
            SetActorPosition(7, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
        if (MemoryWatchers.EnableSeymour.Current == 17)
        {
            SetActorPosition(8, PartyTarget_x, PartyTarget_y, PartyTarget_z);
        }
    }

    private bool SetActorPosition(short? TargetActorID = null, float? Target_x = null, float? Target_y = null, float? Target_z = null, float? Target_rot = null, short? Target_var1 = null)
    {
        Process process = MemoryWatchers.Process;

        MemoryWatchers.ActorArrayLength.Update(process);
        int ActorCount = MemoryWatchers.ActorArrayLength.Current;
        int baseAddress = MemoryWatchers.GetBaseAddress();
        bool actorFound = false;

        if (!(TargetActorID is null))
        {

            for (int i = 0; i < ActorCount; i++)
            {
                MemoryWatcher<short> characterIndex = new MemoryWatcher<short>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x00 + 0x880 * i }));
                characterIndex.Update(process);

                short ActorID = characterIndex.Current;

                if (ActorID == TargetActorID)
                {
                    actorFound = true;

                    MemoryWatcher<float> characterPos_x = new MemoryWatcher<float>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x0C + 0x880 * i }));
                    MemoryWatcher<float> characterPos_y = new MemoryWatcher<float>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x10 + 0x880 * i }));
                    MemoryWatcher<float> characterPos_z = new MemoryWatcher<float>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x14 + 0x880 * i }));
                    MemoryWatcher<float> characterPos_floor = new MemoryWatcher<float>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x16C + 0x880 * i }));
                    MemoryWatcher<float> characterPos_rot = new MemoryWatcher<float>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x168 + 0x880 * i }));
                    MemoryWatcher<short> characterPos_var1 = new MemoryWatcher<short>(new DeepPointer(new IntPtr(baseAddress + 0x1FC44E4), new int[] { 0x824 + 0x880 * i }));

                    if (!(Target_x is null))
                    {
                        WriteValue<float>(characterPos_x, Target_x);
                    }
                    if (!(Target_y is null))
                    {
                        WriteValue<float>(characterPos_floor, Target_y);
                        WriteValue<float>(characterPos_y, Target_y);
                    }
                    if (!(Target_z is null))
                    {
                        WriteValue<float>(characterPos_z, Target_z);
                    }
                    if (!(Target_rot is null))
                    {
                        WriteValue<float>(characterPos_rot, Target_rot);
                    }
                    if (!(Target_var1 is null))
                    {
                        WriteValue<short>(characterPos_var1, Target_var1);
                    }
                }
            }
        }
        return actorFound;
    }

    // Formation Functions

    /// <summary>
    /// Character IDs used in formations and party management.
    /// </summary>
    public enum CharacterId : byte
    {
        Tidus = 0x00,
        Yuna = 0x01,
        Auron = 0x02,
        Kimahri = 0x03,
        Wakka = 0x04,
        Lulu = 0x05,
        Rikku = 0x06,
        Seymour = 0x07,
        Empty = 0xFF
    }

    public enum Formations
    {
        Klikk2,
        PreKimahri,
        PostKimahri,
        BoardingSSLiki,
        PostEchuilles,
        MachinaFights,
        PreOblitzerator,
        PostOblitzerator,
        PreSahagins,
        AuronJoinsTheParty,
        PreGui2,
        PostGui,
        MeetRikku,
        PostCrawler,
        PreSeymour,
        BikanelStart,
        PostZu,
        BikanelRikku,
        ViaPurificoStart,
        HighbridgeStart,
        PreNatus,
        PostBiranYenke
    }

    /// <summary>
    /// Helper method to enable a character (sets enable flag to 17).
    /// </summary>
    private void EnableCharacter(CharacterId characterId)
    {
        MemoryWatcher<byte> watcher = characterId switch
        {
            CharacterId.Tidus => MemoryWatchers.EnableTidus,
            CharacterId.Yuna => MemoryWatchers.EnableYuna,
            CharacterId.Auron => MemoryWatchers.EnableAuron,
            CharacterId.Kimahri => MemoryWatchers.EnableKimahri,
            CharacterId.Wakka => MemoryWatchers.EnableWakka,
            CharacterId.Lulu => MemoryWatchers.EnableLulu,
            CharacterId.Rikku => MemoryWatchers.EnableRikku,
            CharacterId.Seymour => MemoryWatchers.EnableSeymour,
            _ => null
        };

        if (watcher != null)
            WriteValue<byte>(watcher, 17);
    }

    /// <summary>
    /// Helper method to disable a character (sets enable flag to 16 or 0).
    /// </summary>
    private void DisableCharacter(CharacterId characterId, byte disableValue = 16)
    {
        MemoryWatcher<byte> watcher = characterId switch
        {
            CharacterId.Tidus => MemoryWatchers.EnableTidus,
            CharacterId.Yuna => MemoryWatchers.EnableYuna,
            CharacterId.Auron => MemoryWatchers.EnableAuron,
            CharacterId.Kimahri => MemoryWatchers.EnableKimahri,
            CharacterId.Wakka => MemoryWatchers.EnableWakka,
            CharacterId.Lulu => MemoryWatchers.EnableLulu,
            CharacterId.Rikku => MemoryWatchers.EnableRikku,
            CharacterId.Seymour => MemoryWatchers.EnableSeymour,
            _ => null
        };

        if (watcher != null)
            WriteValue<byte>(watcher, disableValue);
    }

    /// <summary>
    /// Helper method to enable multiple characters at once.
    /// </summary>
    private void EnableCharacters(params CharacterId[] characterIds)
    {
        foreach (var id in characterIds)
            EnableCharacter(id);
    }

    /// <summary>
    /// Helper method to disable multiple characters at once.
    /// </summary>
    private void DisableCharacters(byte disableValue, params CharacterId[] characterIds)
    {
        foreach (var id in characterIds)
            DisableCharacter(id, disableValue);
    }

    /// <summary>
    /// Helper method to swap multiple characters with positions sequentially.
    /// Each tuple contains (characterId, targetPosition).
    /// </summary>
    private byte[] SwapCharactersWithPositions(byte[] formation, params (CharacterId character, int position)[] swaps)
    {
        foreach (var (character, position) in swaps)
        {
            formation = SwapCharacterWithPosition(formation, character, position);
        }
        return formation;
    }

    /// <summary>
    /// Helper method to add multiple characters to the formation.
    /// </summary>
    private byte[] AddCharacters(byte[] formation, params CharacterId[] characterIds)
    {
        foreach (var id in characterIds)
        {
            formation = AddCharacter(formation, id);
        }
        return formation;
    }

    private void UpdateFormation(byte[] initialFormation = null)
    {
        byte[] formation = process.ReadBytes(MemoryWatchers.Formation.Address, 10);
        byte initialPosition1 = (byte)CharacterId.Empty;
        byte initialPosition2 = (byte)CharacterId.Empty;
        byte initialPosition3 = (byte)CharacterId.Empty;

        if (!(initialFormation is null))
        {
            initialPosition1 = initialFormation[0];
            initialPosition2 = initialFormation[1];
            initialPosition3 = initialFormation[2];
        }

        if (FormationSwitch.HasValue)
        {
            switch (FormationSwitch)
            {
                case Formations.Klikk2:
                    formation = new byte[] { 0x00, 0x06, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                    break;
                case Formations.PreKimahri:
                    formation = new byte[] { 0x00, 0xFF, 0xFF, 0x01, 0x04, 0x05, 0xFF, 0xFF, 0xFF, 0xFF };
                    break;
                case Formations.PostKimahri:
                    formation = new byte[] { 0x00, 0x01, 0x04, 0xFF, 0xFF, 0x05, 0xFF, 0xFF, 0xFF, 0xFF };
                    break;
                case Formations.BoardingSSLiki:
                    formation = AddCharacter(formation, CharacterId.Kimahri);
                    break;
                case Formations.PostEchuilles:
                    formation = new byte[] { (byte)CharacterId.Lulu, (byte)CharacterId.Wakka, (byte)CharacterId.Tidus, (byte)CharacterId.Yuna, (byte)CharacterId.Kimahri, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    EnableCharacters(CharacterId.Yuna, CharacterId.Kimahri, CharacterId.Lulu); // Yuna, Kimahri, Lulu
                    break;
                case Formations.MachinaFights:
                    formation = new byte[] { (byte)CharacterId.Lulu, (byte)CharacterId.Tidus, (byte)CharacterId.Kimahri, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    DisableCharacters(16, CharacterId.Yuna, CharacterId.Wakka); // Yuna, Wakka
                    break;
                case Formations.PreOblitzerator:
                    formation = new byte[] { (byte)CharacterId.Tidus, (byte)CharacterId.Lulu, (byte)CharacterId.Kimahri, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    break;
                case Formations.PostOblitzerator:
                    formation = new byte[] { (byte)CharacterId.Lulu, (byte)CharacterId.Tidus, (byte)CharacterId.Kimahri, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    break;
                case Formations.PreSahagins:
                    DisableCharacters(16, CharacterId.Kimahri, CharacterId.Lulu); // Kimahri, Lulu
                    EnableCharacter(CharacterId.Wakka); // Wakka
                    formation = new byte[] { (byte)CharacterId.Wakka, (byte)CharacterId.Tidus, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    break;
                case Formations.AuronJoinsTheParty:
                    EnableCharacters(CharacterId.Tidus, CharacterId.Yuna, CharacterId.Auron, CharacterId.Kimahri, CharacterId.Wakka, CharacterId.Lulu); // All main party
                    formation = new byte[] { (byte)CharacterId.Tidus, (byte)CharacterId.Wakka, (byte)CharacterId.Yuna, (byte)CharacterId.Auron, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Lulu, (byte)CharacterId.Kimahri, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    break;
                case Formations.PreGui2:
                    formation = SwapPositionWithFirstEmptyReservePosition(formation, 0);
                    formation = SwapPositionWithFirstEmptyReservePosition(formation, 1);
                    formation = SwapPositionWithFirstEmptyReservePosition(formation, 2);
                    formation = RemoveCharacter(formation, CharacterId.Tidus);
                    formation = RemoveCharacter(formation, CharacterId.Yuna);
                    formation = RemoveCharacter(formation, CharacterId.Wakka);
                    formation = RemoveCharacter(formation, CharacterId.Kimahri);
                    formation = RemoveCharacter(formation, CharacterId.Lulu);
                    formation = RemoveCharacter(formation, CharacterId.Auron);
                    formation = AddCharacters(formation, CharacterId.Seymour, CharacterId.Yuna, CharacterId.Auron);
                    formation = SwapCharactersWithPositions(formation, (CharacterId.Yuna, 0), (CharacterId.Seymour, 1), (CharacterId.Auron, 2));
                    break;
                case Formations.PostGui:
                    EnableCharacters(CharacterId.Tidus, CharacterId.Kimahri, CharacterId.Wakka, CharacterId.Lulu); // Tidus, Kimahri, Wakka, Lulu
                    DisableCharacter(CharacterId.Seymour, 16); // Seymour
                    byte[] newformation = new byte[] { (byte)CharacterId.Yuna, (byte)CharacterId.Empty, (byte)CharacterId.Auron, (byte)CharacterId.Tidus, (byte)CharacterId.Kimahri, (byte)CharacterId.Wakka, (byte)CharacterId.Lulu, (byte)CharacterId.Empty, (byte)CharacterId.Empty, (byte)CharacterId.Empty };
                    newformation = SwapCharactersWithPositions(newformation,
                        ((CharacterId)initialPosition1, 0), ((CharacterId)initialPosition2, 1), ((CharacterId)initialPosition3, 2));
                    formation = newformation;
                    break;
                case Formations.MeetRikku:
                    EnableCharacter(CharacterId.Rikku); // Rikku
                    formation = AddCharacter(formation, CharacterId.Rikku);
                    formation = SwapCharacterWithPosition(formation, CharacterId.Rikku, 3);
                    break;
                case Formations.PostCrawler:
                    formation = RemoveCharacter(formation, CharacterId.Yuna);
                    DisableCharacter(CharacterId.Yuna, 0); // Yuna
                    formation = FillMainPartySlotIfEmpty(formation, CharacterId.Tidus);
                    break;
                case Formations.PreSeymour:
                    EnableCharacter(CharacterId.Yuna); // Yuna
                    formation = AddCharacter(formation, CharacterId.Yuna);
                    formation = SwapCharactersWithPositions(formation, (CharacterId.Tidus, 0), (CharacterId.Yuna, 1), (CharacterId.Kimahri, 2));
                    break;
                case Formations.BikanelStart:
                    formation = RemoveAll(formation);
                    EnableCharacter(CharacterId.Tidus); // Tidus
                    formation = AddCharacter(formation, CharacterId.Tidus);
                    formation = SwapCharacterWithPosition(formation, CharacterId.Tidus, 0);
                    break;
                case Formations.PostZu:
                    EnableCharacter(CharacterId.Wakka); // Wakka
                    formation = AddCharacter(formation, CharacterId.Wakka);
                    break;
                case Formations.BikanelRikku:
                    EnableCharacter(CharacterId.Rikku); // Rikku
                    formation = AddCharacter(formation, CharacterId.Rikku);
                    break;
                case Formations.ViaPurificoStart:
                    formation = RemoveAll(formation);
                    EnableCharacter(CharacterId.Yuna); // Yuna
                    formation = AddCharacter(formation, CharacterId.Yuna);
                    formation = SwapCharacterWithPosition(formation, CharacterId.Yuna, 0);
                    break;
                case Formations.HighbridgeStart:
                    EnableCharacters(CharacterId.Yuna, CharacterId.Auron, CharacterId.Lulu); // Yuna, Auron, Lulu
                    formation = AddCharacters(formation, CharacterId.Yuna, CharacterId.Auron, CharacterId.Lulu);
                    formation = SwapCharactersWithPositions(formation, (CharacterId.Tidus, 0), (CharacterId.Yuna, 1), (CharacterId.Wakka, 2));
                    break;
                case Formations.PreNatus:
                    EnableCharacter(CharacterId.Kimahri); // Kimahri
                    formation = AddCharacter(formation, CharacterId.Kimahri);
                    formation = SwapCharactersWithPositions(formation, (CharacterId.Tidus, 0), (CharacterId.Yuna, 2), (CharacterId.Kimahri, 1));
                    break;
                case Formations.PostBiranYenke:
                    EnableCharacters(CharacterId.Tidus, CharacterId.Yuna, CharacterId.Auron, CharacterId.Wakka, CharacterId.Lulu, CharacterId.Rikku); // All except Kimahri
                    formation = AddCharacter(formation, CharacterId.Tidus);
                    formation = AddCharacter(formation, CharacterId.Yuna);
                    formation = AddCharacter(formation, CharacterId.Auron);
                    formation = AddCharacter(formation, CharacterId.Wakka);
                    formation = AddCharacter(formation, CharacterId.Lulu);
                    formation = AddCharacter(formation, CharacterId.Rikku);
                    formation = SwapCharacterWithPosition(formation, (CharacterId)initialPosition1, 0);
                    formation = SwapCharacterWithPosition(formation, (CharacterId)initialPosition2, 1);
                    formation = SwapCharacterWithPosition(formation, (CharacterId)initialPosition3, 2);
                    break;
            }
            RemoveDuplicates(formation);
            WriteBytes(MemoryWatchers.Formation, formation);
        }
    }

    private void RemoveDuplicates(byte[] formation)
    {
        bool[] characterPresent = new bool[81];

        for (int i = 0; i < formation.Length; i++)
        {
            byte character = formation[i];

            if (character == (byte)CharacterId.Empty)
            {
                continue;
            }

            if (!characterPresent[character])
            {
                characterPresent[character] = true;
            }
            else
            {
                formation[i] = (byte)CharacterId.Empty;
            }
        }
    }

    public int GetFirstEmptyReservePosition(byte[] formation)
    {
        int formationSize = formation.Length;

        for (int i = 3; i < formationSize; i++)
        {
            if (formation[i] == (byte)CharacterId.Empty)
            {
                return i;
            }
        }
        return 0;
    }

    public byte[] RemoveCharacter(byte[] formation, CharacterId character)
    {
        int formationSize = formation.Length;

        for (int i = 0; i < formationSize; i++)
        {
            if (formation[i] == (byte)character)
            {
                formation[i] = (byte)CharacterId.Empty;
            }
        }
        return formation;
    }

    public byte[] RemoveAll(byte[] formation)
    {
        int formationSize = formation.Length;

        for (int i = 0; i < formationSize; i++)
        {
            formation[i] = (byte)CharacterId.Empty;
        }

        DisableCharacters(0, CharacterId.Tidus, CharacterId.Yuna, CharacterId.Auron, CharacterId.Kimahri, CharacterId.Wakka, CharacterId.Lulu, CharacterId.Rikku); // Disable all party members

        return formation;
    }

    public byte[] AddCharacter(byte[] formation, CharacterId character)
    {
        if (Array.IndexOf(formation, (byte)character) == -1)
        {
            int Position = GetFirstEmptyReservePosition(formation);

            formation[Position] = (byte)character;
        }
        return formation;
    }

    public byte[] SwapCharacterWithPosition(byte[] formation, CharacterId character, int newPosition)
    {
        int oldposition = Array.IndexOf(formation, (byte)character);

        byte temp = formation[oldposition];
        formation[oldposition] = formation[newPosition];
        formation[newPosition] = temp;

        return formation;
    }

    public byte[] SwapPositionWithFirstEmptyReservePosition(byte[] formation, int Position)
    {
        int newPosition = GetFirstEmptyReservePosition(formation);

        formation[newPosition] = formation[Position];
        formation[Position] = (byte)CharacterId.Empty;

        return formation;
    }

    public byte[] FillMainPartySlotIfEmpty(byte[] formation, CharacterId character)
    {
        for (int i = 0; i < 3; i++)
        {
            if (formation[i] == (byte)CharacterId.Empty)
            {
                formation = SwapCharacterWithPosition(formation, character, i);
                return formation;
            }
        }
        return formation;
    }

    private void SetRngValues()
    {
        for (int i = 0; i < 68; i++)
        {
            int rngValue = rngRoll(i, (int)SetSeedValue);
            WriteValue<int>(new MemoryWatcher<int>(process.Modules[0].BaseAddress.ToInt32() + MemoryLocations.rngArrayStart.BaseAddress + 0x04 * i), rngValue);
        }
    }

    private int rngRoll(int Index, int CurrentValue)
    {
        int temp;

        temp = CurrentValue * 0x5D588B65 + 0x3C35;
        temp = (temp >> 0x10) + (temp << 0x10);
        SetSeedValue = temp;
        return temp & 0x7FFFFFFF;
    }

}
