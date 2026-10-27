namespace Aseguranza.Clases
{
    public sealed record CertificationSummary(
        int Total,
        int Vigentes,
        int PorVencer,
        int Vencidas,
        int SinCertificar);
}
