using System.Collections.Generic;

using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class OmnisTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 5331 });
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        else if (MemoryWatchers.OmnisTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.Omnis.CheckOffset1) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.OmnisTransition, BaseCutsceneValue + CutsceneOffsets.Omnis.SkipOffset1);

            Stage += 1;
        }
        else if (MemoryWatchers.OmnisTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.Omnis.CheckOffset2) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.OmnisTransition, BaseCutsceneValue + CutsceneOffsets.Omnis.SkipOffset2);

            Stage += 1;
        }
        else if (MemoryWatchers.OmnisTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.Omnis.CheckOffset3) && Stage == 3)
        {
            WriteValue<int>(MemoryWatchers.OmnisTransition, BaseCutsceneValue + CutsceneOffsets.Omnis.SkipOffset3);

            Stage += 1;
        }
        else if (MemoryWatchers.OmnisTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.Omnis.CheckOffset4) && MemoryWatchers.BattleState2.Current == 22 && Stage == 4)
        {
            WriteValue<int>(MemoryWatchers.OmnisTransition, BaseCutsceneValue + CutsceneOffsets.Omnis.SkipOffset4);

            Stage += 1;
        }
    }
}