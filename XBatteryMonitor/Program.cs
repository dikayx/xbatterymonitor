using System.Text.RegularExpressions;
using Windows.Gaming.Input;

namespace XBatteryMonitor
{
    static class Program
    {
        static NotifyIcon? notifyIcon;
        static ToolStripMenuItem? statusMenuItem;

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

            Application.Run();
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
