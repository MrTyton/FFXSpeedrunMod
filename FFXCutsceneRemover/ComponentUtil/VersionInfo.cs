namespace FFXCutsceneRemover.ComponentUtil;

/// <summary>
/// Version information for the FFX Speedrun Mod.
/// Single source of truth for version numbers displayed across GUI and CLI.
/// </summary>
public static class VersionInfo
{
    /// <summary>
    /// Major version number.
    /// </summary>
    public const int MajorVersion = 1;

    /// <summary>
    /// Minor version number.
    /// </summary>
    public const int MinorVersion = 8;

    /// <summary>
    /// Patch version number.
    /// </summary>
    public const int PatchVersion = 1;

    /// <summary>
    /// Full version string in format "1.7.0"
    /// </summary>
    public static string FullVersion => $"{MajorVersion}.{MinorVersion}.{PatchVersion}";

    /// <summary>
    /// Display name for GUI windows and dialogs: "FFX Speedrun Mod v1.7.0"
    /// </summary>
    public static string DisplayName => $"FFX Speedrun Mod v{FullVersion}";

    /// <summary>
    /// Bracketed name for console/log output: "[FFX Speedrunning Mod v1.7.0]"
    /// </summary>
    public static string BracketedName => $"[FFX Speedrunning Mod v{FullVersion}]";
}
