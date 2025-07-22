using ISTA.Portal.Application;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Application;

public class PropertyDetailsDto
{
    public Guid Id { get; set; }
    public string PropertyNumber { get; set; } = "";
    public string? ExternalCode { get; set; } = "";
    public string PostCode { get; set; } = "";
    public string City { get; set; } = "";
    public string Street { get; set; } = "";
    public string HouseNumber { get; set; } = "";
    public string ContractNumber { get; set; } = "";
    public string PartnerCode { get; set; } = "";
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PropertyMigrationStatus MigrationStatus { get; set; }
    public string? IstaSpecialistId { get; set; }
    public PartnerDto Partner { get; set; } = default!;
    public IReadOnlyCollection<DeviceListDto> MainMeters { get; set; } = new List<DeviceListDto>();
    public IReadOnlyCollection<ConsumptionUnitListDto> ConsumptionUnits { get; set; } = new List<ConsumptionUnitListDto>();
    public int DueDateDay { get; set; }
    public int DueDateMonth { get; set; }

    public double? GPSLatitude { get; set; }
    public double? GPSLongitude { get; set; }
}