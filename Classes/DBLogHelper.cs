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

      
    }
}
