using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Globalization;

namespace Aseguranza.Data.SQLite
{
    public sealed class
        SqliteCertificacionAnulacionRepository
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

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    Id,
                    IdCertificacion,
                    TipoAnulacion,
                    FechaInicio,
                    FechaFin,
                    EsPermanente,
                    Comentario,
                    Activa,
                    FechaRegistro,
                    FechaModificacion
                FROM CertificacionAnulacion
                WHERE IdCertificacion = @IdCertificacion
                  AND Activa = 1
                  AND
                  (
                      EsPermanente = 1
                      OR FechaFin IS NULL
                      OR date(FechaFin)
                         >= date('now', 'localtime')
                  )
                ORDER BY
                    datetime(FechaRegistro) DESC,
                    Id DESC
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificacion",
                idCertificacion);

            using SqliteDataReader lector =
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

                FechaInicio = LeerFecha(
                    lector,
                    "FechaInicio"),

                FechaFin = LeerFechaNullable(
                    lector,
                    "FechaFin"),

                EsPermanente =
                    Convert.ToInt32(
                        lector["EsPermanente"]) == 1,

                Comentario = Convert.ToString(
                    lector["Comentario"]),

                Activa =
                    Convert.ToInt32(
                        lector["Activa"]) == 1,

                FechaRegistro = LeerFecha(
                    lector,
                    "FechaRegistro"),

                FechaModificacion =
                    LeerFechaNullable(
                        lector,
                        "FechaModificacion")
            };
        }

        public Mensaje Guardar(
            CertificacionAnulacion anulacion)
        {
            Mensaje? validacion =
                Validar(
                    anulacion,
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

                if (!ExisteCertificacion(
                    conexion,
                    transaccion,
                    anulacion.IdCertificacion))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró la certificación seleccionada."
                    };
                }

                /*
                 * Conserva el historial y evita que existan
                 * dos anulaciones activas simultáneamente.
                 */
                DesactivarAnulacionesActivas(
                    conexion,
                    transaccion,
                    anulacion.IdCertificacion);

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction =
                    transaccion;

                comando.CommandText = """
                    INSERT INTO CertificacionAnulacion
                    (
                        IdCertificacion,
                        TipoAnulacion,
                        FechaInicio,
                        FechaFin,
                        EsPermanente,
                        Comentario,
                        Activa,
                        FechaRegistro,
                        FechaModificacion
                    )
                    VALUES
                    (
                        @IdCertificacion,
                        @TipoAnulacion,
                        @FechaInicio,
                        @FechaFin,
                        @EsPermanente,
                        @Comentario,
                        1,
                        datetime('now', 'localtime'),
                        NULL
                    );
                    """;

                comando.Parameters.AddWithValue(
                    "@IdCertificacion",
                    anulacion.IdCertificacion);

                AgregarParametrosAnulacion(
                    comando,
                    anulacion);

                comando.ExecuteNonQuery();

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Certificación anulada correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        public Mensaje Modificar(
            CertificacionAnulacion anulacion)
        {
            Mensaje? validacion =
                Validar(
                    anulacion,
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

                if (!ExisteAnulacionActiva(
                    conexion,
                    transaccion,
                    anulacion.Id))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró una anulación activa para modificar."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction =
                    transaccion;

                comando.CommandText = """
                    UPDATE CertificacionAnulacion
                    SET
                        TipoAnulacion =
                            @TipoAnulacion,

                        FechaInicio =
                            @FechaInicio,

                        FechaFin =
                            @FechaFin,

                        EsPermanente =
                            @EsPermanente,

                        Comentario =
                            @Comentario,

                        FechaModificacion =
                            datetime('now', 'localtime')

                    WHERE Id = @Id
                      AND Activa = 1;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    anulacion.Id);

                AgregarParametrosAnulacion(
                    comando,
                    anulacion);

                int filasAfectadas =
                    comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró una anulación activa para modificar."
                    };
                }

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Anulación modificada correctamente."
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
                        "La anulación seleccionada no es válida."
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
                 * La eliminación es lógica. El registro permanece
                 * en la base de datos para conservar el historial.
                 */
                comando.CommandText = """
                    UPDATE CertificacionAnulacion
                    SET
                        Activa = 0,
                        FechaModificacion =
                            datetime('now', 'localtime')
                    WHERE Id = @Id
                      AND Activa = 1;
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
                            "No se encontró una anulación activa para eliminar."
                    };
                }

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Anulación eliminada correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static bool ExisteCertificacion(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idCertificacion)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificacion
                WHERE Id = @IdCertificacion;
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificacion",
                idCertificacion);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteAnulacionActiva(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idAnulacion)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM CertificacionAnulacion
                WHERE Id = @Id
                  AND Activa = 1;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                idAnulacion);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static void
            DesactivarAnulacionesActivas(
                SqliteConnection conexion,
                SqliteTransaction transaccion,
                int idCertificacion)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                UPDATE CertificacionAnulacion
                SET
                    Activa = 0,
                    FechaModificacion =
                        datetime('now', 'localtime')
                WHERE IdCertificacion = @IdCertificacion
                  AND Activa = 1;
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificacion",
                idCertificacion);

            comando.ExecuteNonQuery();
        }

        private static void AgregarParametrosAnulacion(
            SqliteCommand comando,
            CertificacionAnulacion anulacion)
        {
            comando.Parameters.AddWithValue(
                "@TipoAnulacion",
                anulacion.TipoAnulacion!
                    .Trim());

            comando.Parameters.AddWithValue(
                "@FechaInicio",
                anulacion.FechaInicio
                    .Date
                    .ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture));

            comando.Parameters.AddWithValue(
                "@FechaFin",
                anulacion.EsPermanente ||
                !anulacion.FechaFin.HasValue
                    ? DBNull.Value
                    : anulacion.FechaFin.Value
                        .Date
                        .ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture));

            comando.Parameters.AddWithValue(
                "@EsPermanente",
                anulacion.EsPermanente
                    ? 1
                    : 0);

            comando.Parameters.AddWithValue(
                "@Comentario",
                anulacion.Comentario!
                    .Trim());
        }

        private static Mensaje? Validar(
            CertificacionAnulacion anulacion,
            bool requiereId)
        {
            if (requiereId &&
                anulacion.Id <= 0)
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

            if (anulacion.TipoAnulacion
                .Trim()
                .Length > 50)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El tipo de anulación no puede exceder 50 caracteres."
                };
            }

            if (anulacion.FechaInicio ==
                default)
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
                        "Debe seleccionar una fecha final para una anulación temporal."
                };
            }

            /*
             * La misma fecha sí es válida: representa
             * una anulación de un solo día.
             */
            if (!anulacion.EsPermanente &&
                anulacion.FechaFin.HasValue &&
                anulacion.FechaFin.Value.Date <
                anulacion.FechaInicio.Date)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La fecha final no puede ser menor que la fecha de inicio."
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

            if (anulacion.Comentario
                .Trim()
                .Length > 500)
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

        private static DateTime LeerFecha(
            SqliteDataReader lector,
            string columna)
        {
            DateTime? fecha =
                LeerFechaNullable(
                    lector,
                    columna);

            return fecha ?? DateTime.MinValue;
        }

        private static DateTime? LeerFechaNullable(
            SqliteDataReader lector,
            string columna)
        {
            int ordinal =
                lector.GetOrdinal(columna);

            if (lector.IsDBNull(ordinal))
            {
                return null;
            }

            string? texto =
                Convert.ToString(
                    lector.GetValue(ordinal));

            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            if (DateTime.TryParse(
                texto,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime fecha))
            {
                return fecha;
            }

            return null;
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