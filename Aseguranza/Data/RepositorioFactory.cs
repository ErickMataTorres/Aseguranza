using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Aseguranza.Data.SQLite;
using Aseguranza.Data.SqlServer;

namespace Aseguranza.Data
{
    public static class RepositorioFactory
    {
        public static ILocalidadRepository CrearLocalidadRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema.ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerLocalidadRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteLocalidadRepository(),

                _ => throw new InvalidOperationException(
                    $"Proveedor de base de datos no soportado: {proveedor}")
            };
        }
    }
}