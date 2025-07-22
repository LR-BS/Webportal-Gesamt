using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;
using SharedKernel.Domain;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionUnits;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionUnits/{consumptionUnitId}/submeters")]
[ApiExplorerSettings(GroupName = "ConsumptionUnits")]

public class ConsumptionUnitSubmeters : ControllerBase
{
    private readonly IConsumptionUnitService consumptionUnitService;

    public ConsumptionUnitSubmeters(IConsumptionUnitService consumptionUnitService)
    {
        this.consumptionUnitService = consumptionUnitService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<DeviceListDto>), 200)]
    public async Task<ActionResult> List(Guid consumptionUnitId, CancellationToken ct)
    {
        return Ok(await consumptionUnitService.GetConsumptionUnitSubmeters(consumptionUnitId, ct));
    }
}