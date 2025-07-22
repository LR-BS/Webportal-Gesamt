using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionUnits;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionUnits/devices/{devicePositionId}/consumptions")]
[ApiExplorerSettings(GroupName = "ConsumptionUnits")]
public class ConsumptionUnitDeviceConsumption : ControllerBase
{
    private readonly IConsumptionUnitService consumptionUnitService;

    public ConsumptionUnitDeviceConsumption(IConsumptionUnitService consumptionUnitService)
    {
        this.consumptionUnitService = consumptionUnitService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<ConsumptionReportDto>), 200)]
    public async Task<ActionResult> List(Guid devicePositionId, CancellationToken ct)
    {
            return Ok(await consumptionUnitService.GetConsumptionUnitDeviceConsumptions(devicePositionId, ct));
    }
}