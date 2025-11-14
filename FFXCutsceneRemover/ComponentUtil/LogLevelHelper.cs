using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace FFXCutsceneRemover.ComponentUtil;

/// <summary>
/// Helper for working with log levels in a type-safe manner.
/// Provides conversions between LogEventLevel enum and display strings.
/// </summary>
public static class LogLevelHelper
{
    private static readonly Dictionary<LogEventLevel, string> DisplayNames = new()
    {
        { LogEventLevel.Verbose, "Verbose" },
        { LogEventLevel.Debug, "Debug" },
        { LogEventLevel.Information, "Information" },
        { LogEventLevel.Warning, "Warning" },
        { LogEventLevel.Error, "Error" },
        { LogEventLevel.Fatal, "Fatal" }
    };

    /// <summary>
    /// Gets all log level display names for populating ComboBox items.
    /// </summary>
    public static string[] GetDisplayNames() => DisplayNames.Values.ToArray();

    /// <summary>
    /// Converts a display name string to a LogEventLevel enum.
    /// </summary>
    /// <param name="displayName">Display name from UI</param>
    /// <returns>Corresponding LogEventLevel, or Information if not found</returns>
    public static LogEventLevel FromDisplayName(string displayName)
    {
        var kvp = DisplayNames.FirstOrDefault(pair => pair.Value.Equals(displayName, StringComparison.OrdinalIgnoreCase));
        return kvp.Key != default ? kvp.Key : LogEventLevel.Information;
    }

    /// <summary>
    /// Converts a LogEventLevel enum to its display name.
    /// </summary>
    /// <param name="level">Log level enum</param>
    /// <returns>Display name string, or "Information" if not found</returns>
    public static string ToDisplayName(LogEventLevel level)
    {
        return DisplayNames.TryGetValue(level, out var name) ? name : "Information";
    }
}
