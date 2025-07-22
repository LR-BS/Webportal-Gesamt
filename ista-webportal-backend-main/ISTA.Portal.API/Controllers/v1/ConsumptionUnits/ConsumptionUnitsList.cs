using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionUnits;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionUnits")]
[ApiExplorerSettings(GroupName = "ConsumptionUnits")]
public class ConsumptionUnitsList : ControllerBase
{
    private readonly IConsumptionUnitService consumptionUnitService;

    public ConsumptionUnitsList(IConsumptionUnitService consumptionUnitService)
    {
        this.consumptionUnitService = consumptionUnitService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithPagination<ConsumptionUnitListDto>), 200)]
    public async Task<ActionResult> List([FromBody] ConsumptionUnitFilterParams? consumptionUnitFilterParams, int pageNum, CancellationToken ct)
    {
        return Ok(await consumptionUnitService.GetConsumptionUnits(consumptionUnitFilterParams, pageNum, ct));
    }
}