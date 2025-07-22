using ISTA.Portal.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/errors/tenants")]
[ApiExplorerSettings(GroupName = "MigrationErrors")]
public class TenantsMigrationErrorsPost : ControllerBase
{
    private readonly ITenantService tenantService;

    public TenantsMigrationErrorsPost(ITenantService tenantService)
    {
        this.tenantService = tenantService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Tenant), 200)]
    public async Task<ActionResult> Post(CancellationToken ct)
    {
        return Ok(await tenantService.FixMigrationErrors(ct));
    } 
}