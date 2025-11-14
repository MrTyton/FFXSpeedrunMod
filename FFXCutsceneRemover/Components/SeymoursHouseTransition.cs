using FFXCutsceneRemover.Constants;
using FFXCutsceneRemover.Logging;
using System;
using System.Diagnostics;

namespace FFXCutsceneRemover;

class SeymoursHouseTransition : Transition
{
    Boolean TalkedToAuron = false;
    Boolean TalkedToWakka = false;
    Boolean TalkedToLulu = false;
    Boolean TalkedToRikku = false;
    public override void Execute(string defaultDescription = "")
    {
        Process process = MemoryWatchers.Process;
        
        if (Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;

            Stage += 1;
        }
        else if (MemoryWatchers.SeymoursHouseTransition1.Current == (BaseCutsceneValue + CutsceneOffsets.SeymoursHouse.CheckOffset1) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.SeymoursHouseTransition1, BaseCutsceneValue + CutsceneOffsets.SeymoursHouse.SkipOffset1);
            Stage += 1;
        }
        else if (MemoryWatchers.SeymoursHouseTransition2.Current == (BaseCutsceneValue + CutsceneOffsets.SeymoursHouse.CheckOffset2) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.SeymoursHouseTransition2, BaseCutsceneValue + CutsceneOffsets.SeymoursHouse.SkipOffset2);
            TalkedToAuron = true;
        }
        else if (MemoryWatchers.SeymoursHouseTransition2.Current == (BaseCutsceneValue + CutsceneOffsets.SeymoursHouse.CheckOffset3) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.SeymoursHouseTransition2, BaseCutsceneValue + CutsceneOffsets.SeymoursHouse.SkipOffset3);
            TalkedToLulu = true;
        }
        else if (MemoryWatchers.NPCLastInteraction.Current == 2 && Stage == 2)
        {
            TalkedToWakka = true;
        }
        else if (MemoryWatchers.NPCLastInteraction.Current == 5 && Stage == 2)
        {
            TalkedToRikku = true;
        }

        if (TalkedToAuron && TalkedToWakka && TalkedToLulu && TalkedToRikku && MemoryWatchers.NPCLastInteraction.Current == 1)
        {
            new Transition { RoomNumber = 197, Description = "Lady Yuna, this way." }.Execute();
        }
    }
}