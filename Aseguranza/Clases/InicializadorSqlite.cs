using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Aseguranza.Clases
{
    public static class InicializadorSqlite
    {
        private const string RutaRelativaScript =
            @"Database\SQLite\AseguranzaSQLite.sql";

        private const string RutaRelativaActualizacionEstado =
            @"Database\SQLite\Migrations\10_TrabajadorEstado.sql";

        public static void Inicializar()
        {
            string rutaBaseDatos = ConfiguracionSistema.ObtenerRutaSqlite();
            if (File.Exists(rutaBaseDatos))
            {
                if (!EstaInicializada())
                {
                    throw new InvalidOperationException(
                        "Ya existe un archivo en la ruta SQLite configurada, " +
                        "pero no tiene un esquema reconocido. No se modificó.");
                }

                ActualizarEsquemaExistente();
                return;
            }

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

        public static void ActualizarEsquemaExistente()
        {
            string rutaBaseDatos =
                ConfiguracionSistema.ObtenerRutaSqlite();

            if (!File.Exists(rutaBaseDatos))
            {
                throw new FileNotFoundException(
                    "No se encontró la base SQLite configurada. Verifique la ruta antes de continuar.",
                    rutaBaseDatos);
            }

            string rutaScript = Path.Combine(
                AppContext.BaseDirectory,
                RutaRelativaActualizacionEstado);

            if (!File.Exists(rutaScript))
            {
                throw new FileNotFoundException(
                    "No se encontró la actualización SQLite de estado de trabajadores.",
                    rutaScript);
            }

            using SqliteConnection conexion = ConexionSqlite.Crear();
            conexion.Open();

            using SqliteCommand comando = conexion.CreateCommand();

            comando.CommandText =
                "SELECT COUNT(*) FROM sqlite_master " +
                "WHERE type = 'table' AND name = 'SchemaVersion';";

            if (Convert.ToInt64(comando.ExecuteScalar()) == 0)
            {
                throw new InvalidOperationException(
                    "La base SQLite no tiene SchemaVersion. No se aplicó ninguna actualización.");
            }

            comando.CommandText =
                "SELECT COUNT(*) FROM SchemaVersion WHERE Version = 10;";
            bool versionAplicada = Convert.ToInt64(comando.ExecuteScalar()) > 0;

            comando.CommandText = "PRAGMA table_info('Trabajador');";
            bool columnaExiste = false;
            using (SqliteDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    if (string.Equals(
                        lector.GetString(1),
                        "Activo",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        columnaExiste = true;
                    }
                }
            }

            if (versionAplicada)
            {
                if (!columnaExiste)
                {
                    throw new InvalidOperationException(
                        "La versión SQLite 10 está registrada, pero falta Trabajador.Activo.");
                }

                comando.CommandText =
                    "SELECT COUNT(*) FROM sqlite_master " +
                    "WHERE type = 'table' AND name = 'TrabajadorEstadoHistorial';";
                if (Convert.ToInt64(comando.ExecuteScalar()) == 0)
                {
                    throw new InvalidOperationException(
                        "La versión SQLite 10 está registrada, pero falta el historial de estado.");
                }

                return;
            }

            // El respaldo se toma antes de comenzar la transacción de esquema.
            string rutaRespaldo = rutaBaseDatos +
                ".pre-estado-v10-" + Guid.NewGuid().ToString("N") + ".bak";
            SqliteConnectionStringBuilder cadenaRespaldo = new()
            {
                DataSource = rutaRespaldo,
                Mode = SqliteOpenMode.ReadWriteCreate
            };
            using (SqliteConnection respaldo =
                new SqliteConnection(cadenaRespaldo.ToString()))
            {
                respaldo.Open();
                conexion.BackupDatabase(respaldo);
            }

            using SqliteTransaction transaccion = conexion.BeginTransaction();
            comando.Transaction = transaccion;

            if (!columnaExiste)
            {
                comando.CommandText =
                    "ALTER TABLE Trabajador ADD COLUMN Activo INTEGER NOT NULL " +
                    "DEFAULT 1 CHECK(Activo IN (0, 1));";
                comando.ExecuteNonQuery();
            }

            comando.CommandText = File.ReadAllText(rutaScript);
            comando.ExecuteNonQuery();
            transaccion.Commit();
        }
    }
}
