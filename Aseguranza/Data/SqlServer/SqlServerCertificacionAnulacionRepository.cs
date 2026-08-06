using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class
        SqlServerCertificacionAnulacionRepository
        : ICertificacionAnulacionRepository
    {
        public CertificacionAnulacion?
            ConsultarPorCertificacion(
                int idCertificacion)
        {
            if (idCertificacion <= 0)
            {
                return null;
            }

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarAnulacionPorCertificacion",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@IdCertificacion",
                SqlDbType.Int).Value =
                    idCertificacion;

            conexion.Open();

            using SqlDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            return new CertificacionAnulacion
            {
                Id = Convert.ToInt32(
                    lector["Id"]),

                IdCertificacion = Convert.ToInt32(
                    lector["IdCertificacion"]),

                TipoAnulacion = Convert.ToString(
                    lector["TipoAnulacion"]),

                FechaInicio = Convert.ToDateTime(
                    lector["FechaInicio"]),

                FechaFin =
                    lector["FechaFin"] is DBNull
                        ? null
                        : Convert.ToDateTime(
                            lector["FechaFin"]),

                EsPermanente = Convert.ToBoolean(
                    lector["EsPermanente"]),

                Comentario = Convert.ToString(
                    lector["Comentario"]),

                Activa = Convert.ToBoolean(
                    lector["Activa"]),

                FechaRegistro = Convert.ToDateTime(
                    lector["FechaRegistro"]),

                FechaModificacion =
                    lector["FechaModificacion"] is DBNull
                        ? null
                        : Convert.ToDateTime(
                            lector["FechaModificacion"])
            };
        }

        public Mensaje Guardar(
            CertificacionAnulacion anulacion)
        {
            Mensaje? validacion =
                Validar(anulacion, requiereId: false);

            if (validacion is not null)
            {
                return validacion;
            }

            return EjecutarGuardado(
                "spGuardarCertificacionAnulacion",
                anulacion,
                incluirIdAnulacion: false);
        }

        public Mensaje Modificar(
            CertificacionAnulacion anulacion)
        {
            Mensaje? validacion =
                Validar(anulacion, requiereId: true);

            if (validacion is not null)
            {
                return validacion;
            }

            return EjecutarGuardado(
                "spModificarCertificacionAnulacion",
                anulacion,
                incluirIdAnulacion: true);
        }

        public Mensaje Eliminar(int id)
        {
            if (id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La anulación seleccionada no es válida."
                };
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spEliminarCertificacionAnulacion",
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

        private static Mensaje EjecutarGuardado(
            string procedimiento,
            CertificacionAnulacion anulacion,
            bool incluirIdAnulacion)
        {
            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    procedimiento,
                    conexion);

                comando.CommandType =
                    CommandType.StoredProcedure;

                if (incluirIdAnulacion)
                {
                    comando.Parameters.Add(
                        "@Id",
                        SqlDbType.Int).Value =
                            anulacion.Id;
                }
                else
                {
                    comando.Parameters.Add(
                        "@IdCertificacion",
                        SqlDbType.Int).Value =
                            anulacion.IdCertificacion;
                }

                comando.Parameters.Add(
                    "@TipoAnulacion",
                    SqlDbType.VarChar,
                    50).Value =
                        anulacion.TipoAnulacion!
                            .Trim();

                comando.Parameters.Add(
                    "@FechaInicio",
                    SqlDbType.Date).Value =
                        anulacion.FechaInicio.Date;

                SqlParameter fechaFin =
                    comando.Parameters.Add(
                        "@FechaFin",
                        SqlDbType.Date);

                fechaFin.Value =
                    anulacion.EsPermanente ||
                    !anulacion.FechaFin.HasValue
                        ? DBNull.Value
                        : anulacion.FechaFin.Value.Date;

                comando.Parameters.Add(
                    "@EsPermanente",
                    SqlDbType.Bit).Value =
                        anulacion.EsPermanente;

                comando.Parameters.Add(
                    "@Comentario",
                    SqlDbType.VarChar,
                    500).Value =
                        anulacion.Comentario!
                            .Trim();

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
            CertificacionAnulacion anulacion,
            bool requiereId)
        {
            if (requiereId && anulacion.Id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La anulación seleccionada no es válida."
                };
            }

            if (!requiereId &&
                anulacion.IdCertificacion <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La certificación seleccionada no es válida."
                };
            }

            if (string.IsNullOrWhiteSpace(
                anulacion.TipoAnulacion))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un tipo de anulación."
                };
            }

            if (anulacion.TipoAnulacion.Trim().Length > 50)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El tipo de anulación no puede exceder 50 caracteres."
                };
            }

            if (anulacion.FechaInicio == default)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La fecha de inicio no es válida."
                };
            }

            if (!anulacion.EsPermanente &&
                !anulacion.FechaFin.HasValue)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar una fecha fin para una anulación temporal."
                };
            }

            if (!anulacion.EsPermanente &&
                anulacion.FechaFin.HasValue &&
                anulacion.FechaFin.Value.Date <
                anulacion.FechaInicio.Date)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La fecha fin no puede ser menor que la fecha inicio."
                };
            }

            if (string.IsNullOrWhiteSpace(
                anulacion.Comentario))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe escribir un comentario de la anulación."
                };
            }

            if (anulacion.Comentario.Trim().Length > 500)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El comentario no puede exceder 500 caracteres."
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
                    lector["Nombre"])
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