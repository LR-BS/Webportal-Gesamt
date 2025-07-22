using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;
using ISTA.Portal.API.Application;
using ISTA.Portal.API.Models;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/statistics/existing")]
[ApiExplorerSettings(GroupName = "Properties")]
public class StatisticsExistingPropertiesList : ControllerBase
{
    private readonly IPropertyService propertyService;

    public StatisticsExistingPropertiesList(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithPagination<PropertyStatisticsDto>), 200)]
    public async Task<ActionResult> List([FromBody] PropertyFilterParams? propertyFilterParams, int pageNum, CancellationToken ct)
    {
        return Ok(await propertyService.ListStatisticsProperties(propertyFilterParams, pageNum, true, ct));
    }
}