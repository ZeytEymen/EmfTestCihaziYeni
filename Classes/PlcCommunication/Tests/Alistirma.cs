using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes.PlcCommunication
{
    public class Alistirma
    {
        public const int adr_test_basla = 70;
        public const int adr_frekans = 72;
        public const int adr_fren_voltaj = 74;
        public const int adr_düz_süre = 78;
        public const int adr_fren_ac_süre = 80;
        public const int adr_fren_kapa_süre = 82;
        public const int adr_ters_süre = 84;
        public const int adr_test_bitti = 86;

        public bool TestBasla { get; set; }
        public short Frekans { get; set; }
        public float FrenVoltaj { get; set; }
        public short FrenAcikSure { get; set; }
        public short FrenKapalıSure { get; set; }
        public short SureSag { get; set; }
        public short SureSol { get; set; }
        public bool TestBitti { get; set; }
    }
}
