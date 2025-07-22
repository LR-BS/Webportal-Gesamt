using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/new")]
[ApiExplorerSettings(GroupName = "Properties")]
public class NewPropertiesList : ControllerBase
{
    private readonly IPropertyService propertyService;

    public NewPropertiesList(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NewPropertyDto>), 200)]
    public async Task<ActionResult> List(CancellationToken ct)
    {
        return Ok(await propertyService.ListNewProperties(ct));
    }
}