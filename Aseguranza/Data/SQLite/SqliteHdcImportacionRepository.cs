using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aseguranza.Data.SQLite
{
    public sealed class SqliteHdcImportacionRepository
        : IHdcImportacionRepository
    {
        public ResultadoImportacionHdc Importar(
            string rutaArchivo,
            ResultadoAnalisisHdc resultado)
        {
            if (resultado is null)
            {
                throw new ArgumentNullException(
                    nameof(resultado));
            }

            if (string.IsNullOrWhiteSpace(
                    rutaArchivo))
            {
                throw new ArgumentException(
                    "No se recibió la ruta del archivo HDC.",
                    nameof(rutaArchivo));
            }

            List<RegistroHdc> preparados =
                resultado.Registros
                    .Where(EsRegistroPreparado)
                    .ToList();

            int nuevos =
                preparados.Count(
                    item => EstadoEs(item, "Nuevo"));

            int actualizados =
                preparados.Count(
                    item => EstadoEs(item, "Actualizar"));

            int sinCambios =
                preparados.Count(
                    item => EstadoEs(item, "Sin cambios"));

            int ignorados =
                resultado.Registros.Count(
                    item => EstadoEs(item, "Ignorar"));

            int excluidos =
                Math.Max(
                    0,
                    resultado.Registros.Count -
                    preparados.Count -
                    ignorados);

            if (preparados.Count == 0)
            {
                throw new InvalidOperationException(
                    "No hay registros preparados para importar.");
            }

            using SqliteConnection conexion =
                ConexionSqlite.Crear();

            conexion.Open();

            string rutaRespaldo =
                CrearRespaldoPrevio(
                    conexion);

            using SqliteTransaction transaccion =
                conexion.BeginTransaction();

            try
            {
                int idImportacion =
                    CrearImportacion(
                        conexion,
                        transaccion,
                        rutaArchivo,
                        resultado,
                        nuevos,
                        actualizados,
                        sinCambios,
                        excluidos,
                        ignorados);

                foreach (RegistroHdc registro
                         in preparados)
                {
                    ValidarRegistroPreparado(
                        conexion,
                        transaccion,
                        registro);

                    int idTrabajador =
                        ObtenerIdTrabajador(
                            conexion,
                            transaccion,
                            registro.Empleado);

                    if (EstadoEs(
                            registro,
                            "Nuevo"))
                    {
                        if (idTrabajador > 0)
                        {
                            throw CrearErrorFila(
                                registro,
                                "El registro estaba clasificado como Nuevo, " +
                                "pero el trabajador ya existe en la base de datos.");
                        }

                        idTrabajador =
                            InsertarTrabajador(
                                conexion,
                                transaccion,
                                registro);
                    }
                    else
                    {
                        if (idTrabajador <= 0)
                        {
                            throw CrearErrorFila(
                                registro,
                                "El trabajador dejó de existir después del análisis HDC.");
                        }

                        if (EstadoEs(
                                registro,
                                "Actualizar"))
                        {
                            ActualizarTrabajador(
                                conexion,
                                transaccion,
                                idTrabajador,
                                registro);
                        }
                    }

                    GuardarPerfilHdc(
                        conexion,
                        transaccion,
                        idTrabajador,
                        idImportacion,
                        registro);
                }

                transaccion.Commit();

                return new ResultadoImportacionHdc
                {
                    IdImportacion =
                        idImportacion,

                    TotalRegistrosArchivo =
                        resultado.Registros.Count,

                    NuevosInsertados =
                        nuevos,

                    Actualizados =
                        actualizados,

                    SinCambios =
                        sinCambios,

                    Excluidos =
                        excluidos,

                    Ignorados =
                        ignorados,

                    RutaRespaldo =
                        rutaRespaldo
                };
            }
            catch
            {
                try
                {
                    transaccion.Rollback();
                }
                catch
                {
                    // Nunca ocultar el error original por un fallo de rollback.
                }

                throw;
            }
        }

        private static bool EsRegistroPreparado(
            RegistroHdc registro)
        {
            return
                EstadoEs(registro, "Nuevo") ||
                EstadoEs(registro, "Actualizar") ||
                EstadoEs(registro, "Sin cambios");
        }

        private static bool EstadoEs(
            RegistroHdc registro,
            string estado)
        {
            return string.Equals(
                registro.Estado?.Trim(),
                estado,
                StringComparison.OrdinalIgnoreCase);
        }

        private static bool EsAccionLinea(
            RegistroHdc registro,
            string accion)
        {
            return string.Equals(
                registro.AccionLinea?.Trim(),
                accion,
                StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidarRegistroPreparado(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            RegistroHdc registro)
        {
            if (string.IsNullOrWhiteSpace(
                    registro.Empleado))
            {
                throw CrearErrorFila(
                    registro,
                    "El número de empleado está vacío.");
            }

            if (string.IsNullOrWhiteSpace(
                    registro.Nombre))
            {
                throw CrearErrorFila(
                    registro,
                    "El nombre del trabajador está vacío.");
            }

            if (registro.IdPlantaSistema <= 0)
            {
                throw CrearErrorFila(
                    registro,
                    "La planta del sistema no es válida.");
            }

            if (registro.IdTurnoSistema <= 0)
            {
                throw CrearErrorFila(
                    registro,
                    "El turno del sistema no es válido.");
            }

            if (!ExisteId(
                    conexion,
                    transaccion,
                    "Planta",
                    registro.IdPlantaSistema))
            {
                throw CrearErrorFila(
                    registro,
                    "La planta resuelta ya no existe en la base de datos.");
            }

            if (!ExisteId(
                    conexion,
                    transaccion,
                    "Turno",
                    registro.IdTurnoSistema))
            {
                throw CrearErrorFila(
                    registro,
                    "El turno resuelto ya no existe en la base de datos.");
            }

            if (registro.IdLineaSistema <= 0)
            {
                return;
            }

            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM Linea
                WHERE Id = @IdLinea
                  AND IdPlanta = @IdPlanta;
                """;

            comando.Parameters.AddWithValue(
                "@IdLinea",
                registro.IdLineaSistema);

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                registro.IdPlantaSistema);

            if (Convert.ToInt64(
                    comando.ExecuteScalar()) == 0)
            {
                throw CrearErrorFila(
                    registro,
                    "La línea resuelta no pertenece a la planta del sistema.");
            }
        }

        private static int CrearImportacion(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string rutaArchivo,
            ResultadoAnalisisHdc resultado,
            int nuevos,
            int actualizados,
            int sinCambios,
            int excluidos,
            int ignorados)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO ImportacionHdc
                (
                    NombreArchivo,
                    NombreHoja,
                    FechaImportacion,
                    TotalRegistros,
                    Nuevos,
                    Actualizados,
                    SinCambios,
                    ConAdvertencias,
                    Observaciones
                )
                VALUES
                (
                    @NombreArchivo,
                    @NombreHoja,
                    datetime('now', 'localtime'),
                    @TotalRegistros,
                    @Nuevos,
                    @Actualizados,
                    @SinCambios,
                    @ConAdvertencias,
                    @Observaciones
                );
                """;

            comando.Parameters.AddWithValue(
                "@NombreArchivo",
                Path.GetFileName(rutaArchivo));

            comando.Parameters.AddWithValue(
                "@NombreHoja",
                string.IsNullOrWhiteSpace(
                    resultado.NombreHoja)
                    ? "HDC"
                    : resultado.NombreHoja.Trim());

            comando.Parameters.AddWithValue(
                "@TotalRegistros",
                resultado.Registros.Count);

            comando.Parameters.AddWithValue(
                "@Nuevos",
                nuevos);

            comando.Parameters.AddWithValue(
                "@Actualizados",
                actualizados);

            comando.Parameters.AddWithValue(
                "@SinCambios",
                sinCambios);

            comando.Parameters.AddWithValue(
                "@ConAdvertencias",
                excluidos);

            comando.Parameters.AddWithValue(
                "@Observaciones",
                "Importación transaccional HDC. " +
                $"Excluidos por equivalencias/revisión: {excluidos:N0}. " +
                $"Ignorados: {ignorados:N0}.");

            comando.ExecuteNonQuery();

            return ObtenerUltimoId(
                conexion,
                transaccion);
        }

        private static int ObtenerIdTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string noReloj)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT Id
                FROM Trabajador
                WHERE NoReloj = @NoReloj COLLATE NOCASE
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                noReloj.Trim());

            object? valor =
                comando.ExecuteScalar();

            if (valor is null ||
                valor == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(
                valor);
        }

        private static int InsertarTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            RegistroHdc registro)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO Trabajador
                (
                    NoReloj,
                    Nombre,
                    RutaFoto,
                    IdLocalidad,
                    IdTurno,
                    IdPlanta,
                    IdLinea
                )
                VALUES
                (
                    @NoReloj,
                    @Nombre,
                    NULL,
                    NULL,
                    @IdTurno,
                    @IdPlanta,
                    @IdLinea
                );
                """;

            AgregarParametrosAsignacion(
                comando,
                registro,
                incluirNoReloj: true);

            comando.ExecuteNonQuery();

            return ObtenerUltimoId(
                conexion,
                transaccion);
        }

        private static int ObtenerUltimoId(
            SqliteConnection conexion,
            SqliteTransaction transaccion)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText =
                "SELECT last_insert_rowid();";

            return Convert.ToInt32(
                Convert.ToInt64(
                    comando.ExecuteScalar()));
        }

        private static void ActualizarTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador,
            RegistroHdc registro)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                UPDATE Trabajador
                SET
                    Nombre = @Nombre,
                    IdTurno = @IdTurno,
                    IdLinea =
                        CASE
                            WHEN @EsSinAsignar = 1
                             AND IdPlanta = @IdPlanta
                                THEN IdLinea
                            WHEN @EsSinAsignar = 1
                                THEN NULL
                            ELSE @IdLinea
                        END,
                    IdPlanta = @IdPlanta
                WHERE Id = @IdTrabajador;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            comando.Parameters.AddWithValue(
                "@EsSinAsignar",
                EsAccionLinea(
                    registro,
                    "SIN_ASIGNAR")
                    ? 1
                    : 0);

            AgregarParametrosAsignacion(
                comando,
                registro,
                incluirNoReloj: false);

            int afectadas =
                comando.ExecuteNonQuery();

            if (afectadas != 1)
            {
                throw CrearErrorFila(
                    registro,
                    "No fue posible actualizar exactamente un trabajador.");
            }
        }

        private static void AgregarParametrosAsignacion(
            SqliteCommand comando,
            RegistroHdc registro,
            bool incluirNoReloj)
        {
            if (incluirNoReloj)
            {
                comando.Parameters.AddWithValue(
                    "@NoReloj",
                    registro.Empleado.Trim());
            }

            comando.Parameters.AddWithValue(
                "@Nombre",
                registro.Nombre
                    .Trim()
                    .ToUpperInvariant());

            comando.Parameters.AddWithValue(
                "@IdTurno",
                registro.IdTurnoSistema);

            comando.Parameters.AddWithValue(
                "@IdPlanta",
                registro.IdPlantaSistema);

            comando.Parameters.AddWithValue(
                "@IdLinea",
                registro.IdLineaSistema > 0
                    ? registro.IdLineaSistema
                    : DBNull.Value);
        }

        private static void GuardarPerfilHdc(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int idTrabajador,
            int idImportacion,
            RegistroHdc registro)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO PerfilHdcTrabajador
                (
                    IdTrabajador,
                    IdImportacionHdc,
                    EmpleadoHdc,
                    NombreHdc,
                    LocalidadHdc,
                    TurnoHdc,
                    FechaServicio,
                    DepartamentoHdc,
                    LineaHdc,
                    PuestoHdc,
                    CategoriaHdc,
                    PositionHdc,
                    FunctionHdc,
                    ProcesoHdc,
                    DptoHdc,
                    FechaUltimaImportacion,
                    EncontradoUltimoHdc
                )
                VALUES
                (
                    @IdTrabajador,
                    @IdImportacionHdc,
                    @EmpleadoHdc,
                    @NombreHdc,
                    @LocalidadHdc,
                    @TurnoHdc,
                    @FechaServicio,
                    @DepartamentoHdc,
                    @LineaHdc,
                    @PuestoHdc,
                    @CategoriaHdc,
                    @PositionHdc,
                    @FunctionHdc,
                    @ProcesoHdc,
                    @DptoHdc,
                    datetime('now', 'localtime'),
                    1
                )
                ON CONFLICT(IdTrabajador)
                DO UPDATE SET
                    IdImportacionHdc = excluded.IdImportacionHdc,
                    EmpleadoHdc = excluded.EmpleadoHdc,
                    NombreHdc = excluded.NombreHdc,
                    LocalidadHdc = excluded.LocalidadHdc,
                    TurnoHdc = excluded.TurnoHdc,
                    FechaServicio = excluded.FechaServicio,
                    DepartamentoHdc = excluded.DepartamentoHdc,
                    LineaHdc = excluded.LineaHdc,
                    PuestoHdc = excluded.PuestoHdc,
                    CategoriaHdc = excluded.CategoriaHdc,
                    PositionHdc = excluded.PositionHdc,
                    FunctionHdc = excluded.FunctionHdc,
                    ProcesoHdc = excluded.ProcesoHdc,
                    DptoHdc = excluded.DptoHdc,
                    FechaUltimaImportacion = datetime('now', 'localtime'),
                    EncontradoUltimoHdc = 1;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            comando.Parameters.AddWithValue(
                "@IdImportacionHdc",
                idImportacion);

            AgregarTexto(
                comando,
                "@EmpleadoHdc",
                registro.Empleado);

            AgregarTexto(
                comando,
                "@NombreHdc",
                registro.Nombre);

            AgregarTexto(
                comando,
                "@LocalidadHdc",
                registro.LocalidadHdc);

            AgregarTexto(
                comando,
                "@TurnoHdc",
                registro.TurnoHdc);

            AgregarTexto(
                comando,
                "@FechaServicio",
                registro.FechaServicio);

            AgregarTexto(
                comando,
                "@DepartamentoHdc",
                registro.DepartamentoHdc);

            AgregarTexto(
                comando,
                "@LineaHdc",
                registro.LineaHdc);

            AgregarTexto(
                comando,
                "@PuestoHdc",
                registro.PuestoHdc);

            AgregarTexto(
                comando,
                "@CategoriaHdc",
                registro.CategoriaHdc);

            AgregarTexto(
                comando,
                "@PositionHdc",
                registro.PositionHdc);

            AgregarTexto(
                comando,
                "@FunctionHdc",
                registro.FunctionHdc);

            AgregarTexto(
                comando,
                "@ProcesoHdc",
                registro.ProcesoHdc);

            AgregarTexto(
                comando,
                "@DptoHdc",
                registro.DptoHdc);

            comando.ExecuteNonQuery();
        }

        private static void AgregarTexto(
            SqliteCommand comando,
            string nombre,
            string? valor)
        {
            comando.Parameters.AddWithValue(
                nombre,
                string.IsNullOrWhiteSpace(valor)
                    ? DBNull.Value
                    : valor.Trim());
        }

        private static bool ExisteId(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string tabla,
            int id)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText =
                $"SELECT COUNT(*) FROM {tabla} WHERE Id = @Id;";

            comando.Parameters.AddWithValue(
                "@Id",
                id);

            return Convert.ToInt64(
                comando.ExecuteScalar()) > 0;
        }

        private static InvalidOperationException CrearErrorFila(
            RegistroHdc registro,
            string mensaje)
        {
            return new InvalidOperationException(
                "No se pudo importar la fila " +
                registro.NumeroFilaExcel +
                " del HDC (Empleado " +
                (string.IsNullOrWhiteSpace(registro.Empleado)
                    ? "sin número"
                    : registro.Empleado.Trim()) +
                "). " +
                mensaje);
        }

        private static string CrearRespaldoPrevio(
            SqliteConnection conexionOrigen)
        {
            string rutaOriginal =
                ConfiguracionSistema.ObtenerRutaSqlite();

            string carpetaOriginal =
                Path.GetDirectoryName(
                    rutaOriginal)
                ?? AppContext.BaseDirectory;

            string carpetaRespaldos =
                Path.Combine(
                    carpetaOriginal,
                    "Backups");

            Directory.CreateDirectory(
                carpetaRespaldos);

            string nombreBase =
                Path.GetFileNameWithoutExtension(
                    rutaOriginal);

            string rutaRespaldo =
                Path.Combine(
                    carpetaRespaldos,
                    nombreBase +
                    "_PreImportHdc_" +
                    DateTime.Now.ToString(
                        "yyyyMMdd_HHmmss") +
                    ".db");

            SqliteConnectionStringBuilder constructor =
                new SqliteConnectionStringBuilder
                {
                    DataSource = rutaRespaldo,
                    Mode = SqliteOpenMode.ReadWriteCreate,
                    Pooling = false
                };

            using SqliteConnection conexionDestino =
                new SqliteConnection(
                    constructor.ToString());

            conexionDestino.Open();

            conexionOrigen.BackupDatabase(
                conexionDestino);

            return rutaRespaldo;
        }
    }
}
