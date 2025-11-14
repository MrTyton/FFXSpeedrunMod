using System.Collections.Generic;

using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class DiveTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        else if (MemoryWatchers.DiveTransition3.Current == (BaseCutsceneValue + CutsceneOffsets.Dive.CheckOffset1) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.DiveTransition, BaseCutsceneValue + CutsceneOffsets.Dive.SkipOffset1);
            WriteValue<int>(MemoryWatchers.DiveTransition2, BaseCutsceneValue + CutsceneOffsets.Dive.SkipOffset2);
            Stage += 1;
        }
        else if (MemoryWatchers.DiveTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Dive.CheckOffset2) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.DiveTransition, BaseCutsceneValue + CutsceneOffsets.Dive.SkipOffset3);
            Stage += 1;
        }
    }
}