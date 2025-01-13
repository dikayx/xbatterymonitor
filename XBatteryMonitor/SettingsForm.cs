using Windows.Gaming.Input;
using Windows.Devices.Power;

namespace XBatteryMonitor
{
    public partial class SettingsForm : Form
    {
        private CancellationTokenSource cancellationTokenSource;

        public event Action<int> ThresholdChanged;

        public SettingsForm()
        {
            InitializeComponent();

            this.Icon = Resources.Icon;

            autostartCheckbox.Checked = Properties.Settings.Default.Autostart;
            batteryThresholdSlider.Value = Properties.Settings.Default.BatteryThreshold;
            notificationIntervalInput.Value = Properties.Settings.Default.NotificationInterval;
            sleepThresholdInput.Value = Properties.Settings.Default.SleepThreshold;

            UpdateThresholdLabel();

            CheckControllerStatus();

            StartMonitoringStatus();
        }

        private async void CheckForUpdatesButton_Click(object sender, EventArgs e)
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
                else
                {
                    MessageBox.Show("You are already using the latest version.", "No Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CheckControllerStatus()
        {
            var gamepad = Gamepad.Gamepads.FirstOrDefault();

            if (gamepad != null)
            {
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
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var gamepad = Gamepad.Gamepads.FirstOrDefault();
                    bool isConnected = gamepad != null;

                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateConnectionStatusLabel(isConnected);

                            if (isConnected)
                            {
                                var batteryReport = gamepad.TryGetBatteryReport();
                                UpdateBatteryPercentageLabel(batteryReport != null
                                    ? GetBatteryPercentage(batteryReport)
                                    : null);
                            }
                            else
                            {
                                UpdateBatteryPercentageLabel(null);
                            }
                        }));
                    }

                    await Task.Delay(2000, cancellationTokenSource.Token);
                }
            }, cancellationTokenSource.Token);
        }

        private static double? GetBatteryPercentage(BatteryReport batteryReport)
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

        private void TestNotificationButton_Click(object sender, EventArgs e)
        {
            NotificationHandler.ShowToastNotification("Test Notification");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Autostart = autostartCheckbox.Checked;
            Properties.Settings.Default.BatteryThreshold = batteryThresholdSlider.Value;
            Properties.Settings.Default.NotificationInterval = (int)notificationIntervalInput.Value;
            Properties.Settings.Default.SleepThreshold = (int)sleepThresholdInput.Value;
            Properties.Settings.Default.CheckForUpdatesRegularly = checkForUpdatesCheckbox.Checked;
            Properties.Settings.Default.Save();

            BatteryMonitor.UpdateSettings(batteryThresholdSlider.Value, (int)notificationIntervalInput.Value, (int)sleepThresholdInput.Value);

            NotificationHandler.ShowMessageBox("Settings saved successfully!");
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            base.OnFormClosed(e);
        }
    }
}
