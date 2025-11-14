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
        public const int CheckOffset = 0xE1C;
        public const int SkipOffset = 0xF5D;
    }

    public static class Tanker
    {
        public const int CheckOffset = 0x3253;
        public const int SkipOffset = 0x3393;
    }

    public static class InsideSin
    {
        public const int CheckOffset = 0x2F1B;
        public const int SkipOffset = 0x37D0;
    }

    public static class Dive
    {
        public const int Stage1CheckOffset = 0xA468;
        public const int Stage1SkipOffset1 = 0xA468;
        public const int Stage1SkipOffset2 = 0x5BCF;
        public const int Stage2CheckOffset = 0xA4C9;
        public const int Stage2SkipOffset = 0xA4C9;
        public const int Stage3CheckOffset = 0x5C42;
        public const int Stage3SkipOffset = 0x5C42;
    }

    public static class Geos
    {
        public const int CheckOffset = 0x1D6A;
        public const int SkipOffset = 0x1ECA;
    }

    public static class Klikk
    {
        public const int CheckOffset = 0x1A4E;
        public const int SkipOffset = 0x1BE9;
    }

    public static class AlBhedBoat
    {
        public const int CheckOffset = 0x3A65;
        public const int SkipOffset = 0x3AB1;
    }

    public static class UnderwaterRuins
    {
        public const int CheckOffset = 0x2B54;
        public const int SkipOffset = 0x2BE6;
        public const int Transition2CheckOffset = 0x1CC8;
        public const int Transition2SkipOffset = 0x1E36;
    }

    public static class UnderwaterRuinsOutside
    {
        public const int CheckOffset = 0x2B7E;
        public const int SkipOffset = 0x2BD4;
    }

    public static class Beach
    {
        public const int CheckOffset = 0x2CFE;
        public const int SkipOffset = 0x2D59;
    }

    public static class Lagoon
    {
        public const int Transition1CheckOffset = 0x19AE;
        public const int Transition1SkipOffset = 0x19AE;
        public const int Transition2CheckOffset = 0x19D7;
        public const int Transition2SkipOffset = 0x19D7;
    }

    public static class Valefor
    {
        public const int CheckOffset = 0x3BBB;
        public const int SkipOffset = 0x3C5C;
    }

    public static class BesaidNight
    {
        public const int Transition1CheckOffset = 0x2D23;
        public const int Transition1SkipOffset = 0x2E22;
        public const int Transition2CheckOffset = 0x2E85;
        public const int Transition2SkipOffset = 0x2EDA;
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
        public const int CheckOffset = 0x58D1;
        public const int SkipOffset = 0x5945;
    }

    public static class Echuilles
    {
        public const int CheckOffset = 0x248A;
        public const int SkipOffset = 0x251C;
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
        public const int CheckOffset = 0x193E;
        public const int SkipOffset = 0x1993;
    }

    public static class KilikaTrials
    {
        public const int CheckOffset = 0x1997;
        public const int SkipOffset = 0x1A04;
    }

    public static class KilikaAntechamber
    {
        public const int CheckOffset = 0x5BD9;
        public const int SkipOffset = 0x5E3C;
    }

    public static class Ifrit
    {
        public const int CheckOffset = 0x34;
        public const int SkipOffset = 0x1E4;
    }

    public static class JechtShot
    {
        public const int CheckOffset = 0x46E6;
        public const int SkipOffset = 0x46E6;
    }

    public static class Oblitzerator
    {
        public const int CheckOffset = 0x3B1C;
        public const int SkipOffset = 0x3BC5;
    }

    public static class Blitzball
    {
        public const int CheckOffset = 0x36BA;
        public const int SkipOffset = 0x36BA;
    }

    public static class Sahagin
    {
        public const int CheckOffset = 0x5B52;
        public const int SkipOffset = 0x5C29;
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
        public const int CheckOffset = 0x1E04;
        public const int SkipOffset = 0x1EA7;
    }

    public static class ChocoboEater
    {
        public const int CheckOffset = 0x4EE5;
        public const int SkipOffset = 0x5020;
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
        public const int CheckOffset = 0x19B8;
        public const int SkipOffset = 0x1A05;
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
        public const int Transition1CheckOffset = 0x1F56;
        public const int Transition1SkipOffset = 0x1FA4;
        public const int Transition2CheckOffset = 0x1FCA;
        public const int Transition2SkipOffset = 0x203B;
    }

    public static class Farplane
    {
        public const int Transition1CheckOffset = 0x4374;
        public const int Transition1SkipOffset = 0x446E;
        public const int Transition2CheckOffset = 0x44E3;
        public const int Transition2SkipOffset = 0x45B5;
    }

    public static class Tromell
    {
        public const int CheckOffset = 0x1CA1;
        public const int SkipOffset = 0x1D14;
    }

    public static class Crawler
    {
        public const int Stage1CheckOffset = 0xF017;
        public const int Stage1SkipOffset = 0xF017;
        public const int Stage2CheckOffset = 0xF0F3;
        public const int Stage2SkipOffset = 0xF0F3;
        public const int Stage3CheckOffset = 0xF113;
        public const int Stage3SkipOffset = 0xF113;
        public const int Stage4CheckOffset = 0xF19E;
        public const int Stage4SkipOffset = 0xF19E;
        public const int Stage5CheckOffset = 0xF437;
        public const int Stage5SkipOffset = 0xF437;
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
        public const int CheckOffset = 0x1DC;
        public const int SkipOffset = 0x1DC;
    }

    public static class Home
    {
        public const int CheckOffset = 0x1DD4;
        public const int SkipOffset = 0x1EE8;
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
        public const int CheckOffset = 0x5293;
        public const int SkipOffset = 0x53B0;
    }

    public static class Bahamut
    {
        public const int CheckOffset = 0x86E;
        public const int SkipOffset = 0x86E;
    }

    public static class Isaaru
    {
        public const int CheckOffset = 0x332C;
        public const int SkipOffset = 0x345F;
    }

    public static class Altana
    {
        public const int CheckOffset = 0x1A8C;
        public const int SkipOffset = 0x1B1A;
    }

    public static class Natus
    {
        public const int CheckOffset = 0x4C47;
        public const int SkipOffset = 0x4D29;
    }

    public static class DefenderX
    {
        public const int CheckOffset = 0x586F;
        public const int SkipOffset = 0x586F;
    }

    public static class Ronso
    {
        public const int CheckOffset = 0x4DB6;
        public const int SkipOffset = 0x4F5B;
    }

    public static class Flux
    {
        public const int CheckOffset = 0x24DE;
        public const int SkipOffset = 0x2585;
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
        public const int CheckOffset = 0x1E5F;
        public const int SkipOffset = 0x1F1D;
    }

    public static class Omnis
    {
        public const int CheckOffset = 0x1F69;
        public const int SkipOffset = 0x2015;
    }

    public static class BFA
    {
        public const int CheckOffset = 0x206B;
        public const int SkipOffset = 0x2117;
    }

    public static class Aeon
    {
        public const int CheckOffset = 0x21B8;
        public const int SkipOffset = 0x2278;
    }

    public static class YuYevon
    {
        public const int CheckOffset = 0x232A;
        public const int SkipOffset = 0x23BD;
    }

    public static class YojimboFayth
    {
        public const int CheckOffset = 0x1C44;
        public const int SkipOffset = 0x1C99;
    }
}
