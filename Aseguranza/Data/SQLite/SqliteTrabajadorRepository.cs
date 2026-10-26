using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteTrabajadorRepository
        : ITrabajadorRepository
    {
        public Trabajador? ConsultarPorNumeroReloj(
            string noReloj)
        {
            if (string.IsNullOrWhiteSpace(noReloj))
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
                    T.Id,
                    T.NoReloj,
                    T.Nombre,
                    T.RutaFoto,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    T.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    COALESCE(
                        Li.Nombre,
                        'SIN ASIGNAR'
                    ) AS NombreLinea,
                    T.Activo
                FROM Trabajador AS T
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Planta AS P
                    ON P.Id = T.IdPlanta
                LEFT JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                WHERE T.NoReloj = @NoReloj
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                noReloj.Trim());

            using SqliteDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return null;
            }

            return MapearTrabajador(lector);
        }

        public DataTable Consultar(
            string textoBuscar,
            bool? activo)
        {
            DataTable tabla = new DataTable();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    T.Id,
                    T.NoReloj,
                    T.Nombre,
                    T.RutaFoto,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    T.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    COALESCE(
                        Li.Nombre,
                        'SIN ASIGNAR'
                    ) AS NombreLinea,
                    T.Activo
                FROM Trabajador AS T
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Planta AS P
                    ON P.Id = T.IdPlanta
                LEFT JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                WHERE (@Activo IS NULL OR T.Activo = @Activo)
                  AND (T.NoReloj LIKE @TextoBuscar COLLATE NOCASE
                   OR T.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR Tu.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR P.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
                          LIKE @TextoBuscar COLLATE NOCASE)
                ORDER BY T.Nombre
                LIMIT 100;
                """;

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");
            comando.Parameters.AddWithValue(
                "@Activo",
                activo.HasValue ? (object)(activo.Value ? 1 : 0) : DBNull.Value);

            using SqliteDataReader lector =
                comando.ExecuteReader();

            PrepararTablaTrabajadores(
                tabla);

            while (lector.Read())
            {
                AgregarFilaTrabajador(
                    tabla,
                    lector);
            }

            return tabla;
        }

        public PagedTableResult ConsultarPaginado(
            WorkerPageRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            const string filtros = """
                FROM Trabajador AS T
                LEFT JOIN Localidad AS Lo ON Lo.Id = T.IdLocalidad
                INNER JOIN Turno AS Tu ON Tu.Id = T.IdTurno
                INNER JOIN Planta AS P ON P.Id = T.IdPlanta
                LEFT JOIN Linea AS Li ON Li.Id = T.IdLinea
                WHERE (@Activo IS NULL OR T.Activo = @Activo)
                  AND (T.NoReloj LIKE @TextoBuscar COLLATE NOCASE
                   OR T.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR COALESCE(Lo.Nombre, '') LIKE @TextoBuscar COLLATE NOCASE
                   OR Tu.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR P.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
                          LIKE @TextoBuscar COLLATE NOCASE)
                """;

            int pageSize = request.ValidPageSize;
            string searchPattern = $"%{request.Search?.Trim() ?? string.Empty}%";
            object activeValue = request.Active.HasValue
                ? (request.Active.Value ? 1 : 0)
                : DBNull.Value;

            using SqliteConnection conexion = ConexionSqlite.Crear();
            conexion.Open();
            using SqliteTransaction transaccion = conexion.BeginTransaction();

            using SqliteCommand contar = conexion.CreateCommand();
            contar.Transaction = transaccion;
            contar.CommandText = "SELECT COUNT(*) " + filtros;
            contar.Parameters.AddWithValue("@TextoBuscar", searchPattern);
            contar.Parameters.AddWithValue("@Activo", activeValue);
            int totalCount = Convert.ToInt32(contar.ExecuteScalar());
            int totalPages = totalCount == 0
                ? 0
                : (totalCount - 1) / pageSize + 1;
            int pageNumber = Math.Min(
                request.ValidPageNumber,
                Math.Max(1, totalPages));
            long offset = ((long)pageNumber - 1) * pageSize;

            using SqliteCommand pagina = conexion.CreateCommand();
            pagina.Transaction = transaccion;
            pagina.CommandText = """
                SELECT
                    T.Id, T.NoReloj, T.Nombre, T.RutaFoto,
                    T.IdTurno, Tu.Nombre AS NombreTurno,
                    T.IdPlanta, P.Nombre AS NombrePlanta,
                    T.IdLinea, COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea,
                    T.Activo
                """ + "\n" + filtros + "\n" + """
                ORDER BY T.Nombre, T.Id
                LIMIT @PageSize OFFSET @Offset;
                """;
            pagina.Parameters.AddWithValue("@TextoBuscar", searchPattern);
            pagina.Parameters.AddWithValue("@Activo", activeValue);
            pagina.Parameters.AddWithValue("@PageSize", pageSize);
            pagina.Parameters.AddWithValue("@Offset", offset);

            DataTable tabla = new DataTable();
            PrepararTablaTrabajadores(tabla);
            using (SqliteDataReader lector = pagina.ExecuteReader())
            {
                while (lector.Read())
                    AgregarFilaTrabajador(tabla, lector);
            }

            transaccion.Commit();
            return new PagedTableResult(tabla, totalCount, pageNumber, pageSize);
        }

        public DataTable ConsultarParaImportacionHdc()
        {
            DataTable tabla =
                CrearTablaParaImportacionHdc();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    T.Id,
                    T.NoReloj,
                    T.Nombre,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    T.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea,
                    T.Activo
                FROM Trabajador AS T
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Planta AS P
                    ON P.Id = T.IdPlanta
                LEFT JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                ORDER BY T.NoReloj;
                """;

            using SqliteDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                DataRow fila = tabla.NewRow();

                fila["Id"] =
                    Convert.ToInt32(lector.GetInt64(0));

                fila["NoReloj"] =
                    lector.IsDBNull(1)
                        ? string.Empty
                        : lector.GetString(1);

                fila["Nombre"] =
                    lector.IsDBNull(2)
                        ? string.Empty
                        : lector.GetString(2);

                fila["IdTurno"] =
                    Convert.ToInt32(lector.GetInt64(3));

                fila["NombreTurno"] =
                    lector.IsDBNull(4)
                        ? string.Empty
                        : lector.GetString(4);

                fila["IdPlanta"] =
                    Convert.ToInt32(lector.GetInt64(5));

                fila["NombrePlanta"] =
                    lector.IsDBNull(6)
                        ? string.Empty
                        : lector.GetString(6);

                fila["IdLinea"] =
                    lector.IsDBNull(7)
                        ? DBNull.Value
                        : Convert.ToInt32(lector.GetInt64(7));

                fila["NombreLinea"] =
                    lector.IsDBNull(8)
                        ? "SIN ASIGNAR"
                        : lector.GetString(8);

                fila["Activo"] = lector.GetInt64(9) != 0;

                tabla.Rows.Add(fila);
            }

            return tabla;
        }

        public DataTable ConsultarEstadoCertificacion(
            string mostrarPor,
            string textoBuscar,
            bool? activo)
        {
            DataTable tabla = new DataTable();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                WITH TrabajadoresEstado AS
                (
                    SELECT
                        T.Id,
                        T.NoReloj,
                        T.Nombre,
                        T.RutaFoto,
                        T.IdTurno,
                        Tu.Nombre AS NombreTurno,
                        T.IdPlanta,
                        P.Nombre AS NombrePlanta,
                        T.IdLinea,
                        COALESCE(
                            Li.Nombre,
                            'SIN ASIGNAR'
                        ) AS NombreLinea,

                        T.Activo,

                        CASE
                            WHEN COUNT(C.Id) = 0
                                THEN 'Sin certificar'

                            WHEN SUM(
                                CASE
                                    WHEN date(C.FechaVencimiento)
                                         < date('now', 'localtime')
                                    THEN 1
                                    ELSE 0
                                END
                            ) > 0
                                THEN 'Vencida'

                            WHEN SUM(
                                CASE
                                    WHEN date(C.FechaVencimiento)
                                         BETWEEN date('now', 'localtime')
                                         AND date(
                                             'now',
                                             'localtime',
                                             '+30 days')
                                    THEN 1
                                    ELSE 0
                                END
                            ) > 0
                                THEN 'Por vencer'

                            ELSE 'Vigente'
                        END AS EstadoCertificacion

                    FROM Trabajador AS T
                    INNER JOIN Turno AS Tu
                        ON Tu.Id = T.IdTurno
                    INNER JOIN Planta AS P
                        ON P.Id = T.IdPlanta
                    LEFT JOIN Linea AS Li
                        ON Li.Id = T.IdLinea

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

                    WHERE (@Activo IS NULL OR T.Activo = @Activo)
                      AND (T.NoReloj
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR T.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR Tu.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR P.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
                              LIKE @TextoBuscar COLLATE NOCASE)

                    GROUP BY
                        T.Id,
                        T.NoReloj,
                        T.Nombre,
                        T.RutaFoto,
                        T.IdTurno,
                        Tu.Nombre,
                        T.IdPlanta,
                        P.Nombre,
                        T.IdLinea,
                        Li.Nombre,
                        T.Activo
                )

                SELECT *
                FROM TrabajadoresEstado
                WHERE @MostrarPor = 'Todas'
                   OR EstadoCertificacion = @MostrarPor
                ORDER BY Nombre
                LIMIT 300;
                """;

            comando.Parameters.AddWithValue(
                "@MostrarPor",
                mostrarPor ?? "Todas");

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");
            comando.Parameters.AddWithValue(
                "@Activo",
                activo.HasValue ? (object)(activo.Value ? 1 : 0) : DBNull.Value);

            using SqliteDataReader lector =
                comando.ExecuteReader();

            PrepararTablaEstadoCertificacion(
                tabla);

            while (lector.Read())
            {
                DataRow fila =
                    tabla.NewRow();

                fila["Id"] =
                    Convert.ToInt32(
                        lector.GetInt64(0));

                fila["NoReloj"] =
                    lector.IsDBNull(1)
                        ? string.Empty
                        : lector.GetString(1);

                fila["Nombre"] =
                    lector.IsDBNull(2)
                        ? string.Empty
                        : lector.GetString(2);

                fila["RutaFoto"] =
                    lector.IsDBNull(3)
                        ? DBNull.Value
                        : lector.GetString(3);

                fila["IdTurno"] =
                    Convert.ToInt32(
                        lector.GetInt64(4));

                fila["NombreTurno"] =
                    lector.IsDBNull(5)
                        ? string.Empty
                        : lector.GetString(5);

                fila["IdPlanta"] =
                    Convert.ToInt32(
                        lector.GetInt64(6));

                fila["NombrePlanta"] =
                    lector.IsDBNull(7)
                        ? string.Empty
                        : lector.GetString(7);

                fila["IdLinea"] =
                    lector.IsDBNull(8)
                        ? DBNull.Value
                        : Convert.ToInt32(
                            lector.GetInt64(8));

                fila["NombreLinea"] =
                    lector.IsDBNull(9)
                        ? "SIN ASIGNAR"
                        : lector.GetString(9);

                fila["EstadoCertificacion"] =
                    lector.IsDBNull(11)
                        ? "Sin certificar"
                        : lector.GetString(11);

                fila["Activo"] = lector.GetInt64(10) != 0;

                tabla.Rows.Add(
                    fila);
            }

            return tabla;
        }

        public Mensaje Guardar(
            Trabajador trabajador)
        {
            Mensaje? validacion =
                Validar(trabajador);

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

                Mensaje? validacionCatalogos =
                    ValidarCatalogosRelacionados(
                        conexion,
                        transaccion,
                        trabajador);

                if (validacionCatalogos is not null)
                {
                    transaccion.Rollback();

                    return validacionCatalogos;
                }

                if (ExisteNumeroRelojDuplicado(
                    conexion,
                    transaccion,
                    trabajador))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "Ya existe un trabajador con ese número de reloj."
                    };
                }

                if (ExisteTrabajador(
                    conexion,
                    transaccion,
                    trabajador.Id))
                {
                    Actualizar(
                        conexion,
                        transaccion,
                        trabajador);

                    transaccion.Commit();

                    return new Mensaje
                    {
                        Id = 3,
                        Nombre =
                            "Se ha modificado correctamente"
                    };
                }

                Insertar(
                    conexion,
                    transaccion,
                    trabajador);

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha guardado correctamente"
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        public bool CambiarEstado(
            int idTrabajador,
            bool activo,
            string origen,
            int? idImportacionHdc = null,
            string? motivo = null)
        {
            using SqliteConnection conexion = ConexionSqlite.Crear();
            conexion.Open();
            using SqliteTransaction transaccion = conexion.BeginTransaction();
            bool cambiado = CambiarEstadoEnTransaccion(
                conexion, transaccion, idTrabajador, activo,
                origen, idImportacionHdc, motivo);
            transaccion.Commit();
            return cambiado;
        }

        internal static bool CambiarEstadoEnTransaccion(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador,
            bool activo,
            string origen,
            int? idImportacionHdc = null,
            string? motivo = null)
        {
            TrabajadorEstadoHistorial.ValidarCambio(
                idTrabajador, origen, idImportacionHdc, motivo);
            using SqliteCommand comando = conexion.CreateCommand();
            comando.Transaction = transaccion;
            comando.CommandText =
                "SELECT Activo FROM Trabajador WHERE Id = @Id;";
            comando.Parameters.AddWithValue("@Id", idTrabajador);

            object? valor = comando.ExecuteScalar();
            if (valor is null || valor == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "No se encontró el trabajador cuyo estado se intenta cambiar.");
            }

            bool anterior = Convert.ToInt64(valor) != 0;
            if (anterior == activo)
            {
                return false;
            }

            comando.CommandText =
                "UPDATE Trabajador SET Activo = @Activo " +
                "WHERE Id = @Id AND Activo = @Anterior;";
            comando.Parameters.AddWithValue("@Activo", activo ? 1 : 0);
            comando.Parameters.AddWithValue("@Anterior", anterior ? 1 : 0);
            if (comando.ExecuteNonQuery() != 1)
            {
                throw new InvalidOperationException(
                    "El estado del trabajador cambió durante la operación.");
            }

            comando.CommandText = """
                INSERT INTO TrabajadorEstadoHistorial
                    (IdTrabajador, ActivoAnterior, ActivoNuevo,
                     FechaCambioUtc, Origen, IdImportacionHdc, Motivo)
                VALUES
                    (@Id, @Anterior, @Activo,
                     @FechaCambioUtc, @Origen, @IdImportacionHdc, @Motivo);
                """;
            comando.Parameters.AddWithValue(
                "@FechaCambioUtc",
                DateTime.UtcNow.ToString("O"));
            comando.Parameters.AddWithValue("@Origen", origen);
            comando.Parameters.AddWithValue(
                "@IdImportacionHdc",
                idImportacionHdc.HasValue
                    ? idImportacionHdc.Value
                    : DBNull.Value);
            comando.Parameters.AddWithValue(
                "@Motivo",
                string.IsNullOrWhiteSpace(motivo)
                    ? DBNull.Value
                    : motivo.Trim());
            comando.ExecuteNonQuery();
            return true;
        }

        public Mensaje Borrar(int id)
        {
            if (id <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El trabajador seleccionado no es válido."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                if (TieneRegistrosRelacionadosActivos(
                    conexion,
                    transaccion,
                    id))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay registros " +
                            "asociados a este trabajador."
                    };
                }

                EliminarExpedientesInactivos(
                    conexion,
                    transaccion,
                    id);

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction =
                    transaccion;

                comando.CommandText = """
                    DELETE FROM Trabajador
                    WHERE Id = @Id;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    id);

                int filasAfectadas =
                    comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró el trabajador."
                    };
                }

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha borrado correctamente"
                };
            }
            catch (SqliteException ex)
                when (ex.SqliteErrorCode == 19)
            {
                return new Mensaje
                {
                    Id = 2,
                    Nombre =
                        "No se puede borrar porque todavía existen " +
                        "registros relacionados con este trabajador."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static DataTable CrearTablaParaImportacionHdc()
        {
            DataTable tabla = new DataTable();

            tabla.Columns.Add("Id", typeof(int));
            tabla.Columns.Add("NoReloj", typeof(string));
            tabla.Columns.Add("Nombre", typeof(string));
            tabla.Columns.Add("IdTurno", typeof(int));
            tabla.Columns.Add("NombreTurno", typeof(string));
            tabla.Columns.Add("IdPlanta", typeof(int));
            tabla.Columns.Add("NombrePlanta", typeof(string));

            DataColumn columnaIdLinea =
                tabla.Columns.Add("IdLinea", typeof(int));

            columnaIdLinea.AllowDBNull = true;

            tabla.Columns.Add("NombreLinea", typeof(string));

            tabla.Columns.Add("Activo", typeof(bool));

            return tabla;
        }

        private static void PrepararTablaTrabajadores(
            DataTable tabla)
        {
            tabla.Columns.Add(
                "Id",
                typeof(int));

            tabla.Columns.Add(
                "NoReloj",
                typeof(string));

            tabla.Columns.Add(
                "Nombre",
                typeof(string));

            DataColumn columnaRutaFoto =
                tabla.Columns.Add(
                    "RutaFoto",
                    typeof(string));

            columnaRutaFoto.AllowDBNull =
                true;

            tabla.Columns.Add(
                "IdTurno",
                typeof(int));

            tabla.Columns.Add(
                "NombreTurno",
                typeof(string));

            tabla.Columns.Add(
                "IdPlanta",
                typeof(int));

            tabla.Columns.Add(
                "NombrePlanta",
                typeof(string));

            DataColumn columnaIdLinea =
                tabla.Columns.Add(
                    "IdLinea",
                    typeof(int));

            columnaIdLinea.AllowDBNull =
                true;

            tabla.Columns.Add(
                "NombreLinea",
                typeof(string));

            tabla.Columns.Add("Activo", typeof(bool));
        }

        private static void PrepararTablaEstadoCertificacion(
            DataTable tabla)
        {
            PrepararTablaTrabajadores(
                tabla);

            tabla.Columns.Add(
                "EstadoCertificacion",
                typeof(string));
        }

        private static void AgregarFilaTrabajador(
            DataTable tabla,
            SqliteDataReader lector)
        {
            DataRow fila =
                tabla.NewRow();

            fila["Id"] =
                Convert.ToInt32(
                    lector.GetInt64(0));

            fila["NoReloj"] =
                lector.IsDBNull(1)
                    ? string.Empty
                    : lector.GetString(1);

            fila["Nombre"] =
                lector.IsDBNull(2)
                    ? string.Empty
                    : lector.GetString(2);

            fila["RutaFoto"] =
                lector.IsDBNull(3)
                    ? DBNull.Value
                    : lector.GetString(3);

            fila["IdTurno"] =
                Convert.ToInt32(
                    lector.GetInt64(4));

            fila["NombreTurno"] =
                lector.IsDBNull(5)
                    ? string.Empty
                    : lector.GetString(5);

            fila["IdPlanta"] =
                Convert.ToInt32(
                    lector.GetInt64(6));

            fila["NombrePlanta"] =
                lector.IsDBNull(7)
                    ? string.Empty
                    : lector.GetString(7);

            fila["IdLinea"] =
                lector.IsDBNull(8)
                    ? DBNull.Value
                    : Convert.ToInt32(
                        lector.GetInt64(8));

            fila["NombreLinea"] =
                lector.IsDBNull(9)
                    ? "SIN ASIGNAR"
                    : lector.GetString(9);

            fila["Activo"] = lector.GetInt64(10) != 0;

            tabla.Rows.Add(
                fila);
        }

        private static Trabajador MapearTrabajador(
            SqliteDataReader lector)
        {
            return new Trabajador
            {
                Id =
                    Convert.ToInt32(
                        lector["Id"]),

                NoReloj =
                    Convert.ToString(
                        lector["NoReloj"]),

                Nombre =
                    Convert.ToString(
                        lector["Nombre"]),

                RutaFoto =
                    lector["RutaFoto"] is DBNull
                        ? null
                        : Convert.ToString(
                            lector["RutaFoto"]),

                IdLocalidad =
                    0,

                NombreLocalidad =
                    null,

                IdTurno =
                    Convert.ToInt32(
                        lector["IdTurno"]),

                NombreTurno =
                    Convert.ToString(
                        lector["NombreTurno"]),

                IdPlanta =
                    Convert.ToInt32(
                        lector["IdPlanta"]),

                NombrePlanta =
                    Convert.ToString(
                        lector["NombrePlanta"]),

                IdLinea =
                    lector["IdLinea"] is DBNull
                        ? 0
                        : Convert.ToInt32(
                            lector["IdLinea"]),

                NombreLinea =
                    Convert.ToString(
                        lector["NombreLinea"]),

                Activo =
                    Convert.ToInt64(lector["Activo"]) != 0
            };
        }

        private static bool ExisteTrabajador(
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

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Trabajador
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteNumeroRelojDuplicado(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            Trabajador trabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Trabajador
                WHERE NoReloj = @NoReloj COLLATE NOCASE
                  AND Id <> @Id;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                trabajador.NoReloj!.Trim());

            comando.Parameters.AddWithValue(
                "@Id",
                trabajador.Id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static Mensaje? ValidarCatalogosRelacionados(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            Trabajador trabajador)
        {
            if (!ExisteId(
                conexion,
                transaccion,
                "Turno",
                trabajador.IdTurno))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El turno seleccionado no existe."
                };
            }

            if (!ExisteId(
                conexion,
                transaccion,
                "Planta",
                trabajador.IdPlanta))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La planta seleccionada no existe."
                };
            }

            if (trabajador.IdLinea <= 0)
            {
                return null;
            }

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Linea
                WHERE Id = @IdLinea
                  AND IdPlanta = @IdPlanta;
                """;

            comando.Parameters.AddWithValue(
                "@IdLinea",
                trabajador.IdLinea);

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                trabajador.IdPlanta);

            if (Convert.ToInt64(
                comando.ExecuteScalar()) == 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La línea seleccionada no pertenece a la planta."
                };
            }

            return null;
        }

        private static bool ExisteId(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string tabla,
            int id)
        {
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

        private static void Insertar(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            Trabajador trabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO Trabajador
                (
                    NoReloj,
                    Nombre,
                    RutaFoto,
                    IdTurno,
                    IdPlanta,
                    IdLinea
                )
                VALUES
                (
                    @NoReloj,
                    @Nombre,
                    @RutaFoto,
                    @IdTurno,
                    @IdPlanta,
                    @IdLinea
                );
                """;

            AgregarParametros(
                comando,
                trabajador);

            comando.ExecuteNonQuery();
        }

        private static void Actualizar(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            Trabajador trabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                UPDATE Trabajador
                SET
                    NoReloj = @NoReloj,
                    Nombre = @Nombre,
                    RutaFoto = @RutaFoto,
                    IdTurno = @IdTurno,
                    IdPlanta = @IdPlanta,
                    IdLinea = @IdLinea
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                trabajador.Id);

            AgregarParametros(
                comando,
                trabajador);

            comando.ExecuteNonQuery();
        }

        private static void AgregarParametros(
            SqliteCommand comando,
            Trabajador trabajador)
        {
            comando.Parameters.AddWithValue(
                "@NoReloj",
                trabajador.NoReloj!.Trim());

            comando.Parameters.AddWithValue(
                "@Nombre",
                trabajador.Nombre!
                    .Trim()
                    .ToUpperInvariant());

            comando.Parameters.AddWithValue(
                "@RutaFoto",
                string.IsNullOrWhiteSpace(
                    trabajador.RutaFoto)
                    ? DBNull.Value
                    : trabajador.RutaFoto.Trim());

            comando.Parameters.AddWithValue(
                "@IdTurno",
                trabajador.IdTurno);

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                trabajador.IdPlanta);

            comando.Parameters.AddWithValue(
                "@IdLinea",
                trabajador.IdLinea > 0
                    ? trabajador.IdLinea
                    : DBNull.Value);
        }

        private static bool TieneRegistrosRelacionadosActivos(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT
                    (
                        SELECT COUNT(*)
                        FROM Certificador
                        WHERE IdTrabajador = @IdTrabajador
                    )
                    +
                    (
                        SELECT COUNT(*)
                        FROM Certificacion
                        WHERE IdTrabajador = @IdTrabajador
                    )
                    +
                    (
                        SELECT COUNT(*)
                        FROM ExpedienteTrabajador
                        WHERE IdTrabajador = @IdTrabajador
                          AND Activo = 1
                    )
                    +
                    (
                        SELECT COUNT(*)
                        FROM TrabajadorEstadoHistorial
                        WHERE IdTrabajador = @IdTrabajador
                    );
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static void EliminarExpedientesInactivos(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                DELETE FROM ExpedienteTrabajador
                WHERE IdTrabajador = @IdTrabajador
                  AND Activo = 0;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            comando.ExecuteNonQuery();
        }

        private static Mensaje? Validar(
            Trabajador trabajador)
        {
            if (string.IsNullOrWhiteSpace(
                trabajador.NoReloj))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El número de reloj es obligatorio."
                };
            }

            if (trabajador.NoReloj.Trim().Length > 10)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El número de reloj no puede exceder 10 caracteres."
                };
            }

            if (string.IsNullOrWhiteSpace(
                trabajador.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre del trabajador es obligatorio."
                };
            }

            if (trabajador.IdTurno <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar un turno válido."
                };
            }

            if (trabajador.IdPlanta <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar una planta válida."
                };
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
