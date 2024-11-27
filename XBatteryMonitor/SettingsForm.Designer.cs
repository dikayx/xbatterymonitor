namespace XBatteryMonitor
{
    partial class SettingsForm
    {
        private CheckBox autostartCheckbox;
        private TrackBar batteryThresholdSlider;
        private NumericUpDown notificationIntervalInput;
        private Button saveButton;

        private void InitializeComponent()
        {
            autostartCheckbox = new CheckBox { Text = "Autostart with Windows", Dock = DockStyle.Top };
            batteryThresholdSlider = new TrackBar { Minimum = 0, Maximum = 100, TickFrequency = 10, Dock = DockStyle.Top };
            notificationIntervalInput = new NumericUpDown { Minimum = 1, Maximum = 60, Dock = DockStyle.Top };
            saveButton = new Button { Text = "Save", Dock = DockStyle.Bottom };

            saveButton.Click += SaveButton_Click;

            // TODO: Add a label that shows the connection status of the controller (connected/disconnected)
            // (Should be able to update this label in real-time)

            Controls.Add(saveButton);
            Controls.Add(notificationIntervalInput);
            Controls.Add(new Label { Text = "Notification Interval (minutes):", Dock = DockStyle.Top });
            Controls.Add(batteryThresholdSlider);
            Controls.Add(new Label { Text = "Battery Threshold (%):", Dock = DockStyle.Top });
            Controls.Add(autostartCheckbox);

            // TODO: Add a live threshold percentage label

            Text = "Settings";
            Width = 300;
            Height = 200;
        }
    }
}
