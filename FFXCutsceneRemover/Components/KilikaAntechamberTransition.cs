using FFXCutsceneRemover.Constants;
using System.Diagnostics;

namespace FFXCutsceneRemover;

class KilikaAntechamberTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        Process process = MemoryWatchers.Process;
        int baseAddress = MemoryWatchers.GetBaseAddress();


        if (Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;

            Stage += 1;
        }
        else if (MemoryWatchers.KilikaAntechamberTransition.Current == (BaseCutsceneValue + CutsceneOffsets.KilikaAntechamber.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.KilikaAntechamberTransition, BaseCutsceneValue + CutsceneOffsets.KilikaAntechamber.SkipOffset);

            Stage += 1;
        }
    }
}