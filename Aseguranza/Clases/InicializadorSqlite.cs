using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Aseguranza.Clases
{
    public static class InicializadorSqlite
    {
        private const string RutaRelativaScript =
            @"Database\SQLite\AseguranzaSQLite.sql";

        public static void Inicializar()
        {
            string rutaScript = Path.Combine(
                AppContext.BaseDirectory,
                RutaRelativaScript);

            if (!File.Exists(rutaScript))
            {
                throw new FileNotFoundException(
                    "No se encontró el script de inicialización SQLite.",
                    rutaScript);
            }

            string script =
                File.ReadAllText(rutaScript);

            if (string.IsNullOrWhiteSpace(script))
            {
                throw new InvalidOperationException(
                    "El script de inicialización SQLite está vacío.");
            }

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            /*
             * IMPORTANTE:
             *
             * AseguranzaSQLite.sql administra su propia transacción mediante
             * BEGIN TRANSACTION / COMMIT.
             *
             * No se crea una transacción adicional desde C#, porque eso
             * provocaría:
             *
             *     cannot start a transaction within a transaction
             *
             * Tampoco se divide el script por ';'. Un CREATE TRIGGER puede
             * contener varios ';' dentro de BEGIN ... END, por lo que dividir
             * por punto y coma rompe sentencias SQL válidas.
             *
             * Microsoft.Data.Sqlite ejecuta el lote completo enviado en
             * CommandText, conservando correctamente las sentencias y triggers
             * definidos en el script.
             */
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText =
                script;

            comando.ExecuteNonQuery();
        }

        public static bool EstaInicializada()
        {
            string rutaBaseDatos =
                ConfiguracionSistema.ObtenerRutaSqlite();

            if (!File.Exists(rutaBaseDatos))
            {
                return false;
            }

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.CommandText =
                """
                SELECT COUNT(*)
                FROM sqlite_master
                WHERE type = 'table'
                  AND name = 'SchemaVersion';
                """;

            long resultado =
                Convert.ToInt64(
                    comando.ExecuteScalar());

            return resultado > 0;
        }
    }
}
