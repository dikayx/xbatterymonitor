using System.Windows.Forms;

namespace XBatteryMonitor
{
    static class Program
    {
        static NotifyIcon notifyIcon;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize NotifyIcon (taskbar tray)
            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information, // Replace with a custom .ico file if desired
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };

            // Add context menu items
            notifyIcon.ContextMenuStrip.Items.Add("Settings", null, (s, e) => ShowSettings());
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => Application.Exit());

            // Start the battery monitor service in the background
            BatteryMonitor.Start();

            // Keep the application running with the tray icon
            Application.Run();
        }

        static void ShowSettings()
        {
            // Open settings form
            using (var settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog();
            }
        }
    }
}