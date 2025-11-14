using FFXCutsceneRemover.Constants;
using FFXCutsceneRemover.Logging;
using System.Collections.Generic;

namespace FFXCutsceneRemover;

class KilikaElevatorTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage += 1;

        }
        else if (MemoryWatchers.KilikaElevatorTransition.Current > (BaseCutsceneValue + CutsceneOffsets.KilikaElevator.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.KilikaElevatorTransition, BaseCutsceneValue + CutsceneOffsets.KilikaElevator.SkipOffset);
            new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { 1 }, Target_x = 0.0f, Target_y = -163.75f, Target_z = -25.0f, Target_var1 = 229 }.Execute();
            DiagnosticLog.Information("Test 1");
            Stage += 1;
        }
    }
}