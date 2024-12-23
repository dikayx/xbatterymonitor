using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Gaming.Input;

namespace XBatteryMonitor
{
    public partial class SettingsForm : Form
    {
        private CancellationTokenSource cancellationTokenSource;

        // Event to notify about threshold changes
        public event Action<int> ThresholdChanged;

        public SettingsForm()
        {
            InitializeComponent();

            // Load saved settings
            autostartCheckbox.Checked = Properties.Settings.Default.Autostart;
            batteryThresholdSlider.Value = Properties.Settings.Default.BatteryThreshold;
            notificationIntervalInput.Value = Properties.Settings.Default.NotificationInterval;

            UpdateThresholdLabel();

            // Immediately check and update the current status
            CheckControllerStatus();

            // Start monitoring controller connection and battery status
            StartMonitoringStatus();
        }

        private void CheckControllerStatus()
        {
            var gamepad = Gamepad.Gamepads.FirstOrDefault();

            if (gamepad != null)
            {
                // Update connection status and battery percentage
                UpdateConnectionStatusLabel(true);
                var batteryReport = gamepad.TryGetBatteryReport();
                if (batteryReport != null)
                {
                    UpdateBatteryPercentageLabel(GetBatteryPercentage(batteryReport));
                }
                else
                {
                    UpdateBatteryPercentageLabel(null);
                }
            }
            else
            {
                UpdateConnectionStatusLabel(false);
                UpdateBatteryPercentageLabel(null);
            }
        }

        private void StartMonitoringStatus()
        {
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                bool lastConnectedState = false;

                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var gamepad = Gamepad.Gamepads.FirstOrDefault();
                    bool isConnected = gamepad != null;

                    if (isConnected != lastConnectedState)
                    {
                        lastConnectedState = isConnected;
                        UpdateConnectionStatusLabel(isConnected);
                    }

                    if (isConnected)
                    {
                        var batteryReport = gamepad.TryGetBatteryReport();
                        if (batteryReport != null)
                        {
                            UpdateBatteryPercentageLabel(GetBatteryPercentage(batteryReport));
                        }
                    }
                    else
                    {
                        UpdateBatteryPercentageLabel(null);
                    }

                    await Task.Delay(1000); // Check status every second
                }
            });
        }

        private double? GetBatteryPercentage(Windows.Devices.Power.BatteryReport batteryReport)
        {
            var remaining = batteryReport.RemainingCapacityInMilliwattHours;
            var full = batteryReport.FullChargeCapacityInMilliwattHours;

            if (remaining.HasValue && full.HasValue && full.Value > 0)
            {
                return (remaining.Value / (double)full.Value) * 100;
            }

            return null;
        }

        private void UpdateConnectionStatusLabel(bool isConnected)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateConnectionStatusLabel(isConnected)));
            }
            else
            {
                connectionStatusLabel.Text = isConnected
                    ? "Controller Status: Connected"
                    : "Controller Status: Disconnected";
            }
        }

        private void UpdateBatteryPercentageLabel(double? percentage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateBatteryPercentageLabel(percentage)));
            }
            else
            {
                batteryPercentageLabel.Text = percentage.HasValue
                    ? $"Battery Percentage: {percentage.Value:0.0}%"
                    : "Battery Percentage: N/A";
            }
        }

        private void BatteryThresholdSlider_ValueChanged(object sender, EventArgs e)
        {
            UpdateThresholdLabel();

            // Notify the app about the updated threshold
            ThresholdChanged?.Invoke(batteryThresholdSlider.Value);
        }

        private void UpdateThresholdLabel()
        {
            batteryThresholdLabel.Text = $"Selected Battery Threshold: {batteryThresholdSlider.Value}%";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Save settings
            Properties.Settings.Default.Autostart = autostartCheckbox.Checked;
            Properties.Settings.Default.BatteryThreshold = batteryThresholdSlider.Value;
            Properties.Settings.Default.NotificationInterval = (int)notificationIntervalInput.Value;
            Properties.Settings.Default.Save();

            // Update the running monitor with new settings
            BatteryMonitor.UpdateSettings(batteryThresholdSlider.Value, (int)notificationIntervalInput.Value);

            MessageBox.Show("Settings saved!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            base.OnFormClosed(e);
        }
    }
}
