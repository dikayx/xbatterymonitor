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

            // Taskbar tray icon
            notifyIcon = new NotifyIcon
            {
                Icon = Resources.Icon,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip(),
                Text = "Checking status..."
            };

            statusMenuItem = new ToolStripMenuItem("Checking status...") { Enabled = false };
            notifyIcon.ContextMenuStrip.Items.Add(statusMenuItem);
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon.ContextMenuStrip.Items.Add("Settings", null, (s, e) => ShowSettings());
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => Application.Exit());

            UpdateTrayTooltip(GetInitialTooltipMessage());
            BatteryMonitor.Start(UpdateTrayTooltip);

            // Add event handlers
            Gamepad.GamepadAdded += (s, e) => BatteryMonitor.Start(UpdateTrayTooltip);
            Gamepad.GamepadRemoved += (s, e) => UpdateTrayTooltip("No controller connected. Waiting...");

            Application.Run();
        }

        static string GetInitialTooltipMessage()
        {
            var gamepad = Gamepad.Gamepads.FirstOrDefault();

            if (gamepad != null)
            {
                var batteryReport = gamepad.TryGetBatteryReport();
                if (batteryReport != null)
                {
                    var batteryPercentage = BatteryMonitor.GetBatteryPercentage(batteryReport);
                    return $"Controller Connected: {batteryPercentage:0.0}% battery";
                }
                return "Controller Connected: Battery status N/A";
            }
            return "No controller connected";
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

            statusMenuItem.Text = shortMessage;
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
