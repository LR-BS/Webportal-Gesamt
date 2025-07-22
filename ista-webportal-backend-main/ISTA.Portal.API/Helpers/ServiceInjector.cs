using ISTA.Portal.Application.Services;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

using SharedKernel.Data;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ISTA.Portal.API.Helpers;

public static class ServiceInjector
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IConsumptionUnitService, ConsumptionUnitService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IConsumptionCalculatorService, ConsumptionCalculatorService>();
    }
}