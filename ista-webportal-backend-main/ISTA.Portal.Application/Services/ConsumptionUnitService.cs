using ISTA.Portal.API.Application;
using ISTA.Portal.Application.Exceptions;
using ISTA.Portal.Application.Logger;
using ISTA.Portal.Application.Services.Abstractions;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Domain;
using System.Data;
using System.Net;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services;

public class ConsumptionUnitService : BaseService, IConsumptionUnitService
{
    private readonly ILogger _logger;

    public ConsumptionUnitService(VDMAdminDbContext _dbContext) : base(_dbContext)
    {
        this._logger = new Logger.Logger();
    }

    public async Task<List<ConsumptionReportDto>> GetConsumptionUnitDeviceConsumptions(Guid devicePositionId, CancellationToken ct)
    {
        return await dbContext.ConsumptionReports.AsNoTracking()
            .Where(a => a.DevicePositionUUID == devicePositionId)
            .Select(report => new ConsumptionReportDto
                    (
                        report.Id,
                        report.DevicePositionUUID,
                        report.Date,
                        report.IsExtrapolated,
                        report.heatingDegreeDays,
                        report.HGTPercentage ?? 0,
                        report.ExtrapolcationFailureCause,
                        report.HGTAdjusted ?? 0,
                        report.UpdateState ?? 0,
                        report.LastValue ?? 0,
                        report.CreateDate,
                        (double)report.MonthlyConsumption!,
                        report.ActionIndex
                    )
            ).ToListAsync(ct);
    }

    public async Task<TenantDetailsDto> UpdateTenantDeliveryAddress(Guid consumptionUnitId, Guid tenantId, TenantDeliveryAddressDto tenantDeliveryAddressDto, CancellationToken ct)
    {
        var tenant = await dbContext.Tenants
            .Where(a => a.Id == tenantId)
            .Where(a => a.ConsumptionUnitId == consumptionUnitId)
            .FirstOrDefaultAsync(ct);

        if (tenant is null) throw new GeneralException("Tenant not found", "tenantId", HttpStatusCode.NotFound);

        tenant.DeliveryAddress = tenantDeliveryAddressDto.GetDeliveryAddress();

        await dbContext.SaveChangesAsync(ct);

        return TenantDetailsDto.Create(tenant);
    }

    public async Task<List<DeviceListDto>> GetConsumptionUnitSubmeters(Guid consumptionUnitId, CancellationToken ct)
    {
        return await dbContext.Devices.AsNoTracking()
            .Include(a => a.ConsumptionUnit)
            .Where(a => a.ConsumptionUnit.IsMainMeter == false)
            .Where(a => a.ConsumptionUnitId == consumptionUnitId)
            .Select(device => DeviceListDto.Create(device))
            .ToListAsync(ct);
    }

    public async Task<ResponseWithPagination<ConsumptionUnitListDto>> GetConsumptionUnits(ConsumptionUnitFilterParams? consumptionUnitFilterParams, int pageNum, CancellationToken ct)
    {
        var list = dbContext.ConsumptionUnits
            .Include(a => a.Property)
            .Include(a => a.Tenants)
            .ApplyFilters(consumptionUnitFilterParams)
            .Select(a => ConsumptionUnitListDto.Create(a))
            .AsQueryable();

        return await list.AddPagination<ConsumptionUnitListDto>(pageNum, ct);
    }

    public async Task<ConsumptionUnitDetailsDto> GetConsumptionUnitDetails(Guid consumptionUnitId, CancellationToken ct)
    {
        var consumptionBeginDate = DateTime.Now.AddMonths(-12);
        var item = await dbContext.ConsumptionUnits.AsNoTracking()
                .Include(a => a.Property)
                .Include(a => a.Tenants)
                .Include(a => a.Devices)
                .Where(a => a.Id == consumptionUnitId)
                .FirstOrDefaultAsync(ct);

        if (item is null) throw new GeneralException("ConsumptionUnitId not found", "consumptionUnitId", HttpStatusCode.NotFound);

        var devicePositions = item.Devices
            .Select(k => k.DevicePositionUUID)
            .Distinct()
            .ToList();

        // create a query to get latest measure date starting from MesuredDate with devices of devicePositions
        /*var latestConsumptionQuery = ("SELECT max(MesuredDate) as Value "
            + "FROM [Consumptions] "
            + "WHERE MesuredDate > '{MesuredDate}' "
            + "AND [DevicePositionUUID] IN ({devicePositions})")
            .Replace("{MesuredDate}", consumptionBeginDate.ToString("dd-MM-yyyy"))
            .Replace("{devicePositions}", string.Join(',', devicePositions.Select(a => "'" + a + "'")));
        */
        DateTime? latestConsumption = null;

        if (devicePositions.Any())
        {
            latestConsumption = await dbContext.Consumptions
                .Where(c => c.MesuredDate > consumptionBeginDate && devicePositions.Contains(c.DevicePositionUUID))
                .MaxAsync(c => (DateTime?)c.MesuredDate, ct);;
        }

        return ConsumptionUnitDetailsDto.Create(item, latestConsumption);
    }
    
