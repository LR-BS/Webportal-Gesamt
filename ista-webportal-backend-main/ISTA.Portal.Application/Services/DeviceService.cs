using ISTA.Portal.API.Application;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services;

public class DeviceService : IDeviceService
{
    private readonly VDMAdminDbContext dbContext;

    public DeviceService(VDMAdminDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Device>> GetDevices(CancellationToken ct)
    {
        return await dbContext.Devices.ToListAsync(ct);
    }
    
    public async Task<List<DeviceErrorsDto>> ListDevicesWithErrors(CancellationToken ct)
    {
        var allowedMigrationStatus = new List<DeviceMigrationStatus?>
        {
            DeviceMigrationStatus.FAILED_TO_DELETE_FROM_WP,
            DeviceMigrationStatus.FAILED_TO_SEND_TO_WP,
            DeviceMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            DeviceMigrationStatus.DEVICECATEGORY_NOT_FOUND
        };
        
        
        return await dbContext.Devices
            .Where(d => allowedMigrationStatus.Contains(d.MigrationStatus))
            .Include(a => a.ConsumptionUnit)
            .ThenInclude(b => b.Property)
            .Select(device => DeviceErrorsDto.Create(device))
            .ToListAsync(ct);
    }
    

    public async Task<List<Device>> FixMigrationErrors(CancellationToken ct)
    {
        var allowedMigrationStatus = new List<DeviceMigrationStatus?>
        {
            DeviceMigrationStatus.FAILED_TO_DELETE_FROM_WP,
            DeviceMigrationStatus.FAILED_TO_SEND_TO_WP,
            DeviceMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            DeviceMigrationStatus.DEVICECATEGORY_NOT_FOUND
        };
        var devices = await dbContext.Devices
            .Where(d => allowedMigrationStatus.Contains(d.MigrationStatus))
            .ToListAsync(ct);
        foreach (Device device in devices)
        {
            if (device.MigrationStatus == DeviceMigrationStatus.FAILED_TO_UPDATE_IN_WP)
            {
                device.MigrationStatus = DeviceMigrationStatus.PREPARED_FOR_UPDTE_WP;
            }
            else
            {
                device.MigrationStatus = DeviceMigrationStatus.PREPARED_FOR_WP;
            }
            
        }
        await dbContext.SaveChangesAsync(ct);

        return devices;
    }
}