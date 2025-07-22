using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application;

public record TenantDto
(
Guid Id,
Guid ConsumptionUnitId,
string TenantName,
DateTime MoveInDate,
DateTime? MoveOutDate,
DeliveryAddress DeliveryAddress,
TenantMigrationStatus MigrationStatus,
string? PropertyNumber
)
{
    public static TenantDto Create(Tenant tenant)
    {
        return new TenantDto(tenant.Id, tenant.ConsumptionUnitId, tenant.Name, tenant.MoveInDate, tenant.MoveOutDate, tenant.DeliveryAddress, tenant.MigrationStatus, tenant.ConsumptionUnit?.Property?.PropertyNumber);
    }
}