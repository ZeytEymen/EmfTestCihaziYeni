using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EmfTestCihazi.Forms.UI
{
    public partial class BaseChartAndGridView : UserControl
    {
        public BaseChartAndGridView()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {

            // Chart'ın arka plan rengini ayarlama
            chartAnlik.BackColor = System.Drawing.Color.LightGray;

            // ChartArea'nın arka plan rengini ayarlama
            chartAnlik.ChartAreas[0].BackColor = System.Drawing.Color.White;

            // Legend'in arka plan rengini ayarlama (Eğer kullanıyorsanız)
            chartAnlik.Legends[0].BackColor = System.Drawing.Color.Transparent;



            // X ve Y eksenlerinin kılavuz çizgilerini ayarlama
            chartAnlik.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(100, 0, 0, 0); // Daha açık siyah
            chartAnlik.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash; // Kesik çizgi stili
            chartAnlik.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1; // Çizgi kalınlığı

            chartAnlik.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(100, 0, 0, 0); // Daha açık siyah
            chartAnlik.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash; // Kesik çizgi stili
            chartAnlik.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1; // Çizgi kalınlığı

            chartAnlik.Series["Volt"].LabelForeColor = System.Drawing.Color.Black; // Etiket metin rengi
            chartAnlik.Series["Volt"].LabelBackColor = System.Drawing.Color.LightGray; // Etiket arka plan rengi (isteğe bağlı)
            chartAnlik.Series["Tork"].LabelForeColor = System.Drawing.Color.Black; // Etiket metin rengi
            chartAnlik.Series["Tork"].LabelBackColor = System.Drawing.Color.LightGray; // Etiket arka plan rengi (isteğe bağlı)
            chartAnlik.Series["Akim"].LabelForeColor = System.Drawing.Color.Black; // Etiket metin rengi
            chartAnlik.Series["Akim"].LabelBackColor = System.Drawing.Color.LightGray; // Etiket arka plan rengi (isteğe bağlı)



            // X ve Y eksen çizgilerini daha saydam yapmak

            chartAnlik.Series["Volt"].IsValueShownAsLabel = true;
            chartAnlik.Series["Tork"].IsValueShownAsLabel = true;
            chartAnlik.Series["Akim"].IsValueShownAsLabel = true;

            chartAnlik.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chartAnlik.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chartAnlik.ChartAreas[0].AxisX.ScaleView.Size = 8;
            chartAnlik.ChartAreas[0].AxisX.ScaleView.Scroll(ScrollType.Last);
            chartAnlik.ChartAreas[0].AxisX.Interval = 1; // Her saniye için bir birim ekle
  
        }

        public void ClearChartAndDataGridView()
        {
            chartAnlik.Series["Volt"].Points.Clear();
            chartAnlik.Series["Tork"].Points.Clear();
            chartAnlik.Series["Akim"].Points.Clear();

            dgvAnlik.Rows.Clear();
        }

        public void AddValues(int sure,double volt, double tork, double akim)
        {
            chartAnlik.Series["Volt"].Points.AddXY(sure, volt);
            chartAnlik.Series["Tork"].Points.AddXY(sure, tork);
            chartAnlik.Series["Akim"].Points.AddXY(sure, akim);

            dgvAnlik.Rows.Add(sure,volt,tork,akim);

            dgvAnlik.FirstDisplayedScrollingRowIndex = dgvAnlik.Rows.Count - 1;

            chartAnlik.ChartAreas[0].AxisX.ScaleView.Scroll(chartAnlik.Series[0].Points.Count - 1);
            chartAnlik.ChartAreas[0].RecalculateAxesScale();
        }
    }
}
