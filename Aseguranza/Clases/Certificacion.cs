using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aseguranza.Clases
{
    public class Certificacion
    {
        public int Id { get; set; }
        public int IdTrabajador { get; set; }
        public int IdProceso { get; set; }
        public DateTime FechaCertificacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdCertificador { get; set; }
        public string? Comentario { get; set; }

        public static DataTable ConsultarVerificacionNoReloj(string noReloj)
        {
            SqlConnection conexion = Clases.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spConsultarVerificacionNoReloj", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@NoReloj", noReloj);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static DataTable ConsultarCertificacionesPorTrabajador(int idTrabajador, string textoBuscar)
        {
            SqlConnection conexion = Clases.Conexion.Conectar();
            SqlCommand command = new SqlCommand("spConsultarCertificacionesPorTrabajador", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdTrabajador", idTrabajador);
            command.Parameters.AddWithValue("@TextoBuscar", textoBuscar);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public Clases.Mensaje GuardarCertificacion()
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spGuardarCertificacion", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdTrabajador", IdTrabajador);
            command.Parameters.AddWithValue("@IdProceso", IdProceso);
            command.Parameters.AddWithValue("@FechaCertificacion", FechaCertificacion);
            command.Parameters.AddWithValue("@IdCertificador", IdCertificador);
            command.Parameters.AddWithValue("@Comentario", Comentario);
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
        public static Clases.Mensaje BorrarCertificacion(int id)
        {
            Clases.Mensaje respuesta = new Clases.Mensaje();
            SqlConnection conexion = Conexion.Conectar();
            SqlCommand command = new SqlCommand("spBorrarCertificacion", conexion);
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
