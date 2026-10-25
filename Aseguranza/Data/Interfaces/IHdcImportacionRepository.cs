using Aseguranza.Clases;

namespace Aseguranza.Data.Interfaces
{
    public interface IHdcImportacionRepository
    {
        ResultadoImportacionHdc Importar(
            string rutaArchivo,
            ResultadoAnalisisHdc resultado);
    }
}
