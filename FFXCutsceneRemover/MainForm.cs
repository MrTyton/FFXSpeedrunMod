using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Serilog.Events;
using FFXCutsceneRemover.ComponentUtil;
using FFXCutsceneRemover.Constants;
using FFXCutsceneRemover.Logging;
using FFXCutsceneRemover.Models;
using FFXCutsceneRemover.Services;

namespace FFXCutsceneRemover;

public class MainForm : Form
{
    // UI Controls
    private GroupBox settingsGroupBox;
    private CheckBox csrOnCheckBox;
    private CheckBox csrBreakOnCheckBox;
    private GroupBox rngGroupBox;
    private RadioButton trueRngRadioButton;
    private RadioButton setSeedRadioButton;
    private RadioButton noRngModRadioButton;
    private ComboBox seedComboBox;
    private Label seedLabel;
    private GroupBox advancedGroupBox;
    private ComboBox logLevelComboBox;
    private Label logLevelLabel;
    private CheckBox autoStartCheckBox;

    private GroupBox categoryGroupBox;
    private Label categoryLabel;
    private LinkLabel categoryRulesLink;

    private GroupBox statusGroupBox;
    private Label connectionStatusLabel;
    private Label gameStatusLabel;
    private Label modStatusLabel;
    private RichTextBox logTextBox;

    private Button startButton;
    private Button stopButton;
    private Button launchGameButton;
    private Button saveConfigButton;
    private Button loadConfigButton;

    // ToolTip component
    private ToolTip toolTip;

    // Background worker for game monitoring
    private BackgroundWorker gameWorker;
    private CancellationTokenSource cancellationTokenSource;
    private System.Windows.Forms.Timer autoStartTimer;

    // Application state
    private CsrConfig csrConfig;
    private Process game;
    private CutsceneRemover cutsceneRemover;
    private RNGMod rngMod;
    private volatile bool isRunning = false; // Made volatile for thread safety
    private bool autoStartEnabled = false;
    private LogEventLevel currentLogLevel = LogEventLevel.Information;
    private bool previousBreakState = false; // Remember break checkbox state when CSR is disabled

    public MainForm()
    {
        InitializeComponent();
        InitializeCustomComponents();

        // Subscribe to diagnostic log events
        DiagnosticLog.LogMessageReceived += OnDiagnosticLogReceived;

        DiagnosticLog.Information("FFX Speedrun Mod GUI started");
    }

    private void InitializeComponent()
    {
        ConfigureFormProperties();
        SetApplicationIcon();
        InitializeToolTip();
        CreateSettingsGroup();
        CreateRngGroup();
        CreateAdvancedSettingsGroup();
        CreateCategoryGroup();
        CreateStatusGroup();
        CreateButtons();
        AddControlsToForm();
    }

