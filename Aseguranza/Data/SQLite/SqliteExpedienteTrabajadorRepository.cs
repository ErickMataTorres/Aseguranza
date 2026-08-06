using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Globalization;

namespace Aseguranza.Data.SQLite
{
    public sealed class
        SqliteExpedienteTrabajadorRepository
        : IExpedienteTrabajadorRepository
    {
        public DataTable Consultar(int idTrabajador)
        {
            DataTable tabla = CrearTablaExpediente();

            if (idTrabajador <= 0)
            {
                return tabla;
            }

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    Id,
                    IdTrabajador,
                    NombreOriginal,
                    NombreArchivo,
                    Extension,
                    RutaArchivo,
                    TipoArchivo,
                    Comentario,
                    FechaRegistro,
                    FechaModificacion
                FROM ExpedienteTrabajador
                WHERE IdTrabajador = @IdTrabajador
                  AND Activo = 1
                ORDER BY
                    datetime(FechaRegistro) DESC,
                    Id DESC;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            using SqliteDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                DataRow fila = tabla.NewRow();

                fila["Id"] =
                    Convert.ToInt32(lector["Id"]);

                fila["IdTrabajador"] =
                    Convert.ToInt32(
                        lector["IdTrabajador"]);

                fila["NombreOriginal"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreOriginal");

                fila["NombreArchivo"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreArchivo");

                fila["Extension"] =
                    LeerTextoODbNull(
                        lector,
                        "Extension");

                fila["RutaArchivo"] =
                    LeerTextoODbNull(
                        lector,
                        "RutaArchivo");

                fila["TipoArchivo"] =
                    LeerTextoODbNull(
                        lector,
                        "TipoArchivo");

                fila["Comentario"] =
                    LeerTextoODbNull(
                        lector,
                        "Comentario");

                fila["FechaRegistro"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaRegistro");

                fila["FechaModificacion"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaModificacion");

                tabla.Rows.Add(fila);
            }

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

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                if (!ExisteTrabajador(
                    conexion,
                    transaccion,
                    expediente.IdTrabajador))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró el trabajador seleccionado."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction = transaccion;

                comando.CommandText = """
                    INSERT INTO ExpedienteTrabajador
                    (
                        IdTrabajador,
                        NombreOriginal,
                        NombreArchivo,
                        Extension,
                        RutaArchivo,
                        TipoArchivo,
                        Comentario,
                        Activo,
                        FechaRegistro,
                        FechaModificacion
                    )
                    VALUES
                    (
                        @IdTrabajador,
                        @NombreOriginal,
                        @NombreArchivo,
                        @Extension,
                        @RutaArchivo,
                        @TipoArchivo,
                        @Comentario,
                        1,
                        datetime('now', 'localtime'),
                        NULL
                    );
                    """;

                comando.Parameters.AddWithValue(
                    "@IdTrabajador",
                    expediente.IdTrabajador);

                AgregarParametrosArchivo(
                    comando,
                    expediente);

                comando.ExecuteNonQuery();

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Archivo agregado correctamente al expediente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
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

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                if (!ExisteExpedienteActivo(
                    conexion,
                    transaccion,
                    expediente.Id))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró el archivo activo que intenta reemplazar."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction = transaccion;

                comando.CommandText = """
                    UPDATE ExpedienteTrabajador
                    SET
                        NombreOriginal = @NombreOriginal,
                        NombreArchivo = @NombreArchivo,
                        Extension = @Extension,
                        RutaArchivo = @RutaArchivo,
                        TipoArchivo = @TipoArchivo,
                        Comentario = @Comentario,
                        FechaModificacion =
                            datetime('now', 'localtime')
                    WHERE Id = @Id
                      AND Activo = 1;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    expediente.Id);

                AgregarParametrosArchivo(
                    comando,
                    expediente);

                int filasAfectadas =
                    comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró el archivo activo que intenta reemplazar."
                    };
                }

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Archivo reemplazado correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
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
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteCommand comando =
                    conexion.CreateCommand();

                /*
                 * La eliminación del registro es lógica.
                 * La ventana administra por separado
                 * la eliminación del archivo físico.
                 */
                comando.CommandText = """
                    UPDATE ExpedienteTrabajador
                    SET
                        Activo = 0,
                        FechaModificacion =
                            datetime('now', 'localtime')
                    WHERE Id = @Id
                      AND Activo = 1;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    id);

                int filasAfectadas =
                    comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró el archivo activo que intenta eliminar."
                    };
                }

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Archivo eliminado correctamente del expediente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static bool ExisteTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador)
        {
            if (idTrabajador <= 0)
            {
                return false;
            }

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Trabajador
                WHERE Id = @IdTrabajador;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteExpedienteActivo(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int id)
        {
            if (id <= 0)
            {
                return false;
            }

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM ExpedienteTrabajador
                WHERE Id = @Id
                  AND Activo = 1;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static void AgregarParametrosArchivo(
            SqliteCommand comando,
            ExpedienteTrabajador expediente)
        {
            comando.Parameters.AddWithValue(
                "@NombreOriginal",
                expediente.NombreOriginal!.Trim());

            comando.Parameters.AddWithValue(
                "@NombreArchivo",
                expediente.NombreArchivo!.Trim());

            comando.Parameters.AddWithValue(
                "@Extension",
                expediente.Extension!.Trim());

            comando.Parameters.AddWithValue(
                "@RutaArchivo",
                expediente.RutaArchivo!.Trim());

            comando.Parameters.AddWithValue(
                "@TipoArchivo",
                string.IsNullOrWhiteSpace(
                    expediente.TipoArchivo)
                    ? DBNull.Value
                    : expediente.TipoArchivo.Trim());

            comando.Parameters.AddWithValue(
                "@Comentario",
                string.IsNullOrWhiteSpace(
                    expediente.Comentario)
                    ? DBNull.Value
                    : expediente.Comentario.Trim());
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

            if (expediente.NombreOriginal
                .Trim()
                .Length > 255)
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

            if (expediente.NombreArchivo
                .Trim()
                .Length > 255)
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

            if (expediente.Extension
                .Trim()
                .Length > 20)
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

            if (expediente.RutaArchivo
                .Trim()
                .Length > 500)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La ruta no puede exceder 500 caracteres."
                };
            }

            if (expediente.TipoArchivo?
                .Trim()
                .Length > 50)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El tipo de archivo no puede exceder 50 caracteres."
                };
            }

