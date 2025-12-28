using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class Conexion
    {
        public static SqlConnection Conectar()
        {
            string cadena = "Server = A;Database = AseguranzaBD;Trusted_Connection = True;Encrypt = True;TrustServerCertificate = True;";
            SqlConnection conexion = new SqlConnection(cadena);
            return conexion;
        }
    }
}
