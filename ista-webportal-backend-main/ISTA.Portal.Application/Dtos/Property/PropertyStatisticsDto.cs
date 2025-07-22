using SharedKernel.Enums;

namespace ISTA.Portal.API.Application;

public record PropertyStatisticsDto
(
      Guid Id,
      string PropertyNumber,
      string? ExternalCode,
      string PostCode,
      string City,
      String HouseNumber,
      string PartnerCode,
      string Street,
      int ActiveDevicesCount,
      decimal ReceivedConsumptionsPercentage,
      DateTime? StartDate,
      PropertyMigrationStatus MigrationStatus
);