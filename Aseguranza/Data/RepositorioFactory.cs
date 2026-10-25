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
                ConfiguracionSistema.ObtenerProveedorBaseDatos();

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
                ConfiguracionSistema.ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerTurnoRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteTurnoRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static IPlantaRepository
            CrearPlantaRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema.ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerPlantaRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqlitePlantaRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static ILineaRepository
            CrearLineaRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema.ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerLineaRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteLineaRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static IProcesoRepository
            CrearProcesoRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema.ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerProcesoRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteProcesoRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static ITrabajadorRepository
            CrearTrabajadorRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerTrabajadorRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteTrabajadorRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static IHdcEquivalenciaRepository
            CrearHdcEquivalenciaRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerHdcEquivalenciaRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteHdcEquivalenciaRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static IHdcImportacionRepository
            CrearHdcImportacionRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SQLite =>
                    new SqliteHdcImportacionRepository(),

                ProveedorBaseDatos.SqlServer =>
                    new SqlServerHdcImportacionRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static ICertificadorRepository
            CrearCertificadorRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerCertificadorRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteCertificadorRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static ICertificacionRepository
            CrearCertificacionRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerCertificacionRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteCertificacionRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static ICertificacionAnulacionRepository
            CrearCertificacionAnulacionRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerCertificacionAnulacionRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteCertificacionAnulacionRepository(),

                _ => throw CrearErrorProveedor(proveedor)
            };
        }

        public static IExpedienteTrabajadorRepository
            CrearExpedienteTrabajadorRepository()
        {
            ProveedorBaseDatos proveedor =
                ConfiguracionSistema
                    .ObtenerProveedorBaseDatos();

            return proveedor switch
            {
                ProveedorBaseDatos.SqlServer =>
                    new SqlServerExpedienteTrabajadorRepository(),

                ProveedorBaseDatos.SQLite =>
                    new SqliteExpedienteTrabajadorRepository(),

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
