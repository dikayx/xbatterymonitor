namespace XBatteryMonitor
{
    partial class SettingsForm
    {
        private CheckBox autostartCheckbox;
        private TrackBar batteryThresholdSlider;
        private NumericUpDown notificationIntervalInput;
        private Button saveButton;
        private Label connectionStatusLabel;
        private Label batteryPercentageLabel;
        private Label batteryThresholdLabel;

        private GroupBox batteryGroupBox;
        private GroupBox notificationGroupBox;
        private GroupBox autostartGroupBox;
        private GroupBox connectionGroupBox;

        private void InitializeComponent()
        {
            autostartCheckbox = new CheckBox { Text = "Autostart with Windows", Dock = DockStyle.Top };
            batteryThresholdSlider = new TrackBar { Minimum = 0, Maximum = 100, TickFrequency = 10, Dock = DockStyle.Top };
            notificationIntervalInput = new NumericUpDown { Minimum = 1, Maximum = 60, Dock = DockStyle.Top };
            saveButton = new Button { Text = "Save", Dock = DockStyle.Bottom };
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
            batteryThresholdLabel = new Label
            {
                Text = "Selected Battery Threshold: 20%",
                Dock = DockStyle.Top,
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            batteryThresholdSlider.ValueChanged += BatteryThresholdSlider_ValueChanged;
            saveButton.Click += SaveButton_Click;

            // Battery Settings GroupBox
            batteryGroupBox = new GroupBox { Text = "Battery Settings", Dock = DockStyle.Top, Padding = new Padding(10), Height = 120 };
            batteryGroupBox.Controls.Add(batteryThresholdSlider);
            batteryGroupBox.Controls.Add(new Label { Text = "Battery Threshold (%):", Dock = DockStyle.Top });
            batteryGroupBox.Controls.Add(batteryThresholdLabel);
            batteryGroupBox.Controls.Add(batteryPercentageLabel);

            // Notification Settings GroupBox
            notificationGroupBox = new GroupBox { Text = "Notification Settings", Dock = DockStyle.Top, Padding = new Padding(10), Height = 90 };
            notificationGroupBox.Controls.Add(notificationIntervalInput);
            notificationGroupBox.Controls.Add(new Label { Text = "Notification Interval (minutes):", Dock = DockStyle.Top });

            // Autostart Settings GroupBox
            autostartGroupBox = new GroupBox { Text = "Autostart Settings", Dock = DockStyle.Top, Padding = new Padding(10), Height = 60 };
            autostartGroupBox.Controls.Add(autostartCheckbox);

            // Connection Status GroupBox
            connectionGroupBox = new GroupBox { Text = "Connection Status", Dock = DockStyle.Top, Padding = new Padding(10), Height = 70 };
            connectionGroupBox.Controls.Add(connectionStatusLabel);
            connectionGroupBox.Controls.Add(batteryPercentageLabel); // Add battery percentage label

            // Adding controls to the form
            Controls.Add(saveButton);
            Controls.Add(notificationGroupBox);
            Controls.Add(batteryGroupBox);
            Controls.Add(connectionGroupBox);
            Controls.Add(autostartGroupBox);

            Text = "Settings";
            Width = 300;
            Height = 400; // Adjusted height to fit all controls
        }
    }
}
