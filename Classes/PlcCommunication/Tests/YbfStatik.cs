using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class YbfStatik
    {
        public const int adr_test_basla = 48;
        public const int adr_frekans = 50;
        public const int adr_baslangic_gerilimi = 52;
        public const int adr_bitis_gerilim = 56;
        public const int adr_algilama_tork = 60;
        public const int adr_test_sonuc = 64;
        public const int adr_test_bitti = 68;

        public bool TestStart { get; set; }

        public int Frekans { get; set; }

        public float BaslangicGerilim { get; set; }

        public float BitisGerilim { get; set; }

        public float TorkAlgilama { get; set; }

        public float TestSonuc { get; set; }

        public bool TestBitti { get; set; }
    }
}
