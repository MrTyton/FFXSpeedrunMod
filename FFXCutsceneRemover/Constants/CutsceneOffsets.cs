namespace FFXCutsceneRemover.Constants;

/// <summary>
/// Memory offsets for cutscene skip transitions.
/// These offsets are added to the BaseCutsceneValue to determine cutscene state.
/// </summary>
public static class CutsceneOffsets
{
    public static class Auron
    {
        public const int CheckOffset = 0x4233;
        public const int SkipOffset = 0x42EE;
    }

    public static class Ammes
    {
        public const int CheckOffset1 = 0x97FA;
        public const int SkipOffset1 = 0x9936;
        public const int CheckOffset2 = 0x9A2C;
    }

    public static class Aftermath
    {
        public const int CheckOffset = 0x13D3;
    }

    public static class Tanker
    {
        public const int CheckOffset = 0x3253;
        public const int SkipOffset = 0x3393;
    }

    public static class InsideSin
    {
        public const int CheckOffset = 0xE65;
        public const int SkipOffset = 0xF43;
    }

    public static class Dive
    {
        public const int CheckOffset1 = 0x2935;
        public const int SkipOffset1 = 0xA468;
        public const int SkipOffset2 = 0x5BCF;
        public const int CheckOffset2 = 0xA4C8;
        public const int SkipOffset3 = 0xA4C9;
    }

    public static class Geos
    {
        public const int CheckOffset = 0xA4F8;
    }

    public static class Klikk
    {
        public const int CheckOffset1 = 0xA523;
        public const int SkipOffset1 = 0xA7E3;
        public const int CheckOffset2 = 0xA7E3;
        public const int SkipOffset2 = 0xA847;
        public const int SkipOffset3 = 0xAF27;
    }

    public static class AlBhedBoat
    {
        public const int CheckOffset = 0xEF32;
        public const int SkipOffset = 0xF127;
    }

    public static class UnderwaterRuins
    {
        public const int CheckOffset = 0x515B;
        public const int SkipOffset = 0x520D;
    }

    public static class UnderwaterRuinsOutside
    {
        public const int CheckOffset = 0x3870;
        public const int SkipOffset = 0x3AB2;
    }

    public static class Beach
    {
        public const int CheckOffset = 0x2CFE;
        public const int SkipOffset = 0x2D59;
    }

    public static class Lagoon
    {
        public const int CheckOffset1 = 0x2D76;
        public const int SkipOffset1 = 0x2E34;
        public const int CheckOffset2 = 0x2EFF;
    }

    public static class Valefor
    {
        public const int SkipOffset = 0xAA4;
    }

    public static class BesaidNight
    {
        public const int CheckOffset1 = 0x68C2;
        public const int SkipOffset1 = 0x6DFB;
        public const int CheckOffset2 = 0x6F32;
        public const int SkipOffset2 = 0x773D;
    }

    public static class Kimahri
    {
        public const int CheckOffset = 0x231A;
        public const int SkipOffset = 0x23F3;
        public const int PostBattleOffset = 0x2AE3;
    }

    public static class YunaBoat
    {
        public const int CheckOffset = 0x23F0;
        public const int SkipOffset = 0x248E;
    }

    public static class SinFin
    {
        public const int CheckOffset1 = 0x76F;
        public const int SkipOffset1 = 0xBF0;
        public const int CheckOffset2 = 0xCFD;
        public const int SkipOffset2 = 0x114A;
    }

    public static class Echuilles
    {
        public const int CheckOffset = 0x20D0;
        public const int SkipOffset = 0x248A;
    }

    public static class Geneaux
    {
        public const int CheckOffset = 0x65CA;
        public const int SkipOffset = 0x67AA;
        public const int CheckOffset2 = 0x67F4;
        public const int SkipOffset2 = 0x6A47;
    }

    public static class KilikaElevator
    {
        public const int CheckOffset = 0x3582;
        public const int SkipOffset = 0x385C;
    }

    public static class KilikaTrials
    {
        public const int CheckOffset = 0x61;
        public const int SkipOffset = 0x108;
    }

    public static class KilikaAntechamber
    {
        public const int CheckOffset = 0x3CDA;
        public const int SkipOffset = 0x3DC6;
    }

    public static class Ifrit
    {
        public const int CheckOffset = 0x34;
        public const int SkipOffset = 0x1E4;
    }

    public static class JechtShot
    {
        public const int CheckOffset1 = 0xF3A7;
        public const int SkipOffset1 = 0xF9E4;
        public const int CheckOffset2 = 0xFAAE;
    }

