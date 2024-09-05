using EmfTestCihazi.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp7;
using MySql.Data.MySqlClient;

namespace EmfTestCihazi
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Programın sadece bir örneğinin çalışmasını istiyoruz bu sebeple mutex ile kitleyeceğiz
            bool createdNew;
            S7Client client = new S7Client();
            using (Mutex mutex = new Mutex(true, "SingleInstanceAppMutex", out createdNew))
            {
                if (createdNew)
                {
                    try
                    {
                        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=emftestdevice00;Uid=root;Pwd=;");
                        conn.Open();
                        if (conn.State == System.Data.ConnectionState.Open)
                            conn.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Veritabanı bağlantısı başarısız, XAMPP uygulamasından Apache ve MySQL servisini aktif ediniz\nHata devam ederse geliştirici ile iletişime geçiniz\nHata Yolu : Program.cs");
                        Environment.Exit(0);
                    }
                    string ipAdress = "10.10.1.86";
                    int plcResult = client.ConnectTo(ipAdress, 0, 0);
                    if (plcResult != 0)
                    {
                        MessageBox.Show($"PLC bağlantısı başarısız \nBağlantı kurulması gereken plc IP Adresi {ipAdress}\nİlgili ağ bağdaştırıcısından IPV4 özelliklerinde IP ailesinin 192.168.0.XXX değerinde olduğunudan emin olunuz\nHata Yolu : Program.cs");
                        Environment.Exit(0);
                    }
                    else
                        client.Disconnect();
                    
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
