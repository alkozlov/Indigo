namespace Indigo.DesktopClient.Model.PenthouseModels
{
    using System;

    public enum ReferenceType : byte
    {
        Subjects = 0,

        StopWords = 1
    }

    public class ReferenceItem
    {
        public String Header { get; set; }

        public ReferenceType Type { get; set; }
    }
}