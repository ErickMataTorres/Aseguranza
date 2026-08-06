using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Globalization;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteCertificacionRepository
        : ICertificacionRepository
    {
        public DataTable ConsultarVerificacionNoReloj(
            string noReloj)
        {
            DataTable tabla =
                CrearTablaVerificacion();

            if (string.IsNullOrWhiteSpace(noReloj))
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
                    T.Id AS IdTrabajador,
                    T.NoReloj,
                    T.Nombre,
                    T.RutaFoto,

                    L.Nombre AS NombreLocalidad,
                    Tu.Nombre AS NombreTurno,
                    Pta.Nombre AS NombrePlanta,
                    Li.Nombre AS NombreLinea,

                    C.Id AS IdCertificacion,
                    Pr.Id AS IdProceso,
                    Pr.Nombre AS Proceso,
                    C.FechaCertificacion,
                    C.FechaVencimiento,

                    CASE
                        WHEN C.FechaVencimiento IS NULL
                            THEN NULL
                        ELSE CAST(
                            julianday(date(C.FechaVencimiento))
                            -
                            julianday(date('now', 'localtime'))
                            AS INTEGER
                        )
                    END AS DiasRestantes,

                    C.Comentario,
                    C.IdCertificador,
                    TCert.Nombre AS NombreCertificador

                FROM Trabajador AS T

                INNER JOIN Localidad AS L
                    ON L.Id = T.IdLocalidad

                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno

                INNER JOIN Linea AS Li
                    ON Li.Id = T.IdLinea

                INNER JOIN Planta AS Pta
                    ON Pta.Id = Li.IdPlanta

                LEFT JOIN Certificacion AS C
                    ON C.IdTrabajador = T.Id
                   AND NOT EXISTS
                   (
                        SELECT 1
                        FROM CertificacionAnulacion AS CA
                        WHERE CA.IdCertificacion = C.Id
                          AND CA.Activa = 1
                          AND
                          (
                              CA.EsPermanente = 1
                              OR CA.FechaFin IS NULL
                              OR date(CA.FechaFin)
                                 >= date('now', 'localtime')
                          )
                   )

                LEFT JOIN Proceso AS Pr
                    ON Pr.Id = C.IdProceso

                LEFT JOIN Certificador AS Cert
                    ON Cert.Id = C.IdCertificador

                LEFT JOIN Trabajador AS TCert
                    ON TCert.Id = Cert.IdTrabajador

                WHERE T.NoReloj =
                    @NoReloj COLLATE NOCASE

                ORDER BY Pr.Nombre;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                noReloj.Trim());

            using SqliteDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                DataRow fila =
                    tabla.NewRow();

                fila["IdTrabajador"] =
                    Convert.ToInt32(
                        lector["IdTrabajador"]);

                fila["NoReloj"] =
                    LeerTextoODbNull(
                        lector,
                        "NoReloj");

                fila["Nombre"] =
                    LeerTextoODbNull(
                        lector,
                        "Nombre");

                fila["RutaFoto"] =
                    LeerTextoODbNull(
                        lector,
                        "RutaFoto");

                fila["NombreLocalidad"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreLocalidad");

                fila["NombreTurno"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreTurno");

                fila["NombrePlanta"] =
                    LeerTextoODbNull(
                        lector,
                        "NombrePlanta");

                fila["NombreLinea"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreLinea");

                fila["IdCertificacion"] =
                    LeerEnteroODbNull(
                        lector,
                        "IdCertificacion");

                fila["IdProceso"] =
                    LeerEnteroODbNull(
                        lector,
                        "IdProceso");

                fila["Proceso"] =
                    LeerTextoODbNull(
                        lector,
                        "Proceso");

                fila["FechaCertificacion"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaCertificacion");

                fila["FechaVencimiento"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaVencimiento");

                fila["DiasRestantes"] =
                    LeerEnteroODbNull(
                        lector,
                        "DiasRestantes");

                fila["Comentario"] =
                    LeerTextoODbNull(
                        lector,
                        "Comentario");

                fila["IdCertificador"] =
                    LeerEnteroODbNull(
                        lector,
                        "IdCertificador");

                fila["NombreCertificador"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreCertificador");

                tabla.Rows.Add(fila);
            }

            return tabla;
        }

        public DataTable ConsultarPorTrabajador(
            int idTrabajador,
            string textoBuscar)
        {
            DataTable tabla =
                CrearTablaCertificaciones();

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
                    C.Id,
                    C.IdTrabajador,
                    P.Id AS IdProceso,
                    P.Nombre AS Proceso,
                    C.FechaCertificacion,
                    C.FechaVencimiento,

                    CAST(
                        julianday(date(C.FechaVencimiento))
                        -
                        julianday(date('now', 'localtime'))
                        AS INTEGER
                    ) AS DiasRestantes,

                    C.Comentario,
                    C.IdCertificador,
                    TCert.Nombre AS NombreCertificador,

                    CASE
                        WHEN CA.Id IS NOT NULL
                            THEN 1
                        ELSE 0
                    END AS EstaAnulada,

                    CA.Id AS IdAnulacion,
                    CA.TipoAnulacion,
                    CA.FechaInicio
                        AS FechaInicioAnulacion,
                    CA.FechaFin
                        AS FechaFinAnulacion,
                    CA.EsPermanente,
                    CA.Comentario
                        AS ComentarioAnulacion

                FROM Certificacion AS C

                INNER JOIN Proceso AS P
                    ON P.Id = C.IdProceso

                INNER JOIN Certificador AS Cert
                    ON Cert.Id = C.IdCertificador

                INNER JOIN Trabajador AS TCert
                    ON TCert.Id = Cert.IdTrabajador

                LEFT JOIN CertificacionAnulacion AS CA
                    ON CA.Id =
                    (
                        SELECT A.Id
                        FROM CertificacionAnulacion AS A
                        WHERE A.IdCertificacion = C.Id
                          AND A.Activa = 1
                          AND
                          (
                              A.EsPermanente = 1
                              OR A.FechaFin IS NULL
                              OR date(A.FechaFin)
                                 >= date('now', 'localtime')
                          )
                        ORDER BY
                            datetime(A.FechaRegistro) DESC,
                            A.Id DESC
                        LIMIT 1
                    )

                WHERE C.IdTrabajador = @IdTrabajador
                  AND
                  (
                      P.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE

                      OR COALESCE(C.Comentario, '')
                          LIKE @TextoBuscar COLLATE NOCASE

                      OR TCert.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE

                      OR COALESCE(CA.Comentario, '')
                          LIKE @TextoBuscar COLLATE NOCASE
                  )

                ORDER BY date(C.FechaVencimiento);
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");

            using SqliteDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                DataRow fila =
                    tabla.NewRow();

                fila["Id"] =
                    Convert.ToInt32(
                        lector["Id"]);

                fila["IdTrabajador"] =
                    Convert.ToInt32(
                        lector["IdTrabajador"]);

                fila["IdProceso"] =
                    Convert.ToInt32(
                        lector["IdProceso"]);

                fila["Proceso"] =
                    LeerTextoODbNull(
                        lector,
                        "Proceso");

                fila["FechaCertificacion"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaCertificacion");

                fila["FechaVencimiento"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaVencimiento");

                fila["DiasRestantes"] =
                    Convert.ToInt32(
                        lector["DiasRestantes"]);

                fila["Comentario"] =
                    LeerTextoODbNull(
                        lector,
                        "Comentario");

                fila["IdCertificador"] =
                    Convert.ToInt32(
                        lector["IdCertificador"]);

                fila["NombreCertificador"] =
                    LeerTextoODbNull(
                        lector,
                        "NombreCertificador");

                fila["EstaAnulada"] =
                    Convert.ToInt32(
                        lector["EstaAnulada"]);

                fila["IdAnulacion"] =
                    LeerEnteroODbNull(
                        lector,
                        "IdAnulacion");

                fila["TipoAnulacion"] =
                    LeerTextoODbNull(
                        lector,
                        "TipoAnulacion");

                fila["FechaInicioAnulacion"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaInicioAnulacion");

                fila["FechaFinAnulacion"] =
                    LeerFechaODbNull(
                        lector,
                        "FechaFinAnulacion");

                fila["EsPermanente"] =
                    LeerEnteroODbNull(
                        lector,
                        "EsPermanente");

                fila["ComentarioAnulacion"] =
                    LeerTextoODbNull(
                        lector,
                        "ComentarioAnulacion");

                tabla.Rows.Add(fila);
            }

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
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                if (!ExisteRegistro(
                    conexion,
                    transaccion,
                    "Trabajador",
                    certificacion.IdTrabajador))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El trabajador seleccionado no existe."
                    };
                }

                int vigenciaMeses =
                    ObtenerVigenciaProceso(
                        conexion,
                        transaccion,
                        certificacion.IdProceso);

                if (vigenciaMeses <= 0)
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El proceso seleccionado no existe " +
                            "o su vigencia no es válida."
                    };
                }

                if (!ExisteRegistro(
                    conexion,
                    transaccion,
                    "Certificador",
                    certificacion.IdCertificador))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El certificador seleccionado no existe."
                    };
                }

                int idCertificacionExistente =
                    ObtenerIdCertificacionExistente(
                        conexion,
                        transaccion,
                        certificacion.IdTrabajador,
                        certificacion.IdProceso);

                if (idCertificacionExistente > 0 &&
                    TieneAnulacionActiva(
                        conexion,
                        transaccion,
                        idCertificacionExistente))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "Esta certificación se encuentra anulada. " +
                            "No se puede modificar ni renovar mientras " +
                            "tenga una anulación activa."
                    };
                }

                DateTime fechaCertificacion =
                    certificacion
                        .FechaCertificacion
                        .Date;

                DateTime fechaVencimiento =
                    fechaCertificacion
                        .AddMonths(vigenciaMeses);

                if (idCertificacionExistente > 0)
                {
                    ActualizarCertificacion(
                        conexion,
                        transaccion,
                        idCertificacionExistente,
                        certificacion,
                        fechaCertificacion,
                        fechaVencimiento);

                    transaccion.Commit();

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "Certificación renovada correctamente."
                    };
                }

                InsertarCertificacion(
                    conexion,
                    transaccion,
                    certificacion,
                    fechaCertificacion,
                    fechaVencimiento);

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Certificación registrada correctamente."
                };
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
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                if (!ExisteRegistro(
                    conexion,
                    transaccion,
                    "Certificacion",
                    id))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No existe la certificación que intenta borrar."
                    };
                }

                using SqliteCommand borrarAnulaciones =
                    conexion.CreateCommand();

                borrarAnulaciones.Transaction =
                    transaccion;

                borrarAnulaciones.CommandText = """
                    DELETE FROM CertificacionAnulacion
                    WHERE IdCertificacion = @IdCertificacion;
                    """;

                borrarAnulaciones.Parameters.AddWithValue(
                    "@IdCertificacion",
                    id);

                borrarAnulaciones.ExecuteNonQuery();

                using SqliteCommand borrarCertificacion =
                    conexion.CreateCommand();

                borrarCertificacion.Transaction =
                    transaccion;

                borrarCertificacion.CommandText = """
                    DELETE FROM Certificacion
                    WHERE Id = @Id;
                    """;

                borrarCertificacion.Parameters.AddWithValue(
                    "@Id",
                    id);

                int filasAfectadas =
                    borrarCertificacion.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No existe la certificación que intenta borrar."
                    };
                }

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Certificación eliminada correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static int ObtenerVigenciaProceso(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idProceso)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT VigenciaMeses
                FROM Proceso
                WHERE Id = @IdProceso
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@IdProceso",
                idProceso);

            object? resultado =
                comando.ExecuteScalar();

            if (resultado is null ||
                resultado is DBNull)
            {
                return 0;
            }

            return Convert.ToInt32(resultado);
        }

        private static int
            ObtenerIdCertificacionExistente(
                SqliteConnection conexion,
                SqliteTransaction transaccion,
                int idTrabajador,
                int idProceso)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT Id
                FROM Certificacion
                WHERE IdTrabajador = @IdTrabajador
                  AND IdProceso = @IdProceso
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            comando.Parameters.AddWithValue(
                "@IdProceso",
                idProceso);

            object? resultado =
                comando.ExecuteScalar();

            if (resultado is null ||
                resultado is DBNull)
            {
                return 0;
            }

            return Convert.ToInt32(resultado);
        }

        private static bool TieneAnulacionActiva(
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
                FROM CertificacionAnulacion
                WHERE IdCertificacion = @IdCertificacion
                  AND Activa = 1
                  AND
                  (
                      EsPermanente = 1
                      OR FechaFin IS NULL
                      OR date(FechaFin)
                         >= date('now', 'localtime')
                  );
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificacion",
                idCertificacion);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteRegistro(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string tabla,
            int id)
        {
            if (id <= 0)
            {
                return false;
            }

            string[] tablasPermitidas =
            {
                "Trabajador",
                "Certificador",
                "Certificacion"
            };

            if (Array.IndexOf(
                tablasPermitidas,
                tabla) < 0)
            {
                throw new InvalidOperationException(
                    "La tabla solicitada no está permitida.");
            }

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText =
                $"SELECT COUNT(*) FROM {tabla} WHERE Id = @Id;";

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static void InsertarCertificacion(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            Certificacion certificacion,
            DateTime fechaCertificacion,
            DateTime fechaVencimiento)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO Certificacion
                (
                    IdTrabajador,
                    IdProceso,
                    FechaCertificacion,
                    FechaVencimiento,
                    IdCertificador,
                    Comentario
                )
                VALUES
                (
                    @IdTrabajador,
                    @IdProceso,
                    @FechaCertificacion,
                    @FechaVencimiento,
                    @IdCertificador,
                    @Comentario
                );
                """;

            AgregarParametrosCertificacion(
                comando,
                certificacion,
                fechaCertificacion,
                fechaVencimiento);

            comando.ExecuteNonQuery();
        }

        private static void ActualizarCertificacion(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idCertificacion,
            Certificacion certificacion,
            DateTime fechaCertificacion,
            DateTime fechaVencimiento)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                UPDATE Certificacion
                SET
                    FechaCertificacion =
                        @FechaCertificacion,

                    FechaVencimiento =
                        @FechaVencimiento,

                    IdCertificador =
                        @IdCertificador,

                    Comentario =
                        @Comentario

                WHERE Id = @IdCertificacion;
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificacion",
                idCertificacion);

            AgregarParametrosCertificacion(
                comando,
                certificacion,
                fechaCertificacion,
                fechaVencimiento);

            comando.ExecuteNonQuery();
        }

        private static void
            AgregarParametrosCertificacion(
                SqliteCommand comando,
                Certificacion certificacion,
                DateTime fechaCertificacion,
                DateTime fechaVencimiento)
        {
            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                certificacion.IdTrabajador);

            comando.Parameters.AddWithValue(
                "@IdProceso",
                certificacion.IdProceso);

            comando.Parameters.AddWithValue(
                "@FechaCertificacion",
                fechaCertificacion.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));

            comando.Parameters.AddWithValue(
                "@FechaVencimiento",
                fechaVencimiento.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));

            comando.Parameters.AddWithValue(
                "@IdCertificador",
                certificacion.IdCertificador);

            comando.Parameters.AddWithValue(
                "@Comentario",
                string.IsNullOrWhiteSpace(
                    certificacion.Comentario)
                    ? DBNull.Value
                    : certificacion.Comentario.Trim());
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

        private static DataTable
            CrearTablaCertificaciones()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "Id",
                typeof(int));

            tabla.Columns.Add(
                "IdTrabajador",
                typeof(int));

            tabla.Columns.Add(
                "IdProceso",
                typeof(int));

            tabla.Columns.Add(
                "Proceso",
                typeof(string));

            tabla.Columns.Add(
                "FechaCertificacion",
                typeof(DateTime));

            tabla.Columns.Add(
                "FechaVencimiento",
                typeof(DateTime));

            tabla.Columns.Add(
                "DiasRestantes",
                typeof(int));

            tabla.Columns.Add(
                "Comentario",
                typeof(string));

            tabla.Columns.Add(
                "IdCertificador",
                typeof(int));

            tabla.Columns.Add(
                "NombreCertificador",
                typeof(string));

            tabla.Columns.Add(
                "EstaAnulada",
                typeof(int));

            tabla.Columns.Add(
                "IdAnulacion",
                typeof(int));

            tabla.Columns.Add(
                "TipoAnulacion",
                typeof(string));

            tabla.Columns.Add(
                "FechaInicioAnulacion",
                typeof(DateTime));

            tabla.Columns.Add(
                "FechaFinAnulacion",
                typeof(DateTime));

            tabla.Columns.Add(
                "EsPermanente",
                typeof(int));

            tabla.Columns.Add(
                "ComentarioAnulacion",
                typeof(string));

            return tabla;
        }

        private static DataTable
            CrearTablaVerificacion()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "IdTrabajador",
                typeof(int));

            tabla.Columns.Add(
                "NoReloj",
                typeof(string));

            tabla.Columns.Add(
                "Nombre",
                typeof(string));

            tabla.Columns.Add(
                "RutaFoto",
                typeof(string));

            tabla.Columns.Add(
                "NombreLocalidad",
                typeof(string));

            tabla.Columns.Add(
                "NombreTurno",
                typeof(string));

            tabla.Columns.Add(
                "NombrePlanta",
                typeof(string));

            tabla.Columns.Add(
                "NombreLinea",
                typeof(string));

            tabla.Columns.Add(
                "IdCertificacion",
                typeof(int));

            tabla.Columns.Add(
                "IdProceso",
                typeof(int));

            tabla.Columns.Add(
                "Proceso",
                typeof(string));

            tabla.Columns.Add(
                "FechaCertificacion",
                typeof(DateTime));

            tabla.Columns.Add(
                "FechaVencimiento",
                typeof(DateTime));

            tabla.Columns.Add(
                "DiasRestantes",
                typeof(int));

            tabla.Columns.Add(
                "Comentario",
                typeof(string));

            tabla.Columns.Add(
                "IdCertificador",
                typeof(int));

            tabla.Columns.Add(
                "NombreCertificador",
                typeof(string));

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

        private static object LeerEnteroODbNull(
            SqliteDataReader lector,
            string columna)
        {
            int ordinal =
                lector.GetOrdinal(columna);

            if (lector.IsDBNull(ordinal))
            {
                return DBNull.Value;
            }

            return Convert.ToInt32(
                lector.GetValue(ordinal));
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