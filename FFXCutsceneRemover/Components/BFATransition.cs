using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class BFATransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        if (base.Stage == 0)
        {
            base.Execute();

            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            base.Stage += 1;

        }
        else if (MemoryWatchers.BFATransition.Current >= (BaseCutsceneValue + CutsceneOffsets.BFA.CheckOffset1) && Stage == 1)
        {
            //WriteValue<int>(MemoryWatchers.BFATransition, BaseCutsceneValue + 0xE1); //Not currently working as desired
            Stage += 1;
        }
        else if (MemoryWatchers.BFATransition.Current >= (BaseCutsceneValue + CutsceneOffsets.BFA.CheckOffset2) && Stage == 2) // 0xA3E5 in event script
        {
            WriteValue<int>(MemoryWatchers.BFATransition, BaseCutsceneValue + CutsceneOffsets.BFA.SkipOffset2); // 0xAE93 in event script
            Stage += 1;
        }
        else if (MemoryWatchers.BFATransition.Current >= (BaseCutsceneValue + CutsceneOffsets.BFA.CheckOffset3) && Stage == 3) // 0xAE9C in event script
        {
            WriteValue<int>(MemoryWatchers.BFATransition, BaseCutsceneValue + CutsceneOffsets.BFA.SkipOffset3); // 0xB070 in event script
            Stage += 1;
        }
        else if (MemoryWatchers.BFATransition.Current >= (BaseCutsceneValue + CutsceneOffsets.BFA.CheckOffset4) && Stage == 4) // 0xB091 in event script
        {
            WriteValue<int>(MemoryWatchers.BFATransition, BaseCutsceneValue + CutsceneOffsets.BFA.SkipOffset4); // 0xB1FE in event script
            Stage += 1;
        }
        else if (MemoryWatchers.BFATransition.Current >= (BaseCutsceneValue + CutsceneOffsets.BFA.CheckOffset5) && Stage == 5) // 0xB204 in event script
        {
            WriteValue<int>(MemoryWatchers.BFATransition, BaseCutsceneValue + CutsceneOffsets.BFA.SkipOffset5); // 0xB45A in event script
            Stage += 1;
        }
    }
}