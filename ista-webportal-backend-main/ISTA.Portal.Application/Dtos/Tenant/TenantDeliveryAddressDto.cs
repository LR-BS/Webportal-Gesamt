using SharedKernel.Domain;

namespace ISTA.Portal.Application;

public record TenantDeliveryAddressDto
(
    string City,
    string Street,
    string PostCode,
    string HouseNumber,
    string Staircase,
    string Floor,
    string Door
)
{
    public DeliveryAddress GetDeliveryAddress()
    {
        return new DeliveryAddress
        {
            City = City,
            Street = Street,
            PostCode = PostCode,
            HouseNumber = HouseNumber,
            Staircase = Staircase,
            Floor = Floor,
            Door = Door
        };
    }
}