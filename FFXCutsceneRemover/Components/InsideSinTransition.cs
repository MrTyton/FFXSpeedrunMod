using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class InsideSinTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.InsideSinTransition.Current > 0)
        {
            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.InsideSinTransition.Current == (BaseCutsceneValue + CutsceneOffsets.InsideSin.CheckOffset) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.InsideSinTransition, BaseCutsceneValue + CutsceneOffsets.InsideSin.SkipOffset);
                Stage += 1;
            }
        }
    }
}