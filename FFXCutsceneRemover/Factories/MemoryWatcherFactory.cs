using System;
using System.Diagnostics;
using System.Reflection;
using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Logging;
using FFXCutsceneRemover.Resources;

namespace FFXCutsceneRemover.Factories;

/// <summary>
/// Factory for initializing memory watchers with reduced boilerplate.
/// Provides helper methods to streamline the MemoryWatchers.Initialize() process.
/// </summary>
public static class MemoryWatcherFactory
{
    /// <summary>
    /// Creates and initializes a MemoryWatcher by looking up the corresponding MemoryLocation property.
    /// </summary>
    /// <typeparam name="T">The type of value the watcher monitors</typeparam>
    /// <param name="watcherName">Name of the watcher (must match property name in MemoryLocations)</param>
    /// <returns>Initialized MemoryWatcher</returns>
    public static MemoryWatcher<T> Create<T>(string watcherName) where T : struct
    {
        var locationProperty = typeof(MemoryLocations).GetProperty(watcherName, BindingFlags.Public | BindingFlags.Static);
        
        if (locationProperty == null)
        {
            DiagnosticLog.Warning($"MemoryLocation property not found for watcher: {watcherName}");
            return null;
        }

        var location = locationProperty.GetValue(null);
        
        // Call GetMemoryWatcher<T> with the location
        var getMemoryWatcherMethod = typeof(MemoryWatchers).GetMethod("GetMemoryWatcher", BindingFlags.NonPublic | BindingFlags.Static);
        if (getMemoryWatcherMethod == null)
        {
            DiagnosticLog.Warning("GetMemoryWatcher method not found in MemoryWatchers");
            return null;
        }

        var genericMethod = getMemoryWatcherMethod.MakeGenericMethod(typeof(T));
        return (MemoryWatcher<T>)genericMethod.Invoke(null, new[] { location });
    }

    /// <summary>
    /// Batch creates multiple MemoryWatchers of the same type.
    /// Useful for initializing groups of watchers with the same data type.
    /// </summary>
    /// <typeparam name="T">The type of value the watchers monitor</typeparam>
    /// <param name="watcherNames">Names of the watchers to create</param>
    /// <param name="setterAction">Action to set each created watcher (e.g., (name, watcher) => FieldName = watcher)</param>
    public static void CreateBatch<T>(string[] watcherNames, Action<string, MemoryWatcher<T>> setterAction) where T : struct
    {
        foreach (var name in watcherNames)
        {
            var watcher = Create<T>(name);
            if (watcher != null)
            {
                setterAction(name, watcher);
            }
        }
    }
}
