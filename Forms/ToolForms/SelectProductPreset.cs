using EmfTestCihazi.Classes;
using MySql.Data.MySqlClient;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmfTestCihazi.Forms.ToolForms
{
    public partial class SelectProductPreset : Form
    {
        readonly DBHelper dbHelper;
        public string Transaction { get; set; }
        public int TypeId { get; set; }
        public int GroupId { get; set; }
        public int ProductId { get; set; }
        public string TypeName { get; set; }
        public string GroupName { get; set; }
        public SelectProductPreset()
        {
            InitializeComponent();
            dbHelper = new DBHelper();
        }
        private async void ShowLabelForLimitedTime(string message, int milliseconds)
        {
            lblDurum.Text = message;
            lblDurum.Visible = true;

            await Task.Delay(milliseconds);

            lblDurum.Text = string.Empty;
            lblDurum.Visible = false;
        }

        private void SavePreset_Load(object sender, EventArgs e)
        {
            dbHelper.FillCombobox(cboxType, "SELECT * FROM product_types", "product_type_name", "product_type_id");
            dbHelper.FillCombobox(cboxGroup, "SELECT * FROM product_groups", "product_group_name", "product_group_id");
            btn_save.Visible = false;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@type_id", cboxType.SelectedValue),
                new MySqlParameter("@group_id",cboxGroup.SelectedValue)
            };
            DataTable dt = dbHelper.GetMultiple("SELECT * FROM products WHERE product_type_id = @type_id AND product_group_id = @group_id", parameters);
            if (dt.Rows.Count == 0)
            {
                if (Transaction == "FromAddProduct")
                {
                    try
                    {
                        int lastId = Convert.ToInt32(dbHelper.ExecuteAndGetId("INSERT INTO `products`(`product_type_id`, `product_group_id`) VALUES (@type_id, @group_id)", parameters));
                        ProductId = lastId;
                        TypeId = Convert.ToInt32(cboxType.SelectedValue);
                        GroupId= Convert.ToInt32(cboxGroup.SelectedValue);
                        TypeName = cboxType.Text;
                        GroupName = cboxGroup.Text;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                ShowLabelForLimitedTime("Bu Fren Tipi ve Grubu için Ürün Tanımı Yok.", 2500);
                btn_save.Visible = false;
                return;
            }
            else if(dt.Rows.Count != 0 && Transaction == "FromAddProduct")
            {
                ShowLabelForLimitedTime("Bu tip ve gruba ait bir kayıt zaten var",5000);
                btn_save.Visible = false;
                return;
            }
            ProductId = int.Parse(dt.Rows[0]["product_id"].ToString());
            TypeId = int.Parse(dt.Rows[0]["product_type_id"].ToString());
            GroupId = int.Parse(dt.Rows[0]["product_group_id"].ToString());
            TypeName = cboxType.Text;
            GroupName = cboxGroup.Text;
            lblDurum.Visible = true;
            lblDurum.Text = $"Seçilen ürün {TypeName} {GroupName} kayıt butonuna basınız";
            btn_save.Visible = true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
