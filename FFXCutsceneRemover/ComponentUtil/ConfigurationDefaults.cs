namespace FFXCutsceneRemover.ComponentUtil;

/// <summary>
/// Default configuration values for the FFX Speedrun Mod.
/// Centralizes magic numbers and default settings.
/// </summary>
public static class ConfigurationDefaults
{
    /// <summary>
    /// Default sleep interval in milliseconds for the main game loop.
    /// Lower = more responsive, higher = less CPU usage.
    /// </summary>
    public const int DefaultSleepInterval = 16;
    
    /// <summary>
    /// Minimum allowed sleep interval in milliseconds.
    /// </summary>
    public const int MinSleepInterval = 1;
    
    /// <summary>
    /// Maximum allowed sleep interval in milliseconds.
    /// </summary>
    public const int MaxSleepInterval = 1000;
    
    /// <summary>
    /// Interval in milliseconds for checking if FFX process has started (auto-start feature).
    /// </summary>
    public const int AutoStartCheckInterval = 2000;
    
    /// <summary>
    /// Delay in milliseconds when searching for game process.
    /// </summary>
    public const int ProcessSearchDelay = 1000;
    
    // Tooltip configuration
    /// <summary>
    /// Time in milliseconds before tooltip automatically disappears.
    /// </summary>
    public const int ToolTipAutoPopDelay = 10000;
    
    /// <summary>
    /// Initial delay in milliseconds before tooltip appears.
    /// </summary>
    public const int ToolTipInitialDelay = 500;
    
    /// <summary>
    /// Delay in milliseconds before tooltip reappears.
    /// </summary>
    public const int ToolTipReshowDelay = 200;
}
