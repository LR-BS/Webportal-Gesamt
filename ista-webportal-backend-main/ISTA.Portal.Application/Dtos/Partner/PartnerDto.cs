using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application;

public record PartnerDto
(
    string Name,
    string Street,
    string PostCode,
    string City,
    string PartnerCode,
    PartnerMigrationStatus MigrationStatus
)
{
    public static PartnerDto Create(Partner partner)
    {
        return new PartnerDto(
        partner.Name, partner.Street, partner.PostalCode, partner.City, partner.PartnerCode, partner.MigrationStatus
        );
    }
};