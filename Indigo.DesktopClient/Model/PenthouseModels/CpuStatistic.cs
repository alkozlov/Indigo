namespace Indigo.DesktopClient.Model.PenthouseModels
{
    using System;

    public class CpuStatistic
    {
        public short CpuUsage { get; set; }

        public Int32 CpuUsageAsInteger
        {
            get { return Convert.ToInt32(this.CpuUsage); }
        }

        public String CpuUsageAsString
        {
            get { return String.Format("{0} %", this.CpuUsage); }
        }

        public CpuStatistic()
        {
            this.CpuUsage = 0;
        }

        public CpuStatistic(short cpuUsage)
        {
            this.CpuUsage = cpuUsage;
        }
    }
}