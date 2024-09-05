using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace EmfTestCihazi.Classes
{
    public class DBHelper
    {
        public string _connString = @"Server=localhost;Database=emftestdevice00;Uid=root;Pwd=;";

        public bool ExecuteQuery(string CommandText, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(CommandText, connection))
                    {
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        if (command.ExecuteNonQuery() == 0)
                            throw new Exception("DBHelper.ExecuteQuery geriye 0 döndürdü. Hiçbir satır etkilenmedi");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : DBHelper.ExecuteQuery");
                return false;
            }
        }
        public object ExecuteAndGetId(string CommandText, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(CommandText, connection))
                    {
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);
                        if (command.ExecuteNonQuery() == 0)
                            throw new Exception("DBHelper.ExecuteQuery geriye 0 döndürdü. Hiçbir satır etkilenmedi");
                        command.CommandText = "SELECT LAST_INSERT_ID();";
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : DBHelper.ExecuteQuery");
                return null;
            }
        }
        public bool RecordExists(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
                            return false;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu : RecordExits");
                return false;
            }
        }

        public DataTable GetMultiple(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu = DBHelper.GetMultiple");
                return null;
            }
        }

        public void FillCombobox(ComboBox cbox, string query, string displayMember, string valueMember)
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(_connString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            cbox.DataSource = dataTable;
                            cbox.DisplayMember = displayMember;
                            cbox.ValueMember = valueMember;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu = DBHelper.FillCombobox");
            }
        }

        public object GetSingleObject(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        object code = cmd.ExecuteScalar();
                        return code;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu = DBHelper.GetSingleObject");
                return null;
            }
        }

        public void AddLog(string message, string path = "Program")
        {
            try
            {
                MySqlParameter[] parameters = {
                    new MySqlParameter("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new MySqlParameter("@path", path),
                    new MySqlParameter("@message", message)};
                ExecuteQuery("INSERT INTO `logs`(`LogDate`,`LogPath`,`Message`) VALUES (@date,@path,@message)", parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Hata Yolu = DBHelper.AddLog()","HATA");
            }

        }
        public DataTable GetLogs()
        {
            try
            {
                return GetMultiple("SELECT * FROM `logs`");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Hata Yolu = DBHelper.GetLogs()", "HATA");
                return null;
            }
        }
    }
}