    public async Task<List<ConsumptionUnitListDto>> ListConsumptionUnitsWithErrors(CancellationToken ct)
    {
        var allowedMigrationStatus = new List<ConsumptionUnitMigrationStatus>
        {
            ConsumptionUnitMigrationStatus.FAILED_TO_SEND_TO_WP,
            ConsumptionUnitMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            ConsumptionUnitMigrationStatus.FAILED_TO_DELETE_FROM_WP
        };
        
        return await dbContext.ConsumptionUnits.AsNoTracking()
            .Include(a => a.Property)
            .Include(a => a.Tenants)
            .Where(a => allowedMigrationStatus.Contains(a.MigrationStatus))
            .Select(a => ConsumptionUnitListDto.Create(a))
            .ToListAsync(ct);
    }
    
    public async Task<List<ConsumptionUnit>> FixMigrationErrors(CancellationToken ct)
    {
        
        dbContext.ChangeTracker.Clear();
        
        var allowedMigrationStatus = new List<ConsumptionUnitMigrationStatus>
        {
            ConsumptionUnitMigrationStatus.FAILED_TO_SEND_TO_WP,
            ConsumptionUnitMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            ConsumptionUnitMigrationStatus.FAILED_TO_DELETE_FROM_WP
        };
        var consumptionUnits = await dbContext.ConsumptionUnits.Where(a => allowedMigrationStatus.Contains(a.MigrationStatus))
            .ToListAsync(ct);
        foreach (var cu in consumptionUnits)
        {
            cu.MigrationStatus = cu.MigrationStatus == ConsumptionUnitMigrationStatus.FAILED_TO_UPDATE_IN_WP ? ConsumptionUnitMigrationStatus.EDITED : ConsumptionUnitMigrationStatus.NOT_SET;
        }

        await dbContext.SaveChangesAsync(ct);

        return consumptionUnits;
    }
}


internal static class ConsumptionUnitExtenstionMethods
{
    public static IQueryable<ConsumptionUnit> ApplyFilters(this IQueryable<ConsumptionUnit> consumptionUnits, ConsumptionUnitFilterParams? consumptionUnitsFilterParams)
    {
        if (consumptionUnitsFilterParams is null) return consumptionUnits;

        if (!string.IsNullOrEmpty(consumptionUnitsFilterParams.TenantName))
        {
            consumptionUnits = consumptionUnits.Where(a => a.Name.Contains(consumptionUnitsFilterParams.TenantName));
        }

        if (!string.IsNullOrEmpty(consumptionUnitsFilterParams.ConsumptionUnitNumber))
        {
            consumptionUnits = consumptionUnits.Where(a => a.ConsumptionUnitNumber!.Contains(consumptionUnitsFilterParams.ConsumptionUnitNumber));
        }

        if (!string.IsNullOrEmpty(consumptionUnitsFilterParams.PropertyNumber))
        {
            consumptionUnits = consumptionUnits.Where(a => a.Property.PropertyNumber!.Contains(consumptionUnitsFilterParams.PropertyNumber));
        }

        if (consumptionUnitsFilterParams.PropertyId != null)
        {
            consumptionUnits = consumptionUnits.Where(a => a.Property.Id == consumptionUnitsFilterParams.PropertyId);
        }

        consumptionUnits = consumptionUnits.Where(a => consumptionUnitsFilterParams.MigrationStatuses.Contains((int)a.MigrationStatus));

        return consumptionUnits;
    }
}