namespace FFXCutsceneRemover.Constants;

/// <summary>
/// Memory offsets for actor data structures.
/// </summary>
public static class ActorOffsets
{
    /// <summary>
    /// Stride between actor entries in the actor array (0x880 bytes).
    /// </summary>
    public const int ActorStride = 0x880;

    /// <summary>
    /// Offset to the actor's character index within the actor structure.
    /// </summary>
    public const int ActorIndex = 0x00;

    /// <summary>
    /// Offset to the actor's X position within the actor structure.
    /// </summary>
    public const int ActorPosX = 0x0C;

    /// <summary>
    /// Offset to the actor's Y position within the actor structure.
    /// </summary>
    public const int ActorPosY = 0x10;

    /// <summary>
    /// Offset to the actor's Z position within the actor structure.
    /// </summary>
    public const int ActorPosZ = 0x14;

    /// <summary>
    /// Offset to the actor's floor Y position within the actor structure.
    /// </summary>
    public const int ActorFloor = 0x16C;
}