    public static class Oblitzerator
    {
        public const int CheckOffset1 = 0x513B;
        public const int SkipOffset1 = 0x5191;
        public const int CheckOffset2 = 0x5194;
        public const int SkipOffset2 = 0x52AE;
        public const int CheckOffset3 = 0x52B4;
        public const int SkipOffset3 = 0x5359;
        public const int SkipOffset4 = 0x569B;
    }

    public static class Blitzball
    {
        public const int CheckOffset = 0x2EA1;
        public const int SkipOffset = 0x301C;
    }

    public static class Sahagin
    {
        public const int CheckOffset = 0xC9;
        public const int SkipOffset = 0x45A;
        public const int CheckOffset2 = 0x491;
        public const int SkipOffset2 = 0x556;
    }

    public static class Garuda
    {
        public const int CheckOffset1 = 0x21;
        public const int SkipOffset1 = 0x2A9;
        public const int CheckOffset2 = 0x566;
        public const int SkipOffset2 = 0x84C;
    }

    public static class Rin
    {
        public const int CheckOffset1 = 0x271;
        public const int SkipOffset1 = 0x466;
        public const int CheckOffset2 = 0x58B;
        public const int CheckOffset3 = 0x5B1;
    }

    public static class ChocoboEater
    {
        public const int CheckOffset = 0x7A5;
        public const int SkipOffset = 0xC96;
    }

    public static class Gui
    {
        public const int CheckOffset = 0x5056;
        public const int SkipOffset = 0x514D;
        public const int Gui2CheckOffset = 0x51A4;
        public const int Gui2SkipOffset = 0x524A;
    }

    public static class Djose
    {
        public const int CheckOffset = 0x160;
        public const int SkipOffset = 0x4ED;
    }

    public static class Ixion
    {
        public const int CheckOffset = 0x2C4C;
        public const int SkipOffset = 0x2CEE;
    }

    public static class Extractor
    {
        public const int CheckOffset = 0x16CA;
        public const int SkipOffset = 0x1772;
    }

    public static class SeymoursHouse
    {
        public const int CheckOffset1 = 0x730E;
        public const int SkipOffset1 = 0x7372;
        public const int CheckOffset2 = 0x7122;
        public const int SkipOffset2 = 0x7263;
        public const int CheckOffset3 = 0x6EC1;
        public const int SkipOffset3 = 0x70B5;
    }

    public static class Farplane
    {
        public const int CheckOffset1 = 0xB12D;
        public const int SkipOffset1 = 0xB350;
        public const int CheckOffset2 = 0xB620;
        public const int SkipOffset2 = 0xB86C;
        public const int CheckOffset3 = 0xBC00;
    }

    public static class Tromell
    {
        public const int CheckOffset = 0x88A9;
        public const int SkipOffset = 0x9339;
    }

    public static class Crawler
    {
        public const int Stage1CheckOffset = 0xEBC2;
        public const int Stage1SkipOffset = 0xF017;
        public const int Stage2CheckOffset = 0xF02D;
        public const int Stage2SkipOffset = 0xF0F3;
        public const int Stage3CheckOffset = 0xF0FC;
        public const int Stage3SkipOffset = 0xF113;
        public const int Stage4CheckOffset = 0xF113;
        public const int Stage4SkipOffset = 0xF19E;
        public const int Stage6CheckOffset = 0xF1E6;
        public const int Stage6SkipOffset = 0xF437;
    }

    public static class Seymour
    {
        public const int CheckOffset = 0x70CF;
        public const int SkipOffset = 0x721B;
        public const int Transition2CheckOffset = 0x7263;
        public const int Transition2SkipOffset = 0x738E;
    }

    public static class Wendigo
    {
        public const int CheckOffset = 0x19BF;
        public const int SkipOffset = 0x1B1E;
        public const int PostCheckOffset = 0x1B7E;
        public const int PostBattleOffset = 0x1E31;
    }

    public static class Spherimorph
    {
        public const int CheckOffset = 0x3477;
        public const int SkipOffset = 0x35A5;
        public const int PostBattleOffset = 0x36CC;
    }

    public static class UnderLake
    {
        public const int CheckOffset = 0x4E50;
        public const int SkipOffset = 0x4F20;
    }

    public static class Bikanel
    {
        public const int CheckOffset = 0x11F;
        public const int SkipOffset = 0x11F;
    }

    public static class Home
    {
        public const int CheckOffset1 = 0x5FDB;
        public const int SkipOffset1 = 0x61CE;
        public const int CheckOffset2 = 0x63AA;
    }

    public static class Evrae
    {
        public const int CheckOffset = 0x5DFC;
        public const int SkipOffset = 0x5F53;
        public const int AirshipCheckOffset = 0x5FAF;
        public const int AirshipSkipOffset = 0x60D3;
    }

