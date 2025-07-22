using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Models;
using ISTA.Portal.Application;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties")]
[ApiExplorerSettings(GroupName = "Properties")]
public class EnrichProperties : ControllerBase
{
    private readonly IPropertyService propertyService;

    public EnrichProperties(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPut]
    [ProducesResponseType(typeof(PropertyEditResponseDto), 200)]
    public async Task<ActionResult> List([FromBody] PropertyEnrichmenForm propertyEnrichmenForm, CancellationToken ct)
    {
        return Ok(await propertyService.EnrichProperties(propertyEnrichmenForm, ct));
    }
}