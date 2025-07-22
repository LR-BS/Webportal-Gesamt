using ISTA.Portal.API.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/errors/devices")]
[ApiExplorerSettings(GroupName = "MigrationErrors")]
public class DevicesMigrationErrorsGet : ControllerBase
{
    private readonly IDeviceService deviceService;

    public DevicesMigrationErrorsGet(IDeviceService deviceService)
    {
        this.deviceService = deviceService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DeviceErrorsDto), 200)]
    public async Task<ActionResult> Get(CancellationToken ct)
    {
        return Ok(await deviceService.ListDevicesWithErrors(ct));
    } 
}