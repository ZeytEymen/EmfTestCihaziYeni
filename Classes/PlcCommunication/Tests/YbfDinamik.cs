using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class YbfDinamik
    {
        public const int adr_test_basla = 88;
        public const int adr_frekans = 90;
        public const int adr_baslangic_gerilimi = 92;
        public const int adr_bitis_gerilim = 98;
        public const int adr_test_sure = 96;
        public const int adr_test_sonuc = 106;
        public const int adr_test_bitti = 110;
        public bool TestStart { get; set; }

        public int Frekans { get; set; }

        public float BaslangicGerilim { get; set; }

        public float BitisGerilim { get; set; }

        public short TestSure { get; set; }

        public float TestSonuc { get; set; }

        public bool TestBitti { get; set; }
    }
}
