namespace ISTA.Portal.Application;

public record PropertyFilterParams
(
    string? Name,
    string? Propertynumber,
    string? Street,
    string? City,
    string? PostCode,
    string? HouseNumber,
    string? PartnerCode);