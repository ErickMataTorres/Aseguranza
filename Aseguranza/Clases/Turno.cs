using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class Turno
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public static DataTable ConsultarTurnos(string textoBuscar)
        {
            SqlConnection conexion = Conexion.Conectar();
            DataTable tabla = new DataTable();
            conexion.Open();
            SqlCommand command = new SqlCommand("spConsultarTurnos", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TextoBuscar", textoBuscar);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(tabla);
            conexion.Close();
            return tabla;
        }
        public static Clases.Mensaje BorrarTurno(int id)
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spBorrarTurno", conexion);
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
        public Clases.Mensaje GuardarTurno()
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spGuardarTurno", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Nombre", Nombre);
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
