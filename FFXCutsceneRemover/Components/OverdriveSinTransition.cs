using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class OverdriveSinTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.FrameCounterFromLoad.Current >= 10 && Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        else if (MemoryWatchers.OverdriveSinTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.OverdriveSin.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.OverdriveSinTransition, BaseCutsceneValue + CutsceneOffsets.OverdriveSin.SkipOffset1);
            Stage += 1;
        }
        else if (MemoryWatchers.BattleState2.Current == 1 && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.OverdriveSinTransition, BaseCutsceneValue + CutsceneOffsets.OverdriveSin.SkipOffset2);
            Stage += 1;
        }
    }
}