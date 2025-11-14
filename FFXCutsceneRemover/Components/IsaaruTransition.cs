using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class IsaaruTransition : Transition
{
    public override void Execute(string defaultDescription = "")
    {
        int baseAddress = MemoryWatchers.GetBaseAddress();

            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;

                Stage += 1;

            }
            else if (MemoryWatchers.IsaaruTransition.Current == BaseCutsceneValue + CutsceneOffsets.Isaaru.CheckOffset1 && Stage == 1)
            {
                Formation = new byte[] { 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                ConsoleOutput = false;
                FullHeal = true;
                base.Execute();

                WriteValue<int>(MemoryWatchers.IsaaruTransition, BaseCutsceneValue + CutsceneOffsets.Isaaru.SkipOffset1);
                Stage += 1;
            }
            else if (MemoryWatchers.IsaaruTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Isaaru.CheckOffset2) && Stage == 2)
            {
                WriteValue<int>(MemoryWatchers.IsaaruTransition, BaseCutsceneValue + CutsceneOffsets.Isaaru.SkipOffset2);
                Stage += 1;
            }
    }
}