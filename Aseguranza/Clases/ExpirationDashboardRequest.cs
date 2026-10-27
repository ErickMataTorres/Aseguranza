using System;

namespace Aseguranza.Clases
{
    public enum ExpirationEventCategory
    {
        Todas = 0,
        Vencidas = 1,
        Proximas = 2
    }

    public sealed class ExpirationDashboardRequest
    {
        public const int DefaultPageSize = 100;

        public bool? Active { get; init; } = true;
        public ExpirationEventCategory Category { get; init; } =
            ExpirationEventCategory.Todas;
        public int HorizonDays { get; init; } = 30;
        public int? PlantId { get; init; }
        public int? LineId { get; init; }
        public int? ShiftId { get; init; }
        public int? ProcessId { get; init; }
        public string Search { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = DefaultPageSize;

        // Null uses the provider's current local calendar date. A supplied date
        // permits deterministic validation without changing production defaults.
        public DateTime? AsOfDate { get; init; }

        public int ValidPageNumber => Math.Max(1, PageNumber);
        public int ValidPageSize =>
            PageSize <= 0 ? DefaultPageSize : Math.Min(PageSize, DefaultPageSize);

        public void Validate()
        {
            if (HorizonDays is not (7 or 15 or 30 or 60 or 90))
                throw new ArgumentOutOfRangeException(nameof(HorizonDays));
            if (!Enum.IsDefined(typeof(ExpirationEventCategory), Category))
                throw new ArgumentOutOfRangeException(nameof(Category));
            if (PlantId is <= 0 || LineId is <= 0 ||
                ShiftId is <= 0 || ProcessId is <= 0)
                throw new ArgumentOutOfRangeException(nameof(PlantId),
                    "Los filtros de catálogo deben tener identificadores positivos.");
            if ((Search ?? string.Empty).Trim().Length > 100)
                throw new ArgumentOutOfRangeException(nameof(Search),
                    "La búsqueda no puede superar 100 caracteres.");
        }
    }
}
