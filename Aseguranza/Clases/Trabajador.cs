using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class Trabajador
    {
        public int Id { get; set; }
        public int NoReloj { get; set; }
        public string? Nombre { get; set; }
        public string? RutaFoto { get; set; }
        public int IdLocalidad { get; set; }
        public int IdTurno { get; set; }
        public int IdPlanta { get; set; }
        public int IdLinea { get; set; }

        public static DataTable ConsultarTrabajadores(string textoBuscar)
        {
            SqlConnection conexion = Conexion.Conectar();
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand command = new SqlCommand("spConsultarTrabajadores", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TextoBuscar", textoBuscar);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(tabla);
            conexion.Close();
            return tabla;
        }
        public static Clases.Mensaje BorrarTrabajador(int id)
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spBorrarTrabajador", conexion);
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
        public Clases.Mensaje GuardarTrabajador()
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spGuardarTrabajador", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@NoReloj", NoReloj);
            command.Parameters.AddWithValue("@Nombre", Nombre);
            command.Parameters.AddWithValue("@RutaFoto", RutaFoto);
            command.Parameters.AddWithValue("@IdLocalidad", IdLocalidad);
            command.Parameters.AddWithValue("@IdTurno", IdTurno);
            command.Parameters.AddWithValue("@IdLinea", IdLinea);
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
