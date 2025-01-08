using Windows.UI.Notifications;

namespace XBatteryMonitor
{
    internal class NotificationHandler
    {
        public static void ShowToastNotification(object message)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textElements = toastXml.GetElementsByTagName("text");

            if (message is string textMessage)
            {
                textElements[0].AppendChild(toastXml.CreateTextNode("Xbox Controller Battery Monitor"));
                textElements[1].AppendChild(toastXml.CreateTextNode(textMessage));
            }
            else if (message is double batteryPercentage)
            {
                textElements[0].AppendChild(toastXml.CreateTextNode("Xbox Controller Battery Low"));
                textElements[1].AppendChild(toastXml.CreateTextNode($"Battery is at {batteryPercentage:0.0}%"));
            }
            else
            {
                return;
            }

            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier(nameof(XBatteryMonitor)).Show(toast);
        }

        public static void ShowMessageBox(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            MessageBox.Show(message, nameof(XBatteryMonitor), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
