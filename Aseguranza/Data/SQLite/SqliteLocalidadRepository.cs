using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteLocalidadRepository : ILocalidadRepository
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
                FROM Localidad
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

        public Mensaje Guardar(Localidad localidad)
        {
            if (string.IsNullOrWhiteSpace(localidad.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre = "El nombre de la localidad es obligatorio."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (ExisteLocalidad(conexion, localidad.Id))
                {
                    Actualizar(conexion, localidad);

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre = "Se ha modificado correctamente"
                    };
                }

                Insertar(conexion, localidad);

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

                if (TieneTrabajadoresRelacionados(conexion, id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay registros " +
                            "asociados a esta localidad"
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Localidad
                    WHERE Id = @Id;
                    """;

                comando.Parameters.AddWithValue("@Id", id);
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

        private static bool ExisteLocalidad(
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
                FROM Localidad
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue("@Id", id);

            return Convert.ToInt64(comando.ExecuteScalar()) > 0;
        }

        private static void Insertar(
            SqliteConnection conexion,
            Localidad localidad)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                INSERT INTO Localidad (Nombre)
                VALUES (@Nombre);
                """;

            comando.Parameters.AddWithValue(
                "@Nombre",
                localidad.Nombre!.Trim());

            comando.ExecuteNonQuery();
        }

        private static void Actualizar(
            SqliteConnection conexion,
            Localidad localidad)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                UPDATE Localidad
                SET Nombre = @Nombre
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue("@Id", localidad.Id);

            comando.Parameters.AddWithValue(
                "@Nombre",
                localidad.Nombre!.Trim());

            comando.ExecuteNonQuery();
        }

        private static bool TieneTrabajadoresRelacionados(
            SqliteConnection conexion,
            int idLocalidad)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Trabajador
                WHERE IdLocalidad = @IdLocalidad;
                """;

            comando.Parameters.AddWithValue(
                "@IdLocalidad",
                idLocalidad);

            return Convert.ToInt64(comando.ExecuteScalar()) > 0;
        }

        private static Mensaje CrearMensajeError(Exception excepcion)
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