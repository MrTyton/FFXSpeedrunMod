using System.Collections.Generic;
using System.Diagnostics;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class SinFinTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 205, 195, 16 });
    public override void Execute(string defaultDescription = "")
    {
        Process process = MemoryWatchers.Process;

        if (MemoryWatchers.SinFinTransition.Current > 0)
        {
            if (CutsceneAltList.Contains(MemoryWatchers.CutsceneAlt.Current) && Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.SinFinTransition.Current;

                Stage = 1;

            }
            else if (MemoryWatchers.SinFinTransition.Current == (BaseCutsceneValue + CutsceneOffsets.SinFin.CheckOffset1) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.SinFinTransition, BaseCutsceneValue + CutsceneOffsets.SinFin.SkipOffset1);

                Transition actorPositions;

                //Position Tidus
                actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { 1 }, Target_x = -29.0f, Target_y = -50.0f, Target_z = 131.5f };
                actorPositions.Execute();

                //Position Sin Fin
                actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { 4209 }, Target_x = 1.0f, Target_z = 945.0f};
                actorPositions.Execute();

                Stage += 1;
            }
            else if (MemoryWatchers.SinFinTransition.Current == (BaseCutsceneValue + CutsceneOffsets.SinFin.CheckOffset2) && MemoryWatchers.BattleState2.Current == 1 && Stage == 2) //200 = Sinscale HP
            {
                process.Suspend();

                new Transition { ForceLoad = false, Storyline = 272, Description = "Post Sin Fin" }.Execute();

                WriteValue<int>(MemoryWatchers.SinFinTransition, BaseCutsceneValue + CutsceneOffsets.SinFin.SkipOffset2);

                Stage += 1;

                process.Resume();
            }
        }
    }
}