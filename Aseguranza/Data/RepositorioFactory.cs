using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Aseguranza.Data.SQLite;
using Aseguranza.Data.SqlServer;
using System;

namespace Aseguranza.Data
{
    public static class RepositorioFactory
    {
        public static ILocalidadRepository
            CrearLocalidadRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerLocalidadRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteLocalidadRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static ITurnoRepository
            CrearTurnoRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerTurnoRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteTurnoRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        private static InvalidOperationException
            CrearErrorProveedor(
                ProveedorBaseDatos proveedor)
        {
            return new InvalidOperationException(
                $"Proveedor de base de datos no soportado: {proveedor}");
        }
    }
}