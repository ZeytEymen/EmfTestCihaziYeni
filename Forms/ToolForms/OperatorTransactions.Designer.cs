namespace EmfTestCihazi.Forms.ToolForms
{
    partial class OperatorTransactions
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
            this.label145 = new System.Windows.Forms.Label();
            this.txt_update_operator = new System.Windows.Forms.TextBox();
            this.btn_update = new System.Windows.Forms.Button();
            this.gbox_update = new System.Windows.Forms.GroupBox();
            this.gbox_add = new System.Windows.Forms.GroupBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_add_operator = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_update_not = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_add_not = new System.Windows.Forms.TextBox();
            this.gbox_update.SuspendLayout();
            this.gbox_add.SuspendLayout();
            this.SuspendLayout();
            // 
            // label145
            // 
            this.label145.AutoSize = true;
            this.label145.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label145.Location = new System.Drawing.Point(7, 24);
            this.label145.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label145.Name = "label145";
            this.label145.Size = new System.Drawing.Size(85, 15);
            this.label145.TabIndex = 17;
            this.label145.Text = "Operatör İsmi";
            // 
            // txt_update_operator
            // 
            this.txt_update_operator.BackColor = System.Drawing.Color.Silver;
            this.txt_update_operator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_update_operator.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_update_operator.Location = new System.Drawing.Point(11, 47);
            this.txt_update_operator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_update_operator.Name = "txt_update_operator";
            this.txt_update_operator.Size = new System.Drawing.Size(160, 20);
            this.txt_update_operator.TabIndex = 18;
            // 
            // btn_update
            // 
            this.btn_update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_update.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_update.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_update.Location = new System.Drawing.Point(10, 86);
            this.btn_update.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(161, 24);
            this.btn_update.TabIndex = 33;
            this.btn_update.Text = "KAYDET ve ÇIK";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // gbox_update
            // 
            this.gbox_update.Controls.Add(this.label2);
            this.gbox_update.Controls.Add(this.txt_update_not);
            this.gbox_update.Controls.Add(this.label145);
            this.gbox_update.Controls.Add(this.btn_update);
            this.gbox_update.Controls.Add(this.txt_update_operator);
            this.gbox_update.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gbox_update.Location = new System.Drawing.Point(12, 12);
            this.gbox_update.Name = "gbox_update";
            this.gbox_update.Size = new System.Drawing.Size(342, 121);
            this.gbox_update.TabIndex = 35;
            this.gbox_update.TabStop = false;
            this.gbox_update.Text = "Güncelleme";
            // 
            // gbox_add
            // 
            this.gbox_add.Controls.Add(this.label3);
            this.gbox_add.Controls.Add(this.txt_add_not);
            this.gbox_add.Controls.Add(this.btn_add);
            this.gbox_add.Controls.Add(this.label1);
            this.gbox_add.Controls.Add(this.txt_add_operator);
            this.gbox_add.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gbox_add.Location = new System.Drawing.Point(12, 139);
            this.gbox_add.Name = "gbox_add";
            this.gbox_add.Size = new System.Drawing.Size(342, 116);
            this.gbox_add.TabIndex = 36;
            this.gbox_add.TabStop = false;
            this.gbox_add.Text = "Ekleme";
            // 
            // btn_add
            // 
            this.btn_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_add.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_add.Location = new System.Drawing.Point(16, 85);
            this.btn_add.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(155, 24);
            this.btn_add.TabIndex = 34;
            this.btn_add.Text = "KAYDET ve ÇIK";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Operatör İsmi";
            // 
            // txt_add_operator
            // 
            this.txt_add_operator.BackColor = System.Drawing.Color.Silver;
            this.txt_add_operator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_add_operator.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_add_operator.Location = new System.Drawing.Point(16, 46);
            this.txt_add_operator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_add_operator.Name = "txt_add_operator";
            this.txt_add_operator.Size = new System.Drawing.Size(155, 20);
            this.txt_add_operator.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(174, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "Not";
            // 
            // txt_update_not
            // 
            this.txt_update_not.BackColor = System.Drawing.Color.Silver;
            this.txt_update_not.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_update_not.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_update_not.Location = new System.Drawing.Point(177, 47);
            this.txt_update_not.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_update_not.Multiline = true;
            this.txt_update_not.Name = "txt_update_not";
            this.txt_update_not.Size = new System.Drawing.Size(153, 63);
            this.txt_update_not.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(174, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 15);
            this.label3.TabIndex = 36;
            this.label3.Text = "Not";
            // 
            // txt_add_not
            // 
            this.txt_add_not.BackColor = System.Drawing.Color.Silver;
            this.txt_add_not.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_add_not.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_add_not.Location = new System.Drawing.Point(177, 46);
            this.txt_add_not.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_add_not.Multiline = true;
            this.txt_add_not.Name = "txt_add_not";
            this.txt_add_not.Size = new System.Drawing.Size(153, 63);
            this.txt_add_not.TabIndex = 37;
            // 
            // OperatorTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(363, 266);
            this.Controls.Add(this.gbox_add);
            this.Controls.Add(this.gbox_update);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OperatorTransactions";
            this.Text = "Operatör İşlemleri";
            this.Load += new System.EventHandler(this.OperatorTransactions_Load);
            this.gbox_update.ResumeLayout(false);
            this.gbox_update.PerformLayout();
            this.gbox_add.ResumeLayout(false);
            this.gbox_add.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label145;
        private System.Windows.Forms.TextBox txt_update_operator;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.GroupBox gbox_update;
        private System.Windows.Forms.GroupBox gbox_add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_add_operator;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_update_not;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_add_not;
    }
}