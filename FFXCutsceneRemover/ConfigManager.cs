using System;
using System.IO;
using System.Text.Json;
using FFXCutsceneRemover.Logging;

namespace FFXCutsceneRemover;

public class ConfigManager
{
    private static readonly string ConfigDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "FFXSpeedrunMod"
    );

    public static void EnsureConfigDirectoryExists()
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            Directory.CreateDirectory(ConfigDirectory);
            DiagnosticLog.Information($"Created config directory: {ConfigDirectory}");
        }
    }

    public static void SaveConfig(CsrConfig config, string filename)
    {
        EnsureConfigDirectoryExists();
        
        string filePath = Path.Combine(ConfigDirectory, filename);
        
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(filePath, json);
            
            DiagnosticLog.Information($"Configuration saved to: {filePath}");
        }
        catch (Exception ex)
        {
            DiagnosticLog.Error($"Failed to save configuration: {ex.Message}");
            throw;
        }
    }

    public static CsrConfig LoadConfig(string filename)
    {
        string filePath = Path.Combine(ConfigDirectory, filename);
        
        if (!File.Exists(filePath))
        {
            DiagnosticLog.Information($"Configuration file not found: {filePath}");
            return null;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            var config = JsonSerializer.Deserialize<CsrConfig>(json);
            
            DiagnosticLog.Information($"Configuration loaded from: {filePath}");
            return config;
        }
        catch (Exception ex)
        {
            DiagnosticLog.Error($"Failed to load configuration: {ex.Message}");
            return null;
        }
    }

    public static bool ConfigExists(string filename)
    {
        string filePath = Path.Combine(ConfigDirectory, filename);
        return File.Exists(filePath);
    }

    public static string[] GetAvailableConfigs()
    {
        EnsureConfigDirectoryExists();
        
        try
        {
            var files = Directory.GetFiles(ConfigDirectory, "*.conf");
            var filenames = new string[files.Length];
            
            for (int i = 0; i < files.Length; i++)
            {
                filenames[i] = Path.GetFileName(files[i]);
            }
            
            return filenames;
        }
        catch (Exception ex)
        {
            DiagnosticLog.Error($"Failed to list configurations: {ex.Message}");
            return Array.Empty<string>();
        }
    }

    public static string GetConfigDirectory()
    {
        return ConfigDirectory;
    }
}
