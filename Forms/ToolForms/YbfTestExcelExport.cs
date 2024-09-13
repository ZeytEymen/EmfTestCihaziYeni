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
using MySql.Data.MySqlClient;

namespace EmfTestCihazi.Forms.ToolForms
{
    public partial class YbfTestExcelExport : Form
    {
        private readonly DBHelper _dbHelper;
        public YbfTestExcelExport()
        {
            InitializeComponent();
            _dbHelper = new DBHelper();
        }
        static string DateQuery;
        static string CompanyQuery;
        static string SerialNoQuery;
        static string ProductQuery;

        public string MakeSqlQuery()
        {
            string queryStart = "SELECT ybf_test.id AS `TEST_ID`, products.product_id AS `PRODUCT_ID`, ybf_test.company_id AS `COMPANY_ID`, ybf_test.operator_id AS `OPERATOR_ID`, ybf_test.serial_no_id AS `SERIAL_NO_ID`, companies.company_name AS `FIRMA`, CONCAT(product_types.product_type_name, ' - ', product_groups.product_group_code) AS `URUN`, product_serial_no.serial_no AS `SERI NO`, operators.FullName AS `TEST SORUMLUSU`, ybf_test.test_date AS `TEST TARIH`, ybf_test.enduktans AS `ENDUKTANS`, ybf_test.direnc AS `BOBIN DIRENC`, ybf_test.hava_aralik AS `HAVA ARALIK`, ybf_test.alistirma AS `ALISTIRMA SURE`, ybf_test.yakalama AS `YAKALAMA VOLTAJ`, ybf_test.birakma AS `BIRAKMA VOLTAJ`, ybf_test.dinamik AS `DINAMIK TORK`, ybf_test.statik AS `STATIK TORK` FROM ybf_test INNER JOIN companies ON ybf_test.company_id = companies.company_id INNER JOIN product_serial_no ON ybf_test.serial_no_id = product_serial_no.id INNER JOIN operators ON ybf_test.operator_id = operators.id INNER JOIN products ON product_serial_no.product_id = products.product_id INNER JOIN product_types ON products.product_type_id = product_types.product_type_id INNER JOIN product_groups ON products.product_group_id = product_groups.product_group_id";
            StringBuilder query = new StringBuilder( queryStart + " WHERE 1=1");
            if (pnl_urun.Enabled)
            {
                query.Append(" AND products.product_id = @product_id");
                if (pnl_sn_aralik.Enabled)
                    query.Append(" AND (product_serial_no.sequence BETWEEN @sn_aralik_start AND @sn_aralik_finish AND product_serial_no.serial_year BETWEEN @sn_tarih_start AND @sn_tarih_finish)");
            }
            if (pnl_test_tarih.Enabled)
                query.Append(" AND ybf_test.test_date BETWEEN @test_tarih_start AND @test_tarih_bitis");
            if (pnl_firma.Enabled)
                query.Append(" AND companies.company_id =  @company_id");
            return query.ToString();
        }
        public void ExecuteSql()
        {

            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@product_id", int.Parse(lbl_urun_id.Text)),
                    new MySqlParameter("@sn_aralik_start", nmr_sn_aralik_basla.Value.ToString()),
                    new MySqlParameter("@sn_aralik_finish", nmr_sn_aralik_bitis.Value.ToString()),
                    new MySqlParameter("@sn_tarih_start", combo_sn_tarih_basla.Text),
                    new MySqlParameter("@sn_tarih_finish", combo_sn_tarih_bitis.Text),
                    new MySqlParameter("@test_tarih_start", datePicker_test_tarih_baslangic.Value.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@test_tarih_bitis", datePicker_test_tarih_bitis.Value.ToString("yyyy-MM-dd")),
                    new MySqlParameter("@company_id", int.Parse(lbl_firma_id.Text)),
                };
                string parameterValues = "";
                foreach (var param in parameters)
                {
                    parameterValues += $"\n {param.ParameterName} =  [{param.Value}]";
                }
                MessageBox.Show(MakeSqlQuery() + " \n \n" + parameterValues);
                DataTable dt = _dbHelper.GetMultiple(MakeSqlQuery(), parameters);
                dgv_query_result.DataSource = dt;
                if(dt.Rows.Count == 0)
                    MessageBox.Show("Bu Filtreye Uygun Bir Kayıt Bulunamadı");
                dgv_query_result.Columns["TEST_ID"].Visible = false;
                dgv_query_result.Columns["PRODUCT_ID"].Visible = false;
                dgv_query_result.Columns["COMPANY_ID"].Visible = false;
                dgv_query_result.Columns["OPERATOR_ID"].Visible = false;
                dgv_query_result.Columns["SERIAL_NO_ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n\n {ex.StackTrace}");
            }
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            ExecuteSql();
        }
        public void MakeQuery()
        {
            string queryStart = "TEST KAYITLARINDAN";
            string query = "";
            string queryFinish = "KAYITLARI GETİR.";
            if (pnl_urun.Enabled)
            {
                query += ProductQuery;
                if (pnl_sn_aralik.Enabled)
                    query += SerialNoQuery;
            }
            if (pnl_test_tarih.Enabled)
                query += DateQuery;
            if (pnl_firma.Enabled)
                query += CompanyQuery;
            query = queryStart + query + queryFinish;

            if (!pnl_firma.Enabled && !pnl_sn_aralik.Enabled && !pnl_test_tarih.Enabled && !pnl_urun.Enabled)
                txt_query_explain.Text = "TÜM TEST KAYITLARINI GETİR";
            else
                txt_query_explain.Text = query;
        }

