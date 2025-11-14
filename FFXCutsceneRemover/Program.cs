using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;
using FFXCutsceneRemover.Logging;
using FFXCutsceneRemover.Services;

namespace FFXCutsceneRemover;

internal sealed class CsrConfigBinder : BinderBase<CsrConfig>
{
    private readonly Option<bool?> _optCsrOn;
    private readonly Option<bool?> _optCsrBreakOn;
    private readonly Option<bool?> _optTrueRngOn;
    private readonly Option<bool?> _optSetSeedOn;
    private readonly Option<int?> _optMtSleepInterval;

    public CsrConfigBinder(Option<bool?> optCsrOn,
                           Option<bool?> optCsrBreakOn,
                           Option<bool?> optTrueRngOn,
                           Option<bool?> optSetSeedOn,
                           Option<int?> optMtSleepInterval)
    {
        _optCsrOn = optCsrOn;
        _optCsrBreakOn = optCsrBreakOn;
        _optTrueRngOn = optTrueRngOn;
        _optSetSeedOn = optSetSeedOn;
        _optMtSleepInterval = optMtSleepInterval;
    }

    private static bool ResolveMandatoryBoolArg(Option<bool?> opt)
    {
        Console.WriteLine(opt.Description);
        return Console.ReadLine()?.ToUpper().StartsWith("Y") ?? false;
    }

    protected override CsrConfig GetBoundValue(BindingContext bindingContext)
    {
        var csr_config = new CsrConfig { };

        csr_config.CsrOn = bindingContext.ParseResult.GetValueForOption(_optCsrOn) ?? ResolveMandatoryBoolArg(_optCsrOn);
        csr_config.CsrBreakOn = csr_config.CsrOn && ResolveMandatoryBoolArg(_optCsrBreakOn);

        csr_config.TrueRngOn = bindingContext.ParseResult.GetValueForOption(_optTrueRngOn) ?? ResolveMandatoryBoolArg(_optTrueRngOn);
        csr_config.SetSeedOn = !csr_config.TrueRngOn && ResolveMandatoryBoolArg(_optSetSeedOn);

        csr_config.MtSleepInterval = bindingContext.ParseResult.GetValueForOption(_optMtSleepInterval) ?? ConfigurationDefaults.DefaultSleepInterval;

        return csr_config;
    }
}

public sealed record CsrConfig
{
    private int _mtSleepInterval = ConfigurationDefaults.DefaultSleepInterval;

    public bool CsrOn { get; set; }
    public bool CsrBreakOn { get; set; }
    public bool TrueRngOn { get; set; }
    public bool SetSeedOn { get; set; }

    public int MtSleepInterval
    {
        get => _mtSleepInterval;
        set => _mtSleepInterval = Math.Clamp(value,
            ConfigurationDefaults.MinSleepInterval,
            ConfigurationDefaults.MaxSleepInterval);
    }

    public uint SelectedSeed { get; set; }
    public bool AutoStart { get; set; }
    public string FfxExecutablePath { get; set; }

    /// <summary>
    /// Validates the configuration and returns any validation errors.
    /// </summary>
    /// <param name="errorMessage">Validation error message if validation fails</param>
    /// <returns>True if valid, false otherwise</returns>
    public bool Validate(out string errorMessage)
    {
        if (SetSeedOn && !GameConstants.PCSeeds.Contains(SelectedSeed))
        {
            errorMessage = $"Invalid seed value: {SelectedSeed}. Seed must be one of the predefined PC seeds.";
            return false;
        }

        if (CsrBreakOn && !CsrOn)
        {
            errorMessage = "CSR Break cannot be enabled without CSR being enabled.";
            return false;
        }

        errorMessage = null;
        return true;
    }
};

public class Program
{
    private static CsrConfig csrConfig;
    private static CutsceneRemover cutsceneRemover = null;
    private static RNGMod rngMod = null;

    private static Process Game = null;

    private static readonly BreakTransition BreakTransition = new BreakTransition { ForceLoad = false, Description = "Break Setup", ConsoleOutput = false, Suspendable = false, Repeatable = true };

    private static uint seedSubmitted;

    // Cutscene Remover Version Number, 0x30 - 0x39 = 0 - 9, 0x48 = decimal point
    private const int majorID = 1;
    private const int minorID = 7;
    private const int patchID = 0;

    static Mutex mutex = new Mutex(true, "CSR");

