using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Localidad
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public static DataTable ConsultarLocalidades(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearLocalidadRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarLocalidad(int id)
        {
            return RepositorioFactory
                .CrearLocalidadRepository()
                .Borrar(id);
        }

        public Mensaje GuardarLocalidad()
        {
            return RepositorioFactory
                .CrearLocalidadRepository()
                .Guardar(this);
        }
    }
}