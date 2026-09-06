using Aseguranza.Clases;
using System.Collections.Generic;
using System.Data;

namespace Aseguranza.Data.Interfaces
{
    public interface IHdcEquivalenciaRepository
    {
        DataTable ConsultarPlantas();

        DataTable ConsultarTurnos();

        DataTable ConsultarLineas();

        Mensaje GuardarPlanta(
            string codigoLocalidadHdc,
            int idPlanta);

        Mensaje GuardarTurno(
            string valorTurnoHdc,
            int idTurno);

        Mensaje GuardarLinea(
            string codigoLocalidadHdc,
            string valorLineaHdc,
            string accion,
            int? idLinea);

        HashSet<string> ConsultarNumerosReloj();
    }
}
