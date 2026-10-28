using System;

namespace Aseguranza.Clases
{
    public enum ExpirationEventState
    {
        Vencida = 1,
        ProximaAVencer = 2
    }

    public sealed record ExpirationEventRow(
        int CertificationId,
        int WorkerId,
        string WorkerNumber,
        string WorkerName,
        int PlantId,
        string PlantName,
        int? LineId,
        string? LineName,
        int ShiftId,
        string ShiftName,
        int ProcessId,
        string ProcessName,
        DateTime CertificationDate,
        DateTime ExpirationDate,
        int DaysUntilExpiration,
        ExpirationEventState EventState,
        bool Active);
}
