using System;
using System.Windows.Forms;
using FFXCutsceneRemover.Logging;

namespace FFXCutsceneRemover;

public class ProgramGUI
{
    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
