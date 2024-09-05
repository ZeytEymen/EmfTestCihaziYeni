using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class AbtfTest
    {
        public const int adr_test_basla = 134;
        public const int adr_frekans = 136;
        public const int adr_test_bitti = 138;

        public bool TestStart { get; set; }
        public short Frekans { get; set; }
        public bool TestBitti { get; set; }
    }
}
