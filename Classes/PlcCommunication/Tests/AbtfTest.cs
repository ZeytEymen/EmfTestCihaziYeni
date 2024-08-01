using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class AbtfTest
    {
        public const int adr_test_basla = 154;
        public const int adr_frekans = 156;
        public const int adr_test_bitti = 158;

        public bool TestBasla { get; set; }
        public short Frekans { get; set; }
        public bool TestBitti { get; set; }
    }
}
