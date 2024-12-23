using System.Diagnostics;
using Windows.Gaming.Input;
using Windows.UI.Notifications;

namespace XBatteryMonitor
{
    public static class BatteryMonitor
    {
        private static CancellationTokenSource cancellationTokenSource;
        private static int batteryThreshold = Properties.Settings.Default.BatteryThreshold;
        private static int notificationInterval = Properties.Settings.Default.NotificationInterval;

        public static void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var gamepad = Gamepad.Gamepads.FirstOrDefault();

                    if (gamepad != null)
                    {
                        Debug.WriteLine("Xbox controller found.");
                        var batteryReport = gamepad.TryGetBatteryReport();
                        if (batteryReport != null)
                        {
                            var batteryPercentage = GetBatteryPercentage(batteryReport);

                            if (batteryPercentage < batteryThreshold)
                            {
                                ShowLowBatteryNotification(batteryPercentage);
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No Xbox controller found.");
                    }

                    //await Task.Delay(notificationInterval * 60 * 1000); // Interval in minutes
                    // For debugging reasons, wait 30 seconds
                    await Task.Delay(30 * 1000);
                }
            });
        }

        public static void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        public static void UpdateSettings(int newThreshold, int newInterval)
        {
            Debug.WriteLine($"Updating settings: Threshold = {newThreshold}%, Interval = {newInterval} minutes");
            batteryThreshold = newThreshold;
            notificationInterval = newInterval;
        }

        private static double GetBatteryPercentage(Windows.Devices.Power.BatteryReport batteryReport)
        {
            var remaining = batteryReport.RemainingCapacityInMilliwattHours;
            var full = batteryReport.FullChargeCapacityInMilliwattHours;

            if (remaining.HasValue && full.HasValue && full.Value > 0)
            {
                return (remaining.Value / (double)full.Value) * 100;
            }

            return 0;
        }

        private static void ShowLowBatteryNotification(double batteryPercentage)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textElements = toastXml.GetElementsByTagName("text");
            textElements[0].AppendChild(toastXml.CreateTextNode("Xbox Controller Battery Low"));
            textElements[1].AppendChild(toastXml.CreateTextNode($"Battery is at {batteryPercentage:0.0}%"));

            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier("XboxControllerBatteryMonitor").Show(toast);
        }
    }
}
