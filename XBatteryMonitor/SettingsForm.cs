namespace XBatteryMonitor
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            // Load saved settings
            autostartCheckbox.Checked = Properties.Settings.Default.Autostart;
            batteryThresholdSlider.Value = Properties.Settings.Default.BatteryThreshold;
            notificationIntervalInput.Value = Properties.Settings.Default.NotificationInterval;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Save settings
            Properties.Settings.Default.Autostart = autostartCheckbox.Checked;
            Properties.Settings.Default.BatteryThreshold = batteryThresholdSlider.Value;
            Properties.Settings.Default.NotificationInterval = (int)notificationIntervalInput.Value;
            Properties.Settings.Default.Save();

            MessageBox.Show("Settings saved!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
