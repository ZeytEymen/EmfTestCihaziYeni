using EmfTestCihazi.Classes;
using MySql.Data.MySqlClient;
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
    public partial class ProductTransactions : Form
    {
        private readonly DBHelper dbHelper;
        public string Transaction { get; set; }
        public int ProductId { get; set; }
        public int TypeId { get; set; }
        public int GroupId { get; set; }
        public string TypeName { get; set; }
        public string GroupName { get; set; }
        public string Volt { get; set; }
        public string Tork { get; set; }
        public string Watt { get; set; }
        public ProductTransactions()
        {
            InitializeComponent();
            dbHelper = new DBHelper();
        }
      
        public void DisableControls(GroupBox box)
        {
            foreach (Control c in box.Controls) 
            {
                c.Enabled = false;
            }
        }
        private void ProductTransactions_Load(object sender, EventArgs e)
        {
            switch (Transaction)
            {
                case "Add":
                    DisableControls(grp_box_update);
                    btn_add.Enabled = false;
                    break;
                case "Update":
                    DisableControls(grp_box_add);
                    btn_update.Enabled = false;
                    break;
                default:
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    break;
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            MessageBox.Show("update tıklandı");
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@id", ProductId),
                    new MySqlParameter("@volt", txt_add_volt.Text),
                    new MySqlParameter("@tork", txt_add_tork.Text),
                    new MySqlParameter("@watt", txt_add_watt.Text),

                };
                dbHelper.ExecuteQuery("UPDATE `products` SET " +
                    "`product_voltage`= @volt," +
                    "`product_torque`= @tork," +
                    "`product_coil_power`= @watt " +
                    "WHERE product_id = @id",parameters);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            SelectProductPreset selectProduct = new SelectProductPreset();
            selectProduct.Transaction = "FromAddProduct";
            if (selectProduct.ShowDialog() == DialogResult.OK)
            {
                lbl_add_type.Text = selectProduct.TypeName;
                lbl_add_group.Text = selectProduct.GroupName;
                ProductId = selectProduct.ProductId;
                btn_add.Enabled = true;
            }
        }
    }
}
