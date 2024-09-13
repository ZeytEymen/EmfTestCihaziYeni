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
    public partial class OperatorTransactions : Form
    {
        private readonly DBHelper dBHelper;
        public OperatorTransactions()
        {
            InitializeComponent();
            dBHelper = new DBHelper();
        }

        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string Note { get; set; }
        public string Transaction { get; set; }

        public void DisableControls(GroupBox box)
        {
            foreach (Control c in box.Controls)
            {
                c.Enabled = false;
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@id", OperatorId),
                    new MySqlParameter("@name", txt_update_operator.Text),
                    new MySqlParameter("@note", txt_update_not.Text)
                };
                dBHelper.ExecuteQuery("UPDATE `operators` SET `FullName`=@name,`Note`=@note WHERE id = @id", parameters);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@name", txt_add_operator.Text),
                    new MySqlParameter("@note", txt_add_not.Text)
                };
                dBHelper.ExecuteQuery("INSERT INTO `operators`(`FullName`, `Note`) VALUES (@name,@note)",parameters);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OperatorTransactions_Load(object sender, EventArgs e)
        {
            switch (Transaction)
            {
                case "Add":
                    DisableControls(gbox_update);
                    break;
                case "Update":
                    txt_update_operator.Text = OperatorName;
                    txt_update_not.Text = Note;
                    DisableControls(gbox_add);
                    break;
                default:
                    break;
            }
        }
    }
}
