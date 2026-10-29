using System;

namespace Aseguranza.Clases
{
    // An immutable snapshot of report filters; pagination is deliberately absent.
    public sealed record ExpirationReportRequest(
        bool? Active,
        ExpirationEventCategory Category,
        int HorizonDays,
        int? PlantId,
        int? LineId,
        int? ShiftId,
        int? ProcessId,
        string Search,
        DateTime AsOfDate)
    {
        public void Validate() => new ExpirationDashboardRequest
        {
            Active = Active,
            Category = Category,
            HorizonDays = HorizonDays,
            PlantId = PlantId,
            LineId = LineId,
            ShiftId = ShiftId,
            ProcessId = ProcessId,
            Search = Search,
            AsOfDate = AsOfDate
        }.Validate();
    }
}
