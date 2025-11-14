using System.Collections.Generic;

using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class BesaidNightTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        //else if (MemoryWatchers.BesaidNightTransition1.Current == (BaseCutsceneValue + 0x6973) && Stage == 1)
        else if (MemoryWatchers.BesaidNightTransition1.Current > (BaseCutsceneValue + CutsceneOffsets.BesaidNight.CheckOffset1) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.BesaidNightTransition1, BaseCutsceneValue + CutsceneOffsets.BesaidNight.SkipOffset1);
            Stage += 1;
        }
        else if (MemoryWatchers.BesaidNightTransition2.Current == (BaseCutsceneValue + CutsceneOffsets.BesaidNight.CheckOffset2) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.BesaidNightTransition2, BaseCutsceneValue + CutsceneOffsets.BesaidNight.SkipOffset2);
            Stage += 1;
        }
    }
}