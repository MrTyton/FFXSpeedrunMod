namespace FFXCutsceneRemover.Constants;

public static class MenuTrigger
{
    public const int NamingTidus = 0x40080000;
    public const int NamingIfrit = 0x40080009;
    public const int NamingIxion = 0x4008000A;
    public const int NamingShiva = 0x4008000B;
    public const int NamingBahamut = 0x4008000C;
}

public static class MenuValue
{
    public const int Standard = 0x4000;
    public const int Extended = 0x20000;
}

// Menu fix values for clearing menu bug
public static class MenuFixValues
{
    public const int MenuValue3Reset = unchecked((int)0xFFFFFFFF);
    public const int MenuValue4Reset = 0x00000000;
    public const byte MenuValue5Reset = 0x00;
    public const int MenuValue6Reset = 0x00000001;
    public const byte MenuValue7Reset = 0x00;
}
