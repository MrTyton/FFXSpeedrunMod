using System.Collections.Generic;

using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class KlikkTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 1137 });
    public override void Execute(string defaultDescription = "")
    {
        if (CutsceneAltList.Contains(MemoryWatchers.CutsceneAlt.Current) && Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        else if (MemoryWatchers.KlikkTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Klikk.CheckOffset1) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.KlikkTransition, BaseCutsceneValue + CutsceneOffsets.Klikk.SkipOffset1);
            Stage += 1;
        }
        else if (MemoryWatchers.KlikkTransition.Current > (BaseCutsceneValue + CutsceneOffsets.Klikk.CheckOffset2) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.KlikkTransition, BaseCutsceneValue + CutsceneOffsets.Klikk.SkipOffset2);
            Stage += 1;
        }
        else if (MemoryWatchers.BattleState2.Current == 1 && Stage == 3)
        {
            WriteValue<int>(MemoryWatchers.KlikkTransition, BaseCutsceneValue + CutsceneOffsets.Klikk.SkipOffset3);
            Stage += 1;
        }
    }
}