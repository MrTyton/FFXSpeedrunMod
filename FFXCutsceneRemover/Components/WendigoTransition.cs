using System.Collections.Generic;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class WendigoTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 1137 });
    public override void Execute(string defaultDescription = "")
    {
        if (MemoryWatchers.WendigoTransition.Current > 0)
        {
            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.WendigoTransition.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.WendigoTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Wendigo.CheckOffset) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.WendigoTransition, BaseCutsceneValue + CutsceneOffsets.Wendigo.SkipOffset);

                Transition actorPositions;
                //Position Party Members off screen
                actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, PositionPartyOffScreen = true, PartyTarget_x = 205.0f, PartyTarget_z = -480.0f };
                actorPositions.Execute();

                //Position Guados
                actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { 4309 }, Target_x = -100.0f, Target_z = -350.0f };
                actorPositions.Execute();

                //Position Wendigo
                actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { 4308 }, Target_x = -100.0f, Target_z = -350.0f };
                actorPositions.Execute();

                Stage += 1;
            }
            else if (MemoryWatchers.WendigoTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Wendigo.PostCheckOffset) && MemoryWatchers.BattleState2.Current == 22 && Stage == 2)
            {
                WriteValue<int>(MemoryWatchers.WendigoTransition, BaseCutsceneValue + CutsceneOffsets.Wendigo.PostBattleOffset);
                Stage += 1;
            }
        }
    }
}