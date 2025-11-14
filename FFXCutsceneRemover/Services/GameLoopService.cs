using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;
using FFXCutsceneRemover.Logging;

namespace FFXCutsceneRemover.Services;

/// <summary>
/// Shared game loop logic used by both GUI and CLI modes.
/// </summary>
internal class GameLoopService
{
    private readonly Process game;
    private readonly CsrConfig config;
    private readonly CutsceneRemover cutsceneRemover;
    private readonly RNGMod rngMod;
    private readonly Action<string> logMessage;
    private readonly Func<bool> shouldContinue;

    private bool newGameSetUp = false;

    public GameLoopService(
        Process game,
        CsrConfig config,
        CutsceneRemover cutsceneRemover,
        RNGMod rngMod,
        Action<string> logMessage,
        Func<bool> shouldContinue)
    {
        this.game = game ?? throw new ArgumentNullException(nameof(game));
        this.config = config ?? throw new ArgumentNullException(nameof(config));
        this.cutsceneRemover = cutsceneRemover;
        this.rngMod = rngMod;
        this.logMessage = logMessage ?? (msg => { });
        this.shouldContinue = shouldContinue ?? (() => true);
    }

    public void Execute(List<byte> startGameIndents, CancellationToken cancellationToken = default)
    {
        var breakTransition = new BreakTransition
        {
            ForceLoad = false,
            Description = "Break Setup",
            ConsoleOutput = false,
            Suspendable = false,
            Repeatable = true
        };

        while (shouldContinue() && !game.HasExited && !cancellationToken.IsCancellationRequested)
        {
            MemoryWatchers.Watchers.UpdateAll(game);

            // Handle new game setup
            HandleNewGameSetup(startGameIndents);

            if (newGameSetUp && MemoryWatchers.RoomNumber.Current == 23)
            {
                newGameSetUp = false;
            }

            // Handle break logic
            HandleBreakLogic(breakTransition);

            // Run mod main loops
            if (config.CsrOn && cutsceneRemover != null)
            {
                cutsceneRemover.MainLoop();
            }

            if (config.TrueRngOn && rngMod != null)
            {
                rngMod.MainLoop();
            }

            // Wait with cancellation support
            if (cancellationToken != default)
            {
                try
                {
                    cancellationToken.WaitHandle.WaitOne(config.MtSleepInterval);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
            else
            {
                Thread.Sleep(config.MtSleepInterval);
            }
        }
    }

    private void HandleNewGameSetup(List<byte> startGameIndents)
    {
        if (!newGameSetUp &&
            MemoryWatchers.RoomNumber.Current == 0 &&
            MemoryWatchers.Storyline.Current == 0 &&
            MemoryWatchers.Dialogue1.Current == 6)
        {
            // Inject seed if configured
            if (config.SetSeedOn)
            {
                logMessage($"Injecting Seed {config.SelectedSeed}");
                new Transition
                {
                    ForceLoad = false,
                    SetSeed = true,
                    SetSeedValue = unchecked((int)config.SelectedSeed),
                    RoomNumberAlt = (short)(Array.IndexOf(GameConstants.PCSeeds, config.SelectedSeed) + 1)
                }.Execute();
            }

            MemoryWatchers.Watchers.UpdateAll(game);

            // Build start game text
            var startGameText = new List<(string, byte)>
            {
                (VersionInfo.BracketedName, startGameIndents[0]),
                ($"", startGameIndents[1]),
                ($"Cutscene Remover: {(config.CsrOn ? "Enabled" : "Disabled")}", startGameIndents[2]),
                ($"Cutscene Remover Break: {(config.CsrBreakOn ? "Enabled" : "Disabled")}", startGameIndents[3]),
                ($"True RNG: {(config.TrueRngOn ? "Enabled" : "Disabled")}", startGameIndents[4]),
                ($"Set Seed: {(MemoryWatchers.RoomNumberAlt.Current != 0 ? GameConstants.PCSeeds[MemoryWatchers.RoomNumberAlt.Current - 1] : "Disabled")}", startGameIndents[5]),
                ($"", startGameIndents[6]),
                ($"Start Game?", startGameIndents[7])
            };

            new NewGameTransition
            {
                ForceLoad = false,
                ConsoleOutput = false,
                startGameText = startGameText
            }.Execute();

            newGameSetUp = true;
        }
    }

    private void HandleBreakLogic(BreakTransition breakTransition)
    {
        if (config.CsrBreakOn && MemoryWatchers.ForceLoad.Current == 0)
        {
            if (MemoryWatchers.RoomNumber.Current == 140 && MemoryWatchers.Storyline.Current == 1300)
            {
                new Transition { RoomNumber = 184, SpawnPoint = 0, Description = "Break" }.Execute();
            }
            else if (MemoryWatchers.RoomNumber.Current == 184 && MemoryWatchers.Storyline.Current == 1300)
            {
                breakTransition.Execute();
            }
            else if (MemoryWatchers.RoomNumber.Current == 158 && MemoryWatchers.Storyline.Current == 1300)
            {
                new Transition { RoomNumber = 140, Storyline = 1310, SpawnPoint = 0, Description = "End of Break + Map + Rikku afraid + tutorial" }.Execute();
            }
        }
        else
        {
            if (MemoryWatchers.RoomNumber.Current == 140 && MemoryWatchers.Storyline.Current == 1300)
            {
                new Transition { RoomNumber = 140, Storyline = 1310, SpawnPoint = 0, Description = "End of Break + Map + Rikku afraid + tutorial" }.Execute();
            }
        }
    }
}
