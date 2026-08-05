using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Planta
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public static DataTable ConsultarPlantas(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearPlantaRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarPlanta(int id)
        {
            return RepositorioFactory
                .CrearPlantaRepository()
                .Borrar(id);
        }

        public Mensaje GuardarPlanta()
        {
            return RepositorioFactory
                .CrearPlantaRepository()
                .Guardar(this);
        }
    }
}