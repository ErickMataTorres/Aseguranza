using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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

            string script = File.ReadAllText(rutaScript);

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            using SqliteTransaction transaccion =
                conexion.BeginTransaction();

            try
            {
                foreach (string instruccion in SepararInstrucciones(script))
                {
                    using SqliteCommand comando =
                        conexion.CreateCommand();

                    comando.Transaction = transaccion;
                    comando.CommandText = instruccion;
                    comando.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
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
                "SELECT COUNT(*) " +
                "FROM sqlite_master " +
                "WHERE type = 'table' " +
                "AND name = 'SchemaVersion';";

            long resultado =
                Convert.ToInt64(comando.ExecuteScalar());

            return resultado > 0;
        }

        private static IEnumerable<string> SepararInstrucciones(
            string script)
        {
            string[] fragmentos =
                script.Split(
                    ';',
                    StringSplitOptions.RemoveEmptyEntries);

            foreach (string fragmento in fragmentos)
            {
                string instruccion = fragmento.Trim();

                if (!string.IsNullOrWhiteSpace(instruccion))
                {
                    yield return instruccion;
                }
            }
        }
    }
}
