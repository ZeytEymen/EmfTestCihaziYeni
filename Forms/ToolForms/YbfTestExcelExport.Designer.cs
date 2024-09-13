namespace EmfTestCihazi.Forms.ToolForms
{
    partial class YbfTestExcelExport
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_query_result = new System.Windows.Forms.DataGridView();
            this.btn_search = new System.Windows.Forms.Button();
            this.chc_urun_dahil = new System.Windows.Forms.CheckBox();
            this.pnl_urun = new System.Windows.Forms.Panel();
            this.pnl_sn_aralik = new System.Windows.Forms.Panel();
            this.combo_sn_tarih_bitis = new System.Windows.Forms.ComboBox();
            this.combo_sn_tarih_basla = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nmr_sn_aralik_bitis = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nmr_sn_aralik_basla = new System.Windows.Forms.NumericUpDown();
            this.chc_sn_dahil = new System.Windows.Forms.CheckBox();
            this.lbl_urun_full_name = new System.Windows.Forms.Label();
            this.btn_urun_sec = new System.Windows.Forms.Button();
            this.chc_firma_dahil = new System.Windows.Forms.CheckBox();
            this.pnl_firma = new System.Windows.Forms.Panel();
            this.lbl_firma_ad = new System.Windows.Forms.Label();
            this.btn_firma_sec = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_query_explain = new System.Windows.Forms.TextBox();
            this.lbl_urun_id = new System.Windows.Forms.Label();
            this.lbl_firma_id = new System.Windows.Forms.Label();
            this.chc_test_tarih = new System.Windows.Forms.CheckBox();
            this.pnl_test_tarih = new System.Windows.Forms.Panel();
            this.label37 = new System.Windows.Forms.Label();
            this.datePicker_test_tarih_baslangic = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.datePicker_test_tarih_bitis = new System.Windows.Forms.DateTimePicker();
            this.btn_export_to_excel = new System.Windows.Forms.Button();
            this.btn_transfer_source_to_report = new System.Windows.Forms.Button();
            this.btn_delete_from_report = new System.Windows.Forms.Button();
            this.dgv_report = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_query_result)).BeginInit();
            this.pnl_urun.SuspendLayout();
            this.pnl_sn_aralik.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_sn_aralik_bitis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_sn_aralik_basla)).BeginInit();
            this.pnl_firma.SuspendLayout();
            this.pnl_test_tarih.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_report)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_query_result
            // 
            this.dgv_query_result.AllowUserToAddRows = false;
            this.dgv_query_result.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.dgv_query_result.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_query_result.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_query_result.BackgroundColor = System.Drawing.Color.Silver;
            this.dgv_query_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_query_result.Location = new System.Drawing.Point(0, 189);
            this.dgv_query_result.Name = "dgv_query_result";
            this.dgv_query_result.ReadOnly = true;
            this.dgv_query_result.RowHeadersVisible = false;
            this.dgv_query_result.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_query_result.Size = new System.Drawing.Size(982, 181);
            this.dgv_query_result.TabIndex = 75;
            // 
            // btn_search
            // 
            this.btn_search.BackColor = System.Drawing.Color.Transparent;
            this.btn_search.BackgroundImage = global::EmfTestCihazi.Properties.Resources.icons8_search_50;
            this.btn_search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_search.FlatAppearance.BorderSize = 0;
            this.btn_search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_search.Location = new System.Drawing.Point(909, 79);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(30, 30);
            this.btn_search.TabIndex = 80;
            this.btn_search.UseVisualStyleBackColor = false;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // chc_urun_dahil
            // 
            this.chc_urun_dahil.AutoSize = true;
            this.chc_urun_dahil.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.chc_urun_dahil.Location = new System.Drawing.Point(12, 11);
            this.chc_urun_dahil.Name = "chc_urun_dahil";
            this.chc_urun_dahil.Size = new System.Drawing.Size(108, 20);
            this.chc_urun_dahil.TabIndex = 84;
            this.chc_urun_dahil.Text = "Ürün Dahil Et";
            this.chc_urun_dahil.UseVisualStyleBackColor = true;
            this.chc_urun_dahil.CheckedChanged += new System.EventHandler(this.chc_urun_dahil_CheckedChanged);
            // 
            // pnl_urun
            // 
            this.pnl_urun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_urun.Controls.Add(this.pnl_sn_aralik);
            this.pnl_urun.Controls.Add(this.chc_sn_dahil);
            this.pnl_urun.Controls.Add(this.lbl_urun_full_name);
            this.pnl_urun.Controls.Add(this.btn_urun_sec);
            this.pnl_urun.Location = new System.Drawing.Point(12, 37);
            this.pnl_urun.Name = "pnl_urun";
            this.pnl_urun.Size = new System.Drawing.Size(363, 98);
            this.pnl_urun.TabIndex = 83;
            // 
            // pnl_sn_aralik
            // 
            this.pnl_sn_aralik.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_sn_aralik.Controls.Add(this.combo_sn_tarih_bitis);
            this.pnl_sn_aralik.Controls.Add(this.combo_sn_tarih_basla);
            this.pnl_sn_aralik.Controls.Add(this.label6);
            this.pnl_sn_aralik.Controls.Add(this.label5);
            this.pnl_sn_aralik.Controls.Add(this.label3);
            this.pnl_sn_aralik.Controls.Add(this.nmr_sn_aralik_bitis);
            this.pnl_sn_aralik.Controls.Add(this.label7);
            this.pnl_sn_aralik.Controls.Add(this.nmr_sn_aralik_basla);
            this.pnl_sn_aralik.Location = new System.Drawing.Point(84, 29);
            this.pnl_sn_aralik.Name = "pnl_sn_aralik";
            this.pnl_sn_aralik.Size = new System.Drawing.Size(273, 59);
            this.pnl_sn_aralik.TabIndex = 95;
            // 
            // combo_sn_tarih_bitis
            // 
            this.combo_sn_tarih_bitis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.combo_sn_tarih_bitis.FormattingEnabled = true;
            this.combo_sn_tarih_bitis.Location = new System.Drawing.Point(209, 31);
            this.combo_sn_tarih_bitis.Name = "combo_sn_tarih_bitis";
            this.combo_sn_tarih_bitis.Size = new System.Drawing.Size(60, 21);
            this.combo_sn_tarih_bitis.TabIndex = 94;
            this.combo_sn_tarih_bitis.Text = "2024";
            this.combo_sn_tarih_bitis.SelectedIndexChanged += new System.EventHandler(this.nmr_sn_tarih_bitis_SelectedIndexChanged);
            // 
            // combo_sn_tarih_basla
            // 
            this.combo_sn_tarih_basla.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.combo_sn_tarih_basla.FormattingEnabled = true;
            this.combo_sn_tarih_basla.Location = new System.Drawing.Point(209, 3);
            this.combo_sn_tarih_basla.Name = "combo_sn_tarih_basla";
            this.combo_sn_tarih_basla.Size = new System.Drawing.Size(60, 21);
            this.combo_sn_tarih_basla.TabIndex = 94;
            this.combo_sn_tarih_basla.Text = "2024";
            this.combo_sn_tarih_basla.SelectedIndexChanged += new System.EventHandler(this.nmr_sn_tarih_basla_SelectedIndexChanged);
            this.combo_sn_tarih_basla.SelectionChangeCommitted += new System.EventHandler(this.combo_sn_tarih_basla_SelectionChangeCommitted);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(43, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 16);
            this.label6.TabIndex = 87;
            this.label6.Text = "Bitiş";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(132, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 16);
            this.label5.TabIndex = 91;
            this.label5.Text = "Başlangıç";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 16);
            this.label3.TabIndex = 86;
            this.label3.Text = "Başlangıç";
            // 
            // nmr_sn_aralik_bitis
            // 
            this.nmr_sn_aralik_bitis.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.nmr_sn_aralik_bitis.Location = new System.Drawing.Point(79, 28);
            this.nmr_sn_aralik_bitis.Name = "nmr_sn_aralik_bitis";
            this.nmr_sn_aralik_bitis.Size = new System.Drawing.Size(47, 21);
            this.nmr_sn_aralik_bitis.TabIndex = 89;
            this.nmr_sn_aralik_bitis.ValueChanged += new System.EventHandler(this.nmt_sn_aralik_bitis_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(170, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 16);
            this.label7.TabIndex = 93;
            this.label7.Text = "Bitiş";
            // 
            // nmr_sn_aralik_basla
            // 
            this.nmr_sn_aralik_basla.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.nmr_sn_aralik_basla.Location = new System.Drawing.Point(79, 4);
            this.nmr_sn_aralik_basla.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nmr_sn_aralik_basla.Name = "nmr_sn_aralik_basla";
            this.nmr_sn_aralik_basla.Size = new System.Drawing.Size(47, 21);
            this.nmr_sn_aralik_basla.TabIndex = 88;
            this.nmr_sn_aralik_basla.ValueChanged += new System.EventHandler(this.nmr_sn_aralik_basla_ValueChanged);
            // 
            // chc_sn_dahil
            // 
            this.chc_sn_dahil.AutoSize = true;
            this.chc_sn_dahil.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.chc_sn_dahil.Location = new System.Drawing.Point(162, 6);
            this.chc_sn_dahil.Name = "chc_sn_dahil";
            this.chc_sn_dahil.Size = new System.Drawing.Size(139, 20);
            this.chc_sn_dahil.TabIndex = 85;
            this.chc_sn_dahil.Text = "SN Aralık Dahil Et";
            this.chc_sn_dahil.UseVisualStyleBackColor = true;
            this.chc_sn_dahil.CheckedChanged += new System.EventHandler(this.chc_sn_dahil_CheckedChanged);
            // 
            // lbl_urun_full_name
            // 
            this.lbl_urun_full_name.AutoSize = true;
            this.lbl_urun_full_name.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_urun_full_name.Location = new System.Drawing.Point(17, 59);
            this.lbl_urun_full_name.Name = "lbl_urun_full_name";
            this.lbl_urun_full_name.Size = new System.Drawing.Size(47, 16);
            this.lbl_urun_full_name.TabIndex = 78;
            this.lbl_urun_full_name.Text = "[URUN]";
            // 
            // btn_urun_sec
            // 
            this.btn_urun_sec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_urun_sec.FlatAppearance.BorderSize = 0;
            this.btn_urun_sec.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_urun_sec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_urun_sec.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_urun_sec.Location = new System.Drawing.Point(10, 6);
            this.btn_urun_sec.Name = "btn_urun_sec";
            this.btn_urun_sec.Size = new System.Drawing.Size(60, 30);
            this.btn_urun_sec.TabIndex = 30;
            this.btn_urun_sec.Text = "Ürün Seç";
            this.btn_urun_sec.UseVisualStyleBackColor = true;
            this.btn_urun_sec.Click += new System.EventHandler(this.btn_urun_sec_Click);
            // 
            // chc_firma_dahil
            // 
            this.chc_firma_dahil.AutoSize = true;
            this.chc_firma_dahil.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.chc_firma_dahil.Location = new System.Drawing.Point(701, 11);
            this.chc_firma_dahil.Name = "chc_firma_dahil";
            this.chc_firma_dahil.Size = new System.Drawing.Size(115, 20);
            this.chc_firma_dahil.TabIndex = 86;
            this.chc_firma_dahil.Text = "Firma Dahil Et";
            this.chc_firma_dahil.UseVisualStyleBackColor = true;
            this.chc_firma_dahil.CheckedChanged += new System.EventHandler(this.chc_firma_dahil_CheckedChanged);
            // 
            // pnl_firma
            // 
            this.pnl_firma.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_firma.Controls.Add(this.lbl_firma_ad);
            this.pnl_firma.Controls.Add(this.btn_firma_sec);
            this.pnl_firma.Location = new System.Drawing.Point(681, 37);
            this.pnl_firma.Name = "pnl_firma";
            this.pnl_firma.Size = new System.Drawing.Size(148, 98);
            this.pnl_firma.TabIndex = 85;
            // 
            // lbl_firma_ad
            // 
            this.lbl_firma_ad.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_firma_ad.Location = new System.Drawing.Point(3, 62);
            this.lbl_firma_ad.Name = "lbl_firma_ad";
            this.lbl_firma_ad.Size = new System.Drawing.Size(140, 17);
            this.lbl_firma_ad.TabIndex = 79;
            this.lbl_firma_ad.Text = "[FIRMA]\r\n";
            this.lbl_firma_ad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_firma_sec
            // 
            this.btn_firma_sec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_firma_sec.FlatAppearance.BorderSize = 0;
            this.btn_firma_sec.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_firma_sec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_firma_sec.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_firma_sec.Location = new System.Drawing.Point(35, 14);
            this.btn_firma_sec.Name = "btn_firma_sec";
            this.btn_firma_sec.Size = new System.Drawing.Size(73, 30);
            this.btn_firma_sec.TabIndex = 31;
            this.btn_firma_sec.Text = "Firma Seç";
            this.btn_firma_sec.UseVisualStyleBackColor = true;
            this.btn_firma_sec.Click += new System.EventHandler(this.btn_firma_sec_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(874, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 16);
            this.label8.TabIndex = 87;
            this.label8.Text = "Sonuçları Getir";
            // 
            // txt_query_explain
            // 
            this.txt_query_explain.BackColor = System.Drawing.Color.Silver;
            this.txt_query_explain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_query_explain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_query_explain.Location = new System.Drawing.Point(12, 142);
            this.txt_query_explain.Multiline = true;
            this.txt_query_explain.Name = "txt_query_explain";
            this.txt_query_explain.Size = new System.Drawing.Size(960, 41);
            this.txt_query_explain.TabIndex = 88;
            // 
            // lbl_urun_id
            // 
            this.lbl_urun_id.AutoSize = true;
            this.lbl_urun_id.Location = new System.Drawing.Point(211, 8);
            this.lbl_urun_id.Name = "lbl_urun_id";
            this.lbl_urun_id.Size = new System.Drawing.Size(13, 13);
            this.lbl_urun_id.TabIndex = 90;
            this.lbl_urun_id.Text = "0";
            this.lbl_urun_id.Visible = false;
            // 
            // lbl_firma_id
            // 
            this.lbl_firma_id.AutoSize = true;
            this.lbl_firma_id.Location = new System.Drawing.Point(242, 8);
            this.lbl_firma_id.Name = "lbl_firma_id";
            this.lbl_firma_id.Size = new System.Drawing.Size(13, 13);
            this.lbl_firma_id.TabIndex = 90;
            this.lbl_firma_id.Text = "0";
            this.lbl_firma_id.Visible = false;
            // 
            // chc_test_tarih
            // 
            this.chc_test_tarih.AutoSize = true;
            this.chc_test_tarih.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold);
            this.chc_test_tarih.Location = new System.Drawing.Point(439, 11);
            this.chc_test_tarih.Name = "chc_test_tarih";
            this.chc_test_tarih.Size = new System.Drawing.Size(155, 20);
            this.chc_test_tarih.TabIndex = 92;
            this.chc_test_tarih.Text = "Test Tarihini Dahil Et";
            this.chc_test_tarih.UseVisualStyleBackColor = true;
            this.chc_test_tarih.CheckedChanged += new System.EventHandler(this.chc_test_tarih_CheckedChanged);
            // 
            // pnl_test_tarih
            // 
            this.pnl_test_tarih.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_test_tarih.Controls.Add(this.label37);
            this.pnl_test_tarih.Controls.Add(this.datePicker_test_tarih_baslangic);
            this.pnl_test_tarih.Controls.Add(this.label1);
            this.pnl_test_tarih.Controls.Add(this.datePicker_test_tarih_bitis);
            this.pnl_test_tarih.Location = new System.Drawing.Point(439, 37);
            this.pnl_test_tarih.Name = "pnl_test_tarih";
            this.pnl_test_tarih.Size = new System.Drawing.Size(193, 98);
            this.pnl_test_tarih.TabIndex = 91;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label37.Location = new System.Drawing.Point(8, 26);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(71, 16);
            this.label37.TabIndex = 77;
            this.label37.Text = "Başlangıç";
            // 
            // datePicker_test_tarih_baslangic
            // 
            this.datePicker_test_tarih_baslangic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.datePicker_test_tarih_baslangic.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker_test_tarih_baslangic.Location = new System.Drawing.Point(85, 25);
            this.datePicker_test_tarih_baslangic.Name = "datePicker_test_tarih_baslangic";
            this.datePicker_test_tarih_baslangic.Size = new System.Drawing.Size(105, 20);
            this.datePicker_test_tarih_baslangic.TabIndex = 76;
            this.datePicker_test_tarih_baslangic.ValueChanged += new System.EventHandler(this.datePicker_test_tarih_baslangic_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(46, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 16);
            this.label1.TabIndex = 79;
            this.label1.Text = "Bitiş";
            // 
            // datePicker_test_tarih_bitis
            // 
            this.datePicker_test_tarih_bitis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.datePicker_test_tarih_bitis.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker_test_tarih_bitis.Location = new System.Drawing.Point(85, 51);
            this.datePicker_test_tarih_bitis.Name = "datePicker_test_tarih_bitis";
            this.datePicker_test_tarih_bitis.Size = new System.Drawing.Size(105, 20);
            this.datePicker_test_tarih_bitis.TabIndex = 78;
            this.datePicker_test_tarih_bitis.ValueChanged += new System.EventHandler(this.datePicker_test_tarih_bitis_ValueChanged);
            // 
            // btn_export_to_excel
            // 
            this.btn_export_to_excel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_export_to_excel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_export_to_excel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_export_to_excel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_export_to_excel.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_export_to_excel.Location = new System.Drawing.Point(798, 376);
            this.btn_export_to_excel.Name = "btn_export_to_excel";
            this.btn_export_to_excel.Size = new System.Drawing.Size(184, 30);
            this.btn_export_to_excel.TabIndex = 94;
            this.btn_export_to_excel.Text = "Seçilen Kayıtlardan Rapor Oluştur";
            this.btn_export_to_excel.UseVisualStyleBackColor = true;
            this.btn_export_to_excel.Click += new System.EventHandler(this.btn_export_to_excel_Click);
            // 
            // btn_transfer_source_to_report
            // 
            this.btn_transfer_source_to_report.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_transfer_source_to_report.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.btn_transfer_source_to_report.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_transfer_source_to_report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_transfer_source_to_report.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_transfer_source_to_report.Location = new System.Drawing.Point(12, 376);
            this.btn_transfer_source_to_report.Name = "btn_transfer_source_to_report";
            this.btn_transfer_source_to_report.Size = new System.Drawing.Size(229, 30);
            this.btn_transfer_source_to_report.TabIndex = 95;
            this.btn_transfer_source_to_report.Text = "Kaynaktan Seçilenleri Rapor Tablosuna Ekle";
            this.btn_transfer_source_to_report.UseVisualStyleBackColor = true;
            this.btn_transfer_source_to_report.Click += new System.EventHandler(this.btn_transfer_source_to_report_Click);
            // 
            // btn_delete_from_report
            // 
            this.btn_delete_from_report.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_delete_from_report.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btn_delete_from_report.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_delete_from_report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_delete_from_report.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_delete_from_report.Location = new System.Drawing.Point(247, 376);
            this.btn_delete_from_report.Name = "btn_delete_from_report";
            this.btn_delete_from_report.Size = new System.Drawing.Size(229, 30);
            this.btn_delete_from_report.TabIndex = 96;
            this.btn_delete_from_report.Text = "Rapor Tablosundan Seçilenleri Kaldır";
            this.btn_delete_from_report.UseVisualStyleBackColor = true;
            this.btn_delete_from_report.Click += new System.EventHandler(this.btn_delete_from_report_Click);
            // 
            // dgv_report
            // 
            this.dgv_report.AllowUserToAddRows = false;
            this.dgv_report.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.dgv_report.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_report.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_report.BackgroundColor = System.Drawing.Color.Silver;
            this.dgv_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_report.Location = new System.Drawing.Point(0, 412);
            this.dgv_report.Name = "dgv_report";
            this.dgv_report.ReadOnly = true;
            this.dgv_report.RowHeadersVisible = false;
            this.dgv_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_report.Size = new System.Drawing.Size(982, 246);
            this.dgv_report.TabIndex = 97;
            // 
            // YbfTestExcelExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.dgv_report);
            this.Controls.Add(this.btn_delete_from_report);
            this.Controls.Add(this.btn_transfer_source_to_report);
            this.Controls.Add(this.btn_export_to_excel);
            this.Controls.Add(this.chc_test_tarih);
            this.Controls.Add(this.pnl_test_tarih);
            this.Controls.Add(this.lbl_firma_id);
            this.Controls.Add(this.lbl_urun_id);
            this.Controls.Add(this.txt_query_explain);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chc_firma_dahil);
            this.Controls.Add(this.pnl_firma);
            this.Controls.Add(this.chc_urun_dahil);
            this.Controls.Add(this.pnl_urun);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.dgv_query_result);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "YbfTestExcelExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEST RAPORLAMA İŞLEMLERİ";
            this.Load += new System.EventHandler(this.YbfTestExcelExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_query_result)).EndInit();
            this.pnl_urun.ResumeLayout(false);
            this.pnl_urun.PerformLayout();
            this.pnl_sn_aralik.ResumeLayout(false);
            this.pnl_sn_aralik.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_sn_aralik_bitis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_sn_aralik_basla)).EndInit();
            this.pnl_firma.ResumeLayout(false);
            this.pnl_test_tarih.ResumeLayout(false);
            this.pnl_test_tarih.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_report)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_query_result;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.CheckBox chc_urun_dahil;
        private System.Windows.Forms.Panel pnl_urun;
        private System.Windows.Forms.CheckBox chc_firma_dahil;
        private System.Windows.Forms.Panel pnl_firma;
        private System.Windows.Forms.Button btn_urun_sec;
        private System.Windows.Forms.NumericUpDown nmr_sn_aralik_basla;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chc_sn_dahil;
        private System.Windows.Forms.Label lbl_urun_full_name;
        private System.Windows.Forms.NumericUpDown nmr_sn_aralik_bitis;
        private System.Windows.Forms.Label lbl_firma_ad;
        private System.Windows.Forms.Button btn_firma_sec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox combo_sn_tarih_bitis;
        private System.Windows.Forms.ComboBox combo_sn_tarih_basla;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_query_explain;
        private System.Windows.Forms.Label lbl_urun_id;
        private System.Windows.Forms.Label lbl_firma_id;
        private System.Windows.Forms.Panel pnl_sn_aralik;
        private System.Windows.Forms.CheckBox chc_test_tarih;
        private System.Windows.Forms.Panel pnl_test_tarih;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.DateTimePicker datePicker_test_tarih_baslangic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker datePicker_test_tarih_bitis;
        private System.Windows.Forms.Button btn_export_to_excel;
        private System.Windows.Forms.Button btn_transfer_source_to_report;
        private System.Windows.Forms.Button btn_delete_from_report;
        private System.Windows.Forms.DataGridView dgv_report;
    }
}