using System;

namespace APC.Models.Shared
{
    /// <summary>
    /// The shared resource across the application
    /// </summary>
    public record Resource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Host { get; init; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double LoTrans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double HiTrans { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double RetPct { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double ITemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int AlarmDel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double BattV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double LineFreq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string LastXfer { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int NumXfers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int TOnBatt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int CumOnBatt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime XOffBatt { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool SelfTest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string StatFlag { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string SerialNo { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime BattDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double NomInV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double NomBattV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double OutputV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int NomPower { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double MaxLineV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double MinTimeL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Apc { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Hostname { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string UpsName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Cable { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Driver { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int MaxTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string UpsMode { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime MasterUpd { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime EndApc { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double LineV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double LoadPct { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double BCharge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double TimeLeft { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public double MBattChg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Firmware { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Status { get; set; } = string.Empty;
    }
}
