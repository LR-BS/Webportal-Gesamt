using SharedKernel.Domain;

namespace ISTA.Portal.Application;

public record TenantDetailsDto
(
Guid Id,
string Name,
DateTime MoveInDate,
DateTime? MoveOutDate,
DeliveryAddress DeliveryAddress
)
{
    public static TenantDetailsDto Create(Tenant tenant)
    {
        return new TenantDetailsDto(tenant.Id, tenant.Name, tenant.MoveInDate, tenant.MoveOutDate, tenant.DeliveryAddress);
    }
}