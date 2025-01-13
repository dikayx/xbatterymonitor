using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace XBatteryMonitor
{
    internal class NotificationHandler
    {
        /// <summary>
        /// Displays a toast notification based on the type of message provided.
        /// </summary>
        /// <param name="message">The message to display. Can be a string or a double (battery percentage).</param>
        public static void ShowToastNotification(object message)
        {
            try
            {
                var toastXml = CreateToastXml(message);
                if (toastXml == null) return;

                var toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier(nameof(XBatteryMonitor)).Show(toast);
            }
            catch (Exception ex)
            {
                // Fallback: Show a message box in case of an error
                ShowMessageBox($"Failed to show toast notification: {ex.Message}", "Notification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays a message box with the given message, title, buttons, and icon.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The title of the message box.</param>
        /// <param name="buttons">The buttons to display on the message box.</param>
        /// <param name="icon">The icon to display on the message box.</param>
        /// <returns>The dialog result.</returns>
        public static DialogResult ShowMessageBox(string message, string caption = null, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            if (string.IsNullOrWhiteSpace(message)) return DialogResult.None;
            return MessageBox.Show(message, caption ?? nameof(XBatteryMonitor), buttons, icon);
        }

        /// <summary>
        /// Creates toast XML content based on the provided message.
        /// </summary>
        /// <param name="message">The message content for the toast.</param>
        /// <returns>XmlDocument representing the toast content.</returns>
        private static XmlDocument CreateToastXml(object message)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textElements = toastXml.GetElementsByTagName("text");

            if (textElements.Length < 2) throw new InvalidOperationException("Invalid toast template.");

            // Customize toast content based on message type
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
                throw new ArgumentException("Unsupported message type for toast notification.");
            }

            return toastXml;
        }
    }
}
