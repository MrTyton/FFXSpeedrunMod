using System.Collections.Generic;
using System.Diagnostics;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class RinTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 16, 1047, 210, 130 });

    public override void Execute(string defaultDescription = "")
    {
        Process process = MemoryWatchers.Process;

        if (MemoryWatchers.RinTransition.Current > 0)
        {
            if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.RinTransition.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.RinTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.Rin.CheckOffset1) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.RinTransition, BaseCutsceneValue + CutsceneOffsets.Rin.SkipOffset1);

                Stage += 1;
            }
            else if (MemoryWatchers.Dialogue1.Current == 92 && Stage == 2)
            {
                WriteValue<int>(MemoryWatchers.RinTransition, BaseCutsceneValue + CutsceneOffsets.Rin.CheckOffset2);

                Stage += 1;
            }
            else if (MemoryWatchers.RinTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Rin.CheckOffset3) && Stage == 3)
            {
                process.Suspend();

                new Transition { Storyline = 767, SpawnPoint = 0 }.Execute();

                Stage += 1;

                process.Resume();
            }
        }
    }
}