        private void YbfTestExcelExport_Load(object sender, EventArgs e)
        {
            for (int i = DateTime.Now.Year; i >= 2006; i--)
            {
                combo_sn_tarih_basla.Items.Add(i.ToString());
            }
            chc_firma_dahil.Checked = false;
            chc_sn_dahil.Checked = false;
            chc_test_tarih.Checked = false;
            chc_urun_dahil.Checked = false;
            pnl_firma.Enabled = false;
            pnl_sn_aralik.Enabled = false;
            pnl_test_tarih.Enabled = false;
            pnl_urun.Enabled = false;
            txt_query_explain.Text = "TÜM TEST KAYITLARINI GETİR";
            ExecuteSql();
            foreach (DataGridViewColumn column in dgv_query_result.Columns)
            {
                dgv_report.Columns.Add((DataGridViewColumn)column.Clone());
            }
        }

        private void chc_test_tarih_CheckedChanged(object sender, EventArgs e)
        {
            pnl_test_tarih.Enabled = chc_test_tarih.Checked;
            if (!chc_test_tarih.Checked)
                MakeQuery();
        }

        private void chc_urun_dahil_CheckedChanged(object sender, EventArgs e)
        {
            pnl_urun.Enabled = chc_urun_dahil.Checked;
            if (!chc_urun_dahil.Checked)
                MakeQuery();
        }

        private void chc_sn_dahil_CheckedChanged(object sender, EventArgs e)
        {
            pnl_sn_aralik.Enabled = chc_sn_dahil.Checked;
            if (!chc_sn_dahil.Checked)
                MakeQuery();
        }

        private void chc_firma_dahil_CheckedChanged(object sender, EventArgs e)
        {
            pnl_firma.Enabled = chc_firma_dahil.Checked;
            if (!chc_firma_dahil.Checked)
                MakeQuery();
        }

