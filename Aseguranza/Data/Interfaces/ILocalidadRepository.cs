using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ILocalidadRepository
    {
        DataTable Consultar(string textoBuscar);

        Mensaje Guardar(Localidad localidad);

        Mensaje Borrar(int id);
    }
}