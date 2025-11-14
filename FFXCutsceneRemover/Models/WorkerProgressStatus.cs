namespace FFXCutsceneRemover.Models;

/// <summary>
/// Status codes for BackgroundWorker progress reporting.
/// </summary>
public enum WorkerProgressStatus
{
    /// <summary>Waiting for game process to start.</summary>
    WaitingForGame = 0,
    
    /// <summary>Successfully connected to the game process.</summary>
    Connected = 1,
    
    /// <summary>Mod is running and processing game state.</summary>
    Running = 2,
    
    /// <summary>Game exited or mod was stopped.</summary>
    Stopped = 3,
    
    /// <summary>RNG seed injection occurred.</summary>
    SeedInjection = 10,
    
    /// <summary>An error occurred.</summary>
    Error = -1
}
