using Aseguranza.Data;
using System;

namespace Aseguranza.Clases
{
    public static class HdcImportacion
    {
        public static ResultadoImportacionHdc Importar(
            string rutaArchivo,
            ResultadoAnalisisHdc resultado)
        {
            if (resultado is null)
            {
                throw new ArgumentNullException(
                    nameof(resultado));
            }

            return RepositorioFactory
                .CrearHdcImportacionRepository()
                .Importar(
                    rutaArchivo,
                    resultado);
        }
    }
}
