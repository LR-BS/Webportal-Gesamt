using System.Globalization;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISTA.Portal.API.Controllers.v1.ConsumptionCalculator;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/consumptionCalculator/duration")]
[ApiExplorerSettings(GroupName = "ConsumptionCalculator")]
public class ConsumptionCalculatorDurationPost(IConsumptionCalculatorService consumptionCalculatorService) : ControllerBase
{
    /// <summary>
    /// Calculates consumption for a given month.
    /// </summary>
    /// <param name="date">The date in "yyyy-MM" format.</param>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult> CalculateMonth(string begindate, string enddate, Guid? propertyId, CancellationToken ct)
    {
        var beginDateObj = DateTime.ParseExact(begindate, "yyyy-MM", CultureInfo.InvariantCulture);
        var endDateObj = DateTime.ParseExact(enddate, "yyyy-MM", CultureInfo.InvariantCulture);
        try
        {
            for (DateTime date = beginDateObj; date <= endDateObj; date = date.AddMonths(1))
            {
                await consumptionCalculatorService.CalculateConsumptionMonth(date, propertyId);
            }
            return StatusCode(200);
        }
        catch (Exception e)
        {
            return StatusCode(501, "Error: " + e.Message);
        }
    }
    
}