    public static class Guards
    {
        public const int CheckOffset = 0x876E;
        public const int SkipOffset = 0x90E1;
        public const int CheckOffset2 = 0x90F0;
    }

    public static class Bahamut
    {
        public const int CheckOffset = 0x680;
        public const int SkipOffset = 0x86E;
    }

    public static class Isaaru
    {
        public const int CheckOffset1 = 0x7F6C + 0x32;
        public const int SkipOffset1 = 0x7F6C + 0x2F8;
        public const int CheckOffset2 = 0x7F6C + 0x37A;
        public const int SkipOffset2 = 0x7F6C + 0x5C4;
    }

    public static class Altana
    {
        public const int CheckOffset = 0x69;
        public const int SkipOffset = 0x307;
        public const int CheckOffset2 = 0x32C;
        public const int SkipOffset2_1 = 0xA7D;
        public const int SkipOffset2_2 = 0x7A2;
        public const int SkipOffset2_3 = 0x3A9;
    }

    public static class Natus
    {
        public const int CheckOffset = 0xE0F0;
        public const int SkipOffset = 0xE2DF;
        public const int CheckOffset2 = 0xE2FD;
        public const int SkipOffset2 = 0xE395;
    }

    public static class DefenderX
    {
        public const int CheckOffset = 0x5451;
        public const int SkipOffset = 0x5451;
    }

    public static class Ronso
    {
        public const int CheckOffset1 = 0x12907;
        public const int SkipOffset1 = 0x13F9F;
        public const int CheckOffset2 = 0x14056;
    }

    public static class Flux
    {
        public const int CheckOffset = 0x5D5C;
        public const int SkipOffset = 0x6BC7;
        public const int PostBattleOffset = 0x6E23;
    }

    public static class Sanctuary
    {
        public const int CheckOffset = 0x33A9;
        public const int SkipOffset = 0x348B;
    }

    public static class SpectralKeeper
    {
        public const int CheckOffset = 0x4E74;
        public const int SkipOffset = 0x4F9F;
        public const int Transition2CheckOffset = 0x5004;
        public const int Transition2SkipOffset = 0x50AA;
    }

    public static class Yunalesca
    {
        public const int CheckOffset1 = 0x5DF0;
        public const int SkipOffset1 = 0x645A;
        public const int CheckOffset2 = 0x64BA;
        public const int SkipOffset2 = 0x67B4;
        public const int PostBattleOffset = 0x6C8D;
    }

    public static class Fins
    {
        public const int CheckOffset = 0x6F5A;
        public const int SkipOffset = 0x70AF;
        public const int AirshipCheckOffset = 0x7107;
        public const int AirshipSkipOffset = 0x7254;
    }

    public static class SinCore
    {
        public const int CheckOffset = 0x1CC5;
        public const int SkipOffset = 0x1DDB;
    }

    public static class OverdriveSin
    {
        public const int CheckOffset = 0x5BF8;
        public const int SkipOffset1 = 0x5E2E;
        public const int SkipOffset2 = 0x5F79;
    }

    public static class Omnis
    {
        public const int CheckOffset1 = 0x1DB0;
        public const int SkipOffset1 = 0x1E2C;
        public const int CheckOffset2 = 0x1E3B;
        public const int SkipOffset2 = 0x25B5;
        public const int CheckOffset3 = 0x25BB;
        public const int SkipOffset3 = 0x274A;
        public const int CheckOffset4 = 0x2786;
        public const int SkipOffset4 = 0x2D84;
    }

    public static class BFA
    {
        public const int CheckOffset1 = 0x42;
        public const int CheckOffset2 = 0xC08C;
        public const int SkipOffset2 = 0xCDBB;
        public const int CheckOffset3 = 0xCDC4;
        public const int SkipOffset3 = 0xCF98;
        public const int CheckOffset4 = 0xCFB9;
        public const int SkipOffset4 = 0xD126;
        public const int CheckOffset5 = 0xD12C;
        public const int SkipOffset5 = 0xD382;
    }

    public static class Aeon
    {
        public const int CheckOffset = 0x21B8;
        public const int SkipOffset = 0x2278;
    }

    public static class YuYevon
    {
        public const int CheckOffset1 = 0x69BD;
        public const int SkipOffset1 = 0x6A9F;
        public const int CheckOffset2 = 0x7B30;
        public const int SkipOffset2 = 0x86D0;
    }

    public static class YojimboFayth
    {
        public const int CheckOffset = 0x2C19;
        public const int SkipOffset = 0x2DFF;
    }
}
