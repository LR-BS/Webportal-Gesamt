using ISTA.Portal.API.Application;
using ISTA.Portal.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;


    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/errors/tenants")]
    [ApiExplorerSettings(GroupName = "MigrationErrors")]
    public class TenantsMigrationErrorsGet : ControllerBase
    {
        private readonly ITenantService tenantService;

        public TenantsMigrationErrorsGet(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TenantDto), 200)]
        public async Task<ActionResult> Get(CancellationToken ct)
        {
            return Ok(await tenantService.ListTenantsWithErrors(ct));
        } 
    }
