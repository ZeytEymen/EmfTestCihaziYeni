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
using EmfTestCihazi.Forms.ToolForms;
using MySql.Data.MySqlClient;
using System.Threading;
using EmfTestCihazi.Forms;
using System.Security.Cryptography;
using System.Timers;
using System.Runtime.CompilerServices;
using Org.BouncyCastle.Asn1.Cmp;
using Newtonsoft.Json;

namespace EmfTestCihazi
{
    public partial class MainForm : Form
    {
        #region GLOBAL VARIABLES
        //Global Program İçi
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
        //PLC Haberleşmesi İçin//
        //
        private readonly S7Client _plc;
        //const string _plcConnectionAdress = "192.168.69.70";
        const string _plcConnectionAdress = "10.10.1.86";

        const int _plcConnectionRack = 0;
        const int _plcConnectionSlot = 0;
        byte[] _buffer = new byte[140];
        //DataBlock Erişimi
        public Classes.PlcCommunication.AbtfTest _abtfTest;
        public Alistirma _alistirma;
        public GlobalValues _globalValues;
        public YbfBirakma _ybfBirakma;
        public YbfYakalama _ybfYakalama;
        public YbfDinamik _ybfDinamik;
        public YbfStatik _ybfStatik;
        //Program içi Global Değişkenler//
        //test esnasında istenilen buton click eventlerini programı hataya sokmaması için eventleri geçersiz kılmaya yarıyor
        bool disableButtonClicks = false;
        #endregion
        public MainForm()
        {
            InitializeComponent();
            _DB = new DBHelper();
            _plc = new S7Client();
            _abtfTest = new Classes.PlcCommunication.AbtfTest();
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
                _DB.AddLog("PLC Bağlantısı Başarısız\nHATA KODU : " + $"0x{connectResult:X5}\nHATA AÇIKLAMASI : \n" + ex);
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
                _DB.AddLog("PLC Bağlantısı Sonlandırılamadız\nHATA KODU : " + $"0x{stopResult:X5}\nHATA AÇIKLAMASI : \n" + ex);
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

                _alistirma.TestStart = S7.GetBitAt(_buffer, Alistirma.adr_test_basla, 0);
                _alistirma.Frekans = S7.GetIntAt(_buffer, Alistirma.adr_frekans);
                _alistirma.FrenVoltaj = S7.GetRealAt(_buffer, Alistirma.adr_fren_voltaj);
                _alistirma.SureSag = S7.GetIntAt(_buffer, Alistirma.adr_düz_süre);
                _alistirma.SureSol = S7.GetIntAt(_buffer, Alistirma.adr_ters_süre);
                _alistirma.FrenAcikSure = S7.GetIntAt(_buffer, Alistirma.adr_fren_ac_süre);
                _alistirma.FrenKapalıSure = S7.GetIntAt(_buffer, Alistirma.adr_fren_kapa_süre);
                _alistirma.TestBitti = S7.GetBitAt(_buffer, Alistirma.adr_test_bitti, 0);

                _abtfTest.TestStart = S7.GetBitAt(_buffer, Classes.PlcCommunication.AbtfTest.adr_test_basla, 0);
                _abtfTest.Frekans = S7.GetIntAt(_buffer, Classes.PlcCommunication.AbtfTest.adr_frekans);
                _abtfTest.TestBitti = S7.GetBitAt(_buffer, Classes.PlcCommunication.AbtfTest.adr_test_bitti, 0);


            }
            catch (Exception ex)
            {
                StopReadindPlcDataBlock();
                StopPlcConnection();
                MessageBox.Show("PLC Data Block Okuması Başarısız\nHATA KODU : " + $"0x{readResult:X5}\nHATA AÇIKLAMASI : \n" + ex, "HATA !!");
                _DB.AddLog("PLC Data Block Okuması Başarısız\nHATA KODU : " + $"0x{readResult:X5}\nHATA AÇIKLAMASI : \n" + ex);
            }
        }
        #endregion

        #region Form Eventleri
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Sidebardaki butonları kullanmak için TabControl butonlarını gizler
            HideTabControlHeaderButtons(tabControlMain);

            //Açılışta default olarak Ybf Test İşlem Sayfasını Açıyorum
            tabControlMain.SelectedTab = tabControlMain.TabPages["tbPageYbfTestIslem"];




