using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteCertificadorRepository
        : ICertificadorRepository
    {
        public DataTable Consultar(string textoBuscar)
        {
            DataTable tabla = new DataTable();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    C.Id,
                    C.IdTrabajador,
                    T.NoReloj,
                    T.Nombre AS NombreTrabajador,
                    T.RutaFoto,
                    T.IdTurno,
                    Tu.Nombre AS NombreTurno,
                    Li.IdPlanta,
                    P.Nombre AS NombrePlanta,
                    T.IdLinea,
                    Li.Nombre AS NombreLinea
                FROM Certificador AS C
                INNER JOIN Trabajador AS T
                    ON T.Id = C.IdTrabajador
                INNER JOIN Turno AS Tu
                    ON Tu.Id = T.IdTurno
                INNER JOIN Linea AS Li
                    ON Li.Id = T.IdLinea
                INNER JOIN Planta AS P
                    ON P.Id = Li.IdPlanta
                WHERE T.NoReloj
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR T.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR Tu.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR P.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                   OR Li.Nombre
                          LIKE @TextoBuscar COLLATE NOCASE
                ORDER BY T.Nombre;
                """;

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");

            using SqliteDataReader lector =
                comando.ExecuteReader();

            tabla.Load(lector);

            return tabla;
        }

        public Mensaje Guardar(
            Certificador certificador)
        {
            if (string.IsNullOrWhiteSpace(
                certificador.NoReloj))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El número de reloj es obligatorio."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                using SqliteTransaction transaccion =
                    conexion.BeginTransaction();

                int idTrabajador =
                    ConsultarIdTrabajador(
                        conexion,
                        transaccion,
                        certificador.NoReloj.Trim());

                if (idTrabajador <= 0)
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El trabajador no existe."
                    };
                }

                if (YaEsCertificador(
                    conexion,
                    transaccion,
                    idTrabajador))
                {
                    transaccion.Rollback();

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "El trabajador ya es certificador."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.Transaction = transaccion;

                comando.CommandText = """
                    INSERT INTO Certificador
                    (
                        IdTrabajador
                    )
                    VALUES
                    (
                        @IdTrabajador
                    );
                    """;

                comando.Parameters.AddWithValue(
                    "@IdTrabajador",
                    idTrabajador);

                comando.ExecuteNonQuery();

                transaccion.Commit();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha guardado correctamente."
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
                        "El certificador seleccionado no es válido."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (!ExisteCertificador(conexion, id))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "El certificador seleccionado no existe."
                    };
                }

                if (TieneCertificacionesRelacionadas(
                    conexion,
                    id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay " +
                            "certificaciones asociadas a este certificador."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Certificador
                    WHERE Id = @Id;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    id);

                comando.ExecuteNonQuery();

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha borrado correctamente."
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static int ConsultarIdTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string noReloj)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT Id
                FROM Trabajador
                WHERE NoReloj = @NoReloj COLLATE NOCASE
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                noReloj);

            object? resultado =
                comando.ExecuteScalar();

            if (resultado is null ||
                resultado is DBNull)
            {
                return 0;
            }

            return Convert.ToInt32(resultado);
        }

        private static bool YaEsCertificador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction = transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificador
                WHERE IdTrabajador = @IdTrabajador;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteCertificador(
            SqliteConnection conexion,
            int id)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificador
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool TieneCertificacionesRelacionadas(
            SqliteConnection conexion,
            int idCertificador)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificacion
                WHERE IdCertificador = @IdCertificador;
                """;

            comando.Parameters.AddWithValue(
                "@IdCertificador",
                idCertificador);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
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