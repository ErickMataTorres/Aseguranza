using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Linea
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int IdPlanta { get; set; }

        public string? NombrePlanta { get; set; }

        public static DataTable ConsultarLineas(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static DataTable ConsultarLineasPorPlanta(
            int idPlanta)
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .ConsultarPorPlanta(idPlanta);
        }

        public static Mensaje BorrarLinea(int id)
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .Borrar(id);
        }

        public Mensaje GuardarLinea()
        {
            return RepositorioFactory
                .CrearLineaRepository()
                .Guardar(this);
        }
    }
}