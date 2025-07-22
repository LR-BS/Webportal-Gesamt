using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application;

public record DeletePropertyListDto
(
      Guid Id,
      string PropertyNumber,
      string? ExternalCode,
      string Address,
      string PartnerCode,
      DateTime? StartDate,
      PropertyMigrationStatus MigrationStatus
)
{
    public static DeletePropertyListDto Create(Property property)
    {
        return new DeletePropertyListDto(
             property.Id,
             property.PropertyNumber,
             property.ExternalCode,
             $"{property.PostCode} - {property.City} {property.Street} - {property.Housenumber}",
             property.PartnerCode,
              property.StartDate,
              property.MigrationStatus
             );
    }
}