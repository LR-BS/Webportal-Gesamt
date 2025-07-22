using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Application;
using ISTA.Portal.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/existing")]
[ApiExplorerSettings(GroupName = "Properties")]
public class ExistingPropertiesList : ControllerBase
{
    private readonly IPropertyService propertyService;

    public ExistingPropertiesList(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithPagination<PropertyListDto>), 200)]
    public async Task<ActionResult> List(int pageNum, [FromBody] PropertyFilterParams? propertyFilterParams, CancellationToken ct)
    {
        return Ok(await propertyService.ListExistingProperties(propertyFilterParams, pageNum, ct));
    }
}