using System.Text.RegularExpressions;
using Windows.Gaming.Input;
using Timer = System.Threading.Timer;

namespace XBatteryMonitor
{
    static class Program
    {
        static NotifyIcon? notifyIcon;
        static ToolStripMenuItem? statusMenuItem;
        private static Timer? updateCheckTimer;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            notifyIcon = new NotifyIcon
            {
                Icon = Resources.Icon,
                Visible = true,
                ContextMenuStrip = CreateContextMenu(),
                Text = "Checking status..."
            };

            BatteryMonitor.Start(UpdateTrayTooltip);

            // Handle gamepad connection events
            Gamepad.GamepadAdded += (s, e) =>
            {
                UpdateTrayTooltip("Controller added. Checking status...");
                BatteryMonitor.Start(UpdateTrayTooltip);
            };

            Gamepad.GamepadRemoved += (s, e) =>
            {
                UpdateTrayTooltip("Controller removed. Waiting for connection...");
            };

            // Start periodic update checks if enabled.
            if (Properties.Settings.Default.CheckForUpdatesRegularly)
            {
                StartUpdateChecks();
            }

            Application.Run();
        }

        static void StartUpdateChecks()
        {
            updateCheckTimer = new Timer(async state => await CheckForUpdates(), null, TimeSpan.Zero, TimeSpan.FromHours(24));
        }

        static async Task CheckForUpdates()
        {
            try
            {
                var updater = new Updater();
                if (await updater.IsUpdateAvailable())
                {
                    var result = MessageBox.Show(
                        "A new update is available. Would you like to install it?",
                        "Update Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        await updater.DownloadAndInstallUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static ContextMenuStrip CreateContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            statusMenuItem = new ToolStripMenuItem("Checking status...") { Enabled = false };
            contextMenu.Items.Add(statusMenuItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Settings", null, (s, e) => ShowSettings());
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            return contextMenu;
        }

        static void UpdateTrayTooltip(string message)
        {
            // Replace commas with periods and truncate if necessary
            message = message.Replace(',', '.');
            if (notifyIcon != null)
            {
                UpdateNotifyIconTooltip(message);
            }

            if (statusMenuItem != null)
            {
                UpdateStatusMenuItem(message);
            }
        }

        private static void UpdateNotifyIconTooltip(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            notifyIcon.Text = message.Length <= 63 ? message : string.Concat(message.AsSpan(0, 60), "...");
        }

        private static void UpdateStatusMenuItem(string message)
        {
            string shortMessage;

            if (message.Contains("Controller Connected", StringComparison.OrdinalIgnoreCase))
            {
                shortMessage = ExtractBatteryLevelMessage(message);
            }
            else
            {
                shortMessage = "No controller connected";
            }

            // Ensure this runs on the UI thread
            if (statusMenuItem.GetCurrentParent().InvokeRequired)
            {
                statusMenuItem.GetCurrentParent().Invoke(new Action(() =>
                {
                    statusMenuItem.Text = shortMessage;
                }));
            }
            else
            {
                statusMenuItem.Text = shortMessage;
            }
        }

        private static string ExtractBatteryLevelMessage(string message)
        {
            // Use regex to extract the battery percentage
            var match = Regex.Match(message, @"(\d{1,3}\.\d)%");
            if (match.Success)
            {
                var batteryLevel = match.Groups[1].Value;
                return $"Battery level: {batteryLevel}%";
            }

            return "Battery level: N/A";
        }

        static void ShowSettings()
        {
            using var settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }
    }
}
