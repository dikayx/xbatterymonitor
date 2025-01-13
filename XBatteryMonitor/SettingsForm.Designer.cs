namespace XBatteryMonitor
{
    partial class SettingsForm
    {
        private GroupBox generalSettingsGroupBox;
        private GroupBox connectionGroupBox;
        private GroupBox batteryGroupBox;
        private GroupBox notificationGroupBox;

        private CheckBox autostartCheckbox;
        private CheckBox checkForUpdatesCheckbox;
        private Button checkForUpdatesButton;

        private Label connectionStatusLabel;
        private Label batteryPercentageLabel;

        private Label batteryThresholdLabel;
        private TrackBar batteryThresholdSlider;

        private NumericUpDown sleepThresholdInput;
        private NumericUpDown notificationIntervalInput;

        private Button saveButton;
        private Button testNotificationButton;

        private void InitializeComponent()
        {
            // General Settings GroupBox
            generalSettingsGroupBox = new GroupBox { Text = "General Settings", Dock = DockStyle.Top, Padding = new Padding(10), Height = 110 };
            autostartCheckbox = new CheckBox { Text = "Autostart with Windows", Dock = DockStyle.Top };
            checkForUpdatesCheckbox = new CheckBox { Text = "Check for updates regularly", Dock = DockStyle.Top };
            checkForUpdatesButton = new Button { Text = "Check for updates", Dock = DockStyle.Top };

            generalSettingsGroupBox.Controls.Add(checkForUpdatesButton);  // Add Check for Updates Button (last)
            generalSettingsGroupBox.Controls.Add(checkForUpdatesCheckbox);  // Add Check for Updates Checkbox
            generalSettingsGroupBox.Controls.Add(autostartCheckbox);  // Add Autostart Checkbox (first)

            // Connection Status GroupBox
            connectionGroupBox = new GroupBox { Text = "Connection Status", Dock = DockStyle.Top, Padding = new Padding(10), Height = 70 };
            connectionStatusLabel = new Label
            {
                Text = "Controller Status: Disconnected",
                Dock = DockStyle.Top,
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            batteryPercentageLabel = new Label
            {
                Text = "Battery Percentage: N/A",
                Dock = DockStyle.Top,
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            connectionGroupBox.Controls.Add(connectionStatusLabel);
            connectionGroupBox.Controls.Add(batteryPercentageLabel);

            // Battery Settings GroupBox
            batteryGroupBox = new GroupBox { Text = "Battery Settings", Dock = DockStyle.Top, Padding = new Padding(10), Height = 120 };
            batteryThresholdSlider = new TrackBar { Minimum = 0, Maximum = 100, TickFrequency = 10, Dock = DockStyle.Top };
            batteryThresholdLabel = new Label
            {
                Text = "Selected Battery Threshold: 20%",
                Dock = DockStyle.Top,
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            batteryGroupBox.Controls.Add(batteryThresholdSlider);
            batteryGroupBox.Controls.Add(new Label { Text = "Battery Threshold (%):", Dock = DockStyle.Top });
            batteryGroupBox.Controls.Add(batteryThresholdLabel);

            // Notification Settings GroupBox
            notificationGroupBox = new GroupBox { Text = "Notification Settings", Dock = DockStyle.Top, Padding = new Padding(10), Height = 160 };
            testNotificationButton = new Button { Text = "Test Notification", Dock = DockStyle.Top };
            sleepThresholdInput = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 5, Dock = DockStyle.Top };
            notificationIntervalInput = new NumericUpDown { Minimum = 1, Maximum = 60, Dock = DockStyle.Top };

            notificationGroupBox.Controls.Add(notificationIntervalInput);
            notificationGroupBox.Controls.Add(new Label { Text = "Notification Interval (minutes):", Dock = DockStyle.Top });
            notificationGroupBox.Controls.Add(sleepThresholdInput);
            notificationGroupBox.Controls.Add(new Label { Text = "Sleep Threshold (minutes):", Dock = DockStyle.Top });
            notificationGroupBox.Controls.Add(testNotificationButton);

            // Save Button
            saveButton = new Button { Text = "Save", Dock = DockStyle.Bottom };

            // Event Handlers
            checkForUpdatesCheckbox.Checked = Properties.Settings.Default.CheckForUpdatesRegularly;
            checkForUpdatesButton.Click += CheckForUpdatesButton_Click;
            batteryThresholdSlider.ValueChanged += BatteryThresholdSlider_ValueChanged;
            saveButton.Click += SaveButton_Click;
            testNotificationButton.Click += TestNotificationButton_Click;

            // Adding Controls to the Form
            Controls.Add(saveButton);
            Controls.Add(notificationGroupBox);
            Controls.Add(batteryGroupBox);
            Controls.Add(connectionGroupBox);
            Controls.Add(generalSettingsGroupBox);

            // Form Properties
            Text = "Settings";
            Width = 300;
            Height = 530;
        }
    }
}
