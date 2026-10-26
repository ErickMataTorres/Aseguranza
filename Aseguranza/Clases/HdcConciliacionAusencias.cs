using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Aseguranza.Clases
{
    public sealed record PlanConciliacionHdc(
        string Alcance, string HuellaEstado,
        IReadOnlyList<PerfilConciliacionHdc> Candidatos);

    public static class HdcConciliacionAusencias
    {
        public const string EstadoArchivoAusente = "AusenteHdc";

        public static string Normalizar(string? texto) =>
            (texto ?? string.Empty).Trim().ToUpperInvariant();

        public static FileStream AbrirFuenteVerificada(
            string rutaArchivo, ResultadoAnalisisHdc resultado)
        {
            if (string.IsNullOrWhiteSpace(resultado.HuellaSha256) ||
                resultado.HuellaSha256.Length != 64)
            {
                throw new InvalidOperationException(
                    "No hay huella del archivo analizado. Analice el HDC de nuevo.");
            }

            FileStream archivo = new FileStream(
                rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                string huella = Convert.ToHexString(SHA256.HashData(archivo));
                if (!string.Equals(huella, resultado.HuellaSha256,
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "El archivo HDC cambió después del análisis. Analícelo de nuevo.");
                }
                return archivo;
            }
            catch
            {
                archivo.Dispose();
                throw;
            }
        }

        public static PlanConciliacionHdc CalcularPlan(
            ResultadoAnalisisHdc resultado, EstadoConciliacionHdc estado)
        {
            if (!resultado.ConciliarAusencias)
            {
                return new PlanConciliacionHdc(
                    string.Empty, string.Empty,
                    Array.Empty<PerfilConciliacionHdc>());
            }

            string[] alcance = resultado.LocalidadesCompletas
                .Select(Normalizar)
                .Where(codigo => codigo.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(codigo => codigo, StringComparer.Ordinal)
                .ToArray();
            if (alcance.Length == 0)
            {
                throw new InvalidOperationException(
                    "Seleccione al menos una localidad HDC con población completa.");
            }

            var crudos = resultado.Registros
                .Where(r => r.EstadoArchivo != EstadoArchivoAusente).ToList();
            var codigosArchivo = crudos.Select(r => Normalizar(r.LocalidadHdc))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (string codigo in alcance)
            {
                if (!codigosArchivo.Contains(codigo))
                {
                    throw new InvalidOperationException(
                        "La localidad HDC " + codigo +
                        " no está en el archivo analizado.");
                }

                bool mapeada = estado.Plantas.Any(p =>
                    Normalizar(p.Codigo) == codigo &&
                    p.Activo && Normalizar(p.Accion) == "MAPEAR" &&
                    p.IdPlanta.GetValueOrDefault() > 0 &&
                    !string.IsNullOrWhiteSpace(p.Nombre));
                if (!mapeada)
                {
                    throw new InvalidOperationException(
                        "La localidad HDC " + codigo +
                        " no tiene una equivalencia de planta válida para conciliar ausencias.");
                }
            }

            var seleccionadas = alcance.ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (crudos.Any(r => string.IsNullOrWhiteSpace(r.Empleado) ||
                                string.IsNullOrWhiteSpace(r.LocalidadHdc)))
            {
                throw new InvalidOperationException(
                    "El HDC contiene empleado o localidad vacíos. Corrija el archivo antes de conciliar ausencias.");
            }

            foreach (var grupo in crudos.GroupBy(
                r => Normalizar(r.Empleado), StringComparer.OrdinalIgnoreCase))
            {
                if (grupo.Count() > 1 &&
                    grupo.Any(r => seleccionadas.Contains(Normalizar(r.LocalidadHdc))))
                {
                    throw new InvalidOperationException(
                        "El empleado " + grupo.Key +
                        " aparece más de una vez y afecta una localidad completa. Corrija y reanalice el HDC.");
                }
            }

            if (crudos.Any(r =>
                    seleccionadas.Contains(Normalizar(r.LocalidadHdc)) &&
                    Normalizar(r.Empleado).Length > 10))
            {
                throw new InvalidOperationException(
                    "Una localidad completa contiene un número de empleado inválido. Corrija y reanalice el HDC.");
            }

            var presentes = crudos.Select(r => Normalizar(r.Empleado))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var perfiles = estado.Perfiles
                .Where(p => seleccionadas.Contains(Normalizar(p.LocalidadHdc)))
                .OrderBy(p => p.IdTrabajador)
                .ToArray();
            var plantas = estado.Plantas
                .Where(p => seleccionadas.Contains(Normalizar(p.Codigo)))
                .OrderBy(p => Normalizar(p.Codigo), StringComparer.Ordinal)
                .ToArray();

            var candidatos = perfiles
                .Where(p => p.Activo &&
                    p.IdImportacionHdc.GetValueOrDefault() > 0 &&
                    Normalizar(p.NoReloj).Length is > 0 and <= 10 &&
                    Normalizar(p.EmpleadoHdc) == Normalizar(p.NoReloj) &&
                    !presentes.Contains(Normalizar(p.NoReloj)))
                .ToArray();

            string serializado = JsonSerializer.Serialize(new { perfiles, plantas });
            string huellaEstado = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(serializado)));
            return new PlanConciliacionHdc(
                string.Join("|", alcance), huellaEstado, candidatos);
        }

        public static void AplicarAnalisis(
            ResultadoAnalisisHdc resultado, EstadoConciliacionHdc estado)
        {
            resultado.Registros.RemoveAll(
                r => r.EstadoArchivo == EstadoArchivoAusente);
            if (!resultado.ConciliarAusencias ||
                resultado.LocalidadesCompletas.Count == 0)
            {
                resultado.AlcanceAnalizado = string.Empty;
                resultado.HuellaEstadoConciliacion = string.Empty;
                return;
            }

            PlanConciliacionHdc plan = CalcularPlan(resultado, estado);
            resultado.AlcanceAnalizado = plan.Alcance;
            resultado.HuellaEstadoConciliacion = plan.HuellaEstado;
            foreach (PerfilConciliacionHdc perfil in plan.Candidatos)
            {
                resultado.Registros.Add(new RegistroHdc
                {
                    EstadoArchivo = EstadoArchivoAusente,
                    Estado = "Inactivar",
                    Empleado = perfil.NoReloj,
                    Nombre = perfil.Nombre,
                    LocalidadHdc = perfil.LocalidadHdc,
                    IdTrabajadorSistema = perfil.IdTrabajador,
                    ActivoActualSistema = true,
                    ExisteTrabajador = true,
                    CambiosDetectados = "Estado activo",
                    Observacion = "Ausente de localidad HDC declarada con población completa."
                });
            }
        }

        public static void ValidarPlanImportacion(
            ResultadoAnalisisHdc resultado, PlanConciliacionHdc plan)
        {
            int[] aprobados = resultado.Registros
                .Where(r => r.EstadoArchivo == EstadoArchivoAusente &&
                            r.Estado == "Inactivar")
                .Select(r => r.IdTrabajadorSistema).OrderBy(id => id).ToArray();
            int[] actuales = plan.Candidatos
                .Select(p => p.IdTrabajador).OrderBy(id => id).ToArray();
            if (resultado.AlcanceAnalizado != plan.Alcance ||
                resultado.HuellaEstadoConciliacion != plan.HuellaEstado ||
                resultado.Inactivados != actuales.Length ||
                !aprobados.SequenceEqual(actuales))
            {
                throw new InvalidOperationException(
                    "Cambió el alcance o la población HDC desde el análisis. Analice de nuevo antes de importar.");
            }
        }
    }
}
