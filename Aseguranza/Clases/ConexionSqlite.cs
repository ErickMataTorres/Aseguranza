using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Aseguranza.Clases
{
    public static class ConexionSqlite
    {
        public static SqliteConnection Crear()
        {
            string rutaBaseDatos =
                ConfiguracionSistema.ObtenerRutaSqlite();

            if (string.IsNullOrWhiteSpace(rutaBaseDatos))
            {
                throw new InvalidOperationException(
                    "No se configuró una ruta válida para la base SQLite.");
            }

            string? carpetaBaseDatos =
                Path.GetDirectoryName(rutaBaseDatos);

            if (!string.IsNullOrWhiteSpace(carpetaBaseDatos))
            {
                Directory.CreateDirectory(carpetaBaseDatos);
            }

            var constructorCadena =
                new SqliteConnectionStringBuilder
                {
                    DataSource = rutaBaseDatos,
                    Mode = SqliteOpenMode.ReadWriteCreate,
                    ForeignKeys = true,
                    Pooling = true,
                    DefaultTimeout = 30
                };

            return new SqliteConnection(
                constructorCadena.ToString());
        }
    }
}