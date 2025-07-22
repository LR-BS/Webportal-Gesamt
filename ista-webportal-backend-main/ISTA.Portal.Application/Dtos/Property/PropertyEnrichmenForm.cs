namespace ISTA.Portal.Application;

public record PropertyEnrichmenForm
(
    Guid PropertyId,
    string City,
    string PostCode,
    string Street,
    string HouseNumber,
    DateTime StartDate,
    DateTime? EndDate,
    int DueDateDay,
    int DueDateMonth,
    int ContractNumber,
    double? GPSLatitude,
    double? GPSLongitude
);