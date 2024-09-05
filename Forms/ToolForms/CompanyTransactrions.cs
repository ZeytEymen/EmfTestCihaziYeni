using EmfTestCihazi.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmfTestCihazi.Forms.ToolForms
{
    public partial class CompanyTransactions : Form
    {
        private readonly DBHelper _DB;
        public CompanyTransactions()
        {
            InitializeComponent();
            _DB = new DBHelper();
        }
        public string Transaction { get; set; }
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Kod { get; set; }
        public string Tel { get; set; }
        public string Not { get; set; }

        private void UpdateCompany_Load(object sender, EventArgs e)
        {
            txt_ad.Text = Ad;
            txt_kod.Text = Kod;
            txt_tel.Text = Tel;
            txt_not.Text = Not;
        }
        public void UpdateCompany()
        {
            try
            {
                MySqlParameter[] parameter = {
                    new MySqlParameter("@id",Id),
                    new MySqlParameter("@name", txt_ad.Text),
                    new MySqlParameter("@code", txt_kod.Text),
                    new MySqlParameter("@tel", txt_tel.Text),
                    new MySqlParameter("@not", txt_not.Text)
                };
                bool updateResult = _DB.ExecuteQuery("UPDATE `companies` SET " +
                    "`company_name`= @name," +
                    "`company_code`= @code," +
                    "`company_phone`= @tel," +
                    "`company_note`= @not" +
                    " WHERE company_id = @id", parameter);
                if (!updateResult)
                    throw new Exception("Veri Güncellenemedi, Hata Yolu : UpdateCompany.btn_duzenle_Click");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddCompany()
        {
            try
            {
                MySqlParameter[] parameter = {
                    new MySqlParameter("@name", txt_ad.Text),
                    new MySqlParameter("@code", txt_kod.Text),
                    new MySqlParameter("@tel", txt_tel.Text),
                    new MySqlParameter("@not", txt_not.Text)
                };
                bool insertResult = _DB.ExecuteQuery("INSERT INTO `companies`" +
                    "(`company_name`," +
                    " `company_code`," +
                    " `company_phone`," +
                    " `company_note`)" +
                    " VALUES " +
                    "(@name,@code,@tel,@not)", parameter);
                if (!insertResult)
                    throw new Exception("Veri Eklenemedi, Hata Yolu : Main.btn_firma_ayarları_ekle_Click");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_duzenle_Click(object sender, EventArgs e)
        {
            switch (Transaction)
            {
                case "Add":
                    AddCompany(); break;
                case "Update":
                    UpdateCompany(); break;
                default:
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    break;
            }
        }
    }
}
