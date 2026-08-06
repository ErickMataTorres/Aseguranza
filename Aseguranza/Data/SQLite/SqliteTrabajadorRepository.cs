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
                    T.IdLocalidad,
                    L.Nombre AS NombreLocalidad,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    Li.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    Li.Nombre AS NombreLinea
                FROM Trabajador AS T
                INNER JOIN Localidad AS L
                    ON L.Id = T.IdLocalidad
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                INNER JOIN Planta AS P
                    ON P.Id = Li.IdPlanta
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
            string textoBuscar)
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
                    T.IdLocalidad,
                    L.Nombre AS NombreLocalidad,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    Li.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    Li.Nombre AS NombreLinea
                FROM Trabajador AS T
                INNER JOIN Localidad AS L
                    ON L.Id = T.IdLocalidad
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                INNER JOIN Planta AS P
                    ON P.Id = Li.IdPlanta
                WHERE T.NoReloj LIKE @TextoBuscar COLLATE NOCASE
                   OR T.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR L.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR Tu.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR P.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR Li.Nombre LIKE @TextoBuscar COLLATE NOCASE
                ORDER BY T.Nombre
                LIMIT 100;
                """;

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");

            using SqliteDataReader lector =
                comando.ExecuteReader();

            tabla.Load(lector);

            return tabla;
        }

        public DataTable ConsultarEstadoCertificacion(
            string mostrarPor,
            string textoBuscar)
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
                        T.IdLocalidad,
                        L.Nombre AS NombreLocalidad,
                        T.IdTurno,
                        Tu.Nombre AS NombreTurno,
                        Li.IdPlanta,
                        P.Nombre AS NombrePlanta,
                        T.IdLinea,
                        Li.Nombre AS NombreLinea,

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
                    INNER JOIN Localidad AS L
                        ON L.Id = T.IdLocalidad
                    INNER JOIN Turno AS Tu
                        ON Tu.Id = T.IdTurno
                    INNER JOIN Linea AS Li
                        ON Li.Id = T.IdLinea
                    INNER JOIN Planta AS P
                        ON P.Id = Li.IdPlanta

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

                    WHERE T.NoReloj
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR T.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR L.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR Tu.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR P.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE
                       OR Li.Nombre
                              LIKE @TextoBuscar COLLATE NOCASE

                    GROUP BY
                        T.Id,
                        T.NoReloj,
                        T.Nombre,
                        T.RutaFoto,
                        T.IdLocalidad,
                        L.Nombre,
                        T.IdTurno,
                        Tu.Nombre,
                        Li.IdPlanta,
                        P.Nombre,
                        T.IdLinea,
                        Li.Nombre
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

            using SqliteDataReader lector =
                comando.ExecuteReader();

            tabla.Load(lector);

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

                if (!ExistenCatalogosRelacionados(
                    conexion,
                    transaccion,
                    trabajador))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "La localidad, el turno o la línea seleccionada no existe."
                    };
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

                if (TieneRegistrosRelacionados(
                    conexion,
                    id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay registros " +
                            "asociados a este trabajador."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

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
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "No se encontró el trabajador."
                    };
                }

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha borrado correctamente"
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static Trabajador MapearTrabajador(
            SqliteDataReader lector)
        {
            return new Trabajador
            {
                Id = Convert.ToInt32(lector["Id"]),

                NoReloj =
                    Convert.ToString(lector["NoReloj"]),

                Nombre =
                    Convert.ToString(lector["Nombre"]),

                RutaFoto =
                    lector["RutaFoto"] is DBNull
                        ? null
                        : Convert.ToString(lector["RutaFoto"]),

                IdLocalidad =
                    Convert.ToInt32(lector["IdLocalidad"]),

                NombreLocalidad =
                    Convert.ToString(
                        lector["NombreLocalidad"]),

                IdTurno =
                    Convert.ToInt32(lector["IdTurno"]),

                NombreTurno =
                    Convert.ToString(
                        lector["NombreTurno"]),

                IdPlanta =
                    Convert.ToInt32(lector["IdPlanta"]),

                NombrePlanta =
                    Convert.ToString(
                        lector["NombrePlanta"]),

                IdLinea =
                    Convert.ToInt32(lector["IdLinea"]),

                NombreLinea =
                    Convert.ToString(
                        lector["NombreLinea"])
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

            comando.Transaction = transaccion;

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

            comando.Transaction = transaccion;

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

        private static bool ExistenCatalogosRelacionados(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            Trabajador trabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT
                    (
                        SELECT COUNT(*)
                        FROM Localidad
                        WHERE Id = @IdLocalidad
                    )
                    *
                    (
                        SELECT COUNT(*)
                        FROM Turno
                        WHERE Id = @IdTurno
                    )
                    *
                    (
                        SELECT COUNT(*)
                        FROM Linea
                        WHERE Id = @IdLinea
                    );
                """;

            comando.Parameters.AddWithValue(
                "@IdLocalidad",
                trabajador.IdLocalidad);

            comando.Parameters.AddWithValue(
                "@IdTurno",
                trabajador.IdTurno);

            comando.Parameters.AddWithValue(
                "@IdLinea",
                trabajador.IdLinea);

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

            comando.Transaction = transaccion;

            comando.CommandText = """
                INSERT INTO Trabajador
                (
                    NoReloj,
                    Nombre,
                    RutaFoto,
                    IdLocalidad,
                    IdTurno,
                    IdLinea
                )
                VALUES
                (
                    @NoReloj,
                    @Nombre,
                    @RutaFoto,
                    @IdLocalidad,
                    @IdTurno,
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

            comando.Transaction = transaccion;

            comando.CommandText = """
                UPDATE Trabajador
                SET
                    NoReloj = @NoReloj,
                    Nombre = @Nombre,
                    RutaFoto = @RutaFoto,
                    IdLocalidad = @IdLocalidad,
                    IdTurno = @IdTurno,
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
                trabajador.RutaFoto!.Trim());

            comando.Parameters.AddWithValue(
                "@IdLocalidad",
                trabajador.IdLocalidad);

            comando.Parameters.AddWithValue(
                "@IdTurno",
                trabajador.IdTurno);

            comando.Parameters.AddWithValue(
                "@IdLinea",
                trabajador.IdLinea);
        }

        private static bool TieneRegistrosRelacionados(
            SqliteConnection conexion,
            int idTrabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

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
                    );
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
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

            if (string.IsNullOrWhiteSpace(
                trabajador.RutaFoto))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La fotografía del trabajador es obligatoria."
                };
            }

            if (trabajador.IdLocalidad <= 0 ||
                trabajador.IdTurno <= 0 ||
                trabajador.IdLinea <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar localidad, turno y línea."
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