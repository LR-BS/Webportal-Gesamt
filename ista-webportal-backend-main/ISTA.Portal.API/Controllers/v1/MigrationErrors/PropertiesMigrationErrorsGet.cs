using ISTA.Portal.API.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Controllers.v1.MigrationErrors;


    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/errors/properties")]
    [ApiExplorerSettings(GroupName = "MigrationErrors")]
    public class PropertyMigrationErrorsGet : ControllerBase
    {
        private readonly IPropertyService propertyService;

        public PropertyMigrationErrorsGet(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PropertyDetailsDto), 200)]
        public async Task<ActionResult> Get(CancellationToken ct)
        {
            return Ok(await propertyService.ListPropertiesWithErrors(ct));
        } 
    }