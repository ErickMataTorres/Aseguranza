using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface IPlantaRepository
    {
        DataTable Consultar(string textoBuscar);

        Mensaje Guardar(Planta planta);

        Mensaje Borrar(int id);
    }
}