using System.Collections.Generic;

namespace Aseguranza.Clases
{
    public sealed class ResultadoAnalisisHdc
    {
        public string NombreHoja { get; set; } = "HDC";

        public List<RegistroHdc> Registros { get; } =
            new List<RegistroHdc>();

        public int TotalRegistros { get; set; }
        public int Nuevos { get; set; }
        public int Actualizados { get; set; }
        public int SinCambios { get; set; }

        // Compatibilidad con el análisis anterior.
        public int Existentes { get; set; }

        public int SinEquivalenciaPlanta { get; set; }
        public int SinEquivalenciaTurno { get; set; }
        public int SinEquivalenciaLinea { get; set; }

        // Conteo informativo; SIN_ASIGNAR ya no es pendiente.
        public int SinAsignar { get; set; }

        public int Ignorados { get; set; }
        public int RegistrosRevisar { get; set; }
        public int RegistrosDuplicados { get; set; }

        public int LocalidadesDistintas { get; set; }
        public int TurnosDistintos { get; set; }
        public int LineasHdcDistintas { get; set; }
    }
}
