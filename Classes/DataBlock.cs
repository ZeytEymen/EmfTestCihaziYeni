using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmfTestCihazi.Classes
{
    public class DataBlock
    {
        /*
         * TIA          C#
         * bool         bool
         * real         float
         * int          short
         */
        public bool TEST_START { get; set; }
        public float FREN_VOLTAJ { get; set; }
        public short FREN_ACIK_SURE { get; set; }
        public short FREN_KAPALI_SURE { get; set; }
        public short SAGA_DONUS { get; set; }
        public short SOLA_DONUS { get; set; }
        public bool TEST_BITTI { get; set; }
        public bool ACIL_STOP { get; set; } // offset 14.1
        // between 14.1 and 15.9 is padding then ->
        public short TEST { get; set; } // offset 16.0



       // public byte padding1;
        public short TEST2 { get; set; } // offset 18.0 test3 should be 18.1 but because of 

        public short TEST3 { get; set; }

    }

}

/*	TEST_START	Bool	0.0	FALSE	False	True	True	True	False	
	FREN_VOLTAJ	Real	2.0	16#0000_0058	False	True	True	True	False	
	FREN_ACIK_SURE	Int	6.0	0	False	True	True	True	False	
	FREN_KAPALI_SURE	Int	8.0	0	False	True	True	True	False	
	SAGA_DONUS	Int	10.0	0	False	True	True	True	False	
	SOLA_DONUS	Int	12.0	0	False	True	True	True	False	
	TEST_BITTI	Bool	14.0	FALSE	False	True	True	True	False	
	ACIL_STOP	Bool	14.1	FALSE	False	True	True	True	False	
	DENEME	Bool	14.2	FALSE	False	True	True	True	False	
*/