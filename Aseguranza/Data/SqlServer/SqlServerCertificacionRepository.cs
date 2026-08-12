using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerCertificacionRepository
        : ICertificacionRepository
    {
        public DataTable ConsultarVerificacionNoReloj(
            string noReloj)
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarVerificacionNoReloj",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@NoReloj",
                SqlDbType.VarChar,
                50).Value =
                    noReloj?.Trim() ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();
            adaptador.Fill(tabla);

            return tabla;
        }

        public DataTable ConsultarPorTrabajador(
            int idTrabajador,
            string textoBuscar)
        {
            DataTable tabla = new DataTable();

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarCertificacionesPorTrabajador",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@IdTrabajador",
                SqlDbType.Int).Value =
                    idTrabajador;

            comando.Parameters.Add(
                "@TextoBuscar",
                SqlDbType.VarChar,
                100).Value =
                    textoBuscar ?? string.Empty;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();
            adaptador.Fill(tabla);

            return tabla;
        }

        public Mensaje Guardar(
            Certificacion certificacion)
        {
            Mensaje? validacion =
                Validar(certificacion);

            if (validacion is not null)
            {
                return validacion;
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spGuardarCertificacion",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@IdTrabajador",
                    SqlDbType.Int).Value =
                        certificacion.IdTrabajador;

                comando.Parameters.Add(
                    "@IdProceso",
                    SqlDbType.Int).Value =
                        certificacion.IdProceso;

                comando.Parameters.Add(
                    "@FechaCertificacion",
                    SqlDbType.Date).Value =
                        certificacion.FechaCertificacion.Date;

                comando.Parameters.Add(
                    "@IdCertificador",
                    SqlDbType.Int).Value =
                        certificacion.IdCertificador;

                SqlParameter parametroComentario =
                    comando.Parameters.Add(
                        "@Comentario",
                        SqlDbType.VarChar,
                        300);

                parametroComentario.Value =
                    string.IsNullOrWhiteSpace(
                        certificacion.Comentario)
                        ? DBNull.Value
                        : certificacion.Comentario.Trim();

                conexion.Open();

                using SqlDataReader lector =
                    comando.ExecuteReader();

                return LeerMensaje(lector);
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        public Mensaje Borrar(int id)
        {
            if (id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La certificación seleccionada no es válida."
                };
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spBorrarCertificacion",
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                comando.Parameters.Add(
                    "@Id",
                    SqlDbType.Int).Value = id;

                conexion.Open();

                using SqlDataReader lector =
                    comando.ExecuteReader();

                return LeerMensaje(lector);
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static Mensaje? Validar(
            Certificacion certificacion)
        {
            if (certificacion.IdTrabajador <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El trabajador seleccionado no es válido."
                };
            }

            if (certificacion.IdProceso <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un proceso válido."
                };
            }

            if (certificacion.IdCertificador <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un certificador válido."
                };
            }

            if (certificacion.FechaCertificacion ==
                default)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La fecha de certificación no es válida."
                };
            }

            if (certificacion.Comentario?.Length > 300)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El comentario no puede exceder 300 caracteres."
                };
            }

            return null;
        }

        private static Mensaje LeerMensaje(
            SqlDataReader lector)
        {
            if (!lector.Read())
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La operación no devolvió un resultado."
                };
            }

            return new Mensaje
            {
                Id = Convert.ToInt32(
                    lector["Id"]),

                Nombre = Convert.ToString(
                    lector["Nombre"]) ?? string.Empty
            };
        }

        private static Mensaje CrearMensajeError(
            Exception excepcion)
        {
            return new Mensaje
            {
                Id = 0,
                Nombre =
                    "Ocurrió un error al acceder a SQL Server. " +
                    excepcion.Message
            };
        }
    }
}