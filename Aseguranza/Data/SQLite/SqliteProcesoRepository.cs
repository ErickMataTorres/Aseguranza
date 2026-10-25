using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteProcesoRepository
        : IProcesoRepository
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
                    Nombre,
                    Descripcion,
                    VigenciaMeses
                FROM Proceso
                WHERE Nombre LIKE @TextoBuscar COLLATE NOCASE
                   OR Descripcion LIKE @TextoBuscar COLLATE NOCASE
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

        public Mensaje Guardar(Proceso proceso)
        {
            Mensaje? validacion = Validar(proceso);

            if (validacion is not null)
            {
                return validacion;
            }

            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (ExisteProceso(conexion, proceso.Id))
                {
                    Actualizar(conexion, proceso);

                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "Se ha modificado correctamente"
                    };
                }

                Insertar(conexion, proceso);

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
            try
            {
                using SqliteConnection conexion =
                    ConexionSqlite.Crear();

                conexion.Open();

                if (TieneCertificacionesRelacionadas(
                    conexion,
                    id))
                {
                    return new Mensaje
                    {
                        Id = 2,
                        Nombre =
                            "No se puede borrar porque hay registros " +
                            "asociados a este proceso."
                    };
                }

                using SqliteCommand comando =
                    conexion.CreateCommand();

                comando.CommandText = """
                    DELETE FROM Proceso
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
                            "No se encontró el proceso."
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

        private static bool ExisteProceso(
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
                FROM Proceso
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
            Proceso proceso)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                INSERT INTO Proceso (
                    Nombre,
                    Descripcion,
                    VigenciaMeses
                )
                VALUES (
                    @Nombre,
                    @Descripcion,
                    @VigenciaMeses
                );
                """;

            AgregarParametros(comando, proceso);
            comando.ExecuteNonQuery();
        }

        private static void Actualizar(
            SqliteConnection conexion,
            Proceso proceso)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                UPDATE Proceso
                SET
                    Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    VigenciaMeses = @VigenciaMeses
                WHERE Id = @Id;
                """;

            comando.Parameters.AddWithValue(
                "@Id",
                proceso.Id);

            AgregarParametros(comando, proceso);
            comando.ExecuteNonQuery();
        }

        private static void AgregarParametros(
            SqliteCommand comando,
            Proceso proceso)
        {
            comando.Parameters.AddWithValue(
                "@Nombre",
                proceso.Nombre!.Trim().ToUpperInvariant());

            comando.Parameters.AddWithValue(
                "@Descripcion",
                proceso.Descripcion!.Trim().ToUpperInvariant());

            comando.Parameters.AddWithValue(
                "@VigenciaMeses",
                proceso.VigenciaMeses);
        }

        private static bool TieneCertificacionesRelacionadas(
            SqliteConnection conexion,
            int idProceso)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Certificacion
                WHERE IdProceso = @IdProceso;
                """;

            comando.Parameters.AddWithValue(
                "@IdProceso",
                idProceso);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static Mensaje? Validar(Proceso proceso)
        {
            if (string.IsNullOrWhiteSpace(proceso.Nombre))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "El nombre del proceso es obligatorio."
                };
            }

            if (string.IsNullOrWhiteSpace(proceso.Descripcion))
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La descripción del proceso es obligatoria."
                };
            }

            if (proceso.VigenciaMeses <= 0)
            {
                return new Mensaje
                {
                    Id = 0,
                    Nombre =
                        "La vigencia debe ser mayor que cero."
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