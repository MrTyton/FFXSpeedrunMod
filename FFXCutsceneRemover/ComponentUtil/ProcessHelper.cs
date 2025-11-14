using System;
using System.Diagnostics;
using System.Linq;

namespace FFXCutsceneRemover.ComponentUtil;

/// <summary>
/// Helper utilities for managing Windows processes.
/// Consolidates process connection and management logic.
/// </summary>
public static class ProcessHelper
{
    /// <summary>
    /// Finds the most recently started process with the given name.
    /// </summary>
    /// <param name="processName">Name of the process to find (without .exe extension)</param>
    /// <returns>The most recent process, or null if none found</returns>
    public static Process FindGameProcess(string processName)
    {
        var processes = Process.GetProcessesByName(processName);
        
        if (processes.Length == 0)
        {
            return null;
        }
        
        // Manual loop is more efficient than LINQ OrderBy for this use case
        Process latestProcess = null;
        DateTime latestStartTime = DateTime.MinValue;
        
        foreach (var proc in processes)
        {
            try
            {
                if (proc.StartTime > latestStartTime)
                {
                    latestStartTime = proc.StartTime;
                    latestProcess = proc;
                }
            }
            catch
            {
                // Process may have exited or we don't have permission, skip it
            }
        }
        
        return latestProcess;
    }
    
    /// <summary>
    /// Checks if a process is still running.
    /// </summary>
    /// <param name="process">Process to check</param>
    /// <returns>True if process exists and is running, false otherwise</returns>
    public static bool IsProcessRunning(Process process)
    {
        if (process == null)
        {
            return false;
        }
        
        try
        {
            return !process.HasExited;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Safely disposes a process, killing it if still running.
    /// Sets the process reference to null.
    /// </summary>
    /// <param name="process">Process to dispose (will be set to null)</param>
    public static void DisposeProcess(ref Process process)
    {
        if (process == null)
        {
            return;
        }
        
        try
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
        }
        catch
        {
            // Ignore errors during kill - process may have already exited
        }
        
        try
        {
            process.Dispose();
        }
        catch
        {
            // Ignore disposal errors
        }
        finally
        {
            process = null;
        }
    }
    
    /// <summary>
    /// Waits for a process matching the given name to start.
    /// </summary>
    /// <param name="processName">Name of the process to find</param>
    /// <param name="checkIntervalMs">How often to check in milliseconds</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The found process, or null if cancelled</returns>
    public static Process WaitForProcess(string processName, int checkIntervalMs = ConfigurationDefaults.ProcessSearchDelay)
    {
        Process process;
        while ((process = FindGameProcess(processName)) == null)
        {
            System.Threading.Thread.Sleep(checkIntervalMs);
        }
        return process;
    }
}
