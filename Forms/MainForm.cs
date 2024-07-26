using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EmfTestCihazi.Classes.TabControlHelper;
using static EmfTestCihazi.Classes.InputValidationHelper;

namespace EmfTestCihazi
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region MainFormLoad
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Sidebardaki butonları kullanmak için TabControl butonlarını gizler
            HideTabControlHeaderButtons(tabControlMain);
        }

        #endregion

        //Uygulamayı Kapatır
        private void pctCloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Uygulamayı görev çubuğuna indirir
        private void pctMinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


       //Sidebar butonlarının ortak click olayı
        private void SideBarButtons_Click(object sender, EventArgs e)
        {
            //Sidebar üzerindeki butonların tıklanma olayında ilgili tabpage sayfası açılır
            OpenTabPageWithButton(sender, tabControlMain);
        }

        //Girdi olarak sadece Tam Sayı kabul edilmesi istenen textboxların ortak keypress(tuşa basılma) eventi
        private void txtBoxKeyPressOnlyDigit(object sender, KeyPressEventArgs e)
        {
            //Sadece TamSayı ve kontrol butonları(backspace ctrl kombinasyonları vs)
            ValidateOnlyDigit(e);
        }
    }
}
