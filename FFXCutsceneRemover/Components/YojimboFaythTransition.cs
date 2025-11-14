using System.Collections.Generic;

using FFXCutsceneRemover.Logging;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class YojimboFaythTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 1281 });
    public override void Execute(string defaultDescription = "")
    {
        if ((MemoryWatchers.CalmLandsFlag.Current & 0x80) == 0x00 && MemoryWatchers.YojimboFaythTransition.Current > 0)
        {
            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
                DiagnosticLog.Information($"Base Cutscene Value: {BaseCutsceneValue:X8}");

                Stage += 1;

            }
            else if (MemoryWatchers.YojimboFaythTransition.Current == (BaseCutsceneValue + CutsceneOffsets.YojimboFayth.CheckOffset) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.YojimboFaythTransition, BaseCutsceneValue + CutsceneOffsets.YojimboFayth.SkipOffset);
                Stage += 1;
                DiagnosticLog.Information($"Test Stage {Stage}");
            }
        }
    }
}