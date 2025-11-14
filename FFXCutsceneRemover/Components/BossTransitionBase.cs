using FFXCutsceneRemover.ComponentUtil;

namespace FFXCutsceneRemover.Components;

/// <summary>
/// Base class for boss battle cutscene transitions.
/// Provides a standardized multi-stage transition pattern that most boss transitions follow.
/// </summary>
public abstract class BossTransitionBase : Transition
{
    /// <summary>
    /// The memory watcher for this specific boss transition.
    /// </summary>
    protected abstract MemoryWatcher<int> TransitionWatcher { get; }

    /// <summary>
    /// Array of (checkOffset, targetOffset) pairs for each stage.
    /// Each stage waits for the cutscene to reach checkOffset, then skips to targetOffset.
    /// </summary>
    protected abstract (int checkOffset, int targetOffset)[] Stages { get; }

    /// <summary>
    /// Executes the multi-stage transition logic.
    /// Stage 0: Initialize when movement is locked.
    /// Stages 1+: Check and skip cutscene based on configured offsets.
    /// </summary>
    public override void Execute(string defaultDescription = "")
    {
        // Stage 0: Initialize the transition
        if (MemoryWatchers.MovementLock.Current == 0x20 && Stage == 0)
        {
            base.Execute();
            BaseCutsceneValue = MemoryWatchers.EventFileStart.Current;
            Stage = 1;
            return;
        }

        // Stages 1+: Execute configured stage transitions
        if (Stage > 0 && Stage <= Stages.Length)
        {
            var (checkOffset, targetOffset) = Stages[Stage - 1];
            if (TransitionWatcher.Current == BaseCutsceneValue + checkOffset)
            {
                WriteValue<int>(TransitionWatcher, BaseCutsceneValue + targetOffset);
                Stage++;
            }
        }
    }
}
