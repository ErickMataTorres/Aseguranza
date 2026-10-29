using System.Collections.Generic;

namespace Aseguranza.Clases
{
    public sealed record LocalidadHdcOpcion(
        string Codigo, string Planta, bool Disponible)
    {
        public override string ToString() =>
            Codigo + " → " + (Disponible ? Planta : "sin equivalencia válida");
    }

    public sealed record PerfilConciliacionHdc(
        int IdTrabajador, string NoReloj, string Nombre, bool Activo,
        string EmpleadoHdc, string LocalidadHdc,
        int? IdImportacionHdc, bool EncontradoUltimoHdc,
        int IdPlanta, int IdTurno, int? IdLinea);

    public sealed record PlantaConciliacionHdc(
        string Codigo, string Accion, int? IdPlanta, bool Activo, string Nombre);

    public sealed class EstadoConciliacionHdc
    {
        public List<PerfilConciliacionHdc> Perfiles { get; } = new();
        public List<PlantaConciliacionHdc> Plantas { get; } = new();
    }
}
