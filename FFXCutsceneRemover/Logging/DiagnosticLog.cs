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

    /// <summary>
    /// Core logging method that formats and writes log messages.
    /// </summary>
    private static void LogMessage(LogEventLevel level, string msg, string mname, string fpath, int lnb)
    {
        string formattedMsg = $"[{TrimFilePath(fpath)}:{lnb}] {mname}: {msg}";
        Log.Write(level, formattedMsg);
        LogMessageReceived?.Invoke(msg, level);
    }

    public static void Fatal(string msg,
                             [CallerMemberName] string mname = "",
                             [CallerFilePath] string fpath = "",
                             [CallerLineNumber] int lnb = 0)
        => LogMessage(LogEventLevel.Fatal, msg, mname, fpath, lnb);

    public static void Error(string msg,
                             [CallerMemberName] string mname = "",
                             [CallerFilePath] string fpath = "",
                             [CallerLineNumber] int lnb = 0)
        => LogMessage(LogEventLevel.Error, msg, mname, fpath, lnb);

    public static void Warning(string msg,
                               [CallerMemberName] string mname = "",
                               [CallerFilePath] string fpath = "",
                               [CallerLineNumber] int lnb = 0)
        => LogMessage(LogEventLevel.Warning, msg, mname, fpath, lnb);

    public static void Information(string msg,
                                   [CallerMemberName] string mname = "",
                                   [CallerFilePath] string fpath = "",
                                   [CallerLineNumber] int lnb = 0)
        => LogMessage(LogEventLevel.Information, msg, mname, fpath, lnb);

    public static void Debug(string msg,
                             [CallerMemberName] string mname = "",
                             [CallerFilePath] string fpath = "",
                             [CallerLineNumber] int lnb = 0)
        => LogMessage(LogEventLevel.Debug, msg, mname, fpath, lnb);

    public static void Verbose(string msg,
                               [CallerMemberName] string mname = "",
                               [CallerFilePath] string fpath = "",
                               [CallerLineNumber] int lnb = 0)
        => LogMessage(LogEventLevel.Verbose, msg, mname, fpath, lnb);
}