            if (expediente.Comentario?
                .Trim()
                .Length > 300)
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

        private static DataTable CrearTablaExpediente()
        {
            DataTable tabla = new DataTable();

            tabla.Columns.Add(
                "Id",
                typeof(int));

            tabla.Columns.Add(
                "IdTrabajador",
                typeof(int));

            tabla.Columns.Add(
                "NombreOriginal",
                typeof(string));

            tabla.Columns.Add(
                "NombreArchivo",
                typeof(string));

            tabla.Columns.Add(
                "Extension",
                typeof(string));

            tabla.Columns.Add(
                "RutaArchivo",
                typeof(string));

            tabla.Columns.Add(
                "TipoArchivo",
                typeof(string));

            tabla.Columns.Add(
                "Comentario",
                typeof(string));

            tabla.Columns.Add(
                "FechaRegistro",
                typeof(DateTime));

            tabla.Columns.Add(
                "FechaModificacion",
                typeof(DateTime));

            return tabla;
        }

        private static object LeerTextoODbNull(
            SqliteDataReader lector,
            string columna)
        {
            int ordinal =
                lector.GetOrdinal(columna);

            if (lector.IsDBNull(ordinal))
            {
                return DBNull.Value;
            }

            return Convert.ToString(
                lector.GetValue(ordinal))
                ?? string.Empty;
        }

        private static object LeerFechaODbNull(
            SqliteDataReader lector,
            string columna)
        {
            int ordinal =
                lector.GetOrdinal(columna);

            if (lector.IsDBNull(ordinal))
            {
                return DBNull.Value;
            }

            string? texto =
                Convert.ToString(
                    lector.GetValue(ordinal));

            if (string.IsNullOrWhiteSpace(texto))
            {
                return DBNull.Value;
            }

            if (DateTime.TryParse(
                texto,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime fecha))
            {
                return fecha;
            }

            return DBNull.Value;
        }

        private static Mensaje CrearMensajeError(
            Exception excepcion)
        {
            return new Mensaje
            {
                Id = 0,
                Nombre =
                    "Ocurrió un error al acceder a SQLite. " +
                    excepcion.Message
            };
        }
    }
}