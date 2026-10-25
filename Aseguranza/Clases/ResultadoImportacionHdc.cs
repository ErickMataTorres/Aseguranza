namespace Aseguranza.Clases
{
    public sealed class ResultadoImportacionHdc
    {
        public int IdImportacion { get; set; }

        public int TotalRegistrosArchivo { get; set; }

        public int NuevosInsertados { get; set; }

        public int Actualizados { get; set; }

        public int SinCambios { get; set; }

        public int Excluidos { get; set; }

        public int Ignorados { get; set; }

        public string RutaRespaldo { get; set; } =
            string.Empty;

        public int TotalProcesados =>
            NuevosInsertados +
            Actualizados +
            SinCambios;
    }
}
