using System.Globalization;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionCalculator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionCalculator/year")]
[ApiExplorerSettings(GroupName = "ConsumptionCalculator")]
public class ConsumptionCalculatorYearPost(IConsumptionCalculatorService consumptionCalculatorService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> List(string date, Guid? propertyId, CancellationToken ct)
    {
        var dateObj = DateTime.ParseExact(date, "yyyy",  CultureInfo.InvariantCulture);
        try
        {
            _ = consumptionCalculatorService.CalculateConsumptionYearAsync(dateObj, propertyId);
            return StatusCode(200);
        }
        catch (Exception e)
        {
            return StatusCode(501, "Error: " + e.Message);
        }
    }
}