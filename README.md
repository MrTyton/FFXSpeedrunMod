# FFX Speedrun Mod

A powerful tool for speedrunning the Windows Steam version of Final Fantasy X HD Remaster. This mod provides cutscene removal, RNG control, and various quality-of-life features designed specifically for speedrunners.

If you encounter any bugs or have any questions, please open an issue on GitHub or contact us on the [FFX Speedrun Discord](https://discord.gg/X3qXHWG).

## Features

- **Cutscene Removal**: Automatically skip cutscenes and dialog during speedruns
- **True RNG**: Make RNG manipulation impossible for True RNG category runs
- **Set Seed**: Inject specific RNG seeds for PC Any% or CSR Any% categories
- **Auto-Start**: Automatically start the mod when FFX is detected
- **Launch Game**: Launch FFX directly from the mod interface
- **Category Selection**: Quick access to speedrun category rules
- **Real-time Logging**: Monitor mod activity with configurable log levels

## Important Notes

* **Speedrun Use Only**: This mod is designed and tested exclusively for speedruns. Using it for normal playthroughs is not supported and may cause game-breaking bugs. If you choose to use it for casual play, manually save regularly and don't rely on auto-saves.

* **Platform Support**: Currently only works on Windows using the Steam release of FFX.

## Installation & Usage

### Getting Started

1. Download your preferred version from the [releases page](https://github.com/HannibalSnekter/FFXSpeedrunMod/releases)
2. Extract the files to a folder of your choice
3. Launch `FFXSpeedrunMod.exe`
4. Configure your settings using the GUI or CLI
5. Launch Final Fantasy X (or use the "Launch Game" button in the GUI)

The order in which you launch the mod and the game does not matter.

### GUI Mode (Default)

The mod features a user-friendly graphical interface:

- **Cutscene Remover**: Enable/disable automatic cutscene skipping
- **RNG Settings**: Choose between True RNG or Set Seed
- **Advanced Settings**: Configure log levels and auto-start behavior
- **Category Selection**: Quick access to speedrun rules and categories
- **Launch Game**: Start FFX directly from the mod

### CLI Mode

For command-line usage, run `FFXSpeedrunMod.exe` with the `--cli` flag and follow the prompts.

## Features Explained

### Cutscene Removal

You don't need to start a new game for cutscene skipping to work. However, if you start from an existing save file, it may take a moment before cutscenes begin skipping automatically.

### True RNG

The True RNG setting makes RNG manipulation impossible by continuously randomizing the game's RNG state.

- **For Speedruns**: Required for the True RNG category
- **For Normal Gameplay**: Makes no perceivable difference

### Set Seed

Inject one of the 256 naturally obtainable RNG seeds into memory when selecting a new game.

- **For Speedruns**: Required for PC Any% or CSR Any% categories
- **Valid Seeds**: Must be one of the 256 naturally obtainable seeds available on PC

Details about valid seed IDs can be found at: https://grayfox96.github.io/FFX-Info/rng/hd-seeds

## Building from Source

Requires .NET 8.0 SDK:

```bash
git clone https://github.com/MrTyton/FFXSpeedrunMod.git
cd FFXSpeedrunMod
dotnet build
```

## Contributing

Contributions are welcome! Please open an issue or pull request on GitHub.

## Credits

Original development by erickt420 and the FFX speedrunning community.

## License

This project is open source. See LICENSE file for details.