    private void ConfigureFormProperties()
    {
        this.Text = VersionInfo.DisplayName;
        this.Size = new Size(800, 600);
        this.MinimumSize = new Size(600, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
    }

    private void SetApplicationIcon()
    {
        try
        {
            // Try to extract icon from the executing assembly (embedded by ApplicationIcon in .csproj)
            string exePath = Assembly.GetExecutingAssembly().Location;
            if (exePath.EndsWith(".dll"))
            {
                // When running with dotnet run, we get a .dll, look for the .exe
                exePath = exePath.Replace(".dll", ".exe");
            }

            if (File.Exists(exePath))
            {
                this.Icon = Icon.ExtractAssociatedIcon(exePath);
            }
            else
            {
                // Fallback to loading from file
                this.Icon = new Icon("FFXCSR_icon.ico");
            }
        }
        catch (Exception ex)
        {
            // Icon loading failed, continue without icon
            Debug.WriteLine($"Failed to load icon: {ex.Message}");
        }
    }

    private void InitializeToolTip()
    {
        toolTip = new ToolTip
        {
            AutoPopDelay = ConfigurationDefaults.ToolTipAutoPopDelay,
            InitialDelay = ConfigurationDefaults.ToolTipInitialDelay,
            ReshowDelay = ConfigurationDefaults.ToolTipReshowDelay,
            ShowAlways = true
        };
    }

    private void CreateSettingsGroup()
    {
        settingsGroupBox = ControlFactory.CreateGroupBox(
            "Cutscene Remover Settings",
            new Point(10, 10),
            new Size(370, 110))
            .WithAnchor(AnchorStyles.Top | AnchorStyles.Left);

        csrOnCheckBox = ControlFactory.CreateCheckBox(
            "Enable Cutscene Remover",
            new Point(20, 30),
            true,
            CsrOnCheckBox_CheckedChanged)
            .WithSize(200, 20)
            .WithTooltip(toolTip, "Automatically skip cutscenes during gameplay");

        csrBreakOnCheckBox = ControlFactory.CreateCheckBox(
            "Enable Cutscene Remover Break",
            new Point(20, 60),
            false,
            null)
            .WithSize(250, 20)
            .WithEnabled(true)
            .WithTooltip(toolTip, "Take a break during the run after Guadosalam");

        settingsGroupBox.Controls.Add(csrOnCheckBox);
        settingsGroupBox.Controls.Add(csrBreakOnCheckBox);
    }

    private void CreateRngGroup()
    {
        rngGroupBox = ControlFactory.CreateGroupBox(
            "RNG Settings",
            new Point(400, 10),
            new Size(370, 110))
            .WithAnchor(AnchorStyles.Top | AnchorStyles.Left);

        noRngModRadioButton = ControlFactory.CreateRadioButton(
            "No RNG Modification",
            new Point(20, 30),
            new Size(180, 20),
            true,
            RngRadioButton_CheckedChanged)
            .WithTooltip(toolTip, "Use the game's default RNG behavior");

        trueRngRadioButton = ControlFactory.CreateRadioButton(
            "True RNG",
            new Point(20, 55),
            new Size(150, 20),
            false,
            RngRadioButton_CheckedChanged)
            .WithTooltip(toolTip, "Use truly random RNG");

        setSeedRadioButton = ControlFactory.CreateRadioButton(
            "Set Seed",
            new Point(20, 80),
            new Size(90, 20),
            false,
            RngRadioButton_CheckedChanged)
            .WithTooltip(toolTip, "Use a specific RNG seed");

        seedLabel = ControlFactory.CreateLabel(
            "Seed:",
            new Point(120, 82),
            new Size(50, 20))
            .WithEnabled(false);

        var seedItems = new List<object> { "--- Select Seed ---" };
        seedItems.AddRange(GameConstants.PCSeeds.Cast<object>());

        seedComboBox = ControlFactory.CreateComboBox(
            new Point(170, 80),
            new Size(180, 20),
            seedItems.ToArray(),
            0)
            .WithEnabled(false)
            .WithTooltip(toolTip, "Select a specific RNG seed value (only active when 'Set Seed' is selected)");

        rngGroupBox.Controls.Add(noRngModRadioButton);
        rngGroupBox.Controls.Add(trueRngRadioButton);
        rngGroupBox.Controls.Add(setSeedRadioButton);
        rngGroupBox.Controls.Add(seedLabel);
        rngGroupBox.Controls.Add(seedComboBox);
    }

    private void CreateAdvancedSettingsGroup()
    {
        advancedGroupBox = ControlFactory.CreateGroupBox(
            "Advanced Settings",
            new Point(10, 130),
            new Size(370, 140))
            .WithAnchor(AnchorStyles.Top | AnchorStyles.Left);

        logLevelLabel = ControlFactory.CreateLabel(
            "Log Level:",
            new Point(20, 32),
            new Size(70, 20));

        logLevelComboBox = ControlFactory.CreateComboBox(
            new Point(100, 30),
            new Size(150, 20),
            LogLevelHelper.GetDisplayNames(),
            -1);
        logLevelComboBox.SelectedItem = LogLevelHelper.ToDisplayName(LogEventLevel.Information);
        logLevelComboBox.SelectedIndexChanged += LogLevelComboBox_SelectedIndexChanged;
        logLevelComboBox.WithTooltip(toolTip,
            "Minimum log level to display:\n" +
            "• Information - Standard operational messages\n" +
            "• Warning - Potential issues that don't stop execution\n" +
            "• Error - Serious problems and failures\n" +
            "• Debug - Detailed diagnostic information\n" +
            "• Verbose - All messages including very detailed trace info");

        autoStartCheckBox = ControlFactory.CreateCheckBox(
            "Automatically start mod when FFX starts",
            new Point(20, 60),
            false,
            AutoStartCheckBox_CheckedChanged)
            .WithSize(330, 20)
            .WithTooltip(toolTip, "When enabled, the mod will automatically start when Final Fantasy X is detected");

        advancedGroupBox.Controls.Add(logLevelLabel);
        advancedGroupBox.Controls.Add(logLevelComboBox);
        advancedGroupBox.Controls.Add(autoStartCheckBox);
    }

    private void CreateCategoryGroup()
    {
        categoryGroupBox = ControlFactory.CreateGroupBox(
            "Speedrun Category",
            new Point(390, 130),
            new Size(380, 140))
            .WithAnchor(AnchorStyles.Top | AnchorStyles.Left);

        categoryLabel = ControlFactory.CreateLabel(
            "Any%",
            new Point(20, 30),
            new Size(340, 40))
            .WithFont(new Font(this.Font.FontFamily, 14, FontStyle.Bold))
            .WithTextAlign(ContentAlignment.MiddleCenter)
            .WithForeColor(Color.DarkBlue)
            .WithTooltip(toolTip, "The speedrun category based on your current settings");

        categoryRulesLink = ControlFactory.CreateLinkLabel(
            "View Category Rules",
            new Point(20, 80),
            new Size(340, 30),
            CategoryRulesLink_LinkClicked)
            .WithFont(new Font(this.Font.FontFamily, 10))
            .WithTextAlign(ContentAlignment.MiddleCenter)
            .WithLinkColors(Color.Blue, Color.Red, Color.Purple)
            .WithTooltip(toolTip, "Click to open the speedrun.com rules for this category");

        categoryGroupBox.Controls.Add(categoryLabel);
        categoryGroupBox.Controls.Add(categoryRulesLink);
    }

    private void CreateStatusGroup()
    {
        statusGroupBox = ControlFactory.CreateGroupBox(
            "Status",
            new Point(10, 280),
            new Size(760, 215))
            .WithAnchor(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);

        connectionStatusLabel = ControlFactory.CreateLabel(
            "Connection: Not Connected",
            new Point(20, 30),
            new Size(720, 20))
            .WithForeColor(Color.Red)
            .WithTooltip(toolTip, "Shows whether the mod is connected to the FFX process");

        gameStatusLabel = ControlFactory.CreateLabel(
            "Game Status: Waiting for FFX",
            new Point(20, 55),
            new Size(720, 20))
            .WithTooltip(toolTip, "Shows the current game state and progress");

        modStatusLabel = ControlFactory.CreateLabel(
            "Mod Status: Stopped",
            new Point(20, 80),
            new Size(720, 20))
            .WithTooltip(toolTip, "Shows what the mod is currently doing");

        logTextBox = ControlFactory.CreateRichTextBox(
            new Point(20, 110),
            new Size(720, 90),
            true)
            .WithAnchor(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom)
            .WithBackColor(Color.Black)
            .WithForeColor(Color.LightGreen)
            .WithFont(new Font("Consolas", 9))
            .WithTooltip(toolTip, "Displays diagnostic messages and mod activity");

        statusGroupBox.Controls.Add(connectionStatusLabel);
        statusGroupBox.Controls.Add(gameStatusLabel);
        statusGroupBox.Controls.Add(modStatusLabel);
        statusGroupBox.Controls.Add(logTextBox);
    }

    private void CreateButtons()
    {
        startButton = ControlFactory.CreateButton(
            "Start Mod",
            new Point(10, 510),
            new Size(120, 35),
            StartButton_Click)
            .WithAnchor(AnchorStyles.Bottom | AnchorStyles.Left)
            .WithTooltip(toolTip, "Start the FFX Speedrun Mod with current settings");

        stopButton = ControlFactory.CreateButton(
            "Stop Mod",
            new Point(140, 510),
            new Size(200, 35),
            StopButton_Click)
            .WithEnabled(false)
            .WithAnchor(AnchorStyles.Bottom | AnchorStyles.Left)
            .WithTooltip(toolTip, "Stop the mod and disconnect from the game");

        launchGameButton = ControlFactory.CreateButton(
            "Launch FFX",
            new Point(350, 510),
            new Size(120, 35),
            LaunchGameButton_Click)
            .WithAnchor(AnchorStyles.Bottom | AnchorStyles.Left)
            .WithTooltip(toolTip, "Launch Final Fantasy X via Steam");

        saveConfigButton = ControlFactory.CreateButton(
            "Save Config",
            new Point(520, 510),
            new Size(120, 35),
            SaveConfigButton_Click)
            .WithAnchor(AnchorStyles.Bottom | AnchorStyles.Right)
            .WithTooltip(toolTip, "Save current settings to a configuration file");

        loadConfigButton = ControlFactory.CreateButton(
            "Load Config",
            new Point(650, 510),
            new Size(120, 35),
            LoadConfigButton_Click)
            .WithAnchor(AnchorStyles.Bottom | AnchorStyles.Right)
            .WithTooltip(toolTip, "Load settings from a saved configuration file");
    }

    private void AddControlsToForm()
    {
        this.Controls.Add(settingsGroupBox);
        this.Controls.Add(rngGroupBox);
        this.Controls.Add(advancedGroupBox);
        this.Controls.Add(categoryGroupBox);
        this.Controls.Add(statusGroupBox);
        this.Controls.Add(startButton);
        this.Controls.Add(stopButton);
        this.Controls.Add(launchGameButton);
        this.Controls.Add(saveConfigButton);
        this.Controls.Add(loadConfigButton);
    }

    private void InitializeCustomComponents()
    {
        // Set up background worker
        gameWorker = new BackgroundWorker
        {
            WorkerSupportsCancellation = true,
            WorkerReportsProgress = true
        };
        gameWorker.DoWork += GameWorker_DoWork;
        gameWorker.ProgressChanged += GameWorker_ProgressChanged;
        gameWorker.RunWorkerCompleted += GameWorker_RunWorkerCompleted;

        // Set up auto-start timer
        autoStartTimer = new System.Windows.Forms.Timer
        {
            Interval = ConfigurationDefaults.AutoStartCheckInterval // Check every 2 seconds
        };
        autoStartTimer.Tick += AutoStartTimer_Tick;
        autoStartTimer.Start(); // Start monitoring immediately

        // Try to load default configuration
        LoadDefaultConfig();

        // Update category display
        UpdateCategory();
    }

    private void LoadDefaultConfig()
    {
        if (ConfigManager.ConfigExists("default.conf"))
        {
            var config = ConfigManager.LoadConfig("default.conf");
            if (config != null)
            {
                ApplyConfigToUI(config);
                LogMessage("Loaded default.conf configuration");
            }
        }
    }

    private void ApplyConfigToUI(CsrConfig config)
    {
        csrOnCheckBox.Checked = config.CsrOn;
        csrBreakOnCheckBox.Checked = config.CsrBreakOn;

        if (config.TrueRngOn)
        {
            trueRngRadioButton.Checked = true;
        }
        else if (config.SetSeedOn)
        {
            setSeedRadioButton.Checked = true;

            // Try to find and select the seed in the combo box
            if (config.SelectedSeed > 0)
            {
                string seedStr = config.SelectedSeed.ToString();
                int index = seedComboBox.Items.IndexOf(seedStr);
                if (index >= 0)
                {
                    seedComboBox.SelectedIndex = index;
                }
            }
        }
        else
        {
            noRngModRadioButton.Checked = true;
        }

        autoStartCheckBox.Checked = config.AutoStart;

        // Update category display
        UpdateCategory();
    }

    private CsrConfig GetConfigFromUI()
    {
        var config = new CsrConfig
        {
            CsrOn = csrOnCheckBox.Checked,
            CsrBreakOn = csrBreakOnCheckBox.Checked,
            TrueRngOn = trueRngRadioButton.Checked,
            SetSeedOn = setSeedRadioButton.Checked,
            MtSleepInterval = ConfigurationDefaults.DefaultSleepInterval,
            AutoStart = autoStartCheckBox.Checked,
            SelectedSeed = GameConstants.PCSeeds[0], // Default to first seed
            FfxExecutablePath = csrConfig?.FfxExecutablePath // Preserve the saved path
        };

        // Index 0 is the placeholder "--- Select Seed ---", real seeds start at index 1
        if (config.SetSeedOn && seedComboBox.SelectedIndex > 0)
        {
            config.SelectedSeed = uint.Parse(seedComboBox.Items[seedComboBox.SelectedIndex].ToString());
        }

        return config;
    }

    private void UpdateCategory()
    {
        bool csrEnabled = csrOnCheckBox.Checked;
        bool trueRng = trueRngRadioButton.Checked;
        bool setSeed = setSeedRadioButton.Checked;
        bool noRng = noRngModRadioButton.Checked;

        string category;
        string rulesUrl;

        if (csrEnabled && trueRng)
        {
            category = "CSR True RNG";
            rulesUrl = "https://www.speedrun.com/ffx?h=Cutscene_Remover-true-rng&rules=category&x=7dg14oxd-p85d1kvl.lr3edv2l";
        }
        else if (csrEnabled && (noRng || setSeed))
        {
            category = "CSR Any%";
            rulesUrl = "https://www.speedrun.com/ffx?h=Cutscene_Remover-any&rules=category&x=7dg14oxd-p85d1kvl.21dpoyp1";
        }
        else if (!csrEnabled && (noRng || setSeed))
        {
            category = "Any%";
            rulesUrl = "https://www.speedrun.com/ffx?h=PC-Any&rules=category&x=zdnqrzqd-ylqx078r.0q5vovnl";
        }
        else
        {
            category = "Unknown";
            rulesUrl = "";
        }

        categoryLabel.Text = category;
        categoryRulesLink.Tag = rulesUrl; // Store URL in Tag property
        categoryRulesLink.Enabled = !string.IsNullOrEmpty(rulesUrl);
    }

    private void CategoryRulesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string url = categoryRulesLink.Tag as string;
        if (!string.IsNullOrEmpty(url))
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void CsrOnCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        csrBreakOnCheckBox.Enabled = csrOnCheckBox.Checked;
        if (!csrOnCheckBox.Checked)
        {
            // Remember the current state before unchecking
            previousBreakState = csrBreakOnCheckBox.Checked;
            csrBreakOnCheckBox.Checked = false;
        }
        else
        {
            // Restore the previous state when re-enabling
            csrBreakOnCheckBox.Checked = previousBreakState;
        }
        UpdateCategory();
    }

