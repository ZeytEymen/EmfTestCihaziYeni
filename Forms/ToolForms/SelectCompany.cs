using EmfTestCihazi.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmfTestCihazi.Forms
{
    public partial class SelectCompany : Form
    {
        private readonly DBHelper db;
        public int CmpID { get; set; }
        public string CmpName { get; set; }
        public string CmpCode { get; set; }
        public string CmpPhone { get; set; }
        public string CmpNote { get; set; }
        public SelectCompany()
        {
            InitializeComponent();
            db = new DBHelper();
        }

        private void SelectCompany_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                dt = db.GetMultiple("SELECT `company_id` AS 'Firma ID', `company_name` AS 'Firma Adı', `company_code` AS 'Firma Kodu', `company_phone` AS 'Firma Telefonu', `company_note` AS 'Not' FROM `companies` WHERE 1");
                dgvCompanyList.DataSource = dt;
                dgvCompanyList.Columns[0].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCompanyList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CmpID = Int32.Parse(dgvCompanyList.Rows[e.RowIndex].Cells[0].Value.ToString());
            CmpName = dgvCompanyList.Rows[e.RowIndex].Cells[1].Value.ToString();
            CmpCode = dgvCompanyList.Rows[e.RowIndex].Cells[2].Value.ToString();
            CmpPhone = dgvCompanyList.Rows[e.RowIndex].Cells[3].Value.ToString();
            CmpNote = dgvCompanyList.Rows[e.RowIndex].Cells[4].Value.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
