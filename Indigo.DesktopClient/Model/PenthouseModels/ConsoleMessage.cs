namespace Indigo.DesktopClient.Model.PenthouseModels
{
    using System;

    public enum ConsoleMessageType : byte
    {
        Success = 0,

        Error = 1,

        Warning = 2,

        Info = 3
    }

    public class ConsoleMessage
    {
        public String Text { get; set; }

        public ConsoleMessageType Type { get; set; }
    }
}