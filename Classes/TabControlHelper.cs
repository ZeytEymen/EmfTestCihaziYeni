using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmfTestCihazi.Classes
{
    public static class TabControlHelper
    {
        //Sidebardaki butonlardan erişim yapacağım için tabcontrolun kendi butonlarını gizliyorum
        public static void HideTabControlHeaderButtons(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        //sender ile butonları alıyor ve tıklanan buton ismine göre tabcontroldaki ilişkili sayfayı açıyor
        public static void OpenTabPageWithButton(object sender, TabControl tabControl)
        {
            try
            {
                Button NavigationButton = sender as Button;
                if (NavigationButton != null)
                {
                    switch (NavigationButton.Name)
                    {
                        case "btnAbtfTestIslemleri":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageAbtfTestIslem"];
                            break;
                        case "btnAbtfAlistirma":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageAbtfAlistirma"];
                            break;
                        case "btnYbfTestIslemleri":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageYbfTestIslemleri"];
                            break;
                        case "btnYbfAlistirma":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageYbfAlistirma"];
                            break;
                        case "btnYbfBirakma":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageYbfBirakma"];
                            break;
                        case "btnYbfYakalama":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageYbfYakalama"];
                            break;
                        case "btnYbfDinamik":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageYbfDinamik"];
                            break;
                        case "btnYbfStatik":
                            tabControl.SelectedTab = tabControl.TabPages["tbPageYbfStatik"];
                            break;
                        default:
                            tabControl.SelectedTab = tabControl.TabPages["tbPageAbtfTestIslem"];
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Navigasyon Butonlarında Hata -> TabControlHelper\n"+ex.Message);
            }
        }
    }
}
