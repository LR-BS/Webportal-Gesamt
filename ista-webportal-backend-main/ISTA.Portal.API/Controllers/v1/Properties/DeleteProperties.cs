using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/deleteRequests")]
[ApiExplorerSettings(GroupName = "Properties-Delete")]
public class DeleteProperties : ControllerBase
{
    private readonly IPropertyService propertyService;

    public DeleteProperties(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPut]
    public async Task<ActionResult> Delete(Guid propertyUUID, CancellationToken ct)
    {
        await propertyService.ConfirmDeleteRequest(propertyUUID, ct);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<DeletePropertyListDto>), 200)]
    public async Task<ActionResult> List([FromBody] PropertyFilterParams? propertyFilterParams, CancellationToken ct)
    {
        return Ok(await propertyService.ListDeleteRequests(propertyFilterParams, ct));
    }
}