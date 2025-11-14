using System.Collections.Generic;
using System.Diagnostics;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;

namespace FFXCutsceneRemover;

class YunalescaTransition : Transition
{
    static private List<short> CutsceneAltList = new List<short>(new short[] { 70, 71, 75, 76 });
    public override void Execute(string defaultDescription = "")
    {
        int baseAddress = MemoryWatchers.GetBaseAddress();

        if (MemoryWatchers.YunalescaTransition.Current > 0)
        {
            Process process = MemoryWatchers.Process;

            if (Stage == 0)
            {
                base.Execute();

                BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
                Stage += 1;

            }
            else if (MemoryWatchers.YunalescaTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Yunalesca.CheckOffset1) && Stage == 1)
            {
                WriteValue<int>(MemoryWatchers.YunalescaTransition, BaseCutsceneValue + CutsceneOffsets.Yunalesca.SkipOffset1);
                Stage += 1;
            }
            else if (MemoryWatchers.YunalescaTransition.Current == (BaseCutsceneValue + CutsceneOffsets.Yunalesca.CheckOffset2) && Stage == 2)
            {
                WriteValue<int>(MemoryWatchers.YunalescaTransition, BaseCutsceneValue + CutsceneOffsets.Yunalesca.SkipOffset2);
                WriteValue<byte>(MemoryWatchers.CutsceneTiming, 0);
                Stage += 1;
            }
            else if (MemoryWatchers.BattleState2.Current == 1 && Stage == 3)
            {
                WriteValue<int>(MemoryWatchers.YunalescaTransition, BaseCutsceneValue + CutsceneOffsets.Yunalesca.PostBattleOffset);
                Stage += 1;
            }
            else if (MemoryWatchers.GilRewardCounter.Current > 0 && Stage == 4)
            {
                Stage += 1;
            }
            else if (MemoryWatchers.GilRewardCounter.Current == 0 && Stage == 5)
            {
                process.Suspend();

                new Transition { MenuCleanup = true, AddRewardItems = true, Description = "Exit Menu", ForceLoad = false }.Execute();

                Stage += 1;

                process.Resume();
            }
        }
    }
}