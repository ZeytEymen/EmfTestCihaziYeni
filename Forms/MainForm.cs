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
            InitializeDataGridView();
            LoadData();
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
        #region MainFormLoad
        private void MainForm_Load(object sender, EventArgs e)
        {
            // _DBlog.AddLog("Program Çalıştırıldı");//Programının ilk açılısında dbye log atıyorum

            //Sidebardaki butonları kullanmak için TabControl butonlarını gizler
            HideTabControlHeaderButtons(tabControlMain);
            //Açılışta default olarak Ybf Test İşlem Sayfasını Açıyorum
            tabControlMain.SelectedTab = tabControlMain.TabPages["tbPageYbfTestIslem"];

            //  StartReadindPlcDataBlock();
            //tmrMain.Start();
        }

        #endregion



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



        private void InitializeDataGridView()
        {


            // ID Kolonu
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "ID";
            idColumn.HeaderText = "ID";
            dgvYbfTestIslem.Columns.Add(idColumn);

            // Name Kolonu
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Name";
            dgvYbfTestIslem.Columns.Add(nameColumn);

            // Age Kolonu
            DataGridViewTextBoxColumn ageColumn = new DataGridViewTextBoxColumn();
            ageColumn.DataPropertyName = "Age";
            ageColumn.HeaderText = "Age";
            dgvYbfTestIslem.Columns.Add(ageColumn);

            // City Kolonu
            DataGridViewTextBoxColumn cityColumn = new DataGridViewTextBoxColumn();
            cityColumn.DataPropertyName = "City";
            cityColumn.HeaderText = "City";
            dgvYbfTestIslem.Columns.Add(cityColumn);

            // Güncelle Butonu Kolonu
            DataGridViewButtonColumn updateButtonColumn = new DataGridViewButtonColumn();
            updateButtonColumn.HeaderText = "Update";
            updateButtonColumn.Text = "Güncelle";
            updateButtonColumn.UseColumnTextForButtonValue = true;
            dgvYbfTestIslem.Columns.Add(updateButtonColumn);

            // Sil Butonu Kolonu
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.HeaderText = "Delete";
            deleteButtonColumn.Text = "Sil";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            dgvYbfTestIslem.Columns.Add(deleteButtonColumn);

        }

        private void LoadData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Columns.Add("City", typeof(string));

            string[] names = { "John", "Jane", "Alice", "Bob", "Chris", "Diana", "Eve", "Frank", "Grace", "Hank" };
            string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose" };

            Random rnd = new Random();
            for (int i = 1; i <= 15; i++)
            {
                DataRow row = table.NewRow();
                row["ID"] = i;
                row["Name"] = names[rnd.Next(names.Length)];
                row["Age"] = rnd.Next(18, 65);
                row["City"] = cities[rnd.Next(cities.Length)];
                table.Rows.Add(row);
            }

            dgvYbfTestIslem.DataSource = table;
        }


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

        private void btnRefreshYbfAlistirma_Click(object sender, EventArgs e)
        {
            //  GetBiseyBisey();
            // lblInfoYbfAlistirmaSonOkuma.Text = DateTime.Now.ToShortTimeString();
            lblInfoYbfAlistirmaFrekans.Text = _alistirma.Frekans.ToString();
            lblInfoYbfAlistirmaFrenAcik.Text = _alistirma.FrenAcikSure.ToString();
            lblInfoYbfAlistirmaFrenKapali.Text = _alistirma.FrenKapalıSure.ToString();
            lblInfoYbfAlistirmaSagaDonus.Text = _alistirma.SureSag.ToString();
            lblInfoYbfAlistirmaSolaDonus.Text = _alistirma.SureSol.ToString();
        }

        private void tmrTest_Tick(object sender, EventArgs e)
        {
            switch (AktifTest)
            {
                case TEST.RODAJ:
                    MessageBox.Show("rodaj");
                    break;
                case TEST.YBF_BIRAKMA:
                    MessageBox.Show("bırakma");
                    break;
                case TEST.YBF_YAKALAMA:
                    MessageBox.Show("yakalama");
                    break;
                case TEST.YBF_DINAMIK:
                    MessageBox.Show("dinamik");
                    break;
                case TEST.YBF_STATIK:
                    MessageBox.Show("statik");
                    break;
                case TEST.ABTF_TEST:
                    MessageBox.Show("abtftest");
                    break;
                default:
                    break;
            }
        }

        private void btn_abtf_alistirma_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.RODAJ;
            tmrTest.Start();
        }

        private void btn_ybf_alistirma_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.RODAJ;
            tmrTest.Start();
        }

        private void btn_abtf_test_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.ABTF_TEST;
            tmrTest.Start();
        }

        private void btn_ybf_birakma_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.YBF_BIRAKMA;
            tmrTest.Start();
        }

        private void btn_ybf_yakalama_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.YBF_YAKALAMA;
            tmrTest.Start();
        }

        private void btn_ybf_dinamik_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.YBF_DINAMIK;
            tmrTest.Start();
        }

        private void btn_ybf_statik_basla_Click(object sender, EventArgs e)
        {
            AktifTest = TEST.YBF_STATIK;
            tmrTest.Start();
        }

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
    }
}
