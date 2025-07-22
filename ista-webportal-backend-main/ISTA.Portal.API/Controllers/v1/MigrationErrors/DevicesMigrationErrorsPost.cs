using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/errors/devices")]
[ApiExplorerSettings(GroupName = "MigrationErrors")]
public class DevicesMigrationErrorsPost : ControllerBase
{
    private readonly IDeviceService deviceService;

    public DevicesMigrationErrorsPost(IDeviceService deviceService)
    {
        this.deviceService = deviceService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Device), 200)]
    public async Task<ActionResult> Post(CancellationToken ct)
    {
        return Ok(await deviceService.FixMigrationErrors(ct));
    } 
}