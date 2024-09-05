namespace EmfTestCihazi.Forms.ToolForms
{
    partial class CompanyTransactions
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
            this.label14 = new System.Windows.Forms.Label();
            this.btn_duzenle = new System.Windows.Forms.Button();
            this.txt_not = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_tel = new System.Windows.Forms.TextBox();
            this.label143 = new System.Windows.Forms.Label();
            this.txt_kod = new System.Windows.Forms.TextBox();
            this.label144 = new System.Windows.Forms.Label();
            this.txt_ad = new System.Windows.Forms.TextBox();
            this.label145 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial Narrow", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label14.Location = new System.Drawing.Point(36, 4);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(164, 16);
            this.label14.TabIndex = 14;
            this.label14.Text = "(*) ile belirtilen alanlar zorunludur";
            // 
            // btn_duzenle
            // 
            this.btn_duzenle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_duzenle.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_duzenle.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_duzenle.Location = new System.Drawing.Point(14, 209);
            this.btn_duzenle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_duzenle.Name = "btn_duzenle";
            this.btn_duzenle.Size = new System.Drawing.Size(228, 32);
            this.btn_duzenle.TabIndex = 18;
            this.btn_duzenle.Text = "KAYDI DÜZENLE ve ÇIK";
            this.btn_duzenle.UseVisualStyleBackColor = true;
            this.btn_duzenle.Click += new System.EventHandler(this.btn_duzenle_Click);
            // 
            // txt_not
            // 
            this.txt_not.BackColor = System.Drawing.Color.Silver;
            this.txt_not.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_not.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_not.Location = new System.Drawing.Point(14, 132);
            this.txt_not.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_not.Multiline = true;
            this.txt_not.Name = "txt_not";
            this.txt_not.Size = new System.Drawing.Size(228, 72);
            this.txt_not.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 112);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 16);
            this.label13.TabIndex = 13;
            this.label13.Text = "Not";
            // 
            // txt_tel
            // 
            this.txt_tel.BackColor = System.Drawing.Color.Silver;
            this.txt_tel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_tel.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_tel.Location = new System.Drawing.Point(120, 85);
            this.txt_tel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_tel.Name = "txt_tel";
            this.txt_tel.Size = new System.Drawing.Size(122, 20);
            this.txt_tel.TabIndex = 10;
            // 
            // label143
            // 
            this.label143.AutoSize = true;
            this.label143.Location = new System.Drawing.Point(6, 85);
            this.label143.Name = "label143";
            this.label143.Size = new System.Drawing.Size(108, 16);
            this.label143.TabIndex = 11;
            this.label143.Text = "Telefon Numarası (*) :";
            // 
            // txt_kod
            // 
            this.txt_kod.BackColor = System.Drawing.Color.Silver;
            this.txt_kod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_kod.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_kod.Location = new System.Drawing.Point(120, 57);
            this.txt_kod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_kod.Name = "txt_kod";
            this.txt_kod.Size = new System.Drawing.Size(122, 20);
            this.txt_kod.TabIndex = 8;
            // 
            // label144
            // 
            this.label144.AutoSize = true;
            this.label144.Location = new System.Drawing.Point(26, 58);
            this.label144.Name = "label144";
            this.label144.Size = new System.Drawing.Size(68, 16);
            this.label144.TabIndex = 9;
            this.label144.Text = "Firma Kodu :";
            // 
            // txt_ad
            // 
            this.txt_ad.BackColor = System.Drawing.Color.Silver;
            this.txt_ad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_ad.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_ad.Location = new System.Drawing.Point(120, 29);
            this.txt_ad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_ad.Name = "txt_ad";
            this.txt_ad.Size = new System.Drawing.Size(122, 20);
            this.txt_ad.TabIndex = 6;
            // 
            // label145
            // 
            this.label145.AutoSize = true;
            this.label145.Location = new System.Drawing.Point(23, 29);
            this.label145.Name = "label145";
            this.label145.Size = new System.Drawing.Size(75, 16);
            this.label145.TabIndex = 7;
            this.label145.Text = "Firma Adı  (*) :";
            // 
            // CompanyTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(249, 249);
            this.Controls.Add(this.btn_duzenle);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txt_not);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txt_ad);
            this.Controls.Add(this.txt_tel);
            this.Controls.Add(this.label143);
            this.Controls.Add(this.label145);
            this.Controls.Add(this.txt_kod);
            this.Controls.Add(this.label144);
            this.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CompanyTransactions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Firma İşlemleri";
            this.Load += new System.EventHandler(this.UpdateCompany_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_duzenle;
        private System.Windows.Forms.TextBox txt_not;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_tel;
        private System.Windows.Forms.Label label143;
        private System.Windows.Forms.TextBox txt_kod;
        private System.Windows.Forms.Label label144;
        private System.Windows.Forms.TextBox txt_ad;
        private System.Windows.Forms.Label label145;
    }
}