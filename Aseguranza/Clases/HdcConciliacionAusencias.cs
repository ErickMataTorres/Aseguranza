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
        IReadOnlyList<PerfilConciliacionHdc> Candidatos,
        IReadOnlyList<(PerfilConciliacionHdc Perfil, RegistroHdc Registro)> FueraDeAlcance);

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
                    Array.Empty<PerfilConciliacionHdc>(),
                    Array.Empty<(PerfilConciliacionHdc, RegistroHdc)>());
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
            var porEmpleado = crudos.GroupBy(r => Normalizar(r.Empleado),
                StringComparer.OrdinalIgnoreCase).ToDictionary(g => g.Key,
                g => g.ToArray(), StringComparer.OrdinalIgnoreCase);
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

            var fuera = perfiles
                .Where(p => p.Activo && p.IdImportacionHdc.GetValueOrDefault() > 0 &&
                    Normalizar(p.NoReloj).Length is > 0 and <= 10 &&
                    Normalizar(p.EmpleadoHdc) == Normalizar(p.NoReloj))
                .Select(p => (Perfil: p, Filas: porEmpleado.GetValueOrDefault(Normalizar(p.NoReloj))))
                .Where(x => x.Filas is { Length: 1 } &&
                    x.Filas[0].EstadoArchivo == "Listo" &&
                    Normalizar(x.Filas[0].LocalidadHdc).Length > 0 &&
                    Normalizar(x.Filas[0].LocalidadHdc) != Normalizar(x.Perfil.LocalidadHdc) &&
                    !estado.Plantas.Any(m =>
                        Normalizar(m.Codigo) == Normalizar(x.Filas[0].LocalidadHdc) &&
                        m.Activo && Normalizar(m.Accion) == "MAPEAR" &&
                        m.IdPlanta.GetValueOrDefault() > 0 &&
                        !string.IsNullOrWhiteSpace(m.Nombre)) &&
                    !estado.Plantas.Any(m =>
                        Normalizar(m.Codigo) == Normalizar(x.Filas[0].LocalidadHdc) &&
                        Normalizar(m.Accion) == "IGNORAR"))
                .Select(x => (x.Perfil, Registro: x.Filas![0]))
                .ToArray();

            string serializado = JsonSerializer.Serialize(new { perfiles, plantas,
                EquivalenciasDestino = estado.Plantas, Fuera = fuera.Select(x => new {
                    x.Perfil.IdTrabajador, x.Registro.Empleado,
                    x.Registro.LocalidadHdc }).ToArray() });
            string huellaEstado = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(serializado)));
            return new PlanConciliacionHdc(
                string.Join("|", alcance), huellaEstado, candidatos, fuera);
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
            foreach (var (perfil, registro) in plan.FueraDeAlcance)
            {
                registro.Estado = "Fuera de alcance";
                registro.ExisteTrabajador = true;
                registro.IdTrabajadorSistema = perfil.IdTrabajador;
                registro.ActivoActualSistema = true;
                registro.CambiosDetectados = "Estado activo, localidad HDC";
                registro.Observacion = "Cambio de localidad " +
                    Normalizar(perfil.LocalidadHdc) + " → " +
                    Normalizar(registro.LocalidadHdc) + " sin equivalencia.";
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
            int[] fueraAprobados = resultado.Registros
                .Where(r => r.Estado == "Fuera de alcance")
                .Select(r => r.IdTrabajadorSistema).OrderBy(id => id).ToArray();
            int[] fueraActuales = plan.FueraDeAlcance
                .Select(x => x.Perfil.IdTrabajador).OrderBy(id => id).ToArray();
            if (resultado.AlcanceAnalizado != plan.Alcance ||
                resultado.HuellaEstadoConciliacion != plan.HuellaEstado ||
                resultado.Inactivados != actuales.Length ||
                resultado.FueraDeAlcance != fueraActuales.Length ||
                !aprobados.SequenceEqual(actuales) ||
                !fueraAprobados.SequenceEqual(fueraActuales))
            {
                throw new InvalidOperationException(
                    "Cambió el alcance o la población HDC desde el análisis. Analice de nuevo antes de importar.");
            }
        }
    }
}
