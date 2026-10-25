using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Aseguranza.Data.SqlServer
{
    public sealed class
        SqlServerExpedienteTrabajadorRepository
        : IExpedienteTrabajadorRepository
    {
        public DataTable Consultar(
            int idTrabajador)
        {
            DataTable tabla =
                new DataTable();

            if (idTrabajador <= 0)
            {
                return tabla;
            }

            using SqlConnection conexion =
                Conexion.Conectar();

            using SqlCommand comando = new SqlCommand(
                "spConsultarExpedienteTrabajador",
                conexion);

            comando.CommandType =
                CommandType.StoredProcedure;

            comando.Parameters.Add(
                "@IdTrabajador",
                SqlDbType.Int).Value =
                    idTrabajador;

            using SqlDataAdapter adaptador =
                new SqlDataAdapter(comando);

            conexion.Open();
            adaptador.Fill(tabla);

            return tabla;
        }

        public Mensaje Guardar(
            ExpedienteTrabajador expediente)
        {
            Mensaje? validacion =
                Validar(
                    expediente,
                    requiereId: false);

            if (validacion is not null)
            {
                return validacion;
            }

            return EjecutarGuardado(
                "spGuardarExpedienteTrabajador",
                expediente,
                incluirId: false);
        }

        public Mensaje Reemplazar(
            ExpedienteTrabajador expediente)
        {
            Mensaje? validacion =
                Validar(
                    expediente,
                    requiereId: true);

            if (validacion is not null)
            {
                return validacion;
            }

            return EjecutarGuardado(
                "spReemplazarExpedienteTrabajador",
                expediente,
                incluirId: true);
        }

        public Mensaje Eliminar(int id)
        {
            if (id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El archivo seleccionado no es válido."
                };
            }

            try
            {
                using SqlConnection conexion =
                    Conexion.Conectar();

                using SqlCommand comando = new SqlCommand(
                    "spEliminarExpedienteTrabajador",
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
            ExpedienteTrabajador expediente,
            bool incluirId)
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

                if (incluirId)
                {
                    comando.Parameters.Add(
                        "@Id",
                        SqlDbType.Int).Value =
                            expediente.Id;
                }
                else
                {
                    comando.Parameters.Add(
                        "@IdTrabajador",
                        SqlDbType.Int).Value =
                            expediente.IdTrabajador;
                }

                comando.Parameters.Add(
                    "@NombreOriginal",
                    SqlDbType.VarChar,
                    255).Value =
                        expediente.NombreOriginal!.Trim();

                comando.Parameters.Add(
                    "@NombreArchivo",
                    SqlDbType.VarChar,
                    255).Value =
                        expediente.NombreArchivo!.Trim();

                comando.Parameters.Add(
                    "@Extension",
                    SqlDbType.VarChar,
                    20).Value =
                        expediente.Extension!.Trim();

                comando.Parameters.Add(
                    "@RutaArchivo",
                    SqlDbType.VarChar,
                    500).Value =
                        expediente.RutaArchivo!.Trim();

                SqlParameter tipoArchivo =
                    comando.Parameters.Add(
                        "@TipoArchivo",
                        SqlDbType.VarChar,
                        50);

                tipoArchivo.Value =
                    string.IsNullOrWhiteSpace(
                        expediente.TipoArchivo)
                        ? DBNull.Value
                        : expediente.TipoArchivo.Trim();

                SqlParameter comentario =
                    comando.Parameters.Add(
                        "@Comentario",
                        SqlDbType.VarChar,
                        300);

                comentario.Value =
                    string.IsNullOrWhiteSpace(
                        expediente.Comentario)
                        ? DBNull.Value
                        : expediente.Comentario.Trim();

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
            ExpedienteTrabajador expediente,
            bool requiereId)
        {
            if (requiereId &&
                expediente.Id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El archivo seleccionado no es válido."
                };
            }

            if (!requiereId &&
                expediente.IdTrabajador <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El trabajador seleccionado no es válido."
                };
            }

            if (string.IsNullOrWhiteSpace(
                expediente.NombreOriginal))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre original del archivo es obligatorio."
                };
            }

            if (expediente.NombreOriginal.Trim().Length > 255)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre original no puede exceder 255 caracteres."
                };
            }

            if (string.IsNullOrWhiteSpace(
                expediente.NombreArchivo))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre interno del archivo es obligatorio."
                };
            }

            if (expediente.NombreArchivo.Trim().Length > 255)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre del archivo no puede exceder 255 caracteres."
                };
            }

            if (string.IsNullOrWhiteSpace(
                expediente.Extension))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La extensión del archivo es obligatoria."
                };
            }

            if (expediente.Extension.Trim().Length > 20)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La extensión no puede exceder 20 caracteres."
                };
            }

            if (string.IsNullOrWhiteSpace(
                expediente.RutaArchivo))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La ruta del archivo es obligatoria."
                };
            }

            if (expediente.RutaArchivo.Trim().Length > 500)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La ruta no puede exceder 500 caracteres."
                };
            }

            if (expediente.TipoArchivo?.Trim().Length > 50)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El tipo de archivo no puede exceder 50 caracteres."
                };
            }

            if (expediente.Comentario?.Trim().Length > 300)
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