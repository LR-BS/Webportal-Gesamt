using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionUnits;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionUnits/{consumptionUnitId}/tenants/{tenantId}/deliveryAddress")]
[ApiExplorerSettings(GroupName = "ConsumptionUnits")]
public class ConsumptionUnitsTenantDeliveryAddressUpdate : ControllerBase
{
    private readonly IConsumptionUnitService consumptionUnitService;

    public ConsumptionUnitsTenantDeliveryAddressUpdate(IConsumptionUnitService consumptionUnitService)
    {
        this.consumptionUnitService = consumptionUnitService;
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithPagination<TenantDto>), 200)]
    public async Task<ActionResult> List([FromBody] TenantDeliveryAddressDto tenantDeliveryAddressDto, Guid consumptionUnitId, Guid tenantId, CancellationToken ct)
    {
        return Ok(await consumptionUnitService.UpdateTenantDeliveryAddress(consumptionUnitId, tenantId, tenantDeliveryAddressDto, ct));
    }
}