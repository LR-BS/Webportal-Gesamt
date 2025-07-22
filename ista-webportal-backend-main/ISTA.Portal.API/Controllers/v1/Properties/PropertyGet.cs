using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/{propertyNumber}")]
[ApiExplorerSettings(GroupName = "Properties")]
public class PropertyGet : ControllerBase
{
    private readonly IPropertyService propertyService;

    public PropertyGet(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PropertyDetailsDto), 200)]
    public async Task<ActionResult> Get(string propertyNumber, CancellationToken ct)
    {
        return Ok(await propertyService.GetProperty(propertyNumber, ct));
    }
}