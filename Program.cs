using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmfTestCihazi
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Programın sadece bir örneğinin çalışmasını istiyoruz bu sebeple mutex ile kitleyeceğiz
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "SingleInstanceAppMutex", out createdNew))
            {
                if (createdNew)
                {
                    // Eğer mutex oluşturulduysa, uygulama örneğini başlat
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    // Uygulama zaten çalışıyor
                    MessageBox.Show("UYGULAMA ZATEN ÇALIŞIYOR YENİSİNİ AÇAMAZSINIZ !!!", "HATA !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
