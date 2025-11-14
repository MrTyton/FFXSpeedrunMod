namespace FFXCutsceneRemover.Constants;

/// <summary>
/// Constants related to battle encounters and states.
/// </summary>
public static class BattleConstants
{
    /// <summary>
    /// Encounter map ID for Sinspawn Gui battle 1.
    /// </summary>
    public const byte EncounterMapGui1 = 27;

    /// <summary>
    /// Encounter map ID for Sinspawn Gui battle 2.
    /// </summary>
    public const byte EncounterMapGui2 = 29;

    /// <summary>
    /// Dialogue value that triggers the Gui battle sequence.
    /// </summary>
    public const byte DialogueTriggerGui = 95;

    /// <summary>
    /// BattleState2 value indicating the player is in battle.
    /// </summary>
    public const byte BattleState2InBattle = 22;

    /// <summary>
    /// BattleState2 value indicating the player is not in battle.
    /// </summary>
    public const byte BattleState2NotInBattle = 0;
}
