using ISTA.Portal.API.Application;
using ISTA.Portal.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/errors/consumption-units")]
[ApiExplorerSettings(GroupName = "MigrationErrors")]
public class ConsumptionUnitsMigrationErrorsGet : ControllerBase
{
    private readonly IConsumptionUnitService consumptionUnitService;

    public ConsumptionUnitsMigrationErrorsGet(IConsumptionUnitService consumptionUnitService)
    {
        this.consumptionUnitService = consumptionUnitService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ConsumptionUnitListDto), 200)]
    public async Task<ActionResult> Get(CancellationToken ct)
    {
        return Ok(await consumptionUnitService.ListConsumptionUnitsWithErrors(ct));
    }
}