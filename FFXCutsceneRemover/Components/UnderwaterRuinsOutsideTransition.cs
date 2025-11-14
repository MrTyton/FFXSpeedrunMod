using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class UnderwaterRuinsOutsideTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        else if (MemoryWatchers.UnderwaterRuinsOutsideTransition.Current >= (BaseCutsceneValue + CutsceneOffsets.UnderwaterRuinsOutside.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.UnderwaterRuinsOutsideTransition, BaseCutsceneValue + CutsceneOffsets.UnderwaterRuinsOutside.SkipOffset);
            Stage += 1;
        }
    }
}