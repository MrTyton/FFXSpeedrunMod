using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class LagoonTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.LagoonTransition2.Current > 0)
        {
            if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.LagoonTransition1.Current == (BaseCutsceneValue + CutsceneOffsets.Lagoon.CheckOffset1) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.LagoonTransition1, BaseCutsceneValue + CutsceneOffsets.Lagoon.SkipOffset1);
                Stage += 1;
            }
            else if (MemoryWatchers.LagoonTransition1.Current == (BaseCutsceneValue + CutsceneOffsets.Lagoon.CheckOffset2) && Stage == 2)
            {
                new Transition { ForceLoad = false, EffectPointer = 0, EffectStatusFlag = 0, CurrentMagicID = -1, CurrentMagicHandle = -1, Description = "Fix Crash" }.Execute();
                Stage += 1;
            }
        }
    }
}