    static void Main(string[] args)
    {
        if (CheckExistingCSR()) return;

        DiagnosticLog.Information($"Cutscene Remover for Final Fantasy X, version {majorID}.{minorID}.{patchID}");
        if (args.Length > 0) DiagnosticLog.Information($"!!! LAUNCHED WITH COMMAND-LINE OPTIONS: {string.Join(' ', args)} !!!");

        Option<bool?> optCsrOn = new Option<bool?>("--csr", "Enable Cutscene Remover (CSR) Mod? [Y/N]");
        Option<bool?> optCsrBreakOn = new Option<bool?>("--csrbreak", "Enable break for CSR? [Y/N]");
        Option<bool?> optTrueRngOn = new Option<bool?>("--truerng", "Enable True RNG Mod? [Y/N]");
        Option<bool?> optSetSeedOn = new Option<bool?>("--setseed", "Enable Set Seed Mod? [Y/N]");
        Option<int?> optMtSleepInterval = new Option<int?>("--mt_sleep_interval", "Specify the main thread sleep interval. [ms]");

        RootCommand rootCmd = new RootCommand("Launches the FFX Cutscene Remover.")
        {
            optCsrOn,
            optCsrBreakOn,
            optTrueRngOn,
            optSetSeedOn,
            optMtSleepInterval
        };

        rootCmd.SetHandler(MainLoop, new CsrConfigBinder(optCsrOn, optCsrBreakOn, optTrueRngOn, optSetSeedOn, optMtSleepInterval));

        rootCmd.Invoke(args);
        return;
    }

    private static void MainLoop(CsrConfig config)
    {
        csrConfig = config;

        if (csrConfig.SetSeedOn)
        {
            SetSeed();
            csrConfig.SelectedSeed = seedSubmitted;
        }

        while (true)
        {
            Game = ConnectToTarget(GameConstants.ProcessName);

            if (Game == null)
            {
                continue;
            }

            MemoryWatchers.Initialize(Game);
            MemoryWatchers.Watchers.UpdateAll(Game);

            byte language = MemoryWatchers.Language.Current;
            List<byte> startGameIndents = StartGameIndents.GetIndents(language, csrConfig.SetSeedOn);

            if (csrConfig.CsrOn)
            {
                cutsceneRemover = new CutsceneRemover(csrConfig.MtSleepInterval);
                cutsceneRemover.Game = Game;
            }

            if (csrConfig.TrueRngOn)
            {
                rngMod = new RNGMod();
                rngMod.Game = Game;
            }

            DiagnosticLog.Information("Starting main loop!");

            // Execute the game loop using the shared service
            var gameLoopService = new GameLoopService(
                Game,
                csrConfig,
                cutsceneRemover,
                rngMod,
                message => DiagnosticLog.Information(message),
                () => !Game.HasExited
            );

            gameLoopService.Execute(startGameIndents);
        }
    }

    private static bool CheckExistingCSR()
    {
        bool isRunning = !mutex.WaitOne(TimeSpan.Zero, true);

        if (isRunning)
        {
            Console.WriteLine("Cutscene Remover is already running!");
            Console.ReadLine();
        }

        return isRunning;
    }

    private static void SetSeed()
    {
        bool seedEnteredByUser = false;

        while (!seedEnteredByUser)
        {
            Console.WriteLine("Enter Seed ID To Run");
            string seedString = Console.ReadLine();
            if (!uint.TryParse(seedString, out seedSubmitted))
            {
                Console.WriteLine("Seed Contained Non-Numeric Characters. Please Try Again.");
                continue;
            }
            if (!GameConstants.PCSeeds.Contains(seedSubmitted))
            {
                Console.WriteLine("Seed is not recognised as a valid PC Seed. Please Try Again.");
            }
            else
            {
                seedEnteredByUser = true;
            }
        }
    }

    private static Process ConnectToTarget(string TargetName)
    {
        Process Game = null;

        try
        {
            Game = ProcessHelper.FindGameProcess(TargetName);
        }
        catch (Win32Exception e)
        {
            DiagnosticLog.Information("Exception: " + e.Message);
        }

        if (!ProcessHelper.IsProcessRunning(Game))
        {
            Game = null;
            Console.Write("\rWaiting to connect to the game. Please launch the game if you haven't yet.");

            Thread.Sleep(500);
        }
        else
        {
            Console.Write("\n");
            DiagnosticLog.Information("Connected to FFX!");
        }

        return Game;
    }
}
