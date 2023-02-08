using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Systems_Integration_Architechture
{
    public class DBClass
    {
        public static string server = "localhost";
        public static string database = "system_integration";
        public static string uid = "root";
        public static string password = "";

        public static string ConnectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        public static MySqlConnection connection = new MySqlConnection(ConnectionString);
    }

}
