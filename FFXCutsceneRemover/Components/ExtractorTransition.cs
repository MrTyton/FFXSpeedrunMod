using FFXCutsceneRemover.Constants;
using FFXCutsceneRemover.Logging;
using System.Collections.Generic;

namespace FFXCutsceneRemover;

class ExtractorTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 1137 });
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.ExtractorTransition.Current > 0)
        {
            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.ExtractorTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Extractor.CheckOffset) && MemoryWatchers.BattleState2.Current == 1 && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.ExtractorTransition, BaseCutsceneValue + CutsceneOffsets.Extractor.SkipOffset);
                Stage += 1;
            }
        }
    }
}