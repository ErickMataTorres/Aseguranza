using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Proceso
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public int VigenciaMeses { get; set; }

        public static DataTable ConsultarProcesos(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearProcesoRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarProceso(int id)
        {
            return RepositorioFactory
                .CrearProcesoRepository()
                .Borrar(id);
        }

        public Mensaje GuardarProceso()
        {
            return RepositorioFactory
                .CrearProcesoRepository()
                .Guardar(this);
        }
    }
}