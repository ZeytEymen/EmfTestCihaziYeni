using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class YbfYakalama
    {
        public const int adr_test_basla = 112;
        public const int adr_frekans = 114;
        public const int adr_baslangic_gerilimi = 116;
        public const int adr_bitis_gerilim = 120;
        public const int adr_algilama_tork = 124;
        public const int adr_test_sonuc = 128;
        public const int adr_test_bitti = 132;

        public bool TestStart { get; set; }

        public int Frekans { get; set; }

        public float BaslangicGerilim { get; set; }

        public float BitisGerilim { get; set; }

        public float TorkAlgilama { get; set; }

        public float TestSonuc { get; set; }

        public bool TestBitti { get; set; }
    }
}
