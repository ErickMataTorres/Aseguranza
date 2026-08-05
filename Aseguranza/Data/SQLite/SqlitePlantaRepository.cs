using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqlitePlantaRepository : IPlantaRepository
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
                FROM Planta
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

        public Mensaje Guardar(Planta planta)
        {
            if (string.IsNullOrWhiteSpace(planta.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre = "El nombre de la planta es obligatorio."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (ExistePlanta(conexion, planta.Id))
                {
                    Actualizar(conexion, planta);

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre = "Se ha modificado correctamente"
                    };
                }

                Insertar(conexion, planta);

                return new Mensaje
                {
                    Id = 1,
                    Nombre = "Se ha registrado correctamente"
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

                if (TieneLineasRelacionadas(conexion, id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay registros " +
                            "asociados a esta planta"
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Planta
                    WHERE Id = @Id;
                    """;

                comando.Parameters.AddWithValue(
                    "@Id",
                    id);

                comando.ExecuteNonQuery();

                return new Mensaje
                {
                    Id = 1,
                    Nombre = "Se ha borrado correctamente"
                };
            }
            catch (Exception ex)
            {
                return CrearMensajeError(ex);
            }
        }

        private static bool ExistePlanta(
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
                FROM Planta
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
            Planta planta)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                INSERT INTO Planta (
                    Nombre
                )
                VALUES (
                    @Nombre
                );
                """;

            comando.Parameters.AddWithValue(
                "@Nombre",
                planta.Nombre!.Trim());

            comando.ExecuteNonQuery();
        }

        private static void Actualizar(
            SqliteConnection conexion,
            Planta planta)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                UPDATE Planta
                SET Nombre = @Nombre
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                planta.Id);

            comando.Parameters.AddWithValue(
                "@Nombre",
                planta.Nombre!.Trim());

            comando.ExecuteNonQuery();
        }

        private static bool TieneLineasRelacionadas(
            SqliteConnection conexion,
            int idPlanta)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Linea
                WHERE IdPlanta = @IdPlanta;
                """;

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                idPlanta);

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