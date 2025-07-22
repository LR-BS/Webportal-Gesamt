using ISTA.Portal.Application;
using SharedKernel.Domain;

namespace ISTA.Portal.API.Application;

public record NewPropertyDto
(
    Guid Id,
    string propertyNumber,
    string Address,
     DateTime ImportDate
)
{
    public static NewPropertyDto Create(Property property)
    {
        return new NewPropertyDto(
              property.Id,
              property.PropertyNumber,
              $"{property.PostCode} - {property.City} {property.Street} - {property.Housenumber}",
              property.ImportedFile.AccessDate
            );
    }
};