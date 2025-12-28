using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class Certificador
    {
        public int Id { get; set; }
        public int IdTrabajador { get; set; }
        public int NoReloj { get; set; }
        public string? Nombre { get; set; }
        public string? RutaFoto { get; set; }
        public int IdTurno { get; set; }
        public string? NombreTurno { get; set; }
        public int IdPlanta { get; set; }
        public string? NombrePlanta { get; set; }

        public static DataTable ConsultarCertificadores()
        {
            SqlConnection conexion = Conexion.Conectar();
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand command = new SqlCommand("spConsultarCertificadores", conexion);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(tabla);
            conexion.Close();
            return tabla;
        }
    }
}
