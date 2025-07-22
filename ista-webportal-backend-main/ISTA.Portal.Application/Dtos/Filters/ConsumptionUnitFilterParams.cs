namespace ISTA.Portal.Application;

public record ConsumptionUnitFilterParams
(
    string? TenantName,
    string? ConsumptionUnitNumber,
    string? PropertyNumber,
    Guid? PropertyId,
    ICollection<int> MigrationStatuses
);