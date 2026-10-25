using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ICertificadorRepository
    {
        DataTable Consultar(string textoBuscar);

        Mensaje Guardar(Certificador certificador);

        Mensaje Borrar(int id);
    }
}