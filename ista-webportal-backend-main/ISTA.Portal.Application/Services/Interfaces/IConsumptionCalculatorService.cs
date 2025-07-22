namespace ISTA.Portal.Application.Services.Interfaces;

public interface IConsumptionCalculatorService
{
    public Task CalculateConsumptionMonth(DateTime dateTime,  Guid? propertyId);
    public Task CalculateConsumptionYearAsync(DateTime dateTime, Guid? propertyId);

    
}