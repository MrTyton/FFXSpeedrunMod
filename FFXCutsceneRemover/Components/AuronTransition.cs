using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Components;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class AuronTransition : BossTransitionBase
{
    protected override MemoryWatcher<int> TransitionWatcher => MemoryWatchers.AuronTransition;

    protected override (int checkOffset, int targetOffset)[] Stages => new[]
    {
        (CutsceneOffsets.Auron.CheckOffset, CutsceneOffsets.Auron.SkipOffset)
    };
}