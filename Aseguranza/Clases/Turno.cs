using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Turno
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public static DataTable ConsultarTurnos(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearTurnoRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarTurno(int id)
        {
            return RepositorioFactory
                .CrearTurnoRepository()
                .Borrar(id);
        }

        public Mensaje GuardarTurno()
        {
            return RepositorioFactory
                .CrearTurnoRepository()
                .Guardar(this);
        }
    }
}