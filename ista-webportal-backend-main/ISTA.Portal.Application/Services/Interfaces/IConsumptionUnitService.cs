using ISTA.Portal.API.Application;
using ISTA.Portal.Application;

using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services.Interfaces;

public interface IConsumptionUnitService
{
    Task<List<DeviceListDto>> GetConsumptionUnitSubmeters(Guid consumptionUnitId, CancellationToken ct);

    Task<List<ConsumptionReportDto>> GetConsumptionUnitDeviceConsumptions(Guid devicePositionId, CancellationToken ct);

    Task<TenantDetailsDto> UpdateTenantDeliveryAddress(Guid consumptionUnitId, Guid tenantId, TenantDeliveryAddressDto tenantDeliveryAddressDto, CancellationToken ct);

    Task<ResponseWithPagination<ConsumptionUnitListDto>> GetConsumptionUnits(ConsumptionUnitFilterParams? consumptionUnitFilterParams, int pageNum, CancellationToken ct);

    Task<ConsumptionUnitDetailsDto> GetConsumptionUnitDetails(Guid consumptionUnitId, CancellationToken ct);
    
    Task<List<ConsumptionUnitListDto>> ListConsumptionUnitsWithErrors(CancellationToken ct);
    
    Task<List<ConsumptionUnit>> FixMigrationErrors(CancellationToken ct);
}