            //_DB.AddLog("Program Çalıştırıldı");//Programının ilk açılısında dbye log atıyorum
            StartReadindPlcDataBlock();
            GetAllValuesFromPLCtoTextBox();//Boş kalmaması adına plcdeki varolan veriler ilgili yerlere yazılı
            RefreshGroupBoxes();//Son okuma kutucuklarını dolduruyorum
            cbox_abtf_test_islem_serino.Visible = false;
            cbox_ybf_test_islem_serino.Visible = false;
            LoadDgvCompany();
            LoadDgvProducts();
            LoadDgvOperators();
            LoadDgvYbfTest();
        }
        public async void LoadDgvYbfTest()
        {
            DataTable dt = null;
            string query = $"SELECT " +
                $"ybf_test.id AS `TEST_ID`, " +
                $"products.product_id AS `PRODUCT_ID`, " +
                $"ybf_test.company_id AS `COMPANY_ID`, " +
                $"ybf_test.operator_id AS `OPERATOR_ID`, " +
                $"ybf_test.serial_no_id AS `SERIAL_NO_ID`, " +
                $"companies.company_name AS `FIRMA`, " +
                $"CONCAT(product_types.product_type_name, ' - ', product_groups.product_group_code) AS `URUN`, " +
                $"product_serial_no.serial_no AS `SERI NO`, " +
                $"operators.FullName AS `TEST SORUMLUSU`, " +
                $"ybf_test.test_date AS `TEST TARIH`, " +
                $"ybf_test.enduktans AS `ENDUKTANS`, " +
                $"ybf_test.direnc AS `BOBIN DIRENC`, " +
                $"ybf_test.hava_aralik AS `HAVA ARALIK`, " +
                $"ybf_test.alistirma AS `ALISTIRMA SURE`, " +
                $"ybf_test.yakalama AS `YAKALAMA VOLTAJ`, " +
                $"ybf_test.birakma AS `BIRAKMA VOLTAJ`, " +
                $"ybf_test.dinamik AS `DINAMIK TORK`, " +
                $"ybf_test.statik AS `STATIK TORK` " +
                $"FROM ybf_test " +
                $"INNER JOIN companies ON ybf_test.company_id = companies.company_id " +
                $"INNER JOIN product_serial_no ON ybf_test.serial_no_id = product_serial_no.id " +
                $"INNER JOIN operators ON ybf_test.operator_id = operators.id " +
                $"INNER JOIN products ON product_serial_no.product_id = products.product_id " +
                $"INNER JOIN product_types ON products.product_type_id = product_types.product_type_id " +
                $"INNER JOIN product_groups ON products.product_group_id = product_groups.product_group_id " +
                $"ORDER BY `TEST TARIH` DESC";
            await Task.Run(() =>
            {
                dt = _DB.GetMultiple(query);
            });
            if (dt == null)
                return;
            dgv_ybf_test_islem.DataSource = dt;
            dgv_ybf_test_islem.Columns["TEST_ID"].Visible = false;
            dgv_ybf_test_islem.Columns["PRODUCT_ID"].Visible = false;
            dgv_ybf_test_islem.Columns["COMPANY_ID"].Visible = false;
            dgv_ybf_test_islem.Columns["OPERATOR_ID"].Visible = false;
            dgv_ybf_test_islem.Columns["SERIAL_NO_ID"].Visible = false;
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

        public void RefreshGroupBoxes()
        {
            string now = DateTime.Now.ToShortTimeString();

            lblInfoAbtfTestSonOkuma.Text = now;
            lblInfoAbtfTestFrekans.Text = _abtfTest.Frekans.ToString();

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

        //Plcdeki verileri sayfadan okumak için olan alanın ortak okuma eventi
        private void RefreshButtons_Click(object sender, EventArgs e)
        {
            RefreshGroupBoxes();
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
        public void CheckPresetExistsAndAdd(int id)
        {
            try
            {
                MySqlParameter[] parameter =
                {
                    new MySqlParameter("@id",id),
                };
                if (!_DB.RecordExists("SELECT * FROM test_presets WHERE product_id = @id", parameter))
                {
                    if (!_DB.ExecuteQuery("INSERT INTO test_presets (`product_id`) VALUES (@id)", parameter))
                        throw new Exception($"Yeni test ön kaydı eklenemedi, CheckPresetExists(int id) = {id}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu = Main.CheckPresetExistsAndAdd");
                _DB.AddLog(ex.Message, "Main.CheckPresetExistsAndAdd");
            }
        }
        public bool CheckPresetExists(int id)
        {
            try
            {
                MySqlParameter[] parameter =
                {
                    new MySqlParameter("@id",id),
                };
                if (!_DB.RecordExists("SELECT * FROM test_presets WHERE product_id = @id", parameter))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu = Main.CheckPresetExists");
                _DB.AddLog(ex.Message, "Main.CheckPresetExists");
                return false;
            }
        }
        #endregion

        #region TimerTicks
        private void tmrMain_Tick(object sender, EventArgs e)
        {
            ReadAllDataBlock();
            txt_ACT_VOLT.Text = _globalValues.ACT_VOLT.ToString();
            txt_ACT_TORK.Text = _globalValues.ACT_TORK.ToString();
            txt_ACT_AKIM.Text = _globalValues.ACT_AKIM.ToString();
            lbl_CMD_VOLT.Text = _globalValues.CMD_VOLT.ToString();

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

        public void StartTest(int startAdress)
        {
            int startAddress = startAdress;
            byte[] tmpBuffer = new byte[2];
            int result = -1;
            try
            {
                S7.SetBitAt(tmpBuffer, 0, 0, true);
                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.StartTest \n" + ex.StackTrace);
                _DB.AddLog(ex.Message, "Main.StartTest");
            }
        }

        #region TestBaşlamaClickEventleri

        private async void btn_abtf_alistirma_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(Alistirma.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_alistirma.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_alistirma.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.RODAJ;
            disableButtonClicks = true;
            txtState.Text = "Rodaj Testine Başlandı";
            baseChartAndGridViewAbtfAlistirma.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private async void btn_ybf_alistirma_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(Alistirma.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_alistirma.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_alistirma.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.RODAJ;
            disableButtonClicks = true;
            txtState.Text = "Rodaj Testine Başlandı";
            baseChartAndGridViewYbfAlistirma.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private async void btn_abtf_test_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(Classes.PlcCommunication.AbtfTest.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_abtfTest.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_abtfTest.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.ABTF_TEST;
            disableButtonClicks = true;
            txtState.Text = "ABTF Testine Başlandı";
            baseChartAndGridViewAbtfTest.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private async void btn_ybf_birakma_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(YbfBirakma.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_ybfBirakma.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_ybfBirakma.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.YBF_BIRAKMA;
            disableButtonClicks = true;
            txtState.Text = "Bırakma Gerilimi Ölçümü Testine Başlandı";
            baseChartAndGridViewBirakma.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private async void btn_ybf_yakalama_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(YbfYakalama.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_ybfYakalama.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_ybfYakalama.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.YBF_YAKALAMA;
            disableButtonClicks = true;
            txtState.Text = "Yakalama Gerilimi Ölçümü Testine Başlandı";
            baseChartAndGridViewYakalama.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private async void btn_ybf_dinamik_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(YbfDinamik.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_ybfDinamik.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_ybfDinamik.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.YBF_DINAMIK;
            disableButtonClicks = true;
            txtState.Text = "Dinamik Tork Ölçümü Testine Başlandı";
            baseChartAndGridViewDinamik.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        private async void btn_ybf_statik_basla_Click(object sender, EventArgs e)
        {
            if (!CheckButtonClick())
                return;
            StartTest(YbfStatik.adr_test_basla);
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_ybfStatik.TestStart)
                        return;
                    Thread.Sleep(50);
                }
                return;
            });
            if (!_ybfStatik.TestStart)
            {
                txtState.Text = "Test Başlatılamadı";
                return;
            }
            AktifTest = TEST.YBF_STATIK;
            disableButtonClicks = true;
            txtState.Text = "Statik Tork Ölçümü Testine Başlandı";
            baseChartAndGridViewStatik.ClearChartAndDataGridView();
            tmrTest.Start();
        }

        #endregion


        public void GetAllValuesFromPLCtoTextBox()
        {
            ReadAllDataBlock();
            txt_abtf_alistirma_frekans.Text = _alistirma.Frekans.ToString();
            txt_abtf_alistirma_fren_voltaj.Text = _alistirma.FrenVoltaj.ToString();
            txt_abtf_alistirma_fren_acik.Text = _alistirma.FrenAcikSure.ToString();
            txt_abtf_alistirma_fren_kapali.Text = _alistirma.FrenKapalıSure.ToString();
            txt_abtf_alistirma_saga_donus.Text = _alistirma.SureSag.ToString();
            txt_abtf_alistirma_sola_donus.Text = _alistirma.SureSol.ToString();

            txt_ybf_alistirma_frekans.Text = _alistirma.Frekans.ToString();
            txt_ybf_alistirma_fren_voltaj.Text = _alistirma.FrenVoltaj.ToString();
            txt_ybf_alistirma_fren_acik.Text = _alistirma.FrenAcikSure.ToString();
            txt_ybf_alistirma_fren_kapali.Text = _alistirma.FrenKapalıSure.ToString();
            txt_ybf_alistirma_saga_donus.Text = _alistirma.SureSag.ToString();
            txt_ybf_alistirma_sola_donus.Text = _alistirma.SureSol.ToString();

            txt_birakma_frekans.Text = _ybfBirakma.Frekans.ToString();
            txt_birakma_baslangic_gerilim.Text = _ybfBirakma.BaslangicGerilim.ToString();
            txt_birakma_bitis_gerilim.Text = _ybfBirakma.BitisGerilim.ToString();
            txt_birakma_tork_seviye.Text = _ybfBirakma.TorkAlgilama.ToString();

            txt_yakalama_frekans.Text = _ybfYakalama.Frekans.ToString();
            txt_yakalama_baslangic_gerilim.Text = _ybfYakalama.BaslangicGerilim.ToString();
            txt_yakalama_bitis_gerilim.Text = _ybfYakalama.BitisGerilim.ToString();
            txt_yakalama_tork_seviye.Text = _ybfYakalama.TorkAlgilama.ToString();

            txt_dinamik_frekans.Text = _ybfDinamik.Frekans.ToString();
            txt_dinamik_baslangic_gerilim.Text = _ybfDinamik.BaslangicGerilim.ToString();
            txt_dinamik_bitis_gerilim.Text = _ybfDinamik.BitisGerilim.ToString();
            txt_dinamik_test_sure.Text = _ybfDinamik.TestSure.ToString();

            txt_statik_frekans.Text = _ybfStatik.Frekans.ToString();
            txt_statik_baslangic_gerilim.Text = _ybfStatik.BaslangicGerilim.ToString();
            txt_statik_bitis_gerilim.Text = _ybfStatik.BitisGerilim.ToString();
            txt_statik_tork_seviye.Text = _ybfStatik.TorkAlgilama.ToString();
        }

        #region PLC AYAR GONDER/GETIR

        #region Ayar Getir
        private void btn_abtf_alistirma_plc_ayar_getir_Click(object sender, EventArgs e)
        {
            txt_abtf_alistirma_frekans.Text = _alistirma.Frekans.ToString();
            txt_abtf_alistirma_fren_voltaj.Text = _alistirma.FrenVoltaj.ToString();
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
        #endregion

        #region Ayar Gönder
        private void btn_abtf_alistirma_plc_ayar_gönder_Click(object sender, EventArgs e)
        {
            int startAddress = Alistirma.adr_frekans;
            byte[] tmpBuffer = new byte[14];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, Alistirma.adr_frekans - startAddress, short.Parse(txt_abtf_alistirma_frekans.Text));
                S7.SetRealAt(tmpBuffer, Alistirma.adr_fren_voltaj - startAddress, float.Parse(txt_abtf_alistirma_fren_voltaj.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_düz_süre - startAddress, short.Parse(txt_abtf_alistirma_saga_donus.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_ters_süre - startAddress, short.Parse(txt_abtf_alistirma_sola_donus.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_fren_ac_süre - startAddress, short.Parse(txt_abtf_alistirma_fren_acik.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_fren_kapa_süre - startAddress, short.Parse(txt_abtf_alistirma_fren_kapali.Text));

                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "ABTF ALIŞTIRMA Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.btn_abtf_alistirma_plc_ayar_gönder_Click");
                _DB.AddLog(ex.Message, "Main.btn_abtf_alistirma_plc_ayar_gönder_Click");
            }
        }

        private void btn_ybf_alistirma_plc_ayar_gonder_Click(object sender, EventArgs e)
        {
            int startAddress = Alistirma.adr_frekans;
            byte[] tmpBuffer = new byte[14];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, Alistirma.adr_frekans - startAddress, short.Parse(txt_ybf_alistirma_frekans.Text));
                S7.SetRealAt(tmpBuffer, Alistirma.adr_fren_voltaj - startAddress, float.Parse(txt_ybf_alistirma_fren_voltaj.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_düz_süre - startAddress, short.Parse(txt_ybf_alistirma_saga_donus.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_ters_süre - startAddress, short.Parse(txt_ybf_alistirma_sola_donus.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_fren_ac_süre - startAddress, short.Parse(txt_ybf_alistirma_fren_acik.Text));
                S7.SetIntAt(tmpBuffer, Alistirma.adr_fren_kapa_süre - startAddress, short.Parse(txt_ybf_alistirma_fren_kapali.Text));

                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "YBF ALIŞTIRMA Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.btn_ybf_alistirma_plc_ayar_gonder_Click");
                _DB.AddLog(ex.Message, "Main.btn_ybf_alistirma_plc_ayar_gonder_Click");
            }

        }

        private void btn_birakma_plc_ayar_gönder_Click(object sender, EventArgs e)
        {
            int startAddress = YbfBirakma.adr_frekans;
            byte[] tmpBuffer = new byte[14];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, YbfBirakma.adr_frekans - startAddress, short.Parse(txt_birakma_frekans.Text));
                S7.SetRealAt(tmpBuffer, YbfBirakma.adr_baslangic_gerilimi - startAddress, float.Parse(txt_birakma_baslangic_gerilim.Text));
                S7.SetRealAt(tmpBuffer, YbfBirakma.adr_bitis_gerilim - startAddress, float.Parse(txt_birakma_bitis_gerilim.Text));
                S7.SetRealAt(tmpBuffer, YbfBirakma.adr_tork_algilama - startAddress, float.Parse(txt_birakma_tork_seviye.Text));

                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "BIRAKMA Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " \nHata Yolu : Main.btn_birakma_plc_ayar_gönder_Click\n");
                _DB.AddLog(ex.Message, "Main.btn_birakma_plc_ayar_gönder_Click");
            }
        }

        private void btn_yakalama_plc_ayar_gonder_Click(object sender, EventArgs e)
        {
            int startAddress = YbfYakalama.adr_frekans;
            byte[] tmpBuffer = new byte[14];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, YbfYakalama.adr_frekans - startAddress, short.Parse(txt_yakalama_frekans.Text));
                S7.SetRealAt(tmpBuffer, YbfYakalama.adr_baslangic_gerilimi - startAddress, float.Parse(txt_yakalama_baslangic_gerilim.Text));
                S7.SetRealAt(tmpBuffer, YbfYakalama.adr_bitis_gerilim - startAddress, float.Parse(txt_yakalama_bitis_gerilim.Text));
                S7.SetRealAt(tmpBuffer, YbfYakalama.adr_algilama_tork - startAddress, float.Parse(txt_yakalama_tork_seviye.Text));

                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "YAKALAMA Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.btn_yakalama_plc_ayar_gonder_Click");
                _DB.AddLog(ex.Message, "Main.btn_yakalama_plc_ayar_gonder_Click");
            }
        }

        private void btn_dinamik_plc_ayar_gonder_Click(object sender, EventArgs e)
        {
            int startAddress = YbfDinamik.adr_frekans;
            byte[] tmpBuffer = new byte[12];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, YbfDinamik.adr_frekans - startAddress, short.Parse(txt_dinamik_frekans.Text));
                S7.SetRealAt(tmpBuffer, YbfDinamik.adr_baslangic_gerilimi - startAddress, float.Parse(txt_dinamik_baslangic_gerilim.Text));
                S7.SetIntAt(tmpBuffer, YbfDinamik.adr_test_sure - startAddress, short.Parse(txt_dinamik_test_sure.Text));
                S7.SetRealAt(tmpBuffer, YbfDinamik.adr_bitis_gerilim - startAddress, float.Parse(txt_dinamik_bitis_gerilim.Text));

                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "DİNAMİK Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.btn_dinamik_plc_ayar_gonder_Click");
                _DB.AddLog(ex.Message, "Main.btn_dinamik_plc_ayar_gonder_Click");
            }
        }

        private void btn_statik_plc_ayar_gonder_Click(object sender, EventArgs e)
        {
            int startAddress = YbfStatik.adr_frekans;
            byte[] tmpBuffer = new byte[14];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, YbfStatik.adr_frekans - startAddress, short.Parse(txt_statik_frekans.Text));
                S7.SetRealAt(tmpBuffer, YbfStatik.adr_baslangic_gerilimi - startAddress, float.Parse(txt_statik_baslangic_gerilim.Text));
                S7.SetRealAt(tmpBuffer, YbfStatik.adr_bitis_gerilim - startAddress, float.Parse(txt_statik_bitis_gerilim.Text));
                S7.SetRealAt(tmpBuffer, YbfStatik.adr_algilama_tork - startAddress, float.Parse(txt_statik_tork_seviye.Text));

                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "STATİK Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.btn_statik_plc_ayar_gonder_Click");
                _DB.AddLog(ex.Message, "Main.btn_statik_plc_ayar_gonder_Click");
            }
        }
        #endregion
        #endregion

        #region DB Preset İşlemleri

        #region ABTF ALIŞTIRMA
        private void btn_abtf_alistirma_ayar_kaydet_Click(object sender, EventArgs e)
        {
            SelectProductPreset kaydet = new SelectProductPreset();
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CheckPresetExistsAndAdd(kaydet.ProductId);
                    string query = "UPDATE `test_presets`" +
                        " SET " +
                        "`p_abtf_alistir_freq`= @freq," +
                        "`p_abtf_alistir_voltage`=@voltage," +
                        "`p_abtf_alistir_time_right`=@right," +
                        "`p_abtf_alistir_time_left`=@left," +
                        "`p_abtf_alistir_brake_open`=@open," +
                        "`p_abtf_alistir_brake_close`=@close" +
                        " WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",kaydet.ProductId),
                        new MySqlParameter("@freq",Convert.ToInt16(txt_abtf_alistirma_frekans.Text)),
                        new MySqlParameter("@voltage",Convert.ToInt16(txt_abtf_alistirma_fren_voltaj.Text)),
                        new MySqlParameter("@right",Convert.ToInt16(txt_abtf_alistirma_saga_donus.Text)),
                        new MySqlParameter("@left",Convert.ToInt16(txt_abtf_alistirma_sola_donus.Text)),
                        new MySqlParameter("@open",Convert.ToInt16(txt_abtf_alistirma_fren_acik.Text)),
                        new MySqlParameter("@close",Convert.ToInt16(txt_abtf_alistirma_fren_kapali.Text))
                    };
                    if (_DB.ExecuteQuery(query, parameters))
                        MessageBox.Show($"[{kaydet.TypeName + " " + kaydet.GroupName}] Ürününe Ait Ön Kayıt Değişikliği Kaydedildi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Hata Yolu = Main.btn_abtf_alistirma_ayar_kaydet_Click");
                    _DB.AddLog(ex.Message + "Main.btn_abtf_alistirma_ayar_kaydet_Click");
                }

            }
        }
        private void btn_abtf_alistirma_ön_ayar_getir_Click(object sender, EventArgs e)
        {
            SelectProductPreset ayarGetir = new SelectProductPreset();
            if (ayarGetir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!CheckPresetExists(ayarGetir.ProductId))
                    {
                        MessageBox.Show("Seçilen Gruba Ait Ön Kayıt Yok Önce Ekleyiniz");
                        return;
                    }
                    string query = "SELECT `p_abtf_alistir_freq`," +
                        " `p_abtf_alistir_voltage`," +
                        " `p_abtf_alistir_time_right`," +
                        " `p_abtf_alistir_time_left`," +
                        " `p_abtf_alistir_brake_open`," +
                        " `p_abtf_alistir_brake_close`" +
                        " FROM `test_presets` WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",ayarGetir.ProductId)
                    };
                    DataTable dt = _DB.GetMultiple(query, parameters);
                    if (dt == null)
                        throw new Exception("Verileri Getirirken Bir Hata Oluştu, -> GetMultiple");
                    txt_abtf_alistirma_frekans.Text = dt.Rows[0]["p_abtf_alistir_freq"].ToString();
                    txt_abtf_alistirma_fren_voltaj.Text = dt.Rows[0]["p_abtf_alistir_voltage"].ToString();
                    txt_abtf_alistirma_saga_donus.Text = dt.Rows[0]["p_abtf_alistir_time_right"].ToString();
                    txt_abtf_alistirma_sola_donus.Text = dt.Rows[0]["p_abtf_alistir_time_left"].ToString();
                    txt_abtf_alistirma_fren_acik.Text = dt.Rows[0]["p_abtf_alistir_brake_open"].ToString();
                    txt_abtf_alistirma_fren_kapali.Text = dt.Rows[0]["p_abtf_alistir_brake_close"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Hata Yolu = Main.btn_abtf_alistirma_ön_ayar_getir_Click");
                    _DB.AddLog(ex.Message + "Main.btn_abtf_alistirma_ön_ayar_getir_Click");
                }

            }
        }


        #endregion

        #region YBF ALIŞTIRMA
        private void btn_ybf_alistirma_on_ayar_getir_Click(object sender, EventArgs e)
        {
            SelectProductPreset ayarGetir = new SelectProductPreset();
            if (ayarGetir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!CheckPresetExists(ayarGetir.ProductId))
                    {
                        MessageBox.Show("Seçilen Gruba Ait Ön Kayıt Yok Önce Ekleyiniz");
                        return;
                    }
                    string query = "SELECT `p_ybf_alistir_freq`," +
                        " `p_ybf_alistir_voltage`," +
                        " `p_ybf_alistir_time_right`," +
                        " `p_ybf_alistir_time_left`," +
                        " `p_ybf_alistir_brake_open`," +
                        " `p_ybf_alistir_brake_close`" +
                        " FROM `test_presets` WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",ayarGetir.ProductId)
                    };
                    DataTable dt = _DB.GetMultiple(query, parameters);
                    if (dt == null)
                        throw new Exception("Verileri Getirirken Bir Hata Oluştu, -> GetMultiple");
                    txt_ybf_alistirma_frekans.Text = dt.Rows[0]["p_ybf_alistir_freq"].ToString();
                    txt_ybf_alistirma_fren_voltaj.Text = dt.Rows[0]["p_ybf_alistir_voltage"].ToString();
                    txt_ybf_alistirma_saga_donus.Text = dt.Rows[0]["p_ybf_alistir_time_right"].ToString();
                    txt_ybf_alistirma_sola_donus.Text = dt.Rows[0]["p_ybf_alistir_time_left"].ToString();
                    txt_ybf_alistirma_fren_acik.Text = dt.Rows[0]["p_ybf_alistir_brake_open"].ToString();
                    txt_ybf_alistirma_fren_kapali.Text = dt.Rows[0]["p_ybf_alistir_brake_close"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Hata Yolu = Main.btn_ybf_alistirma_on_ayar_getir_Click");
                    _DB.AddLog(ex.Message + "Main.btn_ybf_alistirma_on_ayar_getir_Click");
                }
            }
        }
        private void btn_ybf_alistirma_ayar_kaydet_Click(object sender, EventArgs e)
        {
            SelectProductPreset kaydet = new SelectProductPreset();
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CheckPresetExistsAndAdd(kaydet.ProductId);
                    string query = "UPDATE `test_presets`" +
                        " SET " +
                        "`p_ybf_alistir_freq`= @freq," +
                        "`p_ybf_alistir_voltage`=@voltage," +
                        "`p_ybf_alistir_time_right`=@right," +
                        "`p_ybf_alistir_time_left`=@left," +
                        "`p_ybf_alistir_brake_open`=@open," +
                        "`p_ybf_alistir_brake_close`=@close" +
                        " WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",kaydet.ProductId),
                        new MySqlParameter("@freq",Convert.ToInt16(txt_ybf_alistirma_frekans.Text)),
                        new MySqlParameter("@voltage",Convert.ToInt16(txt_ybf_alistirma_fren_voltaj.Text)),
                        new MySqlParameter("@right",Convert.ToInt16(txt_ybf_alistirma_saga_donus.Text)),
                        new MySqlParameter("@left",Convert.ToInt16(txt_ybf_alistirma_sola_donus.Text)),
                        new MySqlParameter("@open",Convert.ToInt16(txt_ybf_alistirma_fren_acik.Text)),
                        new MySqlParameter("@close",Convert.ToInt16(txt_ybf_alistirma_fren_kapali.Text))
                    };
                    if (_DB.ExecuteQuery(query, parameters))
                        MessageBox.Show($"[{kaydet.TypeName + " " + kaydet.GroupName}] Ürününe Ait Ön Kayıt Değişikliği Kaydedildi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Hata Yolu = Main.btn_ybf_alistirma_ayar_kaydet_Click");
                    _DB.AddLog(ex.Message + "Main.btn_ybf_alistirma_ayar_kaydet_Click");
                }

            }
        }
        #endregion

        #region BIRAKMA
        private void btn_birakma_ayar_kaydet_Click(object sender, EventArgs e)
        {
            SelectProductPreset kaydet = new SelectProductPreset();
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CheckPresetExistsAndAdd(kaydet.ProductId);
                    string query = "UPDATE `test_presets`" +
                        " SET " +
                        "`p_birakma_freq`= @freq," +
                        "`p_birakma_voltage_start`=@v_start," +
                        "`p_birakma_voltage_finish`=@v_finish," +
                        "`p_birakma_torque`=@torque" +
                        " WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",kaydet.ProductId),
                        new MySqlParameter("@freq",Convert.ToInt16(txt_birakma_frekans.Text)),
                        new MySqlParameter("@v_start",Convert.ToInt16(txt_birakma_baslangic_gerilim.Text)),
                        new MySqlParameter("@v_finish",Convert.ToInt16(txt_birakma_bitis_gerilim.Text)),
                        new MySqlParameter("@torque",Convert.ToInt16(txt_birakma_tork_seviye.Text))
                    };
                    if (_DB.ExecuteQuery(query, parameters))
                        MessageBox.Show($"[{kaydet.TypeName + " " + kaydet.GroupName}] Ürününe Ait Ön Kayıt Değişikliği Kaydedildi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_birakma_ayar_kaydet_Click");
                    _DB.AddLog(ex.Message + " Main.btn_birakma_ayar_kaydet_Click");
                }
            }
        }
        private void btn_birakma_on_ayar_getir_Click(object sender, EventArgs e)
        {
            SelectProductPreset ayarGetir = new SelectProductPreset();
            if (ayarGetir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!CheckPresetExists(ayarGetir.ProductId))
                    {
                        MessageBox.Show("Seçilen Gruba Ait Ön Kayıt Yok Önce Ekleyiniz");
                        return;
                    }
                    string query = "SELECT `p_birakma_freq`," +
                        " `p_birakma_voltage_start`," +
                        " `p_birakma_voltage_finish`," +
                        " `p_birakma_torque`" +
                        " FROM `test_presets` WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",ayarGetir.ProductId)
                    };
                    DataTable dt = _DB.GetMultiple(query, parameters);
                    if (dt == null)
                        throw new Exception("Verileri Getirirken Bir Hata Oluştu, -> GetMultiple");
                    txt_birakma_frekans.Text = dt.Rows[0]["p_birakma_freq"].ToString();
                    txt_birakma_baslangic_gerilim.Text = dt.Rows[0]["p_birakma_voltage_start"].ToString();
                    txt_birakma_bitis_gerilim.Text = dt.Rows[0]["p_birakma_voltage_finish"].ToString();
                    txt_birakma_tork_seviye.Text = dt.Rows[0]["p_birakma_torque"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_birakma_on_ayar_getir_Click");
                    _DB.AddLog(ex.Message + " Main.btn_birakma_on_ayar_getir_Click");
                }
            }
        }
        #endregion

        #region YAKALAMA
        private void btn_yakalama_on_ayar_getir_Click(object sender, EventArgs e)
        {
            SelectProductPreset ayarGetir = new SelectProductPreset();
            if (ayarGetir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!CheckPresetExists(ayarGetir.ProductId))
                    {
                        MessageBox.Show("Seçilen Gruba Ait Ön Kayıt Yok Önce Ekleyiniz");
                        return;
                    }
                    string query = "SELECT `p_yakalama_freq`," +
                        " `p_yakalama_voltage_start`," +
                        " `p_yakalama_voltage_finish`," +
                        " `p_yakalama_torque`" +
                        " FROM `test_presets` WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",ayarGetir.ProductId)
                    };
                    DataTable dt = _DB.GetMultiple(query, parameters);
                    if (dt == null)
                        throw new Exception("Verileri Getirirken Bir Hata Oluştu, -> GetMultiple");
                    txt_yakalama_frekans.Text = dt.Rows[0]["p_yakalama_freq"].ToString();
                    txt_yakalama_baslangic_gerilim.Text = dt.Rows[0]["p_yakalama_voltage_start"].ToString();
                    txt_yakalama_bitis_gerilim.Text = dt.Rows[0]["p_yakalama_voltage_finish"].ToString();
                    txt_yakalama_tork_seviye.Text = dt.Rows[0]["p_yakalama_torque"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_yakalama_on_ayar_getir_Click");
                    _DB.AddLog(ex.Message + " Main.btn_yakalama_on_ayar_getir_Click");
                }
            }
        }
        private void btn_yakalama_ayar_kaydet_Click(object sender, EventArgs e)
        {
            SelectProductPreset kaydet = new SelectProductPreset();
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CheckPresetExistsAndAdd(kaydet.ProductId);
                    string query = "UPDATE `test_presets`" +
                        " SET " +
                        "`p_yakalama_freq`= @freq," +
                        "`p_yakalama_voltage_start`=@v_start," +
                        "`p_yakalama_voltage_finish`=@v_finish," +
                        "`p_yakalama_torque`=@torque" +
                        " WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",kaydet.ProductId),
                        new MySqlParameter("@freq",Convert.ToInt16(txt_yakalama_frekans.Text)),
                        new MySqlParameter("@v_start",Convert.ToInt16(txt_yakalama_baslangic_gerilim.Text)),
                        new MySqlParameter("@v_finish",Convert.ToInt16(txt_yakalama_bitis_gerilim.Text)),
                        new MySqlParameter("@torque",Convert.ToInt16(txt_yakalama_tork_seviye.Text))
                    };
                    if (_DB.ExecuteQuery(query, parameters))
                        MessageBox.Show($"[{kaydet.TypeName + " " + kaydet.GroupName}] Ürününe Ait Ön Kayıt Değişikliği Kaydedildi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_yakalama_ayar_kaydet_Click");
                    _DB.AddLog(ex.Message + " Main.btn_yakalama_ayar_kaydet_Click");
                }
            }
        }
        #endregion

        #region DINAMIK
        private void btn_dinamik_on_ayar_getir_Click(object sender, EventArgs e)
        {
            SelectProductPreset ayarGetir = new SelectProductPreset();
            if (ayarGetir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!CheckPresetExists(ayarGetir.ProductId))
                    {
                        MessageBox.Show("Seçilen Gruba Ait Ön Kayıt Yok Önce Ekleyiniz");
                        return;
                    }
                    string query = "SELECT `p_dinamik_freq`," +
                        " `p_dinamik_voltage_start`," +
                        " `p_dinamik_voltage_finish`," +
                        " `p_dinamik_test_time`" +
                        " FROM `test_presets` WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",ayarGetir.ProductId)
                    };
                    DataTable dt = _DB.GetMultiple(query, parameters);
                    if (dt == null)
                        throw new Exception("Verileri Getirirken Bir Hata Oluştu, -> GetMultiple");
                    txt_dinamik_frekans.Text = dt.Rows[0]["p_dinamik_freq"].ToString();
                    txt_dinamik_baslangic_gerilim.Text = dt.Rows[0]["p_dinamik_voltage_start"].ToString();
                    txt_dinamik_bitis_gerilim.Text = dt.Rows[0]["p_dinamik_voltage_finish"].ToString();
                    txt_dinamik_test_sure.Text = dt.Rows[0]["p_dinamik_test_time"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_dinamik_on_ayar_getir_Click");
                    _DB.AddLog(ex.Message + " Main.btn_dinamik_on_ayar_getir_Click");
                }
            }
        }
        private void btn_dinamik_ayar_kaydet_Click(object sender, EventArgs e)
        {
            SelectProductPreset kaydet = new SelectProductPreset();
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CheckPresetExistsAndAdd(kaydet.ProductId);
                    string query = "UPDATE `test_presets`" +
                        " SET " +
                        "`p_dinamik_freq`= @freq," +
                        "`p_dinamik_voltage_start`=@v_start," +
                        "`p_dinamik_voltage_finish`=@v_finish," +
                        "`p_dinamik_test_time`=@time" +
                        " WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",kaydet.ProductId),
                        new MySqlParameter("@freq",Convert.ToInt16(txt_dinamik_frekans.Text)),
                        new MySqlParameter("@v_start",Convert.ToInt16(txt_dinamik_baslangic_gerilim.Text)),
                        new MySqlParameter("@v_finish",Convert.ToInt16(txt_dinamik_bitis_gerilim.Text)),
                        new MySqlParameter("@time",Convert.ToInt16(txt_dinamik_test_sure.Text))
                    };
                    if (_DB.ExecuteQuery(query, parameters))
                        MessageBox.Show($"[{kaydet.TypeName + " " + kaydet.GroupName}] Ürününe Ait Ön Kayıt Değişikliği Kaydedildi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_dinamik_ayar_kaydet_Click");
                    _DB.AddLog(ex.Message + " Main.btn_dinamik_ayar_kaydet_Click");
                }

            }
        }
        #endregion

        #region STATIK
        private void btn_statik_on_ayar_getir_Click(object sender, EventArgs e)
        {
            SelectProductPreset ayarGetir = new SelectProductPreset();
            if (ayarGetir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!CheckPresetExists(ayarGetir.ProductId))
                    {
                        MessageBox.Show("Seçilen Gruba Ait Ön Kayıt Yok Önce Ekleyiniz");
                        return;
                    }
                    string query = "SELECT `p_statik_freq`," +
                        " `p_statik_voltage_start`," +
                        " `p_statik_voltage_finish`," +
                        " `p_statik_torque`" +
                        " FROM `test_presets` WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",ayarGetir.ProductId)
                    };
                    DataTable dt = _DB.GetMultiple(query, parameters);
                    if (dt == null)
                        throw new Exception("Verileri Getirirken Bir Hata Oluştu, -> GetMultiple");
                    txt_statik_frekans.Text = dt.Rows[0]["p_statik_freq"].ToString();
                    txt_statik_baslangic_gerilim.Text = dt.Rows[0]["p_statik_voltage_start"].ToString();
                    txt_statik_bitis_gerilim.Text = dt.Rows[0]["p_statik_voltage_finish"].ToString();
                    txt_statik_tork_seviye.Text = dt.Rows[0]["p_statik_torque"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_statik_on_ayar_getir_Click");
                    _DB.AddLog(ex.Message + " Main.btn_statik_on_ayar_getir_Click");
                }
            }
        }
        private void btn_statik_ayar_kaydet_Click(object sender, EventArgs e)
        {
            SelectProductPreset kaydet = new SelectProductPreset();
            if (kaydet.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CheckPresetExistsAndAdd(kaydet.ProductId);
                    string query = "UPDATE `test_presets`" +
                        " SET " +
                        "`p_statik_freq`= @freq," +
                        "`p_statik_voltage_start`=@v_start," +
                        "`p_statik_voltage_finish`=@v_finish," +
                        "`p_statik_torque`=@torque" +
                        " WHERE product_id = @id";
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@id",kaydet.ProductId),
                        new MySqlParameter("@freq",Convert.ToInt16(txt_statik_frekans.Text)),
                        new MySqlParameter("@v_start",Convert.ToInt16(txt_statik_baslangic_gerilim.Text)),
                        new MySqlParameter("@v_finish",Convert.ToInt16(txt_statik_bitis_gerilim.Text)),
                        new MySqlParameter("@torque",Convert.ToInt16(txt_statik_tork_seviye.Text))
                    };
                    if (_DB.ExecuteQuery(query, parameters))
                        MessageBox.Show($"[{kaydet.TypeName + " " + kaydet.GroupName}] Ürününe Ait Ön Kayıt Değişikliği Kaydedildi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Hata Yolu = Main.btn_statik_ayar_kaydet_Click");
                    _DB.AddLog(ex.Message + " Main.btn_statik_ayar_kaydet_Click");
                }
            }
        }
        #endregion


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
            _testSure = 0;
            tmrTest.Stop();
            disableButtonClicks = false;
            //tmrMain.Stop();

            // MessageBox.Show(_DB.RecordExists("SELECT COUNT(*) FROM produacts WHERE product_type_id = 14 AND product_group_id = 5").ToString());

        }

        private async void txtState_TextChanged(object sender, EventArgs e)
        {
            txt_son_islem_saati.Text = DateTime.Now.ToShortTimeString();
            for (int i = 0; i < 20; i++)
            {
                txtState.ForeColor = (txtState.ForeColor == Color.Blue) ? txtState.ForeColor = Color.Red : txtState.ForeColor = Color.Blue;
                await Task.Delay(100);
            }
            txtState.ForeColor = Color.Red;
        }

        private void btnKomutVoltaj_Click(object sender, EventArgs e)
        {
            int result = -1;
            byte[] tempBuffer = new byte[4];
            try
            {
                S7.SetRealAt(tempBuffer, 0, Convert.ToInt16(nmrKomutVoltaj.Value));
                result = _plc.DBWrite(100, GlobalValues.adr_CMD_VOLT, tempBuffer.Length, tempBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : Main.btnKomutVoltaj_Click");
                _DB.AddLog(ex.Message, "Main.btnKomutVoltaj_Click");
            }
        }

        private void btn_ybf_test_ıslem_firma_sec_Click(object sender, EventArgs e)
        {
            SelectCompany frm = new SelectCompany();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lbl_ybf_test_islem_firma_ad.Text = frm.CmpName;
                lbl_ybf_test_islem_firma_kod.Text = frm.CmpCode;
                lbl_ybf_test_islem_firma_tel.Text = frm.CmpPhone;
                lbl_ybf_test_islem_firma_not.Text = frm.CmpNote;
                lbl_ybf_test_islem_firma_id.Text = frm.CmpID.ToString();
            }
        }

        private void btn_ybf_test_ıslem_urun_sec_Click(object sender, EventArgs e)
        {
            SelectProductPreset frm = new SelectProductPreset();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = _DB.GetMultiple("SELECT * FROM products WHERE product_id = '" + frm.ProductId.ToString() + "'");
                lbl_ybf_test_islem_urun_ad.Text = frm.TypeName + " " + frm.GroupName;
                lbl_ybf_test_islem_urun_tork.Text = dt.Rows[0]["product_torque"].ToString();
                lbl_ybf_test_islem_urun_volt.Text = dt.Rows[0]["product_voltage"].ToString();
                lbl_ybf_test_islem_urun_watt.Text = dt.Rows[0]["product_coil_power"].ToString();
                lbl_ybf_test_islem_urun_id.Text = frm.ProductId.ToString();
                cbox_ybf_test_islem_serino.Text = string.Empty;
                _DB.FillCombobox(cbox_ybf_test_islem_serino, "SELECT * FROM product_serial_no WHERE product_id = " + lbl_ybf_test_islem_urun_id.Text + " ORDER BY sequence DESC", "serial_no", "id");
                lbl_ybf_test_islem_urun_full_code.Text = frm.Type_Code + frm.Group_Code;
            }
        }

        private void btn_abtf_test_ıslem_firma_sec_Click(object sender, EventArgs e)
        {
            SelectCompany frm = new SelectCompany();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lbl_abtf_test_islem_firma_ad.Text = frm.CmpName;
                lbl_abtf_test_islem_firma_kod.Text = frm.CmpCode;
                lbl_abtf_test_islem_firma_tel.Text = frm.CmpPhone;
                lbl_abtf_test_islem_firma_not.Text = frm.CmpNote;
                lbl_abtf_test_islem_firma_id.Text = frm.CmpID.ToString();
            }
        }

        private void btn_abtf_test_ıslem_urun_sec_Click(object sender, EventArgs e)
        {
            SelectProductPreset frm = new SelectProductPreset();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = _DB.GetMultiple("SELECT * FROM products WHERE product_id = '" + frm.ProductId.ToString() + "'");
                lbl_abtf_test_islem_urun_ad.Text = frm.TypeName + " " + frm.Group_Code;
                lbl_abtf_test_islem_urun_tork.Text = dt.Rows[0]["product_torque"].ToString();
                lbl_abtf_test_islem_urun_volt.Text = dt.Rows[0]["product_voltage"].ToString();
                lbl_abtf_test_islem_urun_watt.Text = dt.Rows[0]["product_coil_power"].ToString();
                lbl_abtf_test_islem_urun_id.Text = frm.ProductId.ToString();
                cbox_abtf_test_islem_serino.Text = string.Empty;
                _DB.FillCombobox(cbox_abtf_test_islem_serino, "SELECT * FROM product_serial_no WHERE product_id = " + lbl_abtf_test_islem_urun_id.Text + " ORDER BY sequence DESC", "serial_no", "id");
                lbl_abtf_test_islem_urun_full_code.Text = frm.Type_Code + frm.Group_Code;
            }
        }

        private void btn_abtf_test_plc_ayar_gönder_Click(object sender, EventArgs e)
        {
            int startAddress = Classes.PlcCommunication.AbtfTest.adr_frekans;
            byte[] tmpBuffer = new byte[2];
            int result = -1;
            try
            {
                S7.SetIntAt(tmpBuffer, Classes.PlcCommunication.AbtfTest.adr_frekans - startAddress, short.Parse(txt_abtf_test_islem_frekans.Text));
                result = _plc.DBWrite(100, startAddress, tmpBuffer.Length, tmpBuffer);
                if (result != 0)
                    throw new Exception(_plc.ErrorText(result));
                txtState.Text = "ABTF Testi için PLC'e ayarlar gönderildi";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " \nHata Yolu : Main.btn_abtf_test_plc_ayar_gönder_Click\n");
                _DB.AddLog(ex.Message, "Main.btn_abtf_test_plc_ayar_gönder_Click");
            }
        }
        public async void LoadDgvOperators()
        {
            DataTable dt = null;
            await Task.Run(() =>
            {
                dt = _DB.GetMultiple("SELECT `id`, `FullName` AS 'Operator', `Note` AS 'Not' FROM `operators`");
            });
            if (dt == null)
                return;
            _DB.FillCombobox(cbox_abtf_test_islem_operator, "SELECT * FROM operators", "FullName", "id");
            _DB.FillCombobox(cbox_ybf_test_islem_operator, "SELECT * FROM operators", "FullName", "id");
            dgv_veritabani_islem_operator.DataSource = dt;
            dgv_veritabani_islem_operator.Columns[0].Visible = false;
        }

        //Firma Ayarları sayfasındaki datagridviewi doldurur
        public async void LoadDgvCompany()
        {
            DataTable dt = null;
            await Task.Run(() =>
            {
                dt = _DB.GetMultiple("SELECT company_id, company_name AS 'Firma Adı', company_code AS 'Firma Kodu', company_phone AS 'Firma Tel No', company_note AS 'Not' FROM companies");
            });
            if (dt == null)
                return;
            dgv_veritabani_islem_firma.DataSource = dt;
            dgv_veritabani_islem_firma.Columns[0].Visible = false;
        }
        public async void LoadDgvProducts()
        {
            DataTable dt = null;
            await Task.Run(() =>
            {
                dt = _DB.GetMultiple("SELECT products.`product_id`," +
                    " products.`product_type_id`," +
                    " products.`product_group_id`," +
                    " product_types.product_type_name AS `Ürün Tipi`," +
                    " product_groups.product_group_name AS `Ürün Grubu`," +
                    " products.`product_voltage` AS 'VOLTAJ'," +
                    " products.`product_torque` AS 'TORK'," +
                    " products.`product_coil_power` AS 'WATT'" +
                    " FROM " +
                    "`products` " +
                    "INNER JOIN " +
                    "product_types ON products.product_type_id = product_types.product_type_id " +
                    "INNER JOIN " +
                    "product_groups ON products.product_group_id = product_groups.product_group_id;");
            });
            if (dt == null)
                return;
            dgv_veritabani_islem_fren.DataSource = dt;
            dgv_veritabani_islem_fren.Columns[0].Visible = false;
            dgv_veritabani_islem_fren.Columns[1].Visible = false;
            dgv_veritabani_islem_fren.Columns[2].Visible = false;

        }







        private void dgv_veritabani_islem_firma_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CompanyTransactions updateCompany = new CompanyTransactions();
            updateCompany.Text = "Kayıt Güncelle";
            updateCompany.Transaction = "Update";
            updateCompany.Id = int.Parse(dgv_veritabani_islem_firma.Rows[e.RowIndex].Cells[0].Value.ToString());
            updateCompany.Ad = dgv_veritabani_islem_firma.Rows[e.RowIndex].Cells[1].Value.ToString();
            updateCompany.Kod = dgv_veritabani_islem_firma.Rows[e.RowIndex].Cells[2].Value.ToString();
            updateCompany.Tel = dgv_veritabani_islem_firma.Rows[e.RowIndex].Cells[3].Value.ToString();
            updateCompany.Not = dgv_veritabani_islem_firma.Rows[e.RowIndex].Cells[4].Value.ToString();
            if (updateCompany.ShowDialog() == DialogResult.OK)
                txtState.Text = $"{updateCompany.Ad} isimli firmaya ait değişiklik kaydedildi";
            LoadDgvCompany();
        }

        private void btn_veritabani_islem_firma_ekle_Click(object sender, EventArgs e)
        {
            CompanyTransactions addCompany = new CompanyTransactions();
            addCompany.Text = "Kayıt Ekle";
            addCompany.Transaction = "Add";
            if (addCompany.ShowDialog() == DialogResult.OK)
                txtState.Text = $"{addCompany.Ad} isimli firma başarıyla eklendi";
            LoadDgvCompany();
        }

        private void dgv_veritabani_islem_fren_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ProductTransactions updateProduct = new ProductTransactions();
            updateProduct.Transaction = "Update";
            updateProduct.ProductId = int.Parse(dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[0].Value.ToString());
            updateProduct.TypeId = int.Parse(dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[1].Value.ToString());
            updateProduct.GroupId = int.Parse(dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[2].Value.ToString());
            updateProduct.TypeName = dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[3].Value.ToString();
            updateProduct.GroupName = dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[4].Value.ToString();
            updateProduct.Volt = dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[5].Value.ToString();
            updateProduct.Tork = dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[6].Value.ToString();
            updateProduct.Watt = dgv_veritabani_islem_fren.Rows[e.RowIndex].Cells[7].Value.ToString();
            if (updateProduct.ShowDialog() == DialogResult.OK)
                txtState.Text = $"{updateProduct.TypeName} {updateProduct.GroupName} ürününe ait değişiklik kaydedildi";
            LoadDgvProducts();
        }

        private void btn_veritabani_islem_fren_ekle_Click(object sender, EventArgs e)
        {
            ProductTransactions addProduct = new ProductTransactions();
            addProduct.Transaction = "Add";
            if (addProduct.ShowDialog() == DialogResult.OK)
                txtState.Text = $"{addProduct.TypeName} {addProduct.GroupName} ürünü başarıyla eklendi";
            LoadDgvProducts();
        }

        private void btn_veritabani_islem_operator_ekle_Click(object sender, EventArgs e)
        {
            OperatorTransactions frm = new OperatorTransactions();
            frm.Transaction = "Add";
            if (frm.ShowDialog() == DialogResult.OK)
                txtState.Text = $"Operatör başarıyla eklendi";
            LoadDgvOperators();
        }

        private void btn_abtf_test_islem_excel_Click(object sender, EventArgs e)
        {
            try
            {
                txtState.Text = "Excel Raporu Alınıyor";
                AbtfTestExcelExportHelper _excelExport;
                _excelExport = new AbtfTestExcelExportHelper(
                    lbl_abtf_test_islem_urun_ad.Text,
                    txt_abtf_test_islem_seri_no.Text,
                    cbox_abtf_test_islem_operator.Text,
                    lbl_abtf_test_islem_firma_ad.Text,
                    baseChartAndGridViewAbtfTest.GetData());
                _excelExport.ExportToExcel();
                txtState.Text = "Excel Raporuna Oluşturuldu !!! Belgelerim klasöründen erişebilirsiniz.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İlgili alalnların dolu olduğuna emin olunuz\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void chcBox_abtf_test_islem_serino_CheckedChanged(object sender, EventArgs e)
        {
            if (lbl_abtf_test_islem_urun_ad.Text == "-")
            {
                txtState.Text = "İlk Önce Ürün Seçiniz";
                return;
            }
            cbox_abtf_test_islem_serino.Visible = chcBox_abtf_test_islem_serino.Checked;
        }

        private void btn_abtf_test_islem_son_sn_getir_Click(object sender, EventArgs e)
        {
            object last_serial_no = _DB.GetSingleObject("SELECT serial_no FROM `product_serial_no` WHERE product_id =" + lbl_abtf_test_islem_urun_id.Text + " ORDER BY sequence DESC");
            if (last_serial_no != null)
            {
                txt_abtf_test_islem_seri_no.Text = last_serial_no.ToString();
            }

        }

        private void dgv_veritabani_islem_operator_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OperatorTransactions frm = new OperatorTransactions();
            frm.OperatorId = int.Parse(dgv_veritabani_islem_operator.Rows[e.RowIndex].Cells[0].Value.ToString());
            frm.OperatorName = dgv_veritabani_islem_operator.Rows[e.RowIndex].Cells[1].Value.ToString();
            frm.Note = dgv_veritabani_islem_operator.Rows[e.RowIndex].Cells[2].Value.ToString();
            frm.Transaction = "Update";
            if (frm.ShowDialog() == DialogResult.OK)
                txtState.Text = $"Operatör değişikliği kaydedildi";
            LoadDgvOperators();
        }
        public string GenerateSerialNo(string productCode, int sequence)
        {
            return $"{(DateTime.Now.Year % 100).ToString()}{productCode}{sequence.ToString("D4")}";
        }
        private void btn_abtf_test_islem_seri_no_olustur_Click(object sender, EventArgs e)
        {
            if (lbl_abtf_test_islem_urun_id.Text == "urun_id")
                return;

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@p_id",int.Parse(lbl_abtf_test_islem_urun_id.Text)),
                new MySqlParameter("@sequence", nmr_abtf_test_islem_seri_no_sequence.Value),
                new MySqlParameter("@year", DateTime.Now.Year),
            };
            bool serial_no_exist = _DB.RecordExists("SELECT * FROM product_serial_no WHERE product_id = @p_id AND sequence = @sequence AND serial_year = @year", parameters);
            if (serial_no_exist)
            {
                MessageBox.Show("Bu ürün ve sıra numarası için zaten bir kayıt oluşturulmuş aynı serideki ürün için TAMİR ve TEKRAR TEST alanını kullanmalısınız");
                return;
            }
            MySqlParameter[] parameters2 =
            {
                new MySqlParameter("@p_id",int.Parse(lbl_abtf_test_islem_urun_id.Text)),
                new MySqlParameter("@sequence", nmr_abtf_test_islem_seri_no_sequence.Value),
                new MySqlParameter("@year", DateTime.Now.Year),
                new MySqlParameter("@sn", GenerateSerialNo(lbl_abtf_test_islem_urun_full_code.Text, Convert.ToInt32(nmr_abtf_test_islem_seri_no_sequence.Value)))
            };
            int new_sn_id = Convert.ToInt32(_DB.ExecuteAndGetId("INSERT INTO `product_serial_no`(`product_id`, `serial_no`, `sequence`, `serial_year`) VALUES (@p_id,@sn,@sequence,@year)", parameters2));
            lbl_abtf_test_islem_serial_no_id.Text = new_sn_id.ToString();
            txt_abtf_test_islem_seri_no.Text = _DB.GetSingleObject("SELECT serial_no FROM product_serial_no WHERE id = " + new_sn_id).ToString();
            _DB.FillCombobox(cbox_abtf_test_islem_serino, "SELECT * FROM product_serial_no WHERE product_id = " + lbl_abtf_test_islem_urun_id.Text + " ORDER BY sequence DESC", "serial_no", "id");
        }


        private void cbox_abtf_test_islem_serino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lbl_abtf_test_islem_serial_no_id.Text = cbox_abtf_test_islem_serino.SelectedValue.ToString();
            txt_abtf_test_islem_seri_no.Text = cbox_abtf_test_islem_serino.GetItemText(cbox_abtf_test_islem_serino.SelectedItem);
        }

        private void btn_abtf_test_islem_veritabani_ekle_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@sn_id",int.Parse(lbl_abtf_test_islem_serial_no_id.Text)),
                    new MySqlParameter("@cmp_id", int.Parse(lbl_abtf_test_islem_firma_id.Text)),
                    new MySqlParameter("@op_id", cbox_abtf_test_islem_operator.SelectedValue),
                    new MySqlParameter("@date", DateTime.Now.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@test_value", JsonConvert.SerializeObject(baseChartAndGridViewAbtfTest.GetData(),Formatting.Indented))
                };
                _DB.ExecuteQuery("INSERT INTO `abtf_test`" +
                    "(`serial_no_id`, `company_id`, `operator_id`, `test_date`, `test_values`)" +
                    " VALUES (@sn_id,@cmp_id,@op_id,@date,@test_value)",parameters);
            }
            catch (Exception)
            {
                MessageBox.Show("Firma, Ürün seçmeden ve test başlatmadan veritabanına ekleme yapmayınız");
            }
        }

        private void cbox_ybf_test_islem_serino_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lbl_ybf_test_islem_serial_no_id.Text = cbox_ybf_test_islem_serino.SelectedValue.ToString();
            txt_ybf_test_islem_seri_no.Text = cbox_ybf_test_islem_serino.GetItemText(cbox_ybf_test_islem_serino.SelectedItem);
        }

        private void chcBox_ybf_test_islem_serino_CheckedChanged(object sender, EventArgs e)
        {
            if (lbl_ybf_test_islem_urun_ad.Text == "-")
            {
                txtState.Text = "İlk Önce Ürün Seçiniz";
                return;
            }
            cbox_ybf_test_islem_serino.Visible = chcBox_ybf_test_islem_serino.Checked;
        }

        private void btn_ybf_test_islem_seri_no_olustur_Click(object sender, EventArgs e)
        {
            if (lbl_ybf_test_islem_urun_id.Text == "urun_id")
                return;

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@p_id",int.Parse(lbl_ybf_test_islem_urun_id.Text)),
                new MySqlParameter("@sequence", nmr_ybf_test_islem_seri_no_sequence.Value),
                new MySqlParameter("@year", DateTime.Now.Year),
            };
            bool serial_no_exist = _DB.RecordExists("SELECT * FROM product_serial_no WHERE product_id = @p_id AND sequence = @sequence AND serial_year = @year", parameters);
            if (serial_no_exist)
            {
                MessageBox.Show("Bu ürün ve sıra numarası için zaten bir kayıt oluşturulmuş aynı serideki ürün için TAMİR ve TEKRAR TEST alanını kullanmalısınız");
                return;
            }
            MySqlParameter[] parameters2 =
            {
                new MySqlParameter("@p_id",int.Parse(lbl_ybf_test_islem_urun_id.Text)),
                new MySqlParameter("@sequence", nmr_ybf_test_islem_seri_no_sequence.Value),
                new MySqlParameter("@year", DateTime.Now.Year),
                new MySqlParameter("@sn", GenerateSerialNo(lbl_ybf_test_islem_urun_full_code.Text, Convert.ToInt32(nmr_ybf_test_islem_seri_no_sequence.Value)))
            };
            int new_sn_id = Convert.ToInt32(_DB.ExecuteAndGetId("INSERT INTO `product_serial_no`(`product_id`, `serial_no`, `sequence`, `serial_year`) VALUES (@p_id,@sn,@sequence,@year)", parameters2));
            lbl_ybf_test_islem_serial_no_id.Text = new_sn_id.ToString();
            txt_ybf_test_islem_seri_no.Text = _DB.GetSingleObject("SELECT serial_no FROM product_serial_no WHERE id = " + new_sn_id).ToString();
            _DB.FillCombobox(cbox_ybf_test_islem_serino, "SELECT * FROM product_serial_no WHERE product_id = " + lbl_ybf_test_islem_urun_id.Text + " ORDER BY sequence DESC", "serial_no", "id");
        }

        private void btn_ybf_test_islem_veritabani_ekle_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter ("@cmp_id", int.Parse(lbl_ybf_test_islem_firma_id.Text)),
                    new MySqlParameter ("@op_id", cbox_abtf_test_islem_operator.SelectedValue),
                    new MySqlParameter ("@sn_id", int.Parse(lbl_ybf_test_islem_serial_no_id.Text)),
                    new MySqlParameter ("@date", DateTime.Now.ToString("yyyy-MM-dd")),
                    new MySqlParameter ("@yakalama", txtTestIslemYakalamaSonuc.Text),
                    new MySqlParameter ("@birakma",txtTestIslemBirakmaSonuc.Text),
                    new MySqlParameter ("@statik", txtTestIslemStatikSonuc.Text),
                    new MySqlParameter ("@dinamik", txtTestIslemDinamikSonuc.Text),
                    new MySqlParameter ("@alistirma", txtTestIslemBalataAlistir.Text),
                    new MySqlParameter ("@enduktans", txtTestIslemEnduktans.Text),
                    new MySqlParameter ("@direnc", txtTestIslemBobinDirenc.Text),
                    new MySqlParameter ("@hava_aralik", txtTestIslemHavaAralik.Text)
                };
                _DB.ExecuteQuery("INSERT INTO `ybf_test`" +
                    "(`company_id`, `operator_id`, `serial_no_id`, `test_date`, `yakalama`, `birakma`, `dinamik`, `statik`, `alistirma`, `enduktans`, `direnc`, `hava_aralik`)" +
                    " VALUES " +
                    "(@cmp_id,@op_id,@sn_id,@date,@yakalama,@birakma,@statik,@dinamik,@alistirma,@enduktans,@direnc,@hava_aralik)", parameters);
                txtState.Text = "Test Kaydı Veritabanına Başarıyla Eklendi";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"HATA : {ex.Message}\n{ex.StackTrace}");
                return;
            }
        }

        private void btn_ybf_test_islem_raporlama_Click(object sender, EventArgs e)
        {
            YbfTestExcelExport frm = new YbfTestExcelExport();
            frm.Show();
        }
    }
}
