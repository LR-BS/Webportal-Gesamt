using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Models;
using ISTA.Portal.Application;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/{propertyUUID}/mainMeters")]
[ApiExplorerSettings(GroupName = "Properties")]
public class MainMetersList : ControllerBase
{
    private readonly IPropertyService propertyService;

    public MainMetersList(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<List<DeviceListDto>>), 200)]
    public async Task<ActionResult> List(Guid propertyUUID, CancellationToken ct)
    {
        return Ok(await propertyService.ListMainMeters(propertyUUID, ct));
    }
}