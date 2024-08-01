using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace EmfTestCihazi.Classes
{
    public class DBHelper
    {
        public string _connString = @"Server=localhost;Database=emftestdevice00;Uid=root;Pwd=;";

        public DBHelper()
        {
             
        }

        public string AddNew(string CommandText)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(CommandText, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return "İşlem Başarılı";
                        }
                        else
                        {
                            return "Hiçbir satır etkilenmedi";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"İşlem Başarısız: {ex.Message}";
            }
        }
    }
}
