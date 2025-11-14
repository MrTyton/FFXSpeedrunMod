using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class GarudaTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        int baseAddress = MemoryWatchers.GetBaseAddress();
        if (MemoryWatchers.GarudaTransition.Current > 0)
        {
            if (MemoryWatchers.BattleState.Current == 10 && Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.GarudaTransition.Current;

                Stage = 1;

            }
            else if (MemoryWatchers.GarudaTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Garuda.CheckOffset1) && MemoryWatchers.HpEnemyA.Current == 0 && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.GarudaTransition, BaseCutsceneValue + CutsceneOffsets.Garuda.SkipOffset1);
                Stage = 2;
            }
            else if (MemoryWatchers.GarudaTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Garuda.CheckOffset2) && MemoryWatchers.HpEnemyA.Current == 0 && Stage == 2)
            {
                WriteValue<int>(MemoryWatchers.GarudaTransition, BaseCutsceneValue + CutsceneOffsets.Garuda.SkipOffset2);
                Stage = 3;
            }
        }
    }
}