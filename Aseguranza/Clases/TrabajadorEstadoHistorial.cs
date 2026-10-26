using System;

namespace Aseguranza.Clases
{
    public sealed class TrabajadorEstadoHistorial
    {
        public int Id { get; set; }
        public int IdTrabajador { get; set; }
        public bool? ActivoAnterior { get; set; }
        public bool ActivoNuevo { get; set; }
        public DateTime FechaCambioUtc { get; set; }
        public string Origen { get; set; } = string.Empty;
        public int? IdImportacionHdc { get; set; }
        public string? Motivo { get; set; }

        public static void ValidarCambio(
            int idTrabajador,
            string origen,
            int? idImportacionHdc,
            string? motivo)
        {
            if (idTrabajador <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idTrabajador));
            }

            if (origen != "HDC" && origen != "MANUAL")
            {
                throw new ArgumentException(
                    "El origen del cambio de estado no es válido.",
                    nameof(origen));
            }

            if ((origen == "HDC") != idImportacionHdc.HasValue ||
                idImportacionHdc is <= 0)
            {
                throw new ArgumentException(
                    "El identificador de importación debe existir solo para cambios HDC.",
                    nameof(idImportacionHdc));
            }

            if (motivo?.Trim().Length > 500)
            {
                throw new ArgumentException(
                    "El motivo excede los 500 caracteres.",
                    nameof(motivo));
            }
        }
    }
}
