using System;
using System.Collections.Generic;

namespace Aseguranza.Clases
{
    public sealed record ExpirationReportResult(
        IReadOnlyList<ExpirationEventRow> Items,
        ExpirationDashboardSummary Summary,
        DateTime AsOfDate);
}
