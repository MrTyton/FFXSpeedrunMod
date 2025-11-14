using FFXCutsceneRemover.Constants;
using System.Collections.Generic;

namespace FFXCutsceneRemover;

class KilikaTrialsTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 1137 });
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.KilikaTrialsTransition.Current > 0)
        {
            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.KilikaTrialsTransition.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.KilikaTrialsTransition.Current == (BaseCutsceneValue + CutsceneOffsets.KilikaTrials.CheckOffset) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.KilikaTrialsTransition, BaseCutsceneValue + CutsceneOffsets.KilikaTrials.SkipOffset);

                Stage += 1;
            }
        }
    }
}