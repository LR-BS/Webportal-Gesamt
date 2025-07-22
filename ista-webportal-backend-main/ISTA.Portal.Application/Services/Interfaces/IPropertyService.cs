using ISTA.Portal.API.Application;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services.Interfaces;

public interface IPropertyService
{
    Task ConfirmDeleteRequest(Guid propertyUUID, CancellationToken ct);

    Task<List<DeletePropertyListDto>> ListDeleteRequests(PropertyFilterParams? propertyFilterParams, CancellationToken ct);

    Task<ResponseWithPagination<PropertyStatisticsDto>> ListStatisticsProperties(PropertyFilterParams? propertyFilterParams, int pageNum, Boolean sentToWebportal, CancellationToken ct);

    Task<List<NewPropertyDto>> ListNewProperties(CancellationToken ct);

    Task<PropertyDetailsDto> GetProperty(string PropertyNumber, CancellationToken ct);

    Task<PropertyEditResponseDto> EnrichProperties(PropertyEnrichmenForm propertyEnrichmenForm, CancellationToken ct);

    Task<List<DeviceListDto>> ListMainMeters(Guid propertyUUID, CancellationToken ct);

    Task SendProperty(string propertyNumber, CancellationToken ct);

    Task<ResponseWithPagination<PropertyListDto>> ListExistingProperties(PropertyFilterParams? propertyFilterParams, int pageNum, CancellationToken ct);
    
    Task<List<PropertyListDto>> ListPropertiesWithErrors(CancellationToken ct);
    
    Task<List<Property>> FixMigrationErrors(CancellationToken ct);
}