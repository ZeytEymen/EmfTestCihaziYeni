using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class YbfBirakma
    {
        public const int adr_test_basla = 26;
        public const int adr_frekans = 28;
        public const int adr_baslangic_gerilimi = 30;
        public const int adr_bitis_gerilim = 34;
        public const int adr_test_sonuc = 42;
        public const int adr_test_bitti = 46;
        public const int adr_tork_algilama = 38;

        public bool TestStart { get; set; }

        public int Frekans { get; set; }

        public float BaslangicGerilim { get; set; }

        public float BitisGerilim { get; set; }

        public float TorkAlgilama { get; set; }

        public float TestSonuc { get; set; }

        public bool TestBitti { get; set; }
    }
}
