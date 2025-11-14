using System.Collections.Generic;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class IxionTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 1641, 16, 96 });
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.IxionTransition.Current > 0)
        {
            if (MemoryWatchers.State.Current != -1 && Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.IxionTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Ixion.CheckOffset) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.IxionTransition, BaseCutsceneValue + CutsceneOffsets.Ixion.SkipOffset);
                Stage += 1;
            }
        }
    }
}