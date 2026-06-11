using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Clases
{
    public class CertificacionAnulacion
    {
        public int Id { get; set; }
        public int IdCertificacion { get; set; }

        public string? TipoAnulacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool EsPermanente { get; set; }

        public string? Comentario { get; set; }
        public bool Activa { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public static CertificacionAnulacion? ConsultarAnulacionPorCertificacion(int idCertificacion)
        {
            CertificacionAnulacion? anulacion = null;

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spConsultarAnulacionPorCertificacion", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdCertificacion", idCertificacion);

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    anulacion = new CertificacionAnulacion
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        IdCertificacion = Convert.ToInt32(dr["IdCertificacion"]),
                        TipoAnulacion = dr["TipoAnulacion"].ToString(),
                        FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                        FechaFin = dr["FechaFin"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaFin"]),
                        EsPermanente = Convert.ToBoolean(dr["EsPermanente"]),
                        Comentario = dr["Comentario"].ToString(),
                        Activa = Convert.ToBoolean(dr["Activa"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaModificacion = dr["FechaModificacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaModificacion"])
                    };
                }

                dr.Close();
                conexion.Close();
            }
            catch
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }

                throw;
            }

            return anulacion;
        }

        public Mensaje GuardarCertificacionAnulacion()
        {
            Mensaje respuesta = new Mensaje();

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spGuardarCertificacionAnulacion", conexion);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@IdCertificacion", IdCertificacion);
            command.Parameters.AddWithValue("@TipoAnulacion", TipoAnulacion ?? "");
            command.Parameters.AddWithValue("@FechaInicio", FechaInicio.Date);

            command.Parameters.AddWithValue(
                "@FechaFin",
                FechaFin.HasValue ? FechaFin.Value.Date : (object)DBNull.Value
            );

            command.Parameters.AddWithValue("@EsPermanente", EsPermanente);
            command.Parameters.AddWithValue("@Comentario", Comentario ?? "");

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    respuesta.Id = Convert.ToInt32(dr["Id"]);
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

        public Mensaje ModificarCertificacionAnulacion()
        {
            Mensaje respuesta = new Mensaje();

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spModificarCertificacionAnulacion", conexion);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@TipoAnulacion", TipoAnulacion ?? "");
            command.Parameters.AddWithValue("@FechaInicio", FechaInicio.Date);

            command.Parameters.AddWithValue(
                "@FechaFin",
                FechaFin.HasValue ? FechaFin.Value.Date : (object)DBNull.Value
            );

            command.Parameters.AddWithValue("@EsPermanente", EsPermanente);
            command.Parameters.AddWithValue("@Comentario", Comentario ?? "");

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    respuesta.Id = Convert.ToInt32(dr["Id"]);
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

        public static Mensaje EliminarCertificacionAnulacion(int id)
        {
            Mensaje respuesta = new Mensaje();

            SqlConnection conexion = Conexion.Conectar();

            SqlCommand command = new SqlCommand("spEliminarCertificacionAnulacion", conexion);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            try
            {
                conexion.Open();

                SqlDataReader dr = command.ExecuteReader();

                if (dr.Read())
                {
                    respuesta.Id = Convert.ToInt32(dr["Id"]);
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
    }
}