using Aseguranza.Clases;

namespace Aseguranza.Data.Interfaces
{
    public interface IHdcImportacionRepository
    {
        EstadoConciliacionHdc ConsultarEstadoConciliacion();
        ResultadoImportacionHdc Importar(
            string rutaArchivo,
            ResultadoAnalisisHdc resultado);
    }
}
