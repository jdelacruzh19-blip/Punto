using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Punto.Forms
{
    internal class Conexion
    {
        private readonly string cadena;

        public Conexion()
        {
            cadena = "Server = 127.0.0.1; Database = puntodb; Uid = root; Pwd = ; Port = 3306";
        }

        public MySqlConnection getConection()
        {
            try
            {
                MySqlConnection con = new MySqlConnection(cadena);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
