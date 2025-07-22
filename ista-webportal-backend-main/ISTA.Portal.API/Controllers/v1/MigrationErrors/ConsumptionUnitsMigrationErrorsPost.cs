using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/errors/consumption-units")]
[ApiExplorerSettings(GroupName = "MigrationErrors")]
public class ConsumptionUnitsMigrationErrorsPost(IConsumptionUnitService consumptionUnitService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ConsumptionUnit), 200)]
    public async Task<ActionResult> Post(CancellationToken ct)
    {
        return Ok(await consumptionUnitService.FixMigrationErrors(ct));
    } 
}