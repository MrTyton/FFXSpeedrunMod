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
using FFXCutsceneRemover.Logging;

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
    private NumericUpDown sleepIntervalNumeric;
    private Label sleepIntervalLabel;
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
    private bool isRunning = false;
    private bool autoStartEnabled = false;
    private LogEventLevel currentLogLevel = LogEventLevel.Information;
    private bool previousBreakState = false; // Remember break checkbox state when CSR is disabled

    private static readonly uint[] PCSeeds = new uint[] {
        2804382593, 2807284884, 2810252711, 2813220538, 2816122829, 2819090656,
        2822058483, 2825026310, 2827928601, 2830896428, 2833864255, 2836766546,
        2839734373, 2842702200, 2845670027, 2848572318, 2851540145, 2854507972,
        2857410263, 2860378090, 2863345917, 2866313744, 2869216035, 2872183862,
        2875151689, 2878053980, 2881021807, 2883989634, 2886957461, 2889859752,
        2892827579, 2895795406, 2898697697, 2901665524, 2904633351, 2907601178,
        2910503469, 2913471296, 2916439123, 2919341414, 2922309241, 2925277068,
        2928244895, 2931147186, 2934115013, 2937082840, 2939985131, 2942952958,
        2945920785, 2948888612, 2951790903, 2954758730, 2957726557, 2960628848,
        2963596675, 2966564502, 2969532329, 2972434620, 2975402447, 2978370274,
        2981272565, 2984240392, 2987208219, 2990176046, 2993078337, 2996046164,
        2999013991, 3001916282, 3004884109, 3007851936, 3010819763, 3013722054,
        3016689881, 3019657708, 3022559999, 3025527826, 3028495653, 3031463480,
        3034365771, 3037333598, 3040301425, 3043203716, 3046171543, 3049139370,
        3052107197, 3055009488, 3057977315, 3060945142, 3063847433, 3066815260,
        3069783087, 3072750914, 3075653205, 3078621032, 3081588859, 3084491150,
        3087458977, 3090426804, 3093394631, 3096296922, 3099264749, 3102232576,
        3105134867, 3108102694, 3111070521, 3114038348, 3116940639, 3119908466,
        3122876293, 3125778584, 3128746411, 3131714238, 3134682065, 3137584356,
        3140552183, 3143520010, 3146422301, 3149390128, 3152357955, 3155325782,
        3158228073, 3161195900, 3164163727, 3167066018, 3170033845, 3173001672,
        3175969499, 3178871790, 3181839617, 3184807444, 3187709735, 3190677562,
        3193645389, 3196613216, 3199515507, 3202483334, 3205451161, 3208353452,
        3211321279, 3214289106, 3217256933, 3220159224, 3223127051, 3226094878,
        3228997169, 3231964996, 3234932823, 3237900650, 3240802941, 3243770768,
        3246738595, 3249640886, 3252608713, 3255576540, 3258544367, 3261446658,
        3264414485, 3267382312, 3270284603, 3273252430, 3276220257, 3279188084,
        3282090375, 3285058202, 3288026029, 3290928320, 3293896147, 3296863974,
        3299831801, 3302734092, 3305701919, 3308669746, 3311572037, 3314539864,
        3317507691, 3320475518, 3323377809, 3326345636, 3329313463, 3332215754,
        3335183581, 3338151408, 3341119235, 3344021526, 3346989353, 3349957180,
        3352859471, 3355827298, 3358795125, 3361762952, 3364665243, 3367633070,
        3370600897, 3373503188, 3376471015, 3379438842, 3382406669, 3385308960,
        3388276787, 3391244614, 3394146905, 3397114732, 3400082559, 3403050386,
        3405952677, 3408920504, 3411888331, 3414790622, 3417758449, 3420726276,
        3423694103, 3426596394, 3429564221, 3432532048, 3435434339, 3438402166,
        3441369993, 3444337820, 3447240111
    };

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
        this.Text = "FFX Speedrun Mod v1.7.0";
        this.Size = new Size(800, 600);
        this.MinimumSize = new Size(600, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        // Set application icon
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

        // Initialize ToolTip
        toolTip = new ToolTip
        {
            AutoPopDelay = 10000,
            InitialDelay = 500,
            ReshowDelay = 200,
            ShowAlways = true
        };

        // Settings GroupBox
        settingsGroupBox = new GroupBox
        {
            Text = "Cutscene Remover Settings",
            Location = new Point(10, 10),
            Size = new Size(370, 110),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        // Cutscene Remover checkbox
        csrOnCheckBox = new CheckBox
        {
            Text = "Enable Cutscene Remover",
            Location = new Point(20, 30),
            Size = new Size(200, 20),
            Checked = true
        };
        csrOnCheckBox.CheckedChanged += CsrOnCheckBox_CheckedChanged;
        toolTip.SetToolTip(csrOnCheckBox, "Automatically skip cutscenes during gameplay");

        // CSR Break checkbox
        csrBreakOnCheckBox = new CheckBox
        {
            Text = "Enable Cutscene Remover Break",
            Location = new Point(20, 60),
            Size = new Size(250, 20),
            Checked = false,
            Enabled = true
        };
        toolTip.SetToolTip(csrBreakOnCheckBox, "Take a break during the run after Guadosalam");

        // Add controls to settings group
        settingsGroupBox.Controls.Add(csrOnCheckBox);
        settingsGroupBox.Controls.Add(csrBreakOnCheckBox);

        // RNG GroupBox
        rngGroupBox = new GroupBox
        {
            Text = "RNG Settings",
            Location = new Point(400, 10),
            Size = new Size(370, 110),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        // No RNG Mod radio button (default)
        noRngModRadioButton = new RadioButton
        {
            Text = "No RNG Modification",
            Location = new Point(20, 30),
            Size = new Size(180, 20),
            Checked = true
        };
        noRngModRadioButton.CheckedChanged += RngRadioButton_CheckedChanged;
        toolTip.SetToolTip(noRngModRadioButton, "Use the game's default RNG behavior");

        // True RNG radio button
        trueRngRadioButton = new RadioButton
        {
            Text = "True RNG",
            Location = new Point(20, 55),
            Size = new Size(150, 20),
            Checked = false
        };
        trueRngRadioButton.CheckedChanged += RngRadioButton_CheckedChanged;
        toolTip.SetToolTip(trueRngRadioButton, "Use truly random RNG");

        // Set Seed radio button
        setSeedRadioButton = new RadioButton
        {
            Text = "Set Seed",
            Location = new Point(20, 80),
            Size = new Size(90, 20),
            Checked = false
        };
        setSeedRadioButton.CheckedChanged += RngRadioButton_CheckedChanged;
        toolTip.SetToolTip(setSeedRadioButton, "Use a specific RNG seed");

        // Seed dropdown
        seedLabel = new Label
        {
            Text = "Seed:",
            Location = new Point(120, 82),
            Size = new Size(50, 20),
            Enabled = false
        };

        seedComboBox = new ComboBox
        {
            Location = new Point(170, 80),
            Size = new Size(180, 20),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Enabled = false
        };
        seedComboBox.Items.Add("--- Select Seed ---");
        foreach (var seed in PCSeeds)
        {
            seedComboBox.Items.Add(seed.ToString());
        }
        seedComboBox.SelectedIndex = 0;
        toolTip.SetToolTip(seedComboBox, "Select a specific RNG seed value (only active when 'Set Seed' is selected)");

        // Add controls to RNG group
        rngGroupBox.Controls.Add(noRngModRadioButton);
        rngGroupBox.Controls.Add(trueRngRadioButton);
        rngGroupBox.Controls.Add(setSeedRadioButton);
        rngGroupBox.Controls.Add(seedLabel);
        rngGroupBox.Controls.Add(seedComboBox);

        // Advanced Settings GroupBox
        advancedGroupBox = new GroupBox
        {
            Text = "Advanced Settings",
            Location = new Point(10, 130),
            Size = new Size(370, 140),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        // Sleep interval
        sleepIntervalLabel = new Label
        {
            Text = "Main Thread Sleep Interval (ms):",
            Location = new Point(20, 32),
            Size = new Size(200, 20)
        };

        sleepIntervalNumeric = new NumericUpDown
        {
            Location = new Point(230, 30),
            Size = new Size(80, 20),
            Minimum = 1,
            Maximum = 1000,
            Value = 16
        };
        toolTip.SetToolTip(sleepIntervalNumeric, "How often (in milliseconds) the mod checks game state. Lower = more responsive, higher = less CPU usage");

        // Log level
        logLevelLabel = new Label
        {
            Text = "Log Level:",
            Location = new Point(20, 62),
            Size = new Size(70, 20)
        };

        logLevelComboBox = new ComboBox
        {
            Location = new Point(100, 60),
            Size = new Size(150, 20),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        logLevelComboBox.Items.AddRange(new object[]
        {
            "Information",
            "Warning",
            "Error",
            "Debug",
            "Verbose"
        });
        logLevelComboBox.SelectedIndex = 0; // Default to Information
        logLevelComboBox.SelectedIndexChanged += LogLevelComboBox_SelectedIndexChanged;
        toolTip.SetToolTip(logLevelComboBox,
            "Minimum log level to display:\n" +
            "• Information - Standard operational messages\n" +
            "• Warning - Potential issues that don't stop execution\n" +
            "• Error - Serious problems and failures\n" +
            "• Debug - Detailed diagnostic information\n" +
            "• Verbose - All messages including very detailed trace info");

        // Auto-start checkbox
        autoStartCheckBox = new CheckBox
        {
            Text = "Automatically start mod when FFX starts",
            Location = new Point(20, 90),
            Size = new Size(330, 20),
            Checked = false
        };
        autoStartCheckBox.CheckedChanged += AutoStartCheckBox_CheckedChanged;
        toolTip.SetToolTip(autoStartCheckBox, "When enabled, the mod will automatically start when Final Fantasy X is detected");

        advancedGroupBox.Controls.Add(sleepIntervalLabel);
        advancedGroupBox.Controls.Add(sleepIntervalNumeric);
        advancedGroupBox.Controls.Add(logLevelLabel);
        advancedGroupBox.Controls.Add(logLevelComboBox);
        advancedGroupBox.Controls.Add(autoStartCheckBox);

        // Category GroupBox
        categoryGroupBox = new GroupBox
        {
            Text = "Speedrun Category",
            Location = new Point(390, 130),
            Size = new Size(380, 140),
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };

        categoryLabel = new Label
        {
            Text = "Any%",
            Location = new Point(20, 30),
            Size = new Size(340, 40),
            Font = new Font(this.Font.FontFamily, 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.DarkBlue
        };
        toolTip.SetToolTip(categoryLabel, "The speedrun category based on your current settings");

        categoryRulesLink = new LinkLabel
        {
            Text = "View Category Rules",
            Location = new Point(20, 80),
            Size = new Size(340, 30),
            Font = new Font(this.Font.FontFamily, 10),
            TextAlign = ContentAlignment.MiddleCenter,
            LinkColor = Color.Blue,
            ActiveLinkColor = Color.Red,
            VisitedLinkColor = Color.Purple
        };
        categoryRulesLink.LinkClicked += CategoryRulesLink_LinkClicked;
        toolTip.SetToolTip(categoryRulesLink, "Click to open the speedrun.com rules for this category");

        categoryGroupBox.Controls.Add(categoryLabel);
        categoryGroupBox.Controls.Add(categoryRulesLink);

        // Status GroupBox
        statusGroupBox = new GroupBox
        {
            Text = "Status",
            Location = new Point(10, 280),
            Size = new Size(760, 215),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
        };

        connectionStatusLabel = new Label
        {
            Text = "Connection: Not Connected",
            Location = new Point(20, 30),
            Size = new Size(720, 20),
            ForeColor = Color.Red
        };
        toolTip.SetToolTip(connectionStatusLabel, "Shows whether the mod is connected to the FFX process");

        gameStatusLabel = new Label
        {
            Text = "Game Status: Waiting for FFX",
            Location = new Point(20, 55),
            Size = new Size(720, 20)
        };
        toolTip.SetToolTip(gameStatusLabel, "Shows the current game state and progress");

        modStatusLabel = new Label
        {
            Text = "Mod Status: Stopped",
            Location = new Point(20, 80),
            Size = new Size(720, 20)
        };
        toolTip.SetToolTip(modStatusLabel, "Shows what the mod is currently doing");

        logTextBox = new RichTextBox
        {
            Location = new Point(20, 110),
            Size = new Size(720, 90),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            ReadOnly = true,
            BackColor = Color.Black,
            ForeColor = Color.LightGreen,
            Font = new Font("Consolas", 9)
        };
        toolTip.SetToolTip(logTextBox, "Displays diagnostic messages and mod activity");

        statusGroupBox.Controls.Add(connectionStatusLabel);
        statusGroupBox.Controls.Add(gameStatusLabel);
        statusGroupBox.Controls.Add(modStatusLabel);
        statusGroupBox.Controls.Add(logTextBox);

        // Buttons
        startButton = new Button
        {
            Text = "Start Mod",
            Location = new Point(10, 510),
            Size = new Size(120, 35),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };
        startButton.Click += StartButton_Click;
        toolTip.SetToolTip(startButton, "Start the FFX Speedrun Mod with current settings");

        stopButton = new Button
        {
            Text = "Stop Mod",
            Location = new Point(140, 510),
            Size = new Size(200, 35),
            Enabled = false,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };
        stopButton.Click += StopButton_Click;
        toolTip.SetToolTip(stopButton, "Stop the mod and disconnect from the game");

        // Launch Game button
        launchGameButton = new Button
        {
            Text = "Launch FFX",
            Location = new Point(350, 510),
            Size = new Size(120, 35),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };
        launchGameButton.Click += LaunchGameButton_Click;
        toolTip.SetToolTip(launchGameButton, "Launch Final Fantasy X via Steam");

        // Config buttons
        saveConfigButton = new Button
        {
            Text = "Save Config",
            Location = new Point(520, 510),
            Size = new Size(120, 35),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        saveConfigButton.Click += SaveConfigButton_Click;
        toolTip.SetToolTip(saveConfigButton, "Save current settings to a configuration file");

        loadConfigButton = new Button
        {
            Text = "Load Config",
            Location = new Point(650, 510),
            Size = new Size(120, 35),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        loadConfigButton.Click += LoadConfigButton_Click;
        toolTip.SetToolTip(loadConfigButton, "Load settings from a saved configuration file");

        // Add all controls to form
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
            Interval = 2000 // Check every 2 seconds
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

        sleepIntervalNumeric.Value = config.MtSleepInterval;
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
            MtSleepInterval = (int)sleepIntervalNumeric.Value,
            AutoStart = autoStartCheckBox.Checked,
            SelectedSeed = PCSeeds[0], // Default to first seed
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
        currentLogLevel = logLevelComboBox.SelectedIndex switch
        {
            0 => LogEventLevel.Information,
            1 => LogEventLevel.Warning,
            2 => LogEventLevel.Error,
            3 => LogEventLevel.Debug,
            4 => LogEventLevel.Verbose,
            _ => LogEventLevel.Information
        };
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
            var processes = Process.GetProcessesByName("FFX");
            if (processes.Length > 0)
            {
                // Give FFX a moment to fully initialize before connecting
                // This prevents connecting too early when the game process just started
                var process = processes[0];
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
            game = ConnectToTarget("FFX");
            if (game == null)
            {
                worker.ReportProgress(0, "Waiting for game...");
                Thread.Sleep(1000);
            }
        }

        if (!isRunning || game == null)
        {
            return;
        }

        worker.ReportProgress(1, "Connected to FFX!");

        // Initialize memory watchers
        MemoryWatchers.Initialize(game);
        MemoryWatchers.Watchers.UpdateAll(game);

        // Get language and set up start game indents
        byte language = MemoryWatchers.Language.Current;
        List<byte> startGameIndents = GetStartGameIndents(language);

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

        worker.ReportProgress(2, "Mod running");

        // Main game loop
        bool newGameSetUp = false;
        bool seedInjected = false;

        var BreakTransition = new BreakTransition
        {
            ForceLoad = false,
            Description = "Break Setup",
            ConsoleOutput = false,
            Suspendable = false,
            Repeatable = true
        };

        while (isRunning && !game.HasExited)
        {
            try
            {
                MemoryWatchers.Watchers.UpdateAll(game);

                // New game setup logic
                if (!newGameSetUp && MemoryWatchers.RoomNumber.Current == 0 &&
                    MemoryWatchers.Storyline.Current == 0 && MemoryWatchers.Dialogue1.Current == 6)
                {
                    if (csrConfig.SetSeedOn)
                    {
                        worker.ReportProgress(10, $"Injecting Seed {csrConfig.SelectedSeed}");
                        new Transition
                        {
                            ForceLoad = false,
                            SetSeed = true,
                            SetSeedValue = unchecked((int)csrConfig.SelectedSeed),
                            RoomNumberAlt = (short)Array.IndexOf(PCSeeds, csrConfig.SelectedSeed)
                        }.Execute();
                        seedInjected = true;
                    }

                    MemoryWatchers.Watchers.UpdateAll(game);

                    var startGameText = new List<(string, byte)>
                    {
                        ($"[FFX Speedrunning Mod v1.7.0]", startGameIndents[0]),
                        ($"", startGameIndents[1]),
                        ($"Cutscene Remover: {(csrConfig.CsrOn ? "Enabled" : "Disabled")}", startGameIndents[2]),
                        ($"Cutscene Remover Break: {(csrConfig.CsrBreakOn ? "Enabled" : "Disabled")}", startGameIndents[3]),
                        ($"True RNG: {(csrConfig.TrueRngOn ? "Enabled" : "Disabled")}", startGameIndents[4]),
                        ($"Set Seed: {(MemoryWatchers.RoomNumberAlt.Current != 0 ? PCSeeds[MemoryWatchers.RoomNumberAlt.Current] : "Disabled")}", startGameIndents[5]),
                        ($"", startGameIndents[6]),
                        ($"Start Game?", startGameIndents[7])
                    };

                    new NewGameTransition { ForceLoad = false, ConsoleOutput = false, startGameText = startGameText }.Execute();

                    newGameSetUp = true;
                }

                if (newGameSetUp && MemoryWatchers.RoomNumber.Current == 23)
                {
                    newGameSetUp = false;
                }

                // Break logic
                if (csrConfig.CsrBreakOn && MemoryWatchers.ForceLoad.Current == 0)
                {
                    if (MemoryWatchers.RoomNumber.Current == 140 && MemoryWatchers.Storyline.Current == 1300)
                    {
                        new Transition { RoomNumber = 184, SpawnPoint = 0, Description = "Break" }.Execute();
                    }
                    else if (MemoryWatchers.RoomNumber.Current == 184 && MemoryWatchers.Storyline.Current == 1300)
                    {
                        BreakTransition.Execute();
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

                // Run mod main loops
                if (csrConfig.CsrOn && cutsceneRemover != null)
                {
                    cutsceneRemover.MainLoop();
                }

                if (csrConfig.TrueRngOn && rngMod != null)
                {
                    rngMod.MainLoop();
                }

                Thread.Sleep(csrConfig.MtSleepInterval);
            }
            catch (Exception ex)
            {
                worker.ReportProgress(-1, $"Error: {ex.Message}");
                break;
            }
        }

        worker.ReportProgress(3, "Game exited or mod stopped");
    }

    private List<byte> GetStartGameIndents(byte language)
    {
        switch (language)
        {
            case 0x00: // Japanese
                return new List<byte>() {
                    0x43, 0x00, 0x47, 0x43, 0x4b,
                    csrConfig.SetSeedOn ? (byte)0x48 : (byte)0x4b,
                    0x00, 0x4e
                };
            case 0x09: // Korean
                return new List<byte>() {
                    0x43, 0x00, 0x46, 0x43, 0x4a,
                    csrConfig.SetSeedOn ? (byte)0x48 : (byte)0x4a,
                    0x00, 0x4d
                };
            case 0x0A: // Chinese
                return new List<byte>() {
                    0x43, 0x00, 0x46, 0x43, 0x4a,
                    csrConfig.SetSeedOn ? (byte)0x46 : (byte)0x4a,
                    0x00, 0x4d
                };
            default: // English and others
                return new List<byte>() {
                    0x43, 0x00, 0x45, 0x41, 0x4a,
                    csrConfig.SetSeedOn ? (byte)0x47 : (byte)0x4a,
                    0x00, 0x4d
                };
        }
    }

    private void GameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        string message = e.UserState as string;

        switch (e.ProgressPercentage)
        {
            case 0: // Waiting for game
                connectionStatusLabel.Text = "Connection: " + message;
                connectionStatusLabel.ForeColor = Color.Orange;
                gameStatusLabel.Text = "Game Status: Waiting for FFX";
                stopButton.Text = "Stop Mod";
                break;
            case 1: // Connected
                connectionStatusLabel.Text = "Connection: Connected";
                connectionStatusLabel.ForeColor = Color.Green;
                gameStatusLabel.Text = "Game Status: FFX Running";
                stopButton.Text = "Stop Mod (Close FFX First)";
                LogMessage(message);
                break;
            case 2: // Running
                modStatusLabel.Text = "Mod Status: Running";
                modStatusLabel.ForeColor = Color.Green;
                LogMessage(message);
                break;
            case 3: // Stopped
                gameStatusLabel.Text = "Game Status: " + message;
                stopButton.Text = "Stop Mod";
                break;
            case 10: // Seed injection
                LogMessage(message);
                break;
            case -1: // Error
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
            var processes = Process.GetProcessesByName(targetName);
            return processes.OrderByDescending(x => x.StartTime)
                     .FirstOrDefault(x => !x.HasExited);
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
