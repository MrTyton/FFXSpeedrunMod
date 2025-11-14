using System.Diagnostics;
using System.Linq;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class FluxTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        Process process = MemoryWatchers.Process;

        if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;

            Stage += 1;

        }
        else if (MemoryWatchers.FluxTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Flux.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.FluxTransition, BaseCutsceneValue + CutsceneOffsets.Flux.SkipOffset);
            Stage += 1;
        }
        else if (MemoryWatchers.BattleState2.Current == 1 && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.FluxTransition, BaseCutsceneValue + CutsceneOffsets.Flux.PostBattleOffset);
            Stage += 1;
        }
        else if (MemoryWatchers.GilRewardCounter.Current > 0 && Stage == 3)
        {
            Stage += 1;
        }
        else if (MemoryWatchers.GilRewardCounter.Current == 0 && Stage == 4)
        {
            process.Suspend();

            new Transition { MenuCleanup = true, AddRewardItems = true, Description = "Exit Menu", ForceLoad = false }.Execute();

            Stage += 1;

            process.Resume();
        }
    }
}