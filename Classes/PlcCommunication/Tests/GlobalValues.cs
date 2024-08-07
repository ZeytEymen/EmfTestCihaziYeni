using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class GlobalValues
    {
        public const int adr_ACT_VOLT = 4;
        public const int adr_ACT_TORK = 8;
        public const int adr_ACT_AKIM = 14;
        public const int adr_FREN_220 = 18;
        public const int adr_FREN_24 = 24;
        public const int adr_CMD_VOLT = 20;
        public const int adr_ACIL_STOP = 12;

        public float CMD_VOLT { get; set; }
        public float ACT_VOLT { get; set; }
        public float ACT_TORK { get; set; }
        public float ACT_AKIM { get; set; }
        public bool FREN_220 { get; set; }
        public bool FREN_24 { get; set; }
        public bool ACIL_STOP { get; set; }
    }
}
