using System.Diagnostics;
using ISTA.Portal.Application.Config;
using ISTA.Portal.Application.Logger;
using ISTA.Portal.Application.Services.Abstractions;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using SharedKernel.Data;

namespace ISTA.Portal.Application.Services;

public class ConsumptionCalculatorService(VDMAdminDbContext dbContext, IOptions<ConsumptionCalculatorSettings> calculatorOptions)
    : BaseService(dbContext), IConsumptionCalculatorService
{
    private readonly Logger.Logger _logger = new Logger.Logger();

    public Task CalculateConsumptionMonth(DateTime dateTime, Guid? propertyId)
    {
        //get month from datetime but add 0 if month is less than 10
        var month = dateTime.Month < 10 ? $"0{dateTime.Month}" : dateTime.Month.ToString();
        //get year from datetime
        var year = dateTime.Year;
        _logger.LogInformation($"Calculating consumption for month {month} and year {year}");
#if DEBUG
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        CallExeFileDebug($"01.{month}.{year}", propertyId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#else
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CallExeFileAsync($"01.{month}.{year}", propertyId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#endif
        return Task.CompletedTask;
    }

    public async Task CalculateConsumptionYearAsync(DateTime dateTime, Guid? propertyId)
    {
        //get year from datetime
        var year = dateTime.Year;
        _logger.LogInformation($"Calculating consumption for year {year}");
        for (var i = 1; i <= 12; i++)
        {
            var month = i < 10 ? $"0{i}" : i.ToString();

#if DEBUG
            await CallExeFileDebug($"01.{month}.{year}", propertyId);
#else
            await CallExeFileAsync($"01.{month}.{year}", propertyId);
#endif
        }
    }



    

    private async Task CallExeFileAsync(string arguments, Guid? propertyId)
    { 
        if (propertyId != null)
            arguments = arguments + " -propertyId " + propertyId;
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = calculatorOptions.Value.ExecutablePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        while (!process.StandardOutput.EndOfStream)
        {
            var line = await process.StandardOutput.ReadLineAsync();
            _logger.LogInformation(line);
        }

        await process.WaitForExitAsync();
    }

    private async Task CallExeFileDebug(string arguments, Guid? devicePosition = null)
    {
        if (devicePosition != null)
            arguments = arguments + " -propertyId " + devicePosition;
        
        _logger.LogInformation($"C:\\ISTA.VDMA.2.1\\ConsumptionCalculatorAgent\\Runner.exe {arguments}");
        //wait for 10 seconds
        await Task.Delay(10000);
    }
}