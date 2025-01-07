using System.Diagnostics;
using System.Windows.Forms;
using Windows.Gaming.Input;

namespace XBatteryMonitor
{
    static class Program
    {
        static NotifyIcon notifyIcon;
        static ToolStripMenuItem statusMenuItem;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize NotifyIcon (taskbar tray)
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
            if (notifyIcon != null)
            {
                message = message.Replace(',', '.');
                notifyIcon.Text = message.Length <= 63 ? message : message.Substring(0, 60) + "...";
            }

            if (statusMenuItem != null)
            {
                string shortMessage;

                if (message.Contains("Controller Connected"))
                {
                    // Use regex to extract the battery percentage
                    var match = System.Text.RegularExpressions.Regex.Match(message, @"(\d{1,3}.\d)%");
                    if (match.Success)
                    {
                        // Convert the extracted value to standard format (e.g., 70.0%)
                        var batteryLevel = match.Groups[1].Value.Replace(',', '.');
                        shortMessage = $"Battery level: {batteryLevel}%";
                    }
                    else
                    {
                        shortMessage = "Battery level: N/A";
                    }
                }
                else
                {
                    shortMessage = "No controller connected";
                }

                statusMenuItem.Text = shortMessage;
            }
        }

        static void ShowSettings()
        {
            using (var settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog();
            }
        }
    }
}
