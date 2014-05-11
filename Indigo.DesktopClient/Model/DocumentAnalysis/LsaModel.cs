using System.Drawing;

namespace Indigo.DesktopClient.Model.DocumentAnalysis
{
    using System;

    public class LsaModel
    {
        public Int32? DocumentId { get; set; }

        public String DocumentName { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public Int32 Radius { get; set; }

        public Brush Brush { get; set; }
    }
}