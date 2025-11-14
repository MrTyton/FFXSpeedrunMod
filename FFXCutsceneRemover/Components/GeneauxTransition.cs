using System.Collections.Generic;
using System.Diagnostics;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class GeneauxTransition : Transition
{
    static private byte[] formation = new byte[] { 0x00, 0x01, 0x03, 0x04, 0x05, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

    static private List<short> CutsceneAltList = new List<short>(new short[] { 265, 1173, 1174 });
    public override void Execute(string defaultDescription = "")
    {
        Process process = MemoryWatchers.Process;

        if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage = 1;

        }
        else if (MemoryWatchers.GeneauxTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Geneaux.CheckOffset) && Stage == 1)
        {
            WriteValue<int>(MemoryWatchers.GeneauxTransition, BaseCutsceneValue + CutsceneOffsets.Geneaux.SkipOffset);

            formation = process.ReadBytes(MemoryWatchers.Formation.Address, 10);

            Transition actorPositions;
            //Position Party Member 1
            actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { (short)(formation[0] + 1) }, Target_x = -6.565f, Target_y = -159.997f, Target_z = 551.024f };
            actorPositions.Execute();

            //Position Party Member 2
            actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { (short)(formation[1] + 1) }, Target_x = 31.147f, Target_y = -159.997f, Target_z = 514.762f };
            actorPositions.Execute();

            //Position Party Member 3
            actorPositions = new Transition { ForceLoad = false, ConsoleOutput = false, TargetActorIDs = new short[] { (short)(formation[2] + 1) }, Target_x = 43.509f, Target_y = -159.997f, Target_z = 571.721f };
            actorPositions.Execute();

            Stage += 1;
        }
        else if (MemoryWatchers.GeneauxTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Geneaux.CheckOffset2) && Stage == 2)
        {
            WriteValue<int>(MemoryWatchers.GeneauxTransition, BaseCutsceneValue + CutsceneOffsets.Geneaux.SkipOffset2);
            Stage += 1;
        }
    }
}