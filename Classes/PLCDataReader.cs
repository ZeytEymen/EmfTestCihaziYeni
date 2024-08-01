using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmfTestCihazi.Forms.ToolForms;
using MySqlX.XDevAPI;
using Sharp7;
using System.Windows.Forms;
using ZstdSharp.Unsafe;

namespace EmfTestCihazi.Classes
{
    public class PLCDataReader
    {
        public bool _isCollectting = false;
        private const int _readerInterval = 100;
        private readonly S7Client _client;
        private byte[] _buffers = new byte[176];

        #region PLC DATA BLOCK ADRESLERİ
        //AKTÜEL VERİLER VE KOMUT ADRESLERİ
        public const int adr_ACT_VOLT = 4;
        public const int adr_ACT_TORK = 8;
        public const int adr_ACT_AKIM = 14;
        public const int adr_FREN_220 = 18;
        public const int adr_FREN_24 = 24;
        public const int adr_CMD_VOLT = 4;
        //YBF ALIŞTIRMA
        public const int adr_YBF_alistirma_test_basla = 70;
        public const int adr_YBF_alistirma_frekans = 72;
        public const int adr_YBF_alistirma_baslangic_gerilimi = 74;
        public const int adr_YBF_alistirma_düz_süre = 78;
        public const int adr_YBF_alistirma_fren_ac_süre = 80;
        public const int adr_YBF_alistirma_fren_kapa_süre = 82;
        public const int adr_YBF_alistirma_ters_süre = 84;
        public const int adr_YBF_alistirma_test_bitti = 86;
        //YBF YAKALAMA
        public const int adr_YBF_yakalama_test_basla = 112;
        public const int adr_YBF_yakalama_frekans = 114;
        public const int adr_YBF_yakalama_baslangic_gerilimi = 116;
        public const int adr_YBF_yakalama_bitis_gerilim = 120;
        public const int adr_YBF_yakalama_algilama_tork = 124;
        public const int adr_YBF_yakalama_test_sonuc = 128;
        public const int adr_YBF_yakalama_test_bitti = 132;
        //YBF BIRAKMA
        public const int adr_YBF_birakma_test_basla = 26;
        public const int adr_YBF_birakma_frekans = 28;
        public const int adr_YBF_birakma_baslangic_gerilimi = 30;
        public const int adr_YBF_birakma_bitis_gerilim = 34;
        public const int adr_YBF_birakma_test_sonuc = 42;
        public const int adr_YBF_birakma_test_bitti = 46;
        public const int adr_YBF_birakma_tork_algilama = 38;
        //YBF DİNAMİK
        public const int adr_YBF_dinamik_test_basla = 88;
        public const int adr_YBF_dinamik_frekans = 90;
        public const int adr_YBF_dinamik_baslangic_gerilimi = 92;
        public const int adr_YBF_dinamik_bitis_gerilim = 98;
        public const int adr_YBF_dinamik_test_sure = 96;
        public const int adr_YBF_dinamik_test_sonuc = 106;
        public const int adr_YBF_dinamik_test_bitti = 110;
        //YBF STATİK
        public const int adr_test_basla = 48;
        public const int adr_frekans = 50;
        public const int adr_baslangic_gerilimi = 52;
        public const int adr_bitis_gerilim = 56;
        public const int adr_algilama_tork = 60;
        public const int adr_test_sonuc = 64;
        public const int adr_test_bitti = 68;
        #endregion
        ///public double ACT_Volt;
        //private  double act_volt;
        public double ACT_Volt { get; set; }
        //public static double ACT_Tork { get; set; }
        //public static double ACT_Akim { get; set; }

        public PLCDataReader(S7Client client)
        {
            _client = client;
        }
        public async Task StartReadingData()
        {
            _isCollectting = true;
            try
            {
                //_client.ConnectTo("192.168.69.70", 0, 0);
                await Task.Run(() => ReadPlcData());
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantı Hatası : " + ex.Message);
            }

        }
        public void StopReadingData()
        {
            _isCollectting = false;
            _client.Disconnect();
        }
        private async Task ReadPlcData()
        {
            Random rndm = new Random();
            while (_isCollectting)
            {
                try
                {
                    ACT_Volt = rndm.NextDouble() * 50;
                    //_client.DBRead(100, 0, _buffers.Length, _buffers);
                    //ACT_Volt = S7.GetRealAt(_buffers, adr_ACT_VOLT);
                    //ACT_Tork = S7.GetRealAt(_buffers, adr_ACT_TORK);
                    //ACT_Akim = S7.GetRealAt(_buffers, adr_ACT_AKIM);

                }
                catch (Exception ex)
                {
                    StopReadingData();
                    MessageBox.Show("ReadPlcData esnasında hata " + ex.Message);
                }

                await Task.Delay(_readerInterval);
            }
        }
    }
}
