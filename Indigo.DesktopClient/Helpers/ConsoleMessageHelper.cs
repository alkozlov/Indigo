namespace Indigo.DesktopClient.Helpers
{
    using System;
    using Indigo.DesktopClient.Model.PenthouseModels;

    public class ConsoleMessageHelper
    {
        public static ConsoleMessage GetSuccessMessage(String messageText)
        {
            ConsoleMessage consoleMessage = new ConsoleMessage
            {
                Text = messageText,
                Type = ConsoleMessageType.Success
            };

            return consoleMessage;
        }

        public static ConsoleMessage GetErrorMessage(String messageText)
        {
            ConsoleMessage consoleMessage = new ConsoleMessage
            {
                Text = messageText,
                Type = ConsoleMessageType.Error
            };

            return consoleMessage;
        }

        public static ConsoleMessage GetWarningMessage(String messageText)
        {
            ConsoleMessage consoleMessage = new ConsoleMessage
            {
                Text = messageText,
                Type = ConsoleMessageType.Warning
            };

            return consoleMessage;
        }

        public static ConsoleMessage GetInfoMessage(String messageText)
        {
            ConsoleMessage consoleMessage = new ConsoleMessage
            {
                Text = messageText,
                Type = ConsoleMessageType.Info
            };

            return consoleMessage;
        }
    }
}