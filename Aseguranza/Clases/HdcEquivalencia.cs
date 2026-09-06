using Aseguranza.Data;
using System.Collections.Generic;
using System.Data;

namespace Aseguranza.Clases
{
    public static class HdcEquivalencia
    {
        public static DataTable ConsultarPlantas()
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .ConsultarPlantas();
        }

        public static DataTable ConsultarTurnos()
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .ConsultarTurnos();
        }

        public static DataTable ConsultarLineas()
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .ConsultarLineas();
        }

        public static Mensaje GuardarPlanta(
            string codigoLocalidadHdc,
            int idPlanta)
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .GuardarPlanta(
                    codigoLocalidadHdc,
                    idPlanta);
        }

        public static Mensaje GuardarTurno(
            string valorTurnoHdc,
            int idTurno)
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .GuardarTurno(
                    valorTurnoHdc,
                    idTurno);
        }

        public static Mensaje GuardarLinea(
            string codigoLocalidadHdc,
            string valorLineaHdc,
            string accion,
            int? idLinea)
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .GuardarLinea(
                    codigoLocalidadHdc,
                    valorLineaHdc,
                    accion,
                    idLinea);
        }

        public static HashSet<string>
            ConsultarNumerosReloj()
        {
            return RepositorioFactory
                .CrearHdcEquivalenciaRepository()
                .ConsultarNumerosReloj();
        }
    }
}
