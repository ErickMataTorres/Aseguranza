using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Certificador
    {
        public int Id { get; set; }

        public int IdTrabajador { get; set; }

        public string? NoReloj { get; set; }

        public string? Nombre { get; set; }

        public string? RutaFoto { get; set; }

        public int IdTurno { get; set; }

        public string? NombreTurno { get; set; }

        public int IdPlanta { get; set; }

        public string? NombrePlanta { get; set; }

        public int IdLinea { get; set; }

        public string? NombreLinea { get; set; }

        public static DataTable ConsultarCertificadores(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearCertificadorRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public Mensaje GuardarCertificador()
        {
            return RepositorioFactory
                .CrearCertificadorRepository()
                .Guardar(this);
        }

        public static Mensaje BorrarCertificador(
            int id)
        {
            return RepositorioFactory
                .CrearCertificadorRepository()
                .Borrar(id);
        }
    }
}