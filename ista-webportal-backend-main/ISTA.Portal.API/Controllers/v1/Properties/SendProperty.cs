using Microsoft.AspNetCore.Mvc;
using ISTA.Portal.Application.Services.Interfaces;
using ISTA.Portal.API.Application;

namespace ISTA.Portal.API.Controllers.v1.Properties;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/properties/sendToWebportal")]
[ApiExplorerSettings(GroupName = "Properties")]
public class SendProperty : ControllerBase
{
    private readonly IPropertyService propertyService;

    public SendProperty(IPropertyService propertyService)
    {
        this.propertyService = propertyService;
    }

    [HttpPut]
    public async Task Put(string propertyNumber, CancellationToken ct)
    {
        await propertyService.SendProperty(propertyNumber, ct);
    }
}