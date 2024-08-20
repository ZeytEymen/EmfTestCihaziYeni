using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EmfTestCihazi.Classes.TabControlHelper;
using static EmfTestCihazi.Classes.InputValidationHelper;
using EmfTestCihazi.Classes;
using Sharp7;
using EmfTestCihazi.Classes.PlcCommunication;


namespace EmfTestCihazi
{
    public partial class MainForm : Form
    {
        public enum TEST
        {
            RODAJ = 1,
            YBF_BIRAKMA = 2,
            YBF_YAKALAMA = 3,
            YBF_DINAMIK = 4,
            YBF_STATIK = 5,
            ABTF_TEST = 6
        }

        TEST AktifTest;
        int _testSure = 0;

        //Helpers//
        private readonly DBHelper _DB;
        private readonly DBLogHelper _DBlog;

        //PLC Haberleşmesi İçin//
        //
        private readonly S7Client _plc;
        const string _plcConnectionAdress = "192.168.69.70";
        const int _plcConnectionRack = 0;
        const int _plcConnectionSlot = 0;
        byte[] _buffer = new byte[140];
        //
        public AbtfTest _abtfTest;
        public Alistirma _alistirma;
        public GlobalValues _globalValues;
        public YbfBirakma _ybfBirakma;
        public YbfYakalama _ybfYakalama;
        public YbfDinamik _ybfDinamik;
        public YbfStatik _ybfStatik;

        //Program içi Global Değişkenler//
        //test esnasında istenilen buton click eventlerini programı hataya sokmaması için eventleri geçersiz kılmaya yarıyor
        bool disableButtonClicks = false;

        public MainForm()
        {
            InitializeComponent();
            _DB = new DBHelper();
            _DBlog = new DBLogHelper(_DB);
            _plc = new S7Client();
            _abtfTest = new AbtfTest();
            _alistirma = new Alistirma();
            _globalValues = new GlobalValues();
            _ybfBirakma = new YbfBirakma();
            _ybfYakalama = new YbfYakalama();
            _ybfDinamik = new YbfDinamik();
            _ybfStatik = new YbfStatik();
        }


        #region PLC_METHODLARI

        public int StartPlcConnection()
        {
            int connectResult = -1;
            try
            {
                connectResult = _plc.ConnectTo(_plcConnectionAdress, _plcConnectionRack, _plcConnectionSlot);
                if (connectResult != 0)
                    throw new Exception(_plc.ErrorText(connectResult));
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantısı Başarısız\nHATA KODU : " + $"0x{connectResult:X5}\nHATA AÇIKLAMASI : \n" + ex, "HATA !!");
                _DBlog.AddLog("PLC Bağlantısı Başarısız\nHATA KODU : " + $"0x{connectResult:X5}\nHATA AÇIKLAMASI : \n" + ex);
            }
            return connectResult;
        }

        public int StopPlcConnection()
        {
            int stopResult = 0;
            try
            {
                if (_plc.Connected)
                {
                    stopResult = _plc.Disconnect();
                    if (stopResult != 0)
                        throw new Exception(_plc.ErrorText(stopResult));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantısı Sonlandırılamadı\nHATA KODU : " + $"0x{stopResult:X5}\nHATA AÇIKLAMASI : \n" + ex, "HATA !!");
                _DBlog.AddLog("PLC Bağlantısı Sonlandırılamadız\nHATA KODU : " + $"0x{stopResult:X5}\nHATA AÇIKLAMASI : \n" + ex);
            }
            return stopResult;
        }

        public void StartReadindPlcDataBlock()
        {
            try
            {
                if (StartPlcConnection() == 0)
                    tmrMain.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("\"PLC Bağlantısı Başarısız\nFROM : StartReadindPlcDataBlock" + ex.Message, "HATA");
            }

        }

        public void StopReadindPlcDataBlock()
        {
            try
            {
                tmrMain.Stop();
                StopPlcConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantısı Sonlandırılamadı\nFROM : StopReadindPlcDataBlock" + ex.Message, "HATA");
            }
        }



        public void ReadAllDataBlock()
        {
            int readResult = -1;
            try
            {
                readResult = _plc.DBRead(100, 0, _buffer.Length, _buffer);
                if (readResult != 0)
                    throw new Exception(_plc.ErrorText(readResult));

                _globalValues.ACT_VOLT = S7.GetRealAt(_buffer, GlobalValues.adr_ACT_VOLT);
                _globalValues.ACT_TORK = S7.GetRealAt(_buffer, GlobalValues.adr_ACT_TORK);
                _globalValues.ACT_AKIM = S7.GetRealAt(_buffer, GlobalValues.adr_ACT_AKIM);
                _globalValues.ACIL_STOP = S7.GetBitAt(_buffer, GlobalValues.adr_ACIL_STOP, 0);
                _globalValues.FREN_24 = S7.GetBitAt(_buffer, GlobalValues.adr_FREN_24, 0);
                _globalValues.FREN_220 = S7.GetBitAt(_buffer, GlobalValues.adr_FREN_220, 0);
                _globalValues.CMD_VOLT = S7.GetRealAt(_buffer, GlobalValues.adr_CMD_VOLT);

                _ybfYakalama.TestStart = S7.GetBitAt(_buffer, YbfYakalama.adr_test_basla, 0);
                _ybfYakalama.Frekans = S7.GetIntAt(_buffer, YbfYakalama.adr_frekans);
                _ybfYakalama.BaslangicGerilim = S7.GetRealAt(_buffer, YbfYakalama.adr_baslangic_gerilimi);
                _ybfYakalama.BitisGerilim = S7.GetRealAt(_buffer, YbfYakalama.adr_bitis_gerilim);
                _ybfYakalama.TorkAlgilama = S7.GetRealAt(_buffer, YbfYakalama.adr_algilama_tork);
                _ybfYakalama.TestSonuc = S7.GetRealAt(_buffer, YbfYakalama.adr_test_sonuc);
                _ybfYakalama.TestBitti = S7.GetBitAt(_buffer, YbfYakalama.adr_test_bitti, 0);

                _ybfBirakma.TestStart = S7.GetBitAt(_buffer, YbfBirakma.adr_test_basla, 0);
                _ybfBirakma.Frekans = S7.GetIntAt(_buffer, YbfBirakma.adr_frekans);
                _ybfBirakma.BaslangicGerilim = S7.GetRealAt(_buffer, YbfBirakma.adr_baslangic_gerilimi);
                _ybfBirakma.BitisGerilim = S7.GetRealAt(_buffer, YbfBirakma.adr_bitis_gerilim);
                _ybfBirakma.TorkAlgilama = S7.GetRealAt(_buffer, YbfBirakma.adr_tork_algilama);
                _ybfBirakma.TestSonuc = S7.GetRealAt(_buffer, YbfBirakma.adr_test_sonuc);
                _ybfBirakma.TestBitti = S7.GetBitAt(_buffer, YbfBirakma.adr_test_bitti, 0);

                _ybfStatik.TestStart = S7.GetBitAt(_buffer, YbfStatik.adr_test_basla, 0);
                _ybfStatik.Frekans = S7.GetIntAt(_buffer, YbfStatik.adr_frekans);
                _ybfStatik.BaslangicGerilim = S7.GetRealAt(_buffer, YbfStatik.adr_baslangic_gerilimi);
                _ybfStatik.BitisGerilim = S7.GetRealAt(_buffer, YbfStatik.adr_bitis_gerilim);
                _ybfStatik.TorkAlgilama = S7.GetRealAt(_buffer, YbfStatik.adr_algilama_tork);
                _ybfStatik.TestSonuc = S7.GetRealAt(_buffer, YbfStatik.adr_test_sonuc);
                _ybfStatik.TestBitti = S7.GetBitAt(_buffer, YbfStatik.adr_test_bitti, 0);

                _ybfDinamik.TestStart = S7.GetBitAt(_buffer, YbfDinamik.adr_test_basla, 0);
                _ybfDinamik.Frekans = S7.GetIntAt(_buffer, YbfDinamik.adr_frekans);
                _ybfDinamik.BaslangicGerilim = S7.GetRealAt(_buffer, YbfDinamik.adr_baslangic_gerilimi);
                _ybfDinamik.BitisGerilim = S7.GetRealAt(_buffer, YbfDinamik.adr_bitis_gerilim);
                _ybfDinamik.TestSure = S7.GetIntAt(_buffer, YbfDinamik.adr_test_sure);
                _ybfDinamik.TestSonuc = S7.GetRealAt(_buffer, YbfDinamik.adr_test_sonuc);
                _ybfDinamik.TestBitti = S7.GetBitAt(_buffer, YbfDinamik.adr_test_bitti, 0);

                _alistirma.TestBasla = S7.GetBitAt(_buffer, Alistirma.adr_test_basla, 0);
                _alistirma.Frekans = S7.GetIntAt(_buffer, Alistirma.adr_frekans);
                _alistirma.FrenVoltaj = S7.GetRealAt(_buffer, Alistirma.adr_fren_voltaj);
                _alistirma.SureSag = S7.GetIntAt(_buffer, Alistirma.adr_düz_süre);
                _alistirma.SureSol = S7.GetIntAt(_buffer, Alistirma.adr_ters_süre);
                _alistirma.FrenAcikSure = S7.GetIntAt(_buffer, Alistirma.adr_fren_ac_süre);
                _alistirma.FrenKapalıSure = S7.GetIntAt(_buffer, Alistirma.adr_fren_kapa_süre);
                _alistirma.TestBitti = S7.GetBitAt(_buffer, Alistirma.adr_test_bitti, 0);

                _abtfTest.TestBasla = S7.GetBitAt(_buffer, AbtfTest.adr_test_basla, 0);
                _abtfTest.Frekans = S7.GetIntAt(_buffer, AbtfTest.adr_frekans);
                _abtfTest.TestBitti = S7.GetBitAt(_buffer, AbtfTest.adr_test_bitti, 0);

            }
            catch (Exception ex)
            {
                StopReadindPlcDataBlock();
                StopPlcConnection();
                MessageBox.Show("PLC Data Block Okuması Başarısız\nHATA KODU : " + $"0x{readResult:X5}\nHATA AÇIKLAMASI : \n" + ex, "HATA !!");
                _DBlog.AddLog("PLC Data Block Okuması Başarısız\nHATA KODU : " + $"0x{readResult:X5}\nHATA AÇIKLAMASI : \n" + ex);
            }
        }




        #endregion

        #region Form Eventleri
        private void MainForm_Load(object sender, EventArgs e)
        {
            // _DBlog.AddLog("Program Çalıştırıldı");//Programının ilk açılısında dbye log atıyorum

            //Sidebardaki butonları kullanmak için TabControl butonlarını gizler
            HideTabControlHeaderButtons(tabControlMain);
            //Açılışta default olarak Ybf Test İşlem Sayfasını Açıyorum
            tabControlMain.SelectedTab = tabControlMain.TabPages["tbPageYbfTestIslem"];

            // Herhangi bir veri yenileme butonuna click eventi verdim sayfa açıldığında boş veriler gözükmesin diye
          

            //  StartReadindPlcDataBlock();
            //tmrMain.Start();

        }

        //Uygulamayı görev çubuğuna indirir
        private void btnMinimizeApplicaton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //Uygulamayı Kapatır
        private void btnCloseApplication_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region OrtakEventler
        //Sidebar butonlarının ortak click olayı
        private void SideBarButtons_Click(object sender, EventArgs e)
        {
            if (disableButtonClicks)
            {
                MessageBox.Show("Test esnasında diğer sayfalara erişim sağlayamazsınız\nLütfen testin bitmesini bekleyin.", "HATA !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Sidebar üzerindeki butonların tıklanma olayında ilgili tabpage sayfası açılır
            OpenTabPageWithButton(sender, tabControlMain);
        }

        //Girdi olarak sadece Tam Sayı kabul edilmesi istenen textboxların ortak keypress(tuşa basılma) eventi
        private void txtBoxKeyPressOnlyDigit(object sender, KeyPressEventArgs e)
        {
            //Sadece TamSayı ve kontrol butonları(backspace ctrl kombinasyonları vs)
            ValidateOnlyDigit(e);
        }
        #endregion

        #region Methodlar
        private bool CheckButtonClick()
        {
            if (disableButtonClicks)
            {
                MessageBox.Show("Test esnasında diğer kontrollere eririm sağlayamazsınız\nLütfen testin bitmesini bekleyin.", "HATA !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        #endregion

        #region TimerTicks
        private void tmrMain_Tick(object sender, EventArgs e)
        {
            //ReadActualValues();

            //txt_ACT_VOLT.Text = float.Parse(d).ToString();
            //txt_ACT_VOLT.Text = _globalValues.ACT_VOLT.ToString();
            //txt_ACT_TORK.Text = _globalValues.ACT_TORK.ToString();
            //txt_ACT_AKIM.Text = _globalValues.ACT_AKIM.ToString();

            if (_globalValues.ACIL_STOP)
                flashTimer.Start();
            else
            {
                flashTimer.Stop();
                pctEmergencyStop.Visible = true;
            }
        }

        private void flashTimer_Tick(object sender, EventArgs e)
        {
            pctEmergencyStop.Visible = !pctEmergencyStop.Visible;
        }

        private void tmrTest_Tick(object sender, EventArgs e)
        {
            _testSure++;
            switch (AktifTest)
            {
                case TEST.RODAJ:
                    baseChartAndGridViewAbtfAlistirma.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    baseChartAndGridViewYbfAlistirma.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    if (_alistirma.TestBitti)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Rodaj Testi Bitti";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    if (_globalValues.ACIL_STOP)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Rodaj Testi 'ACİL STOP ' ile sonlandırıldı";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    break;

                case TEST.YBF_BIRAKMA:
                    baseChartAndGridViewBirakma.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    if (_ybfBirakma.TestBitti)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Bırakma Gerilimi Ölçümü Testi Bitti";
                        txtBirakmaTestSonuc.Text = _ybfBirakma.TestSonuc.ToString();
                        txtTestIslemBirakmaSonuc.Text = _ybfBirakma.TestSonuc.ToString();
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    if (_globalValues.ACIL_STOP)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Bırakma Gerilimi Ölçümü 'ACİL STOP ' ile sonlandırıldı";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    break;

                case TEST.YBF_YAKALAMA:
                    baseChartAndGridViewYakalama.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    if (_ybfYakalama.TestBitti)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Yakalama Gerilimi Ölçümü Testi Bitti";
                        txtYakalamaTestSonuc.Text = _ybfYakalama.TestSonuc.ToString();
                        txtTestIslemYakalamaSonuc.Text = _ybfYakalama.TestSonuc.ToString();
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    if (_globalValues.ACIL_STOP)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Yakalama Gerilimi Ölçümü 'ACİL STOP ' ile sonlandırıldı";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    break;

                case TEST.YBF_DINAMIK:
                    baseChartAndGridViewDinamik.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    if (_ybfDinamik.TestBitti)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Dinamik Tork Ölçümü Testi Bitti";
                        txtDinamikTestSonuc.Text = _ybfDinamik.TestSonuc.ToString();
                        txtTestIslemDinamikSonuc.Text = _ybfDinamik.TestSonuc.ToString();
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    if (_globalValues.ACIL_STOP)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Dinamik Tork Ölçümü 'ACİL STOP ' ile sonlandırıldı";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    break;

                case TEST.YBF_STATIK:
                    baseChartAndGridViewStatik.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    if (_ybfStatik.TestBitti)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Statik Tork Ölçümü Testi Bitti";
                        txtStatikTestSonuc.Text = _ybfStatik.TestSonuc.ToString();
                        txtTestIslemStatikSonuc.Text = _ybfStatik.TestSonuc.ToString();
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    if (_globalValues.ACIL_STOP)
                    {
                        tmrTest.Stop();
                        txtState.Text = "Statik Tork Ölçümü 'ACİL STOP ' ile sonlandırıldı";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    break;

                case TEST.ABTF_TEST:
                    baseChartAndGridViewAbtfTest.AddValues(_testSure, _globalValues.ACT_VOLT, _globalValues.ACT_TORK, _globalValues.ACT_AKIM);
                    if (_abtfTest.TestBitti)
                    {
                        tmrTest.Stop();
                        txtState.Text = "ABTF Testi Bitti";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    if (_globalValues.ACIL_STOP)
                    {
                        tmrTest.Stop();
                        txtState.Text = "ABTF Testi 'ACİL STOP ' ile sonlandırıldı";
                        disableButtonClicks = false;
                        _testSure = 0;
                    }
                    break;

                default:
                    tmrTest.Stop();
                    MessageBox.Show("Beklenmeyen Şekilde Test Başlatıldı");
                    disableButtonClicks = false;
                    _testSure = 0;
                    break;
            }
        }

        #endregion

        #region TestBaşlamaClickEventleri

        private void btn_abtf_alistirma_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.RODAJ;
            disableButtonClicks = true;
            txtState.Text = "Rodaj Testine Başlandı";
            baseChartAndGridViewAbtfAlistirma.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private void btn_ybf_alistirma_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.RODAJ;
            disableButtonClicks = true;
            txtState.Text = "Rodaj Testine Başlandı";
            baseChartAndGridViewYbfAlistirma.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private void btn_abtf_test_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.ABTF_TEST;
            disableButtonClicks = true;
            txtState.Text = "ABTF Testine Başlandı";
            baseChartAndGridViewAbtfTest.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private void btn_ybf_birakma_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.YBF_BIRAKMA;
            disableButtonClicks = true;
            txtState.Text = "Bırakma Gerilimi Ölçümü Testine Başlandı";
            baseChartAndGridViewBirakma.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private void btn_ybf_yakalama_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.YBF_YAKALAMA;
            disableButtonClicks = true;
            txtState.Text = "Yakalama Gerilimi Ölçümü Testine Başlandı";
            baseChartAndGridViewYakalama.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private void btn_ybf_dinamik_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.YBF_DINAMIK;
            disableButtonClicks = true;
            txtState.Text = "Dinamik Tork Ölçümü Testine Başlandı";
            baseChartAndGridViewDinamik.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private void btn_ybf_statik_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            AktifTest = TEST.YBF_STATIK;
            disableButtonClicks = true;
            txtState.Text = "Statik Tork Ölçümü Testine Başlandı";
            baseChartAndGridViewStatik.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        #endregion


        private void radio_testOnAyar_checkedChanged(object sender, EventArgs e)
        {
            if (radio_testOnAyar_yeniKayit.Checked)
            {
                grpBox_testOnAyar_yeniKayit.Visible = true;
                grpBox_testOnAyar_varolanKayit.Visible = false;
            }
            else
            {
                grpBox_testOnAyar_yeniKayit.Visible = false;
                grpBox_testOnAyar_varolanKayit.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tmrTest.Stop();
            disableButtonClicks = false;
            tmrMain.Stop();
        }

        //Plcdeki verileri sayfadan okumak için olan alanın ortak okuma eventi
        private void RefreshButtons_Click(object sender, EventArgs e)
        {
            string now = DateTime.Now.ToShortTimeString();

            lblInfoAbtfAlistirmaSonOkuma.Text = now;
            lblInfoAbtfAlistirmaFrekans.Text = _alistirma.Frekans.ToString();
            lblInfoAbtfAlistirmaFrenVoltaj.Text = _alistirma.FrenVoltaj.ToString();
            lblInfoAbtfAlistirmaFrenAcik.Text = _alistirma.FrenAcikSure.ToString();
            lblInfoAbtfAlistirmaFrenKapali.Text = _alistirma.FrenKapalıSure.ToString();
            lblInfoAbtfAlistirmaSagaDonus.Text = _alistirma.SureSag.ToString();
            lblInfoAbtfAlistirmaSolaDonus.Text = _alistirma.SureSol.ToString();

            lblInfoYbfAlistirmaSonOkuma.Text = now;
            lblInfoYbfAlistirmaFrekans.Text = _alistirma.Frekans.ToString();
            lblInfoYbfAlistirmaFrenVoltaj.Text = _alistirma.FrenVoltaj.ToString();
            lblInfoYbfAlistirmaFrenAcik.Text = _alistirma.FrenAcikSure.ToString();
            lblInfoYbfAlistirmaFrenKapali.Text = _alistirma.FrenKapalıSure.ToString();
            lblInfoYbfAlistirmaSagaDonus.Text = _alistirma.SureSag.ToString();
            lblInfoYbfAlistirmaSolaDonus.Text = _alistirma.SureSol.ToString();

            lblInfoYakalamaSonOkuma.Text = now;
            lblInfoYakalamaFrekans.Text = _ybfYakalama.Frekans.ToString();
            lblInfoYakalamaBaslangicGerilim.Text = _ybfYakalama.BaslangicGerilim.ToString();
            lblInfoYakalamaBitisGerilim.Text = _ybfYakalama.BitisGerilim.ToString();
            lblInfoYakalamaTorkSeviye.Text = _ybfYakalama.TorkAlgilama.ToString();

            lblInfoBirakmaSonOkuma.Text = now;
            lblInfoBirakmaFrekans.Text = _ybfBirakma.Frekans.ToString();
            lblInfoBirakmaBaslangicGerilim.Text = _ybfBirakma.BaslangicGerilim.ToString();
            lblInfoBirakmaBitisGerilim.Text = _ybfBirakma.BitisGerilim.ToString();
            lblInfoBirakmaTorkSeviye.Text = _ybfBirakma.TorkAlgilama.ToString();

            lblInfoStatikSonOkuma.Text = now;
            lblInfoStatikFrekans.Text = _ybfStatik.Frekans.ToString();
            lblInfoStatikBaslangicGerilim.Text = _ybfStatik.BaslangicGerilim.ToString();
            lblInfoStatikBitisGerilim.Text = _ybfStatik.BitisGerilim.ToString();
            lblInfoStatikTorkSeviye.Text = _ybfStatik.TorkAlgilama.ToString();

            lblInfoDinamikSonOkuma.Text = now;
            lblInfoDinamikFrekans.Text = _ybfDinamik.Frekans.ToString();
            lblInfoDinamikBaslangicGerilim.Text = _ybfDinamik.BaslangicGerilim.ToString();
            lblInfoDinamikBitisGerilim.Text = _ybfDinamik.BitisGerilim.ToString();
            lblInfoDinamikTestSure.Text = _ybfDinamik.TestSure.ToString();
        }

        private void btn_abtf_alistirma_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_abtf_alistirma_frekans.Text = _alistirma.Frekans.ToString();
            txt_abtf_alistirma_fren_voltaj.Text =_alistirma.FrenVoltaj.ToString();
            txt_abtf_alistirma_fren_acik.Text = _alistirma.FrenAcikSure.ToString();
            txt_abtf_alistirma_fren_kapali.Text = _alistirma.FrenKapalıSure.ToString();
            txt_abtf_alistirma_saga_donus.Text = _alistirma.SureSag.ToString();
            txt_abtf_alistirma_sola_donus.Text = _alistirma.SureSol.ToString();
        }

        private void btn_ybf_alistirma_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_ybf_alistirma_frekans.Text = _alistirma.Frekans.ToString();
            txt_ybf_alistirma_fren_voltaj.Text = _alistirma.FrenVoltaj.ToString();
            txt_ybf_alistirma_fren_acik.Text = _alistirma.FrenAcikSure.ToString();
            txt_ybf_alistirma_fren_kapali.Text = _alistirma.FrenKapalıSure.ToString();
            txt_ybf_alistirma_saga_donus.Text = _alistirma.SureSag.ToString();
            txt_ybf_alistirma_sola_donus.Text = _alistirma.SureSol.ToString();
        }

        private void btn_birakma_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_birakma_frekans.Text = _ybfBirakma.Frekans.ToString();
            txt_birakma_baslangic_gerilim.Text = _ybfBirakma.BaslangicGerilim.ToString();
            txt_birakma_bitis_gerilim.Text = _ybfBirakma.BitisGerilim.ToString();
            txt_birakma_tork_seviye.Text = _ybfBirakma.TorkAlgilama.ToString();
        }

        private void btn_yakalama_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_yakalama_frekans.Text = _ybfYakalama.Frekans.ToString();
            txt_yakalama_baslangic_gerilim.Text = _ybfYakalama.BaslangicGerilim.ToString();
            txt_yakalama_bitis_gerilim.Text = _ybfYakalama.BitisGerilim.ToString();
            txt_yakalama_tork_seviye.Text = _ybfYakalama.TorkAlgilama.ToString();
        }

        private void btn_dinamik_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_dinamik_frekans.Text = _ybfDinamik.Frekans.ToString();
            txt_dinamik_baslangic_gerilim.Text = _ybfDinamik.BaslangicGerilim.ToString();
            txt_dinamik_bitis_gerilim.Text = _ybfDinamik.BitisGerilim.ToString();
            txt_dinamik_test_sure.Text = _ybfDinamik.TestSure.ToString();
        }

        private void btn_statik_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_statik_frekans.Text = _ybfStatik.Frekans.ToString();
            txt_statik_baslangic_gerilim.Text = _ybfStatik.BaslangicGerilim.ToString();
            txt_statik_bitis_gerilim.Text = _ybfStatik.BitisGerilim.ToString();
            txt_statik_tork_seviye.Text = _ybfStatik.TorkAlgilama.ToString();
        }
    }
}


/*
 *   private void btnRefreshYbfAlistirma_Click(object sender, EventArgs e)
        {
            //  GetBiseyBisey();
            // lblInfoYbfAlistirmaSonOkuma.Text = DateTime.Now.ToShortTimeString();
            lblInfoYbfAlistirmaFrekans.Text = _alistirma.Frekans.ToString();
            lblInfoYbfAlistirmaFrenAcik.Text = _alistirma.FrenAcikSure.ToString();
            lblInfoYbfAlistirmaFrenKapali.Text = _alistirma.FrenKapalıSure.ToString();
            lblInfoYbfAlistirmaSagaDonus.Text = _alistirma.SureSag.ToString();
            lblInfoYbfAlistirmaSolaDonus.Text = _alistirma.SureSol.ToString();
        }
 */