using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class ExpedienteTrabajador
    {
        public int Id { get; set; }
        public int IdTrabajador { get; set; }

        public string? NombreOriginal { get; set; }
        public string? NombreArchivo { get; set; }
        public string? Extension { get; set; }
        public string? RutaArchivo { get; set; }
        public string? TipoArchivo { get; set; }
        public string? Comentario { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public static DataTable ConsultarExpedienteTrabajador(int idTrabajador)
        {
            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spConsultarExpedienteTrabajador", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdTrabajador", idTrabajador);

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();

            da.Fill(dt);

            return dt;
        }

        public Mensaje GuardarExpedienteTrabajador()
        {
            Mensaje respuesta = new Mensaje();

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spGuardarExpedienteTrabajador", conexion);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@IdTrabajador", IdTrabajador);
            command.Parameters.AddWithValue("@NombreOriginal", NombreOriginal ?? "");
            command.Parameters.AddWithValue("@NombreArchivo", NombreArchivo ?? "");
            command.Parameters.AddWithValue("@Extension", Extension ?? "");
            command.Parameters.AddWithValue("@RutaArchivo", RutaArchivo ?? "");

            command.Parameters.AddWithValue(
                "@TipoArchivo",
                string.IsNullOrWhiteSpace(TipoArchivo) ? DBNull.Value : TipoArchivo
            );

            command.Parameters.AddWithValue(
                "@Comentario",
                string.IsNullOrWhiteSpace(Comentario) ? DBNull.Value : Comentario
            );

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    respuesta.Id = int.Parse(dr["Id"].ToString()!);
                    respuesta.Nombre = dr["Nombre"].ToString();
                }

                dr.Close();
                conexion.Close();
            }
            catch (Exception error)
            {
                respuesta.Id = 0;
                respuesta.Nombre = error.Message;

                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            return respuesta;
        }

        public Mensaje ReemplazarExpedienteTrabajador()
        {
            Mensaje respuesta = new Mensaje();

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spReemplazarExpedienteTrabajador", conexion);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@NombreOriginal", NombreOriginal ?? "");
            command.Parameters.AddWithValue("@NombreArchivo", NombreArchivo ?? "");
            command.Parameters.AddWithValue("@Extension", Extension ?? "");
            command.Parameters.AddWithValue("@RutaArchivo", RutaArchivo ?? "");

            command.Parameters.AddWithValue(
                "@TipoArchivo",
                string.IsNullOrWhiteSpace(TipoArchivo) ? DBNull.Value : TipoArchivo
            );

            command.Parameters.AddWithValue(
                "@Comentario",
                string.IsNullOrWhiteSpace(Comentario) ? DBNull.Value : Comentario
            );

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    respuesta.Id = int.Parse(dr["Id"].ToString()!);
                    respuesta.Nombre = dr["Nombre"].ToString();
                }

                dr.Close();
                conexion.Close();
            }
            catch (Exception error)
            {
                respuesta.Id = 0;
                respuesta.Nombre = error.Message;

                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            return respuesta;
        }

        public static Mensaje EliminarExpedienteTrabajador(int id)
        {
            Mensaje respuesta = new Mensaje();

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spEliminarExpedienteTrabajador", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    respuesta.Id = int.Parse(dr["Id"].ToString()!);
                    respuesta.Nombre = dr["Nombre"].ToString();
                }

                dr.Close();
                conexion.Close();
            }
            catch (Exception error)
            {
                respuesta.Id = 0;
                respuesta.Nombre = error.Message;

                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            return respuesta;
        }

        public static string ObtenerTipoArchivo(string extension)
        {
            extension = extension.ToLower().Trim();

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                return "Imagen";

            if (extension == ".pdf")
                return "PDF";

            if (extension == ".doc" || extension == ".docx")
                return "Word";

            if (extension == ".xls" || extension == ".xlsx")
                return "Excel";

            return "Documento";
        }
    }
}