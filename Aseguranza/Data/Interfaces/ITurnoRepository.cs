using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ITurnoRepository
    {
        DataTable Consultar(string textoBuscar);

        Mensaje Guardar(Turno turno);

        Mensaje Borrar(int id);
    }
}