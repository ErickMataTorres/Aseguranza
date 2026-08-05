using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteTurnoRepository : ITurnoRepository
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
                    Id,
                    Nombre
                FROM Turno
                WHERE Nombre LIKE @TextoBuscar COLLATE NOCASE
                ORDER BY Nombre;
                """;

            comando.Parameters.AddWithValue(
                "@TextoBuscar",
                $"%{textoBuscar ?? string.Empty}%");

            using SqliteDataReader lector =
                comando.ExecuteReader();

            tabla.Load(lector);

            return tabla;
        }

        public Mensaje Guardar(Turno turno)
        {
            if (string.IsNullOrWhiteSpace(turno.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre = "El nombre del turno es obligatorio."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (ExisteTurno(conexion, turno.Id))
                {
                    Actualizar(conexion, turno);

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "Se ha modificado correctamente"
                    };
                }

                Insertar(conexion, turno);

                return new Mensaje
                {
                    Id = 1,
                    Nombre =
                        "Se ha registrado correctamente"
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        public Mensaje Borrar(int id)
        {
            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (TieneTrabajadoresRelacionados(
                    conexion,
                    id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay registros " +
                            "asociados a este turno"
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Turno
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
                        "Se ha borrado correctamente"
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static bool ExisteTurno(
            SqliteConnection conexion,
            int id)
        {
            if (id <= 0)
            {
                return false;
            }

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Turno
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static void Insertar(
            SqliteConnection conexion,
            Turno turno)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                INSERT INTO Turno (
                    Nombre
                )
                VALUES (
                    @Nombre
                );
                """;

            comando.Parameters.AddWithValue(
                "@Nombre",
                turno.Nombre!.Trim());

            comando.ExecuteNonQuery();
        }

        private static void Actualizar(
            SqliteConnection conexion,
            Turno turno)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                UPDATE Turno
                SET Nombre = @Nombre
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                turno.Id);

            comando.Parameters.AddWithValue(
                "@Nombre",
                turno.Nombre!.Trim());

            comando.ExecuteNonQuery();
        }

        private static bool TieneTrabajadoresRelacionados(
            SqliteConnection conexion,
            int idTurno)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Trabajador
                WHERE IdTurno = @IdTurno;
                """;

            comando.Parameters.AddWithValue(
                "@IdTurno",
                idTurno);

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