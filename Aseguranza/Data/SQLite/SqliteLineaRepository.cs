using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteLineaRepository : ILineaRepository
    {
        public DataTable Consultar(string textoBuscar)
        {
            DataTable tabla =
                CrearTablaConsultaGeneral();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    l.Id,
                    l.Nombre,
                    l.IdPlanta,
                    p.Nombre AS NombrePlanta
                FROM Linea AS l
                INNER JOIN Planta AS p
                    ON p.Id = l.IdPlanta
                WHERE l.Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR p.Nombre LIKE @TextoBuscar COLLATE NOCASE
                ORDER BY
                    p.Nombre,
                    l.Nombre;
                """;

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
                        lector.GetInt64(0));

                fila["Nombre"] =
                    lector.IsDBNull(1)
                        ? string.Empty
                        : lector.GetString(1);

                fila["IdPlanta"] =
                    Convert.ToInt32(
                        lector.GetInt64(2));

                fila["NombrePlanta"] =
                    lector.IsDBNull(3)
                        ? string.Empty
                        : lector.GetString(3);

                tabla.Rows.Add(
                    fila);
            }

            return tabla;
        }

        public DataTable ConsultarPorPlanta(int idPlanta)
        {
            DataTable tabla =
                CrearTablaConsultaPorPlanta();

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    Id,
                    Nombre
                FROM Linea
                WHERE IdPlanta = @IdPlanta
                ORDER BY Nombre;
                """;

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                idPlanta);

            using SqliteDataReader lector =
                comando.ExecuteReader();

            while (lector.Read())
            {
                DataRow fila =
                    tabla.NewRow();

                fila["Id"] =
                    Convert.ToInt32(
                        lector.GetInt64(0));

                fila["Nombre"] =
                    lector.IsDBNull(1)
                        ? string.Empty
                        : lector.GetString(1);

                tabla.Rows.Add(
                    fila);
            }

            return tabla;
        }

        private static DataTable CrearTablaConsultaGeneral()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "Id",
                typeof(int));

            tabla.Columns.Add(
                "Nombre",
                typeof(string));

            tabla.Columns.Add(
                "IdPlanta",
                typeof(int));

            tabla.Columns.Add(
                "NombrePlanta",
                typeof(string));

            return tabla;
        }

        private static DataTable CrearTablaConsultaPorPlanta()
        {
            DataTable tabla =
                new DataTable();

            tabla.Columns.Add(
                "Id",
                typeof(int));

            tabla.Columns.Add(
                "Nombre",
                typeof(string));

            return tabla;
        }

        public Mensaje Guardar(Linea linea)
        {
            if (string.IsNullOrWhiteSpace(linea.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre de la línea es obligatorio."
                };
            }

            if (linea.IdPlanta <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "Debe seleccionar una planta válida."
                };
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (!ExistePlanta(
                    conexion,
                    linea.IdPlanta))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "La planta seleccionada no existe."
                    };
                }

                if (ExisteDuplicado(conexion, linea))
                {
                    return new Mensaje
                    {
                        Id = 0,
                        Nombre =
                            "Ya existe una línea con ese nombre " +
                            "en la planta seleccionada."
                    };
                }

                if (ExisteLinea(conexion, linea.Id))
                {
                    Actualizar(conexion, linea);

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "Se ha modificado correctamente"
                    };
                }

                Insertar(conexion, linea);

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
                            "asociados a esta línea."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Linea
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

        private static bool ExistePlanta(
            SqliteConnection conexion,
            int idPlanta)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Planta
                WHERE Id = @IdPlanta;
                """;

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                idPlanta);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteLinea(
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
                FROM Linea
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static bool ExisteDuplicado(
            SqliteConnection conexion,
            Linea linea)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Linea
                WHERE IdPlanta = @IdPlanta
                  AND Nombre = @Nombre COLLATE NOCASE
                  AND Id <> @Id;
                """;

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                linea.IdPlanta);

            comando.Parameters.AddWithValue(
                "@Nombre",
                linea.Nombre!.Trim());

            comando.Parameters.AddWithValue(
                "@Id",
                linea.Id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static void Insertar(
            SqliteConnection conexion,
            Linea linea)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                INSERT INTO Linea (
                    Nombre,
                    IdPlanta
                )
                VALUES (
                    @Nombre,
                    @IdPlanta
                );
                """;

            comando.Parameters.AddWithValue(
                "@Nombre",
                linea.Nombre!.Trim().ToUpperInvariant());

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                linea.IdPlanta);

            comando.ExecuteNonQuery();
        }

        private static void Actualizar(
            SqliteConnection conexion,
            Linea linea)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                UPDATE Linea
                SET
                    Nombre = @Nombre,
                    IdPlanta = @IdPlanta
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                linea.Id);

            comando.Parameters.AddWithValue(
                "@Nombre",
                linea.Nombre!.Trim().ToUpperInvariant());

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                linea.IdPlanta);

            comando.ExecuteNonQuery();
        }

        private static bool TieneTrabajadoresRelacionados(
            SqliteConnection conexion,
            int idLinea)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Trabajador
                WHERE IdLinea = @IdLinea;
                """;

            comando.Parameters.AddWithValue(
                "@IdLinea",
                idLinea);

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