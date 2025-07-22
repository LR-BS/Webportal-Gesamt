using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Models;
using ISTA.Portal.Application;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/statistics/new")]
[ApiExplorerSettings(GroupName = "Properties")]
public class StatisticsNewPropertiesList : ControllerBase
{
    private readonly IPropertyService propertyService;

    public StatisticsNewPropertiesList(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithPagination<PropertyStatisticsDto>), 200)]
    public async Task<ActionResult> List([FromBody] PropertyFilterParams? propertyFilterParams, int pageNum, CancellationToken ct)
    {
        return Ok(await propertyService.ListStatisticsProperties(propertyFilterParams, pageNum, false, ct));
    }
}