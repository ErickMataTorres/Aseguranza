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
        public string? NoReloj { get; set; }
        public string? Nombre { get; set; }
        public string? RutaFoto { get; set; }
        public int IdTurno { get; set; }
        public string? NombreTurno { get; set; }
        public int IdPlanta { get; set; }
        public string? NombrePlanta { get; set; }

        public static DataTable ConsultarCertificadores(string textoBuscar)
        {
            SqlConnection conexion = Conexion.Conectar();
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand command = new SqlCommand("spConsultarCertificadores", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TextoBuscar", textoBuscar);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(tabla);
            conexion.Close();
            return tabla;
        }
        public Clases.Mensaje GuardarCertificador()
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spGuardarCertificador", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@NoReloj", NoReloj);
            conexion.Open();
            SqlDataReader dr = command.ExecuteReader();
            try
            {
                if (dr.Read())
                {
                    respuesta.Id = int.Parse(dr["Id"].ToString()!);
                    respuesta.Nombre = dr["Nombre"].ToString();
                }
            }
            catch (Exception error)
            {
                respuesta.Nombre = error.Message;
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return respuesta;
        }
        public static Clases.Mensaje BorrarCertificador(int id)
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spBorrarCertificador", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);
            conexion.Open();
            SqlDataReader dr = command.ExecuteReader();
            try
            {
                if (dr.Read())
                {
                    respuesta.Id = int.Parse(dr["Id"].ToString()!);
                    respuesta.Nombre = dr["Nombre"].ToString();
                }
            }
            catch (Exception error)
            {
                respuesta.Nombre = error.Message;
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return respuesta;
        }
    }
}
