using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionUnits;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionUnits/{consumptionUnitId}")]
[ApiExplorerSettings(GroupName = "ConsumptionUnits")]
public class ConsumptionUnitsGet : ControllerBase
{
    private readonly IConsumptionUnitService consumptionUnitService;

    public ConsumptionUnitsGet(IConsumptionUnitService consumptionUnitService)
    {
        this.consumptionUnitService = consumptionUnitService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ConsumptionUnitDetailsDto), 200)]
    public async Task<ActionResult> List(Guid consumptionUnitId, CancellationToken ct)
    {
        return Ok(await consumptionUnitService.GetConsumptionUnitDetails(consumptionUnitId, ct));
    }
}