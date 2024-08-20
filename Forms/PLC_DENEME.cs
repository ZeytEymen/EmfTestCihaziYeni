using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp7;
using EmfTestCihazi.Classes;

namespace EmfTestCihazi.Forms
{
    public partial class PLC_DENEME : Form
    {
        public PLC_DENEME()
        {
            InitializeComponent();
        }
        DataBlock dblock = new DataBlock();
        private S7Client _plc;
       // const string _plcConnectionAdress = "10.10.1.86";
        const string _plcConnectionAdress = "192.168.69.70";

        const int _plcConnectionRack = 0;
        const int _plcConnectionSlot = 1;
        byte[] _buffer = new byte[16];


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //   var plc = new S7().pl
                _plc = new S7Client();
                int result = _plc.ConnectTo(_plcConnectionAdress, _plcConnectionRack, _plcConnectionSlot);
                if (result == 0)
                {
                    MessageBox.Show("Bağlantı Başarılı");
                }
                else
                    MessageBox.Show("Bağlantı Başarısız" + _plc.ErrorText(result));

            }
            catch (Exception)
            {

                throw;
            }
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
          //  PLC.Open();
            timer1.Start();

            //var ACIL_STOP = PLC.Read("DB1.DBX14.1");
           // double val = 88;
         //   PLC.Write("DB1.DBD2", (float)6);
            //MessageBox.Show(ACIL_STOP.ToString());
        }

        DataBlock aaa = new DataBlock();
        // DataBlock.ALISTIRMA  sssAlistir = new DataBlock.ALISTIRMA();
        public struct ALISTIRMA
        {
            public bool deneme2 { get; set; }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int db = 1;
            //if (PLC.IsConnected)
            //    PLC.ReadClass(aaa, db);
        
            l_test_start.Text = aaa.TEST_START.ToString();
            l_fren_volt.Text = aaa.FREN_VOLTAJ.ToString();
            l_fren_acik.Text = aaa.FREN_ACIK_SURE.ToString();
            l_fren_kaapli.Text = aaa.FREN_KAPALI_SURE.ToString();
            l_sag.Text = aaa.SAGA_DONUS.ToString();
            l_sol.Text = aaa.SOLA_DONUS.ToString();
            l_test_Biii.Text = aaa.TEST_BITTI.ToString();
            l_acs.Text = aaa.ACIL_STOP.ToString();
            l_deneme.Text = aaa.TEST.ToString();
            l_deneme2.Text = aaa.TEST2.ToString();
            l_deneme3.Text = aaa.TEST3.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if (!PLC.IsConnected)
            //    return;
            //PLC.Write("DB1.DBX0.0", bool.Parse(t_start.Text));
            //PLC.Write("DB1.DBD2.0", float.Parse(t_volt.Text));
            //PLC.Write("DB1.DBW6.0", short.Parse(t_acik.Text));
            //PLC.Write("DB1.DBW8.0", short.Parse(t_kapali.Text));
            //PLC.Write("DB1.DBW10.0", short.Parse(t_sag.Text));
            //PLC.Write("DB1.DBW12.0", short.Parse(t_sol.Text));
            //PLC.Write("DB1.DBX14.0", bool.Parse(t_bitti.Text));
            //PLC.Write("DB1.DBX14.1", bool.Parse(t_acs.Text));
            //PLC.Write("DB1.DBX14.2", bool.Parse(t_deneme.Text));


        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void PLC_DENEME_Load(object sender, EventArgs e)
        {

        }
    }
}
/*
 *     // DataBlock.ALISTIRMA aalistir = (DataBlock.ALISTIRMA)PLC.ReadStruct(typeof(DataBlock.ALISTIRMA), db,16);
            // var denemee = PLC.ReadStruct(typeof(DataBlock.ALISTIRMA), db, 14);
            // ALISTIRMAS aLISTIRMAS = (ALISTIRMAS)PLC.ReadStruct(typeof(ALISTIRMAS), 16);
            // var dataBlock = PLC.ReadClass<DataBlock>(1); // 1 burada Data Block numarasıdır

            // ALISTIRMA yapısını oku (bu örnekte DB1, byte offset 16'dan başlar)
          //  var alistirmaStruct = PLC.ReadStruct<ALISTIRMA>(1, 16); // 16 byte offset değeri
                                                                    //   ALISTIRMA alistirmaStruc1t = (ALISTIRMA)PLC.ReadStruct<ALISTIRMA>(1,16) // 16 byte offset değeri


 */