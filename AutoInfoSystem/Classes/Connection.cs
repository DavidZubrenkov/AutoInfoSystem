using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace AutoInfoSystem
{
    public class Connection
    {
        public MySqlConnection con1 = new MySqlConnection("server = localhost; uid=root;password=;database=db32"); // храним подключение
    }
}
