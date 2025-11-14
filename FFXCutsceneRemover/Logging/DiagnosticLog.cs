// DiagnosticLog.cs - logging basics

// System imports
using System;
using System.IO;
using System.Runtime.CompilerServices;

// Third-party imports
using Serilog;
using Serilog.Events;

namespace FFXCutsceneRemover.Logging;

public static class DiagnosticLog
{
    // Event that GUI can subscribe to for real-time log messages
    public static event Action<string, LogEventLevel> LogMessageReceived;

    static DiagnosticLog()
    {
        string rootPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ??
                          Environment.ExpandEnvironmentVariables("%APPDATA%/FFXCutsceneRemover/Logs");

        Log.Logger = new LoggerConfiguration().
                     MinimumLevel.Debug().
                     WriteTo.Console(LogEventLevel.Information).
                     WriteTo.File(Path.Combine(rootPath, "csr-log.log"),
                                  LogEventLevel.Debug).
                     CreateLogger();
    }

    public static string TrimFilePath(string filePath)
    {
        return Path.GetFileName(filePath);
    }

    public static void Fatal(string                    msg,
                             [CallerMemberName] string mname = "",
                             [CallerFilePath]   string fpath = "",
                             [CallerLineNumber] int    lnb   = 0)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Fatal(formattedMsg);
        LogMessageReceived?.Invoke(msg, LogEventLevel.Fatal);
    }

    public static void Error(string                    msg,
                             [CallerMemberName] string mname = "",
                             [CallerFilePath]   string fpath = "",
                             [CallerLineNumber] int    lnb   = 0)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Error(formattedMsg);
        LogMessageReceived?.Invoke(msg, LogEventLevel.Error);
    }

    public static void Warning(string                    msg,
                               [CallerMemberName] string mname = "",
                               [CallerFilePath]   string fpath = "",
                               [CallerLineNumber] int    lnb   = 0)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Warning(formattedMsg);
        LogMessageReceived?.Invoke(msg, LogEventLevel.Warning);
    }

    public static void Information(string                    msg,
                                   [CallerMemberName] string mname = "",
                                   [CallerFilePath]   string fpath = "",
                                   [CallerLineNumber] int    lnb   = 0)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Information(formattedMsg);
        LogMessageReceived?.Invoke(msg, LogEventLevel.Information);
    }

    public static void Debug(string                    msg,
                             [CallerMemberName] string mname = "",
                             [CallerFilePath]   string fpath = "",
                             [CallerLineNumber] int    lnb   = 0)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Debug(formattedMsg);
        LogMessageReceived?.Invoke(msg, LogEventLevel.Debug);
    }

    public static void Verbose(string                    msg,
                               [CallerMemberName] string mname = "",
                               [CallerFilePath]   string fpath = "",
                               [CallerLineNumber] int    lnb   = 0)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Verbose(formattedMsg);
        LogMessageReceived?.Invoke(msg, LogEventLevel.Verbose);
    }
}