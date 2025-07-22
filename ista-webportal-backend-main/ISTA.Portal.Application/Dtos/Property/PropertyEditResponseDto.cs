using SharedKernel.Enums;

namespace ISTA.Portal.API.Application;

public class PropertyEditResponseDto
{
    public Guid Id { get; set; }
    public string PropertyNumber { get; set; } = "";
    public string? ExternalCode { get; set; } = "";
    public string PostCode { get; set; } = "";
    public string City { get; set; } = "";
    public string Street { get; set; } = "";
    public string HouseNumber { get; set; } = "";
    public string PartnerCode { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public PropertyMigrationStatus MigrationStatus { get; set; }
    public string ContractNumber { get; set; } = "";
    public string? IstaSpecialistId { get; set; }
    public double? GPSLatitude { get; set; }
    public double? GPSLongitude { get; set; }
}