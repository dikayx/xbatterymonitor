using Windows.Devices.Power;
using Windows.Gaming.Input;
using Windows.UI.Notifications;

namespace XBatteryMonitor
{
    public static class BatteryMonitor
    {
        private static CancellationTokenSource? cancellationTokenSource;
        private static int batteryThreshold = Properties.Settings.Default.BatteryThreshold;
        private static int notificationInterval = Properties.Settings.Default.NotificationInterval;
        private static int sleepThreshold = Properties.Settings.Default.SleepThreshold * 60;
        private static bool isSleeping = false;

        public static void Start(Action<string> updateTrayTooltip)
        {
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                DateTime lastConnectedTime = DateTime.UtcNow;

                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var gamepad = Gamepad.Gamepads.FirstOrDefault();

                    if (gamepad != null)
                    {
                        if (isSleeping)
                        {
                            isSleeping = false;
                            updateTrayTooltip("Controller connected. Resuming...");
                        }

                        var batteryReport = gamepad.TryGetBatteryReport();
                        var batteryPercentage = batteryReport != null
                            ? GetBatteryPercentage(batteryReport)
                            : -1;

                        string tooltipText = batteryPercentage >= 0
                            ? $"Controller Connected: {batteryPercentage:0.0}% battery"
                            : "Controller Connected: Battery status unknown";

                        if (batteryPercentage < batteryThreshold)
                        {
                            ShowLowBatteryNotification(batteryPercentage);
                        }

                        updateTrayTooltip(tooltipText);
                        lastConnectedTime = DateTime.UtcNow;
                    }
                    else
                    {
                        if (!isSleeping && (DateTime.UtcNow - lastConnectedTime).TotalSeconds >= sleepThreshold)
                        {
                            isSleeping = true;
                            updateTrayTooltip("No controller connected. Going to sleep...");
                        }

                        if (!isSleeping)
                        {
                            updateTrayTooltip("No controller connected");
                        }
                    }

                    await Task.Delay(isSleeping ? sleepThreshold * 1000 : notificationInterval * 1000);
                }
            });
        }

        public static void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        public static void UpdateSettings(int newThreshold, int newInterval, int newSleepThreshold)
        {
            batteryThreshold = newThreshold;
            notificationInterval = newInterval;
            sleepThreshold = newSleepThreshold * 60; // Convert to seconds
        }

        private static double GetBatteryPercentage(BatteryReport batteryReport)
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
            NotificationHandler.ShowToastNotification(batteryPercentage);
        }
    }
}
