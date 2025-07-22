using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Application;

public record PropertyListDto
(
      Guid Id,
      string PropertyNumber,
      string? ExternalCode,
      string PostCode,
      string City,
      String HouseNumber,
      string PartnerCode,
      string Street,
      DateTime? StartDate,
      PropertyMigrationStatus MigrationStatus,
      string MigrationStatusString
)
{
    public static PropertyListDto Create(Property entity)
    {
        return new PropertyListDto(entity.Id,
            entity.PropertyNumber,
            entity.ExternalCode,
            entity.PostCode ?? "",
            entity.City ?? "",
            entity.Housenumber ?? "",
            entity.PartnerCode,
            entity.Street ?? "",
            entity.StartDate,
            entity.MigrationStatus,
            entity.MigrationStatus.ToString());
    }
};