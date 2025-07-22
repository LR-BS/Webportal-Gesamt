using System.Globalization;
using ISTA.Portal.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionCalculator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionCalculator/month")]
[ApiExplorerSettings(GroupName = "ConsumptionCalculator")]
public class ConsumptionCalculatorMonthPost(IConsumptionCalculatorService consumptionCalculatorService) : ControllerBase
{
    /// <summary>
    /// Calculates consumption for a given month.
    /// </summary>
    /// <param name="date">The date in "yyyy-MM" format.</param>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> CalculateMonth(string date, Guid? propertyId, CancellationToken ct)
    {
        var dateObj = DateTime.ParseExact(date, "yyyy-MM", CultureInfo.InvariantCulture);
        try
        {
            await consumptionCalculatorService.CalculateConsumptionMonth(dateObj, propertyId);
            return StatusCode(200);
        }
        catch (Exception e)
        {
            return StatusCode(501, "Error: " + e.Message);
        }
    }
    
}