using System.Collections.Generic;

namespace FFXCutsceneRemover.Constants;

/// <summary>
/// Provides language-specific indentation values for the new game start screen.
/// </summary>
public static class StartGameIndents
{
    private static readonly Dictionary<byte, byte[]> BaseIndentsByLanguage = new()
    {
        [0x00] = new byte[] { 0x43, 0x00, 0x47, 0x43, 0x4b, 0x4b, 0x00, 0x4e }, // Japanese
        [0x01] = new byte[] { 0x43, 0x00, 0x45, 0x41, 0x4a, 0x4a, 0x00, 0x4d }, // English
        [0x02] = new byte[] { 0x43, 0x00, 0x46, 0x42, 0x4a, 0x4a, 0x00, 0x4d }, // French
        [0x03] = new byte[] { 0x43, 0x00, 0x45, 0x41, 0x4a, 0x4a, 0x00, 0x4d }, // Spanish
        [0x04] = new byte[] { 0x43, 0x00, 0x45, 0x42, 0x4a, 0x4a, 0x00, 0x4d }, // German
        [0x05] = new byte[] { 0x43, 0x00, 0x45, 0x42, 0x4a, 0x4a, 0x00, 0x4d }, // Italian
        [0x09] = new byte[] { 0x43, 0x00, 0x46, 0x43, 0x4a, 0x4a, 0x00, 0x4d }, // Korean
        [0x0A] = new byte[] { 0x43, 0x00, 0x46, 0x43, 0x4a, 0x4a, 0x00, 0x4d }  // Chinese
    };

    private static readonly Dictionary<byte, byte> SeedIndentAdjustments = new()
    {
        [0x00] = 0x48, // Japanese
        [0x01] = 0x47, // English
        [0x02] = 0x47, // French
        [0x03] = 0x47, // Spanish
        [0x04] = 0x47, // German
        [0x05] = 0x47, // Italian
        [0x09] = 0x47, // Korean
        [0x0A] = 0x46  // Chinese
    };

    private static readonly byte[] DefaultIndents = new byte[]
        { 0x43, 0x00, 0x45, 0x41, 0x4a, 0x4a, 0x00, 0x4d };

    /// <summary>
    /// Gets the indentation values for a specific language.
    /// </summary>
    /// <param name="language">The language code from the game.</param>
    /// <param name="setSeedOn">Whether set seed mode is enabled.</param>
    /// <returns>A list of 8 indent byte values.</returns>
    public static List<byte> GetIndents(byte language, bool setSeedOn)
    {
        // Get base indents for language or use default
        byte[] baseIndents = BaseIndentsByLanguage.TryGetValue(language, out var value)
            ? value
            : DefaultIndents;

        var indents = new List<byte>(baseIndents);

        // Adjust seed indent if set seed is enabled (index 5)
        if (setSeedOn && SeedIndentAdjustments.TryGetValue(language, out byte adjustment))
        {
            indents[5] = adjustment;
        }

        return indents;
    }
}
