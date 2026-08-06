using Aseguranza.Clases;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface ITrabajadorRepository
    {
        Trabajador? ConsultarPorNumeroReloj(
            string noReloj);

        DataTable Consultar(
            string textoBuscar);

        DataTable ConsultarEstadoCertificacion(
            string mostrarPor,
            string textoBuscar);

        Mensaje Guardar(
            Trabajador trabajador);

        Mensaje Borrar(
            int id);
    }
}