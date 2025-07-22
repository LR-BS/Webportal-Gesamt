using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application;

public record ConsumptionUnitListDto
(
   Guid Id,
   string Name,
   string ConsumptionUnitNumber,
   string PropertyNumber,
   Double Area,
   string Street,
   string HouseNumber,
   string Block,
   string Staircase,
   string Floor,
   string Door,
    Boolean IsMainMeter,
   ConsumptionUnitMigrationStatus MigrationStatus
)
{
    public static ConsumptionUnitListDto Create(ConsumptionUnit a)
    {
        return new ConsumptionUnitListDto(
             a.Id,
             a.Name,
             a.ConsumptionUnitNumber ?? "",
             a.Property.PropertyNumber,
             a.Area,
             a.Street ?? "",
             a.HouseNnumber ?? "",
             a.Block ?? "",
             a.Staircase ?? "",
             a.Floor ?? "",
             a.Door ?? "",
               a.IsMainMeter,
             a.MigrationStatus
            );
    }
}