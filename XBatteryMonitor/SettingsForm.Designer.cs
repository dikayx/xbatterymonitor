namespace XBatteryMonitor
{
    partial class SettingsForm
    {
        private TabControl settingsTabControl;
        private TabPage generalTab;
        private TabPage connectionTab;
        private TabPage batteryTab;
        private TabPage notificationsTab;

        private CheckBox autostartCheckbox;
        private CheckBox checkForUpdatesCheckbox;
        private Button checkForUpdatesButton;

        private Label connectionStatusLabel;
        private Label batteryPercentageLabel;

        private Label batteryThresholdLabel;
        private TrackBar batteryThresholdSlider;

        private NumericUpDown sleepThresholdInput;
        private Label sleepThresholdLabel;
        private NumericUpDown notificationIntervalInput;
        private Label notificationIntervalLabel;
        private Button testNotificationButton;

        private Button saveButton;

        private void InitializeComponent()
        {
            settingsTabControl = new TabControl();
            generalTab = new TabPage();
            checkForUpdatesButton = new Button();
            checkForUpdatesCheckbox = new CheckBox();
            autostartCheckbox = new CheckBox();
            connectionTab = new TabPage();
            connectionStatusLabel = new Label();
            batteryPercentageLabel = new Label();
            batteryTab = new TabPage();
            batteryThresholdSlider = new TrackBar();
            batteryThresholdLabel = new Label();
            notificationsTab = new TabPage();
            testNotificationButton = new Button();
            sleepThresholdInput = new NumericUpDown();
            sleepThresholdLabel = new Label();
            notificationIntervalInput = new NumericUpDown();
            notificationIntervalLabel = new Label();
            saveButton = new Button();
            settingsTabControl.SuspendLayout();
            generalTab.SuspendLayout();
            connectionTab.SuspendLayout();
            batteryTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)batteryThresholdSlider).BeginInit();
            notificationsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sleepThresholdInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)notificationIntervalInput).BeginInit();
            SuspendLayout();
            // 
            // settingsTabControl
            // 
            settingsTabControl.Controls.Add(generalTab);
            settingsTabControl.Controls.Add(connectionTab);
            settingsTabControl.Controls.Add(batteryTab);
            settingsTabControl.Controls.Add(notificationsTab);
            settingsTabControl.Dock = DockStyle.Fill;
            settingsTabControl.Location = new Point(0, 0);
            settingsTabControl.Name = "settingsTabControl";
            settingsTabControl.SelectedIndex = 0;
            settingsTabControl.Size = new Size(350, 150);
            settingsTabControl.TabIndex = 0;
            // 
            // generalTab
            // 
            generalTab.Controls.Add(checkForUpdatesButton);
            generalTab.Controls.Add(checkForUpdatesCheckbox);
            generalTab.Controls.Add(autostartCheckbox);
            generalTab.Location = new Point(4, 24);
            generalTab.Name = "generalTab";
            generalTab.Size = new Size(342, 122);
            generalTab.TabIndex = 0;
            generalTab.Text = "General";
            // 
            // checkForUpdatesButton
            // 
            checkForUpdatesButton.Location = new Point(10, 80);
            checkForUpdatesButton.Name = "checkForUpdatesButton";
            checkForUpdatesButton.Size = new Size(120, 30);
            checkForUpdatesButton.TabIndex = 0;
            checkForUpdatesButton.Text = "Check for updates";
            // 
            // checkForUpdatesCheckbox
            // 
            checkForUpdatesCheckbox.AutoSize = true;
            checkForUpdatesCheckbox.Location = new Point(10, 40);
            checkForUpdatesCheckbox.Name = "checkForUpdatesCheckbox";
            checkForUpdatesCheckbox.Size = new Size(171, 19);
            checkForUpdatesCheckbox.TabIndex = 1;
            checkForUpdatesCheckbox.Text = "Check for updates regularly";
            // 
            // autostartCheckbox
            // 
            autostartCheckbox.AutoSize = true;
            autostartCheckbox.Location = new Point(10, 10);
            autostartCheckbox.Name = "autostartCheckbox";
            autostartCheckbox.Size = new Size(153, 19);
            autostartCheckbox.TabIndex = 2;
            autostartCheckbox.Text = "Autostart with Windows";
            // 
            // connectionTab
            // 
            connectionTab.Controls.Add(connectionStatusLabel);
            connectionTab.Controls.Add(batteryPercentageLabel);
            connectionTab.Location = new Point(4, 24);
            connectionTab.Name = "connectionTab";
            connectionTab.Size = new Size(342, 122);
            connectionTab.TabIndex = 1;
            connectionTab.Text = "Connection";
            // 
            // connectionStatusLabel
            // 
            connectionStatusLabel.AutoSize = true;
            connectionStatusLabel.Location = new Point(10, 10);
            connectionStatusLabel.Name = "connectionStatusLabel";
            connectionStatusLabel.Size = new Size(173, 15);
            connectionStatusLabel.TabIndex = 0;
            connectionStatusLabel.Text = "Controller Status: Disconnected";
            // 
            // batteryPercentageLabel
            // 
            batteryPercentageLabel.AutoSize = true;
            batteryPercentageLabel.Location = new Point(10, 40);
            batteryPercentageLabel.Name = "batteryPercentageLabel";
            batteryPercentageLabel.Size = new Size(134, 15);
            batteryPercentageLabel.TabIndex = 1;
            batteryPercentageLabel.Text = "Battery Percentage: N/A";
            // 
            // batteryTab
            // 
            batteryTab.Controls.Add(batteryThresholdSlider);
            batteryTab.Controls.Add(batteryThresholdLabel);
            batteryTab.Location = new Point(4, 24);
            batteryTab.Name = "batteryTab";
            batteryTab.Size = new Size(342, 122);
            batteryTab.TabIndex = 2;
            batteryTab.Text = "Battery";
            // 
            // batteryThresholdSlider
            // 
            batteryThresholdSlider.Location = new Point(10, 40);
            batteryThresholdSlider.Maximum = 100;
            batteryThresholdSlider.Name = "batteryThresholdSlider";
            batteryThresholdSlider.Size = new Size(324, 45);
            batteryThresholdSlider.TabIndex = 0;
            batteryThresholdSlider.TickFrequency = 10;
            // 
            // batteryThresholdLabel
            // 
            batteryThresholdLabel.AutoSize = true;
            batteryThresholdLabel.Location = new Point(10, 10);
            batteryThresholdLabel.Name = "batteryThresholdLabel";
            batteryThresholdLabel.Size = new Size(174, 15);
            batteryThresholdLabel.TabIndex = 1;
            batteryThresholdLabel.Text = "Selected Battery Threshold: 20%";
            // 
            // notificationsTab
            // 
            notificationsTab.Controls.Add(testNotificationButton);
            notificationsTab.Controls.Add(sleepThresholdInput);
            notificationsTab.Controls.Add(sleepThresholdLabel);
            notificationsTab.Controls.Add(notificationIntervalInput);
            notificationsTab.Controls.Add(notificationIntervalLabel);
            notificationsTab.Location = new Point(4, 24);
            notificationsTab.Name = "notificationsTab";
            notificationsTab.Size = new Size(342, 122);
            notificationsTab.TabIndex = 3;
            notificationsTab.Text = "Notifications";
            // 
            // testNotificationButton
            // 
            testNotificationButton.Location = new Point(10, 80);
            testNotificationButton.Name = "testNotificationButton";
            testNotificationButton.Size = new Size(120, 30);
            testNotificationButton.TabIndex = 0;
            testNotificationButton.Text = "Test Notification";
            // 
            // sleepThresholdInput
            // 
            sleepThresholdInput.Location = new Point(200, 40);
            sleepThresholdInput.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            sleepThresholdInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            sleepThresholdInput.Name = "sleepThresholdInput";
            sleepThresholdInput.Size = new Size(120, 23);
            sleepThresholdInput.TabIndex = 1;
            sleepThresholdInput.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // sleepThresholdLabel
            // 
            sleepThresholdLabel.AutoSize = true;
            sleepThresholdLabel.Location = new Point(10, 40);
            sleepThresholdLabel.Name = "sleepThresholdLabel";
            sleepThresholdLabel.Size = new Size(147, 15);
            sleepThresholdLabel.TabIndex = 2;
            sleepThresholdLabel.Text = "Sleep Threshold (minutes):";
            // 
            // notificationIntervalInput
            // 
            notificationIntervalInput.Location = new Point(200, 10);
            notificationIntervalInput.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            notificationIntervalInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            notificationIntervalInput.Name = "notificationIntervalInput";
            notificationIntervalInput.Size = new Size(120, 23);
            notificationIntervalInput.TabIndex = 3;
            notificationIntervalInput.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // notificationIntervalLabel
            // 
            notificationIntervalLabel.AutoSize = true;
            notificationIntervalLabel.Location = new Point(10, 10);
            notificationIntervalLabel.Name = "notificationIntervalLabel";
            notificationIntervalLabel.Size = new Size(169, 15);
            notificationIntervalLabel.TabIndex = 4;
            notificationIntervalLabel.Text = "Notification Interval (minutes):";
            // 
            // saveButton
            // 
            saveButton.Dock = DockStyle.Bottom;
            saveButton.Location = new Point(0, 150);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(350, 30);
            saveButton.TabIndex = 1;
            saveButton.Text = "Save";
            // 
            // SettingsForm
            // 
            ClientSize = new Size(350, 180);
            Controls.Add(settingsTabControl);
            Controls.Add(saveButton);
            Name = "SettingsForm";
            Text = "Settings";
            settingsTabControl.ResumeLayout(false);
            generalTab.ResumeLayout(false);
            generalTab.PerformLayout();
            connectionTab.ResumeLayout(false);
            connectionTab.PerformLayout();
            batteryTab.ResumeLayout(false);
            batteryTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)batteryThresholdSlider).EndInit();
            notificationsTab.ResumeLayout(false);
            notificationsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)sleepThresholdInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)notificationIntervalInput).EndInit();
            ResumeLayout(false);
        }
    }
}
