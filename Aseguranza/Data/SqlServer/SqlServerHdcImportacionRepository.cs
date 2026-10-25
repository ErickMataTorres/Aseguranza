using Aseguranza.Clases;
using Aseguranza.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Aseguranza.Data.SqlServer
{
    public sealed class SqlServerHdcImportacionRepository
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

            using SqlConnection conexion =
                Conexion.Conectar();

            conexion.Open();

            ValidarEstructuraBase(
                conexion);

            using SqlTransaction transaccion =
                conexion.BeginTransaction(
                    IsolationLevel.ReadCommitted);

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
                                "pero el trabajador ya existe en la base de datos. " +
                                "Vuelva a analizar el HDC antes de importar.");
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
                                "El trabajador dejó de existir después del análisis HDC. " +
                                "Vuelva a analizar el archivo antes de importar.");
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
                        else if (EstadoEs(
                                     registro,
                                     "Sin cambios") &&
                                 !TrabajadorCoincideConHdc(
                                     conexion,
                                     transaccion,
                                     idTrabajador,
                                     registro))
                        {
                            throw CrearErrorFila(
                                registro,
                                "El trabajador cambió después del análisis y ya no coincide " +
                                "con el HDC. Vuelva a analizar antes de importar.");
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
                        string.Empty
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

        private static void ValidarEstructuraBase(
            SqlConnection conexion)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.CommandText = """
                SELECT
                    CASE
                        WHEN OBJECT_ID(N'dbo.Trabajador', N'U') IS NOT NULL
                         AND OBJECT_ID(N'dbo.Planta', N'U') IS NOT NULL
                         AND OBJECT_ID(N'dbo.Turno', N'U') IS NOT NULL
                         AND OBJECT_ID(N'dbo.Linea', N'U') IS NOT NULL
                         AND OBJECT_ID(N'dbo.ImportacionHdc', N'U') IS NOT NULL
                         AND OBJECT_ID(N'dbo.PerfilHdcTrabajador', N'U') IS NOT NULL
                         AND COL_LENGTH(N'dbo.Trabajador', N'IdPlanta') IS NOT NULL
                         AND COL_LENGTH(N'dbo.Trabajador', N'IdLinea') IS NOT NULL
                        THEN 1
                        ELSE 0
                    END;
                """;

            if (Convert.ToInt32(
                    comando.ExecuteScalar()) != 1)
            {
                throw new InvalidOperationException(
                    "SQL Server no tiene completa la estructura requerida para " +
                    "la importación HDC. Ejecute primero el script de verificación " +
                    "de la Fase 2D2.");
            }

            comando.CommandText = """
                SELECT COUNT(*)
                FROM sys.columns
                WHERE object_id = OBJECT_ID(N'dbo.Trabajador')
                  AND name = N'IdLinea'
                  AND is_nullable = 1;
                """;

            if (Convert.ToInt32(
                    comando.ExecuteScalar()) != 1)
            {
                throw new InvalidOperationException(
                    "dbo.Trabajador.IdLinea debe permitir NULL antes de importar HDC.");
            }
        }

        private static void ValidarRegistroPreparado(
            SqlConnection conexion,
            SqlTransaction transaccion,
            RegistroHdc registro)
        {
            if (string.IsNullOrWhiteSpace(
                    registro.Empleado))
            {
                throw CrearErrorFila(
                    registro,
                    "El número de empleado está vacío.");
            }

            if (registro.Empleado.Trim().Length > 10)
            {
                throw CrearErrorFila(
                    registro,
                    "El número de empleado excede los 10 caracteres admitidos por Trabajador.NoReloj.");
            }

            if (string.IsNullOrWhiteSpace(
                    registro.Nombre))
            {
                throw CrearErrorFila(
                    registro,
                    "El nombre del trabajador está vacío.");
            }

            if (registro.Nombre.Trim().Length > 200)
            {
                throw CrearErrorFila(
                    registro,
                    "El nombre excede los 200 caracteres admitidos por Trabajador.Nombre.");
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

            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM dbo.Linea
                WHERE Id = @IdLinea
                  AND IdPlanta = @IdPlanta;
                """;

            comando.Parameters.Add(
                "@IdLinea",
                SqlDbType.Int).Value =
                    registro.IdLineaSistema;

            comando.Parameters.Add(
                "@IdPlanta",
                SqlDbType.Int).Value =
                    registro.IdPlantaSistema;

            if (Convert.ToInt32(
                    comando.ExecuteScalar()) == 0)
            {
                throw CrearErrorFila(
                    registro,
                    "La línea resuelta no pertenece a la planta del sistema.");
            }
        }

        private static int CrearImportacion(
            SqlConnection conexion,
            SqlTransaction transaccion,
            string rutaArchivo,
            ResultadoAnalisisHdc resultado,
            int nuevos,
            int actualizados,
            int sinCambios,
            int excluidos,
            int ignorados)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO dbo.ImportacionHdc
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
                    SYSDATETIME(),
                    @TotalRegistros,
                    @Nuevos,
                    @Actualizados,
                    @SinCambios,
                    @ConAdvertencias,
                    @Observaciones
                );

                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;

            comando.Parameters.Add(
                "@NombreArchivo",
                SqlDbType.VarChar,
                260).Value =
                    Path.GetFileName(
                        rutaArchivo);

            comando.Parameters.Add(
                "@NombreHoja",
                SqlDbType.VarChar,
                100).Value =
                    string.IsNullOrWhiteSpace(
                        resultado.NombreHoja)
                        ? "HDC"
                        : resultado.NombreHoja.Trim();

            comando.Parameters.Add(
                "@TotalRegistros",
                SqlDbType.Int).Value =
                    resultado.Registros.Count;

            comando.Parameters.Add(
                "@Nuevos",
                SqlDbType.Int).Value =
                    nuevos;

            comando.Parameters.Add(
                "@Actualizados",
                SqlDbType.Int).Value =
                    actualizados;

            comando.Parameters.Add(
                "@SinCambios",
                SqlDbType.Int).Value =
                    sinCambios;

            comando.Parameters.Add(
                "@ConAdvertencias",
                SqlDbType.Int).Value =
                    excluidos;

            comando.Parameters.Add(
                "@Observaciones",
                SqlDbType.VarChar,
                1000).Value =
                    "Importación transaccional HDC desde SQL Server/Azure SQL. " +
                    $"Excluidos por equivalencias/revisión: {excluidos:N0}. " +
                    $"Ignorados: {ignorados:N0}.";

            return Convert.ToInt32(
                comando.ExecuteScalar());
        }

        private static int ObtenerIdTrabajador(
            SqlConnection conexion,
            SqlTransaction transaccion,
            string noReloj)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT TOP (1) Id
                FROM dbo.Trabajador
                WHERE NoReloj = @NoReloj;
                """;

            comando.Parameters.Add(
                "@NoReloj",
                SqlDbType.VarChar,
                10).Value =
                    noReloj.Trim();

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
            SqlConnection conexion,
            SqlTransaction transaccion,
            RegistroHdc registro)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                INSERT INTO dbo.Trabajador
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

                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;

            AgregarParametrosAsignacion(
                comando,
                registro,
                incluirNoReloj: true);

            return Convert.ToInt32(
                comando.ExecuteScalar());
        }

        private static void ActualizarTrabajador(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int idTrabajador,
            RegistroHdc registro)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                UPDATE dbo.Trabajador
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

            comando.Parameters.Add(
                "@IdTrabajador",
                SqlDbType.Int).Value =
                    idTrabajador;

            AgregarParametroEsSinAsignar(
                comando,
                registro);

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

        private static bool TrabajadorCoincideConHdc(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int idTrabajador,
            RegistroHdc registro)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT COUNT(*)
                FROM dbo.Trabajador
                WHERE Id = @IdTrabajador
                  AND UPPER(LTRIM(RTRIM(Nombre))) = @Nombre
                  AND IdTurno = @IdTurno
                  AND IdPlanta = @IdPlanta
                  AND
                  (
                      @EsSinAsignar = 1
                      OR (IdLinea IS NULL AND @IdLinea IS NULL)
                      OR IdLinea = @IdLinea
                  );
                """;

            comando.Parameters.Add(
                "@IdTrabajador",
                SqlDbType.Int).Value =
                    idTrabajador;

            AgregarParametroEsSinAsignar(
                comando,
                registro);

            AgregarParametrosAsignacion(
                comando,
                registro,
                incluirNoReloj: false);

            return Convert.ToInt32(
                comando.ExecuteScalar()) == 1;
        }

        private static void AgregarParametroEsSinAsignar(
            SqlCommand comando,
            RegistroHdc registro)
        {
            comando.Parameters.Add(
                "@EsSinAsignar",
                SqlDbType.Bit).Value =
                    EsAccionLinea(
                        registro,
                        "SIN_ASIGNAR");
        }

        private static void AgregarParametrosAsignacion(
            SqlCommand comando,
            RegistroHdc registro,
            bool incluirNoReloj)
        {
            if (incluirNoReloj)
            {
                comando.Parameters.Add(
                    "@NoReloj",
                    SqlDbType.VarChar,
                    10).Value =
                        registro.Empleado.Trim();
            }

            comando.Parameters.Add(
                "@Nombre",
                SqlDbType.VarChar,
                200).Value =
                    registro.Nombre
                        .Trim()
                        .ToUpperInvariant();

            comando.Parameters.Add(
                "@IdTurno",
                SqlDbType.Int).Value =
                    registro.IdTurnoSistema;

            comando.Parameters.Add(
                "@IdPlanta",
                SqlDbType.Int).Value =
                    registro.IdPlantaSistema;

            SqlParameter parametroLinea =
                comando.Parameters.Add(
                    "@IdLinea",
                    SqlDbType.Int);

            parametroLinea.Value =
                registro.IdLineaSistema > 0
                    ? registro.IdLineaSistema
                    : DBNull.Value;
        }

        private static void GuardarPerfilHdc(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int idTrabajador,
            int idImportacion,
            RegistroHdc registro)
        {
            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                UPDATE dbo.PerfilHdcTrabajador
                SET
                    IdImportacionHdc = @IdImportacionHdc,
                    EmpleadoHdc = @EmpleadoHdc,
                    NombreHdc = @NombreHdc,
                    LocalidadHdc = @LocalidadHdc,
                    TurnoHdc = @TurnoHdc,
                    FechaServicio = @FechaServicio,
                    DepartamentoHdc = @DepartamentoHdc,
                    LineaHdc = @LineaHdc,
                    PuestoHdc = @PuestoHdc,
                    CategoriaHdc = @CategoriaHdc,
                    PositionHdc = @PositionHdc,
                    FunctionHdc = @FunctionHdc,
                    ProcesoHdc = @ProcesoHdc,
                    DptoHdc = @DptoHdc,
                    FechaUltimaImportacion = SYSDATETIME(),
                    EncontradoUltimoHdc = 1
                WHERE IdTrabajador = @IdTrabajador;

                IF @@ROWCOUNT = 0
                BEGIN
                    INSERT INTO dbo.PerfilHdcTrabajador
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
                        SYSDATETIME(),
                        1
                    );
                END;
                """;

            comando.Parameters.Add(
                "@IdTrabajador",
                SqlDbType.Int).Value =
                    idTrabajador;

            comando.Parameters.Add(
                "@IdImportacionHdc",
                SqlDbType.Int).Value =
                    idImportacion;

            AgregarTexto(
                comando,
                "@EmpleadoHdc",
                registro.Empleado,
                50);

            AgregarTexto(
                comando,
                "@NombreHdc",
                registro.Nombre,
                200);

            AgregarTexto(
                comando,
                "@LocalidadHdc",
                registro.LocalidadHdc,
                50);

            AgregarTexto(
                comando,
                "@TurnoHdc",
                registro.TurnoHdc,
                50);

            AgregarFecha(
                comando,
                "@FechaServicio",
                registro.FechaServicio,
                registro);

            AgregarTexto(
                comando,
                "@DepartamentoHdc",
                registro.DepartamentoHdc,
                100);

            AgregarTexto(
                comando,
                "@LineaHdc",
                registro.LineaHdc,
                100);

            AgregarTexto(
                comando,
                "@PuestoHdc",
                registro.PuestoHdc,
                150);

            AgregarTexto(
                comando,
                "@CategoriaHdc",
                registro.CategoriaHdc,
                100);

            AgregarTexto(
                comando,
                "@PositionHdc",
                registro.PositionHdc,
                100);

            AgregarTexto(
                comando,
                "@FunctionHdc",
                registro.FunctionHdc,
                100);

            AgregarTexto(
                comando,
                "@ProcesoHdc",
                registro.ProcesoHdc,
                100);

            AgregarTexto(
                comando,
                "@DptoHdc",
                registro.DptoHdc,
                100);

            comando.ExecuteNonQuery();
        }

        private static void AgregarTexto(
            SqlCommand comando,
            string nombre,
            string? valor,
            int longitud)
        {
            SqlParameter parametro =
                comando.Parameters.Add(
                    nombre,
                    SqlDbType.VarChar,
                    longitud);

            string texto =
                valor?.Trim()
                ?? string.Empty;

            parametro.Value =
                string.IsNullOrWhiteSpace(
                    texto)
                    ? DBNull.Value
                    : texto;
        }

        private static void AgregarFecha(
            SqlCommand comando,
            string nombre,
            string? valor,
            RegistroHdc registro)
        {
            SqlParameter parametro =
                comando.Parameters.Add(
                    nombre,
                    SqlDbType.Date);

            string texto =
                valor?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(
                    texto))
            {
                parametro.Value =
                    DBNull.Value;

                return;
            }

            string[] formatos =
            {
                "dd/MM/yyyy",
                "d/M/yyyy",
                "yyyy-MM-dd",
                "M/d/yyyy",
                "MM/dd/yyyy"
            };

            if (DateTime.TryParseExact(
                    texto,
                    formatos,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime fecha) ||
                DateTime.TryParse(
                    texto,
                    CultureInfo.GetCultureInfo("es-MX"),
                    DateTimeStyles.None,
                    out fecha))
            {
                parametro.Value =
                    fecha.Date;

                return;
            }

            throw CrearErrorFila(
                registro,
                "La fecha de servicio no tiene un formato reconocible: " +
                texto + ".");
        }

        private static bool ExisteId(
            SqlConnection conexion,
            SqlTransaction transaccion,
            string tabla,
            int id)
        {
            string nombreTabla =
                tabla switch
                {
                    "Planta" => "dbo.Planta",
                    "Turno" => "dbo.Turno",
                    _ => throw new ArgumentOutOfRangeException(
                        nameof(tabla))
                };

            using SqlCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText =
                $"SELECT COUNT(*) FROM {nombreTabla} WHERE Id = @Id;";

            comando.Parameters.Add(
                "@Id",
                SqlDbType.Int).Value =
                    id;

            return Convert.ToInt32(
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
    }
}
