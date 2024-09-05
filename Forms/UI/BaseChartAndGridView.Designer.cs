namespace EmfTestCihazi.Forms.UI
{
    partial class BaseChartAndGridView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chartAnlik = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvAnlik = new System.Windows.Forms.DataGridView();
            this.ColumnSure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVolt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAkim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chartAnlik)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnlik)).BeginInit();
            this.SuspendLayout();
            // 
            // chartAnlik
            // 
            chartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chartAnlik.ChartAreas.Add(chartArea1);
            this.chartAnlik.Dock = System.Windows.Forms.DockStyle.Bottom;
            legend1.Name = "Legend1";
            this.chartAnlik.Legends.Add(legend1);
            this.chartAnlik.Location = new System.Drawing.Point(0, 140);
            this.chartAnlik.Name = "chartAnlik";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            series1.LabelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            series1.Legend = "Legend1";
            series1.Name = "Volt";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Tork";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Legend = "Legend1";
            series3.Name = "Akim";
            this.chartAnlik.Series.Add(series1);
            this.chartAnlik.Series.Add(series2);
            this.chartAnlik.Series.Add(series3);
            this.chartAnlik.Size = new System.Drawing.Size(870, 300);
            this.chartAnlik.TabIndex = 3;
            this.chartAnlik.Text = "chart1";
            // 
            // dgvAnlik
            // 
            this.dgvAnlik.AllowUserToAddRows = false;
            this.dgvAnlik.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.dgvAnlik.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAnlik.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAnlik.BackgroundColor = System.Drawing.Color.Silver;
            this.dgvAnlik.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvAnlik.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAnlik.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSure,
            this.ColumnVolt,
            this.ColumnTork,
            this.ColumnAkim});
            this.dgvAnlik.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvAnlik.Location = new System.Drawing.Point(0, 0);
            this.dgvAnlik.Name = "dgvAnlik";
            this.dgvAnlik.ReadOnly = true;
            this.dgvAnlik.RowHeadersVisible = false;
            this.dgvAnlik.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAnlik.Size = new System.Drawing.Size(870, 141);
            this.dgvAnlik.TabIndex = 14;
            // 
            // ColumnSure
            // 
            this.ColumnSure.HeaderText = "SÜRE";
            this.ColumnSure.Name = "ColumnSure";
            this.ColumnSure.ReadOnly = true;
            // 
            // ColumnVolt
            // 
            this.ColumnVolt.HeaderText = "VOLT";
            this.ColumnVolt.Name = "ColumnVolt";
            this.ColumnVolt.ReadOnly = true;
            // 
            // ColumnTork
            // 
            this.ColumnTork.HeaderText = "TORK";
            this.ColumnTork.Name = "ColumnTork";
            this.ColumnTork.ReadOnly = true;
            // 
            // ColumnAkim
            // 
            this.ColumnAkim.HeaderText = "AKIM";
            this.ColumnAkim.Name = "ColumnAkim";
            this.ColumnAkim.ReadOnly = true;
            // 
            // BaseChartAndGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvAnlik);
            this.Controls.Add(this.chartAnlik);
            this.Name = "BaseChartAndGridView";
            this.Size = new System.Drawing.Size(870, 440);
            ((System.ComponentModel.ISupportInitialize)(this.chartAnlik)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnlik)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartAnlik;
        private System.Windows.Forms.DataGridView dgvAnlik;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSure;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVolt;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTork;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAkim;
    }
}