    private void RngRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        // Enable seed selection only when Set Seed is selected
        bool isSeedSelected = setSeedRadioButton.Checked;
        seedComboBox.Enabled = isSeedSelected;
        seedLabel.Enabled = isSeedSelected;
        UpdateCategory();
    }

    private void LogLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentLogLevel = LogLevelHelper.FromDisplayName(logLevelComboBox.SelectedItem?.ToString() ?? "Information");
    }

    private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        autoStartEnabled = autoStartCheckBox.Checked;
    }

    private void AutoStartTimer_Tick(object sender, EventArgs e)
    {
        // Only check if auto-start is enabled and mod is not already running
        if (autoStartEnabled && !isRunning)
        {
            // Check if FFX process exists
            var process = ProcessHelper.FindGameProcess(GameConstants.ProcessName);
            if (process != null)
            {
                // Give FFX a moment to fully initialize before connecting
                // This prevents connecting too early when the game process just started
                try
                {
                    // Check if the process has been running for at least 2 seconds
                    // This ensures the game is initialized enough for memory access
                    var uptime = DateTime.Now - process.StartTime;
                    if (uptime.TotalSeconds >= 2)
                    {
                        // FFX is running and initialized, start the mod
                        LogMessage("FFX detected! Auto-starting mod...");
                        StartButton_Click(this, EventArgs.Empty);
                    }
                }
                catch
                {
                    // If we can't access StartTime, the process might not be ready yet
                    // Just skip this tick and try again next time
                }
            }
        }
    }

    private void SaveConfigButton_Click(object sender, EventArgs e)
    {
        using (var dialog = new SaveFileDialog())
        {
            dialog.Filter = "Configuration files (*.conf)|*.conf|All files (*.*)|*.*";
            dialog.DefaultExt = "conf";
            dialog.FileName = "default.conf";
            dialog.InitialDirectory = ConfigManager.GetConfigDirectory();
            dialog.Title = "Save Configuration";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var config = GetConfigFromUI();
                    string filename = Path.GetFileName(dialog.FileName);
                    ConfigManager.SaveConfig(config, filename);

                    LogMessage($"Configuration saved: {filename}");
                    MessageBox.Show($"Configuration saved successfully to:\n{dialog.FileName}",
                        "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    LogMessage($"Error saving configuration: {ex.Message}");
                    MessageBox.Show($"Failed to save configuration:\n{ex.Message}",
                        "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void LoadConfigButton_Click(object sender, EventArgs e)
    {
        using (var dialog = new OpenFileDialog())
        {
            dialog.Filter = "Configuration files (*.conf)|*.conf|All files (*.*)|*.*";
            dialog.DefaultExt = "conf";
            dialog.InitialDirectory = ConfigManager.GetConfigDirectory();
            dialog.Title = "Load Configuration";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filename = Path.GetFileName(dialog.FileName);
                    var config = ConfigManager.LoadConfig(filename);

                    if (config != null)
                    {
                        ApplyConfigToUI(config);
                        LogMessage($"Configuration loaded: {filename}");
                        MessageBox.Show($"Configuration loaded successfully from:\n{dialog.FileName}",
                            "Load Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        LogMessage($"Failed to load configuration: {filename}");
                        MessageBox.Show($"Failed to load configuration from:\n{dialog.FileName}",
                            "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Error loading configuration: {ex.Message}");
                    MessageBox.Show($"Failed to load configuration:\n{ex.Message}",
                        "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    private void LaunchGameButton_Click(object sender, EventArgs e)
    {
        try
        {
            string ffxPath = null;

            // First, check if we have a saved path in the config
            if (csrConfig != null && !string.IsNullOrEmpty(csrConfig.FfxExecutablePath) && File.Exists(csrConfig.FfxExecutablePath))
            {
                ffxPath = csrConfig.FfxExecutablePath;
            }
            else
            {
                // Try common Steam library locations for FFX executable
                string[] possiblePaths = new[]
                {
                    @"C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\FFX.exe",
                    @"C:\Program Files\Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\FFX.exe",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\FFX.exe"),
                    @"D:\SteamLibrary\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\FFX.exe",
                    @"E:\SteamLibrary\steamapps\common\FINAL FANTASY FFX&FFX-2 HD Remaster\FFX.exe"
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        ffxPath = path;
                        break;
                    }
                }
            }

            // If we still haven't found it, prompt the user to locate it
            if (ffxPath == null)
            {
                var result = MessageBox.Show(
                    "Could not find FFX.exe in common Steam library locations.\n\n" +
                    "Would you like to browse for the FFX.exe file?",
                    "Game Not Found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (var openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Title = "Locate FFX.exe";
                        openFileDialog.Filter = "FFX Executable|FFX.exe|All Executables|*.exe";
                        openFileDialog.InitialDirectory = @"C:\Program Files (x86)\Steam\steamapps\common";

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            ffxPath = openFileDialog.FileName;

                            // Save the path by updating csrConfig and saving it
                            csrConfig = GetConfigFromUI();
                            csrConfig.FfxExecutablePath = ffxPath;

                            // Try to save to a default config file
                            try
                            {
                                ConfigManager.SaveConfig(csrConfig, "default.conf");
                                LogMessage($"FFX path saved to configuration: {ffxPath}");
                            }
                            catch (Exception saveEx)
                            {
                                LogMessage($"Warning: Could not save FFX path to config: {saveEx.Message}");
                            }
                        }
                    }
                }
            }

            // Launch the game if we have a valid path
            if (ffxPath != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = ffxPath,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(ffxPath)
                });
                LogMessage($"Launching Final Fantasy X from: {ffxPath}");
            }
            else
            {
                LogMessage("FFX launch cancelled by user");
            }
        }
        catch (Exception ex)
        {
            LogMessage($"Failed to launch FFX: {ex.Message}");
            MessageBox.Show($"Failed to launch Final Fantasy X:\n{ex.Message}",
                "Launch Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        // Create config from UI using helper method
        csrConfig = GetConfigFromUI();

        // Disable settings while running
        settingsGroupBox.Enabled = false;
        rngGroupBox.Enabled = false;
        advancedGroupBox.Enabled = false;
        startButton.Enabled = false;
        stopButton.Enabled = false;
        saveConfigButton.Enabled = false;
        loadConfigButton.Enabled = false;

        isRunning = true;
        cancellationTokenSource = new CancellationTokenSource();

        LogMessage("Starting FFX Speedrun Mod...");
        LogMessage($"CSR: {csrConfig.CsrOn}, CSR Break: {csrConfig.CsrBreakOn}");
        LogMessage($"True RNG: {csrConfig.TrueRngOn}, Set Seed: {csrConfig.SetSeedOn}");
        if (csrConfig.SetSeedOn)
        {
            LogMessage($"Selected Seed: {csrConfig.SelectedSeed}");
        }

        modStatusLabel.Text = "Mod Status: Starting...";
        modStatusLabel.ForeColor = Color.Orange;

        // Start background worker
        gameWorker.RunWorkerAsync();
    }

    private void StopButton_Click(object sender, EventArgs e)
    {
        StopMod();
    }

    private void StopMod()
    {
        isRunning = false;
        cancellationTokenSource?.Cancel();

        LogMessage("Stopping mod...");
        modStatusLabel.Text = "Mod Status: Stopped";
        modStatusLabel.ForeColor = Color.Gray;

        settingsGroupBox.Enabled = true;
        rngGroupBox.Enabled = true;
        advancedGroupBox.Enabled = true;
        startButton.Enabled = true;
        stopButton.Enabled = false;
        saveConfigButton.Enabled = true;
        loadConfigButton.Enabled = true;
    }

    private void GameWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        var worker = sender as BackgroundWorker;

        // Try to connect to game
        while (isRunning && game == null)
        {
            game = ConnectToTarget(GameConstants.ProcessName);
            if (game == null)
            {
                worker.ReportProgress((int)WorkerProgressStatus.WaitingForGame, GameConstants.FormText.ConnectionWaiting);
                Thread.Sleep(ConfigurationDefaults.ProcessSearchDelay);
            }
        }

        if (!isRunning || game == null)
        {
            return;
        }

        worker.ReportProgress((int)WorkerProgressStatus.Connected, GameConstants.FormText.ConnectionConnected);

        // Initialize memory watchers
        MemoryWatchers.Initialize(game);
        MemoryWatchers.Watchers.UpdateAll(game);

        // Get language and set up start game indents
        byte language = MemoryWatchers.Language.Current;
        List<byte> startGameIndents = StartGameIndents.GetIndents(language, csrConfig.SetSeedOn);

        // Initialize components based on config
        if (csrConfig.CsrOn)
        {
            cutsceneRemover = new CutsceneRemover(csrConfig.MtSleepInterval);
            cutsceneRemover.Game = game;
        }

        if (csrConfig.TrueRngOn)
        {
            rngMod = new RNGMod();
            rngMod.Game = game;
        }

        worker.ReportProgress((int)WorkerProgressStatus.Running, "Mod running");

        // Execute the game loop using the shared service
        var gameLoopService = new GameLoopService(
            game,
            csrConfig,
            cutsceneRemover,
            rngMod,
            message => worker.ReportProgress((int)WorkerProgressStatus.SeedInjection, message),
            () => isRunning
        );

        try
        {
            gameLoopService.Execute(startGameIndents, cancellationTokenSource?.Token ?? CancellationToken.None);
        }
        catch (Exception ex)
        {
            worker.ReportProgress((int)WorkerProgressStatus.Error, $"Error: {ex.Message}");
        }

        worker.ReportProgress((int)WorkerProgressStatus.Stopped, "Game exited or mod stopped");
    }

    private void GameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        string message = e.UserState as string;

        switch ((WorkerProgressStatus)e.ProgressPercentage)
        {
            case WorkerProgressStatus.WaitingForGame:
                connectionStatusLabel.Text = "Connection: " + message;
                connectionStatusLabel.ForeColor = Color.Orange;
                gameStatusLabel.Text = "Game Status: Waiting for FFX";
                stopButton.Text = "Stop Mod";
                break;
            case WorkerProgressStatus.Connected:
                connectionStatusLabel.Text = "Connection: Connected";
                connectionStatusLabel.ForeColor = Color.Green;
                gameStatusLabel.Text = "Game Status: FFX Running";
                stopButton.Text = "Stop Mod (Close FFX First)";
                LogMessage(message);
                break;
            case WorkerProgressStatus.Running:
                modStatusLabel.Text = "Mod Status: Running";
                modStatusLabel.ForeColor = Color.Green;
                LogMessage(message);
                break;
            case WorkerProgressStatus.Stopped:
                gameStatusLabel.Text = "Game Status: " + message;
                stopButton.Text = "Stop Mod";
                break;
            case WorkerProgressStatus.SeedInjection:
                LogMessage(message);
                break;
            case WorkerProgressStatus.Error:
                LogMessage(message);
                break;
        }
    }

    private void GameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            LogMessage($"Error: {e.Error.Message}");
            MessageBox.Show($"An error occurred: {e.Error.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        game = null;
        cutsceneRemover = null;
        rngMod = null;

        connectionStatusLabel.Text = "Connection: Disconnected";
        connectionStatusLabel.ForeColor = Color.Red;

        if (isRunning)
        {
            StopMod();
        }
    }

    private Process ConnectToTarget(string targetName)
    {
        try
        {
            return ProcessHelper.FindGameProcess(targetName);
        }
        catch (Exception ex)
        {
            DiagnosticLog.Information($"Exception connecting to game: {ex.Message}");
            return null;
        }
    }

    private void LogMessage(string message, Color? color = null)
    {
        if (logTextBox.InvokeRequired)
        {
            logTextBox.Invoke(new Action<string, Color?>(LogMessage), message, color);
            return;
        }

        string timestamp = DateTime.Now.ToString("HH:mm:ss");

        // Save current selection
        int selectionStart = logTextBox.SelectionStart;
        int selectionLength = logTextBox.SelectionLength;

        // Move to end and add new text
        logTextBox.SelectionStart = logTextBox.TextLength;
        logTextBox.SelectionLength = 0;

        // Set color for the message
        logTextBox.SelectionColor = color ?? Color.LightGreen;
        logTextBox.AppendText($"[{timestamp}] {message}\n");

        // Restore selection
        logTextBox.SelectionStart = selectionStart;
        logTextBox.SelectionLength = selectionLength;
        logTextBox.SelectionColor = logTextBox.ForeColor;

        // Scroll to bottom
        logTextBox.SelectionStart = logTextBox.TextLength;
        logTextBox.ScrollToCaret();
    }

    private void OnDiagnosticLogReceived(string message, LogEventLevel level)
    {
        // Filter messages based on selected log level
        // Only show messages at or above the current log level
        if (level < currentLogLevel)
        {
            return;
        }

        Color color = level switch
        {
            LogEventLevel.Fatal => Color.Red,
            LogEventLevel.Error => Color.OrangeRed,
            LogEventLevel.Warning => Color.Yellow,
            LogEventLevel.Information => Color.LightGreen,
            LogEventLevel.Debug => Color.Cyan,
            LogEventLevel.Verbose => Color.Gray,
            _ => Color.LightGreen
        };

        LogMessage(message, color);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (isRunning)
        {
            var result = MessageBox.Show(
                "The mod is still running. Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            StopMod();
            Thread.Sleep(500); // Give time for cleanup
        }

        // Unsubscribe from log events
        DiagnosticLog.LogMessageReceived -= OnDiagnosticLogReceived;

        base.OnFormClosing(e);
    }
}
