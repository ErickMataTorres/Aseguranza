using Aseguranza.Data;
using System.Data;

namespace Aseguranza.Clases
{
    public class Trabajador
    {
        public int Id { get; set; }

        public string? NoReloj { get; set; }

        public string? Nombre { get; set; }

        public string? RutaFoto { get; set; }

        public int IdLocalidad { get; set; }

        public string? NombreLocalidad { get; set; }

        public int IdTurno { get; set; }

        public string? NombreTurno { get; set; }

        public int IdPlanta { get; set; }

        public string? NombrePlanta { get; set; }

        public int IdLinea { get; set; }

        public string? NombreLinea { get; set; }

        public static Trabajador? ConsultarTrabajador(
            string noReloj)
        {
            return RepositorioFactory
                .CrearTrabajadorRepository()
                .ConsultarPorNumeroReloj(noReloj);
        }

        public static DataTable ConsultarTrabajadores(
            string textoBuscar)
        {
            return RepositorioFactory
                .CrearTrabajadorRepository()
                .Consultar(textoBuscar ?? string.Empty);
        }

        public static DataTable
            ConsultarTrabajadoresEstadoCertificacion(
                string mostrarPor,
                string textoBuscar)
        {
            return RepositorioFactory
                .CrearTrabajadorRepository()
                .ConsultarEstadoCertificacion(
                    mostrarPor ?? string.Empty,
                    textoBuscar ?? string.Empty);
        }

        public static Mensaje BorrarTrabajador(
            int id)
        {
            return RepositorioFactory
                .CrearTrabajadorRepository()
                .Borrar(id);
        }

        public Mensaje GuardarTrabajador()
        {
            return RepositorioFactory
                .CrearTrabajadorRepository()
                .Guardar(this);
        }
    }
}