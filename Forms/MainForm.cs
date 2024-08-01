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
        //Helpers//
        private readonly DBHelper _DB;
        private readonly DBLogHelper _DBlog;

        //PLC Haberleşmesi İçin//
        private readonly S7Client _plc;
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
            _DBlog.AddLog("Program Çalıştırıldı");//Programının ilk açılısında dbye log atıyorum
            _plc = new S7Client();
            _abtfTest = new AbtfTest();
            _alistirma = new Alistirma();
            _globalValues = new GlobalValues();
            _ybfBirakma = new YbfBirakma();
            _ybfYakalama = new YbfYakalama();
            _ybfDinamik = new YbfDinamik();
            _ybfStatik = new YbfStatik();
        }

        public int StartPlcConnection()
        {
            int connectResult = -1;
            try
            {
                connectResult = _plc.ConnectTo("192.168.69.70", 0, 0);
                if (connectResult != 0)
                    MessageBox.Show("PLC Bağlantısı Başarısız\nHATA KODU : " + $"0x{connectResult:X5}\nHATA AÇIKLAMASI : \n" + _plc.ErrorText(connectResult), "HATA !!");
                _DBlog.AddLog("PLC Bağlantısı Başarısız\nHATA KODU : " + $"0x{connectResult:X5}\nHATA AÇIKLAMASI : \n" + _plc.ErrorText(connectResult));
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantısı Başarısız\n" + ex.Message, "HATA");
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
                    {
                        MessageBox.Show("PLC Bağlantısı Sonlandırılamadı\nHATA KODU : " + $"0x{stopResult:X5}\nHATA AÇIKLAMASI : \n" + _plc.ErrorText(stopResult), "HATA !!");
                        _DBlog.AddLog("PLC Bağlantısı Sonlandırılamadı\nHATA KODU : " + $"0x{stopResult:X5}\nHATA AÇIKLAMASI : \n" + _plc.ErrorText(stopResult));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantısı Sonlandırılamadı\nFROM : StopPlcConnection" + ex.Message, "HATA");
            }
            return stopResult;
        }

        public void StartReadindPlcDataBlock()
        {
            try
            {
                StartPlcConnection();
                tmrMain.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC Bağlantısı Sonlandırılamadı\nFROM : StopPlcConnection" + ex.Message, "HATA");
            }

        }

        #region MainFormLoad
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Sidebardaki butonları kullanmak için TabControl butonlarını gizler
            HideTabControlHeaderButtons(tabControlMain);
            //Açılışta default olarak Ybf Test İşlem Sayfasını Açıyorum
            tabControlMain.SelectedTab = tabControlMain.TabPages["tbPageYbfTestIslem"];
            StartPlcConnection();

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

    }
}
