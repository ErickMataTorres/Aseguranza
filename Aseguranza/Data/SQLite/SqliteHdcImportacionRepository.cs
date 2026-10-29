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
        public EstadoConciliacionHdc ConsultarEstadoConciliacion()
        {
            using SqliteConnection conexion = ConexionSqlite.Crear();
            conexion.Open();
            using SqliteTransaction transaccion = conexion.BeginTransaction();
            EstadoConciliacionHdc estado =
                CargarEstadoConciliacion(conexion, transaccion);
            transaccion.Commit();
            return estado;
        }

        private static EstadoConciliacionHdc CargarEstadoConciliacion(
            SqliteConnection conexion, SqliteTransaction? transaccion)
        {
            EstadoConciliacionHdc estado = new();
            using SqliteCommand comando = conexion.CreateCommand();
            comando.Transaction = transaccion;
            comando.CommandText = """
                SELECT T.Id, T.NoReloj, T.Nombre, T.Activo,
                       P.EmpleadoHdc, P.LocalidadHdc, P.IdImportacionHdc,
                       P.EncontradoUltimoHdc,
                       T.IdPlanta, T.IdTurno, T.IdLinea
                FROM Trabajador AS T
                INNER JOIN PerfilHdcTrabajador AS P ON P.IdTrabajador = T.Id
                ORDER BY T.Id;
                """;
            using (SqliteDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    estado.Perfiles.Add(new PerfilConciliacionHdc(
                        lector.GetInt32(0), lector.GetString(1),
                        lector.GetString(2), lector.GetInt64(3) != 0,
                        lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
                        lector.IsDBNull(5) ? string.Empty : lector.GetString(5),
                        lector.IsDBNull(6) ? null : lector.GetInt32(6),
                        lector.GetInt64(7) != 0,
                        lector.GetInt32(8), lector.GetInt32(9),
                        lector.IsDBNull(10) ? null : lector.GetInt32(10)));
                }
            }
            comando.CommandText = """
                SELECT E.CodigoLocalidadHdc, E.Accion, E.IdPlanta,
                       E.Activo, COALESCE(P.Nombre, '')
                FROM EquivalenciaPlantaHdc AS E
                LEFT JOIN Planta AS P ON P.Id = E.IdPlanta
                ORDER BY E.CodigoLocalidadHdc;
                """;
            using (SqliteDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    estado.Plantas.Add(new PlantaConciliacionHdc(
                        lector.GetString(0), lector.GetString(1),
                        lector.IsDBNull(2) ? null : lector.GetInt32(2),
                        lector.GetInt64(3) != 0, lector.GetString(4)));
                }
            }
            return estado;
        }

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

            using FileStream fuente =
                HdcConciliacionAusencias.AbrirFuenteVerificada(rutaArchivo, resultado);

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

            int reactivados =
                preparados.Count(
                    item => EstadoEs(item, "Reactivar"));
            int inactivados = resultado.ConciliarAusencias
                ? resultado.Inactivados : 0;
            int fueraDeAlcance = resultado.ConciliarAusencias
                ? resultado.FueraDeAlcance : 0;
            int transferidos = preparados.Count(item => EstadoEs(item, "Transferir"));

            int sinCambios =
                preparados.Count(
                    item => EstadoEs(item, "Sin cambios"));

            int ignorados =
                resultado.Registros.Count(
                    item => EstadoEs(item, "Ignorar"));

            int excluidos =
                Math.Max(
                    0,
                    resultado.TotalRegistros -
                    preparados.Count -
                    fueraDeAlcance -
                    ignorados);

            if (preparados.Count == 0 && inactivados == 0 && fueraDeAlcance == 0)
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
                        reactivados,
                        inactivados + fueraDeAlcance,
                        sinCambios,
                        excluidos,
                        ignorados);

                PlanConciliacionHdc plan = resultado.ConciliarAusencias
                    ? HdcConciliacionAusencias.CalcularPlan(
                        resultado, CargarEstadoConciliacion(conexion, transaccion))
                    : new PlanConciliacionHdc(
                        string.Empty, string.Empty,
                        Array.Empty<PerfilConciliacionHdc>(),
                        Array.Empty<(PerfilConciliacionHdc, RegistroHdc)>());
                HdcConciliacionAusencias.ValidarPlanImportacion(resultado, plan);

                GuardarAlcance(conexion, transaccion, idImportacion, resultado);

                foreach (RegistroHdc registro
                         in preparados)
                {
                    ValidarRegistroPreparado(
                        conexion,
                        transaccion,
                        registro);

                    (int idTrabajador, bool activoActual) =
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

                        if (idTrabajador != registro.IdTrabajadorSistema ||
                            activoActual != registro.ActivoActualSistema)
                        {
                            throw CrearErrorFila(
                                registro,
                                "El trabajador o su estado Activo cambió después del análisis. " +
                                "Vuelva a analizar antes de importar.");
                        }

                        if (EstadoEs(registro, "Transferir"))
                        {
                            ValidarTransferenciaActual(
                                conexion, transaccion, idTrabajador, registro);
                        }

                        if (EstadoEs(
                                registro,
                                "Actualizar") || EstadoEs(registro, "Transferir") ||
                            (EstadoEs(registro, "Reactivar") &&
                             !string.IsNullOrWhiteSpace(registro.CambiosDetectados)))
                        {
                            ActualizarTrabajador(
                                conexion,
                                transaccion,
                                idTrabajador,
                                registro);
                        }
                        else if ((EstadoEs(registro, "Sin cambios") ||
                                  EstadoEs(registro, "Reactivar")) &&
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

                        if (EstadoEs(registro, "Reactivar") &&
                            !SqliteTrabajadorRepository.CambiarEstadoEnTransaccion(
                                conexion, transaccion, idTrabajador,
                                true, "HDC", idImportacion))
                        {
                            throw CrearErrorFila(
                                registro,
                                "No se pudo reactivar al trabajador. Vuelva a analizar el HDC.");
                        }
                    }

                    GuardarPerfilHdc(
                        conexion,
                        transaccion,
                        idTrabajador,
                        idImportacion,
                        registro);
                }

                InactivarCandidatos(
                    conexion, transaccion, idImportacion, plan);
                InactivarFueraDeAlcance(
                    conexion, transaccion, idImportacion, plan);

                transaccion.Commit();

                return new ResultadoImportacionHdc
                {
                    IdImportacion =
                        idImportacion,

                    TotalRegistrosArchivo =
                        resultado.TotalRegistros,

                    NuevosInsertados =
                        nuevos,

                    Actualizados =
                        actualizados,

                    Reactivados =
                        reactivados,
                    Inactivados =
                        inactivados,
                    Transferidos = transferidos,
                    FueraDeAlcance = fueraDeAlcance,

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
                EstadoEs(registro, "Transferir") ||
                EstadoEs(registro, "Reactivar") ||
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
            int reactivados,
            int inactivados,
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
                    ConciliarAusencias,
                    HuellaSha256,
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
                    @ConciliarAusencias,
                    @HuellaSha256,
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
                resultado.TotalRegistros);

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
                "@ConciliarAusencias", resultado.ConciliarAusencias ? 1 : 0);
            comando.Parameters.AddWithValue(
                "@HuellaSha256", resultado.HuellaSha256);

            comando.Parameters.AddWithValue(
                "@Observaciones",
                "Importación transaccional HDC. " +
                $"Reactivados: {reactivados:N0}. " +
                $"Inactivados: {inactivados:N0}. " +
                $"Excluidos por equivalencias/revisión: {excluidos:N0}. " +
                $"Ignorados: {ignorados:N0}.");

            comando.ExecuteNonQuery();

            return ObtenerUltimoId(
                conexion,
                transaccion);
        }

        private static void GuardarAlcance(
            SqliteConnection conexion, SqliteTransaction transaccion,
            int idImportacion, ResultadoAnalisisHdc resultado)
        {
            if (!resultado.ConciliarAusencias) return;
            foreach (string codigo in resultado.LocalidadesCompletas)
            {
                using SqliteCommand comando = conexion.CreateCommand();
                comando.Transaction = transaccion;
                comando.CommandText = """
                    INSERT INTO ImportacionHdcAlcance
                        (IdImportacionHdc, CodigoLocalidadHdc)
                    VALUES (@IdImportacion, @Codigo);
                    """;
                comando.Parameters.AddWithValue("@IdImportacion", idImportacion);
                comando.Parameters.AddWithValue(
                    "@Codigo", HdcConciliacionAusencias.Normalizar(codigo));
                comando.ExecuteNonQuery();
            }
        }

        private static void ValidarTransferenciaActual(
            SqliteConnection conexion, SqliteTransaction transaccion,
            int idTrabajador, RegistroHdc registro)
        {
            using SqliteCommand comando = conexion.CreateCommand();
            comando.Transaction = transaccion;
            comando.CommandText = """
                SELECT T.Nombre, T.IdPlanta, T.IdTurno, T.IdLinea,
                       P.LocalidadHdc, E.IdPlanta, E.Accion, E.Activo
                FROM Trabajador AS T
                INNER JOIN PerfilHdcTrabajador AS P ON P.IdTrabajador = T.Id
                LEFT JOIN EquivalenciaPlantaHdc AS E
                    ON UPPER(TRIM(E.CodigoLocalidadHdc)) = @Localidad
                WHERE T.Id = @Id;
                """;
            comando.Parameters.AddWithValue("@Id", idTrabajador);
            comando.Parameters.AddWithValue("@Localidad",
                HdcConciliacionAusencias.Normalizar(registro.LocalidadHdc));
            using SqliteDataReader lector = comando.ExecuteReader();
            if (!lector.Read() ||
                !string.Equals(lector.GetString(0).Trim(),
                    registro.NombreActualSistema.Trim(), StringComparison.OrdinalIgnoreCase) ||
                lector.GetInt32(1) != registro.IdPlantaActualSistema ||
                lector.GetInt32(2) != registro.IdTurnoActualSistema ||
                (lector.IsDBNull(3) ? 0 : lector.GetInt32(3)) != registro.IdLineaActualSistema ||
                HdcConciliacionAusencias.Normalizar(lector.GetString(4)) !=
                    HdcConciliacionAusencias.Normalizar(registro.LocalidadAnteriorHdc) ||
                lector.IsDBNull(5) || lector.GetInt32(5) != registro.IdPlantaSistema ||
                lector.IsDBNull(6) || lector.GetString(6).Trim().ToUpperInvariant() != "MAPEAR" ||
                lector.GetInt64(7) == 0)
                throw CrearErrorFila(registro,
                    "Cambió el trabajador, su localidad HDC o la equivalencia de planta. Vuelva a analizar.");
        }

        private static void InactivarFueraDeAlcance(
            SqliteConnection conexion, SqliteTransaction transaccion,
            int idImportacion, PlanConciliacionHdc plan)
        {
            foreach (var (perfil, registro) in plan.FueraDeAlcance)
            {
                string motivo = "Cambio de localidad " +
                    HdcConciliacionAusencias.Normalizar(perfil.LocalidadHdc) + " -> " +
                    HdcConciliacionAusencias.Normalizar(registro.LocalidadHdc) +
                    " sin equivalencia; fuera del alcance administrado.";
                if (!SqliteTrabajadorRepository.CambiarEstadoEnTransaccion(
                    conexion, transaccion, perfil.IdTrabajador,
                    false, "HDC", idImportacion, motivo))
                    throw CrearErrorFila(registro,
                        "El estado del trabajador cambió. Vuelva a analizar el HDC.");
                GuardarPerfilHdc(conexion, transaccion, perfil.IdTrabajador,
                    idImportacion, registro);
            }
        }

        private static void InactivarCandidatos(
            SqliteConnection conexion, SqliteTransaction transaccion,
            int idImportacion, PlanConciliacionHdc plan)
        {
            foreach (PerfilConciliacionHdc perfil in plan.Candidatos)
            {
                string motivo = "Ausente de la población HDC completa de " +
                    HdcConciliacionAusencias.Normalizar(perfil.LocalidadHdc) + ".";
                if (!SqliteTrabajadorRepository.CambiarEstadoEnTransaccion(
                    conexion, transaccion, perfil.IdTrabajador,
                    false, "HDC", idImportacion, motivo))
                {
                    throw new InvalidOperationException(
                        "El estado del trabajador cambió durante la conciliación HDC.");
                }
                using SqliteCommand comando = conexion.CreateCommand();
                comando.Transaction = transaccion;
                comando.CommandText = """
                    UPDATE PerfilHdcTrabajador
                    SET EncontradoUltimoHdc = 0
                    WHERE IdTrabajador = @Id;
                    """;
                comando.Parameters.AddWithValue("@Id", perfil.IdTrabajador);
                if (comando.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "Cambió el perfil HDC durante la conciliación. Se revirtió la importación.");
                }
            }
        }

        private static (int Id, bool Activo) ObtenerIdTrabajador(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            string noReloj)
        {
            using SqliteCommand comando =
                conexion.CreateCommand();

            comando.Transaction =
                transaccion;

            comando.CommandText = """
                SELECT Id, Activo
                FROM Trabajador
                WHERE NoReloj = @NoReloj COLLATE NOCASE
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@NoReloj",
                noReloj.Trim());

            using SqliteDataReader lector = comando.ExecuteReader();
            if (!lector.Read())
            {
                return (0, false);
            }

            return (lector.GetInt32(0), lector.GetInt64(1) != 0);
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

        private static bool TrabajadorCoincideConHdc(
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
                SELECT
                    Nombre,
                    IdTurno,
                    IdPlanta,
                    IdLinea
                FROM Trabajador
                WHERE Id = @IdTrabajador
                LIMIT 1;
                """;

            comando.Parameters.AddWithValue(
                "@IdTrabajador",
                idTrabajador);

            using SqliteDataReader lector =
                comando.ExecuteReader();

            if (!lector.Read())
            {
                return false;
            }

            string nombreActual =
                lector.IsDBNull(0)
                    ? string.Empty
                    : lector.GetString(0).Trim();

            int idTurnoActual =
                lector.GetInt32(1);

            int idPlantaActual =
                lector.GetInt32(2);

            int idLineaActual =
                lector.IsDBNull(3)
                    ? 0
                    : lector.GetInt32(3);

            bool lineaCoincide =
                EsAccionLinea(
                    registro,
                    "SIN_ASIGNAR") ||
                idLineaActual == registro.IdLineaSistema;

            return
                string.Equals(
                    nombreActual,
                    registro.Nombre.Trim(),
                    StringComparison.OrdinalIgnoreCase) &&
                idTurnoActual == registro.IdTurnoSistema &&
                idPlantaActual == registro.IdPlantaSistema &&
                lineaCoincide;
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