        private void btn_urun_sec_Click(object sender, EventArgs e)
        {
            SelectProductPreset frm = new SelectProductPreset();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lbl_urun_id.Text = frm.ProductId.ToString();
                lbl_urun_full_name.Text = frm.TypeName + " " + frm.Group_Code;
                ProductQuery = $" ÜRÜN = [ {lbl_urun_full_name.Text} ] OLAN ";
                MakeQuery();
            }
        }

        private void nmr_sn_aralik_basla_ValueChanged(object sender, EventArgs e)
        {
            SerialNoQuery = $" SERİ NO ARALIĞI = [ {nmr_sn_aralik_basla.Value} ile {nmr_sn_aralik_bitis.Value} ], SERİ NO TARİH ARALIĞI = [ {combo_sn_tarih_basla.Text} ile {combo_sn_tarih_bitis.Text} ] OLAN ";
            nmr_sn_aralik_bitis.Minimum = nmr_sn_aralik_basla.Value;
            MakeQuery();
        }

        private void nmt_sn_aralik_bitis_ValueChanged(object sender, EventArgs e)
        {
            SerialNoQuery = $" SERİ NO ARALIĞI = [ {nmr_sn_aralik_basla.Value} ile {nmr_sn_aralik_bitis.Value} ], SERİ NO TARİH ARALIĞI = [ {combo_sn_tarih_basla.Text} ile {combo_sn_tarih_bitis.Text} ] OLAN ";
            MakeQuery();
        }

        private void nmr_sn_tarih_basla_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialNoQuery = $" SERİ NO ARALIĞI = [ {nmr_sn_aralik_basla.Value} ile {nmr_sn_aralik_bitis.Value} ], SERİ NO TARİH ARALIĞI = [ {combo_sn_tarih_basla.Text} ile {combo_sn_tarih_bitis.Text} ] OLAN ";
            MakeQuery();
        }

        private void nmr_sn_tarih_bitis_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialNoQuery = $" SERİ NO ARALIĞI = [ {nmr_sn_aralik_basla.Value} ile {nmr_sn_aralik_bitis.Value} ], SERİ NO TARİH ARALIĞI = [ {combo_sn_tarih_basla.Text} ile {combo_sn_tarih_bitis.Text} ] OLAN ";
            MakeQuery();
        }

        private void datePicker_test_tarih_baslangic_ValueChanged(object sender, EventArgs e)
        {
            DateQuery = $" TEST TARİHLERİ = [ {datePicker_test_tarih_baslangic.Value.ToShortDateString()} ile {datePicker_test_tarih_bitis.Value.ToShortDateString()} ] OLAN ";
            datePicker_test_tarih_bitis.MinDate = datePicker_test_tarih_baslangic.Value;
            MakeQuery();
        }

        private void datePicker_test_tarih_bitis_ValueChanged(object sender, EventArgs e)
        {
            DateQuery = $" TEST TARİHLERİ = [ {datePicker_test_tarih_baslangic.Value.ToShortDateString()} ile {datePicker_test_tarih_bitis.Value.ToShortDateString()} ] OLAN ";
            MakeQuery();
        }

        private void btn_firma_sec_Click(object sender, EventArgs e)
        {
            SelectCompany frm = new SelectCompany();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lbl_firma_ad.Text = frm.CmpName;
                lbl_firma_id.Text = frm.CmpID.ToString();
                CompanyQuery = $" FİRMA = [ {lbl_firma_ad.Text} ] OLAN ";
                MakeQuery();
            }
        }

        private void combo_sn_tarih_basla_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if(combo_sn_tarih_bitis.Items.Count > 0)
                combo_sn_tarih_bitis.Items.Clear();
            int year = int.Parse(combo_sn_tarih_basla.GetItemText(combo_sn_tarih_basla.SelectedItem));

            for (int i = year; i <= DateTime.Now.Year; i++)
            {
                combo_sn_tarih_bitis.Items.Add(i.ToString());
            }
            combo_sn_tarih_bitis.SelectedIndex = 0;
        }

        private void btn_transfer_source_to_report_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_query_result.SelectedRows)
            {
                bool alreadyExists = false;

                foreach (DataGridViewRow reportRow in dgv_report.Rows)
                {
                    bool isMatch = true;
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (!row.Cells[i].Value.Equals(reportRow.Cells[i].Value))
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    if (isMatch)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                if (!alreadyExists)
                {
                    int rowIndex = dgv_report.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dgv_report.Rows[rowIndex].Cells[i].Value = row.Cells[i].Value;
                    }
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                else
                {
                    MessageBox.Show("Zaten Eklenmiş Satırlar Bulundu ve İlgili Satır Eklemesi İptal Edildi");
                }
            }
        }

        private void btn_delete_from_report_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_report.SelectedRows)
            {
                foreach (DataGridViewRow originalRow in dgv_query_result.Rows)
                {
                    bool isMatch = true;
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (!row.Cells[i].Value.Equals(originalRow.Cells[i].Value))
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    if (isMatch)
                    {
                        originalRow.DefaultCellStyle.BackColor = dgv_query_result.DefaultCellStyle.BackColor;
                        break;
                    }
                }
                dgv_report.Rows.Remove(row);
            }
        }
        public DataTable GetData()
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn column in dgv_report.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }
            foreach (DataGridViewRow row in dgv_report.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dgv_report.Columns.Count; i++)
                    {
                        dr[i] = row.Cells[i].Value ?? DBNull.Value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private void btn_export_to_excel_Click(object sender, EventArgs e)
        {
            YbfTestExcelExportHelper exporter = new YbfTestExcelExportHelper(lbl_urun_full_name.Text,lbl_firma_ad.Text, GetData());
            exporter.ExportToExcel();
        }
    }
}
