using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EmfTestCihazi.Classes
{
    public class DBLogHelper
    {
        private readonly DBHelper dbHelper;
        public DBLogHelper(DBHelper DBHelper)
        {
            dbHelper = DBHelper;
        }

        public void AddLog(string Message)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(dbHelper._connString))
                {
                    connection.Open();
                    string query = "INSERT INTO `logs`(`LogDate`, `Message`) VALUES (@date,@message)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@message", Message);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public DataTable GetLogs()
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(dbHelper._connString))
                {
                    connection.Open();
                    string query = "SELECT * FROM `logs`";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loglar Getirilirken Bir Hata Oluştu " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
    }
}
