using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Components;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class KimahriTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
        {
            base.Execute();
            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage = 1;
        }
        else if (MemoryWatchers.KimahriTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.Kimahri.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.KimahriTransition, BaseCutsceneValue + CutsceneOffsets.Kimahri.SkipOffset);
            Stage = 2;
        }
        else if (MemoryWatchers.BattleState2.Current == 1 && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.KimahriTransition, BaseCutsceneValue + CutsceneOffsets.Kimahri.PostBattleOffset);
            Stage = 3;
        }
    }
}