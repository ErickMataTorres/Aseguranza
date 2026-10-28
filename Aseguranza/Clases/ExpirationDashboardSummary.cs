namespace Aseguranza.Clases
{
    public sealed record ExpirationDashboardSummary(
        int ExpiredEvents,
        int UpcomingEvents,
        int AffectedWorkers,
        int AffectedProcesses);
}
