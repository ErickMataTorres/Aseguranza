namespace Aseguranza.Clases
{
    public sealed class RegistroHdc
    {
        public int NumeroFilaExcel { get; set; }

        public string EstadoArchivo { get; set; } = "Listo";
        public string Estado { get; set; } = "Pendiente";
        public string Observacion { get; set; } = string.Empty;

        public string Empleado { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string LocalidadHdc { get; set; } = string.Empty;
        public string TurnoHdc { get; set; } = string.Empty;
        public string FechaServicio { get; set; } = string.Empty;
        public string DepartamentoHdc { get; set; } = string.Empty;
        public string LineaHdc { get; set; } = string.Empty;
        public string PuestoHdc { get; set; } = string.Empty;
        public string CategoriaHdc { get; set; } = string.Empty;
        public string PositionHdc { get; set; } = string.Empty;
        public string FunctionHdc { get; set; } = string.Empty;
        public string ProcesoHdc { get; set; } = string.Empty;
        public string DptoHdc { get; set; } = string.Empty;

        // Información resuelta contra los catálogos.
        public bool ExisteTrabajador { get; set; }
        public int IdTrabajadorSistema { get; set; }

        public int IdPlantaSistema { get; set; }
        public string PlantaSistema { get; set; } = string.Empty;

        public int IdTurnoSistema { get; set; }
        public string TurnoSistema { get; set; } = string.Empty;

        public string AccionLinea { get; set; } = string.Empty;
        public int IdLineaSistema { get; set; }
        public string LineaSistema { get; set; } = string.Empty;

        // Información actual, solo si el trabajador ya existe.
        public string NombreActualSistema { get; set; } = string.Empty;

        public int IdPlantaActualSistema { get; set; }
        public string PlantaActualSistema { get; set; } = string.Empty;

        public int IdTurnoActualSistema { get; set; }
        public string TurnoActualSistema { get; set; } = string.Empty;

        public int IdLineaActualSistema { get; set; }
        public string LineaActualSistema { get; set; } = string.Empty;

        public string CambiosDetectados { get; set; } = string.Empty;
    }
}
