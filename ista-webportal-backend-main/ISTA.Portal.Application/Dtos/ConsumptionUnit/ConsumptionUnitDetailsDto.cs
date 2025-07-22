using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application;

public record ConsumptionUnitDetailsDto
(
   Guid Id,
   string Name,
   string ConsumptionUnitNumber,
   string PropertyNumber,
   string City,
   string PostCode,
   Double Area,
   string Street,
   string HouseNumber,
   string Block,
   string Staircase,
   string Floor,
   string Door,
    TenantDto Tenant,
    DateTime? LastConsumptionReceivedDate,
    List<Guid> DevicePositionUUIDS,
    Boolean IsMainMeter,
   ConsumptionUnitMigrationStatus MigrationStatus
)
{
    public static ConsumptionUnitDetailsDto Create(ConsumptionUnit a, DateTime? lastConsumptionReceivedDate)
    {
        return new ConsumptionUnitDetailsDto(
             a.Id,
             a.Name,
             a.ConsumptionUnitNumber ?? "",
             a.Property.PropertyNumber,
              a.Property?.City ?? "",
              a.Property?.PostCode ?? "",
             a.Area,
             a.Street ?? "",
             a.HouseNnumber ?? "",
             a.Block ?? "",
             a.Staircase ?? "",
             a.Floor ?? "",
             a.Door ?? "",
             TenantDto.Create(a.ActiveTenant),
             lastConsumptionReceivedDate,
             a.Devices.Select(a => a.DevicePositionUUID).ToList(),
             a.IsMainMeter,
             a.MigrationStatus
            );
    }
}