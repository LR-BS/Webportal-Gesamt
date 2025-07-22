using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services.Interfaces;

public class TenantService: ITenantService
{
    private readonly VDMAdminDbContext dbContext;

    public TenantService(VDMAdminDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    
    public async Task<List<TenantDto>> ListTenantsWithErrors(CancellationToken ct)
    {
        var allowedMigrationStatus = new List<TenantMigrationStatus>
        {
            TenantMigrationStatus.FAILED_TO_SEND_TO_WP,
            TenantMigrationStatus.FAILED_TO_UPATE_IN_WP
        };
        return await dbContext.Tenants
            .Where(t => allowedMigrationStatus.Contains(t.MigrationStatus))
            .Include(a => a.ConsumptionUnit)
            .ThenInclude(b => b.Property)
            .Select(tenant => TenantDto.Create(tenant))
            .ToListAsync(ct);
    }
    
    public async Task<List<Tenant>> FixMigrationErrors(CancellationToken ct)
    {
        var allowedMigrationStatus = new List<TenantMigrationStatus>
        {
            TenantMigrationStatus.FAILED_TO_SEND_TO_WP,
            TenantMigrationStatus.FAILED_TO_UPATE_IN_WP
        };
        var tenants = await dbContext.Tenants
            .Where(t => allowedMigrationStatus.Contains(t.MigrationStatus))
            .ToListAsync(ct);
        foreach (Tenant tenant in tenants)
        {
            tenant.MigrationStatus = tenant.MigrationStatus == TenantMigrationStatus.FAILED_TO_UPATE_IN_WP ? TenantMigrationStatus.PREPARED_FOR_UPDATE_TO_WP : TenantMigrationStatus.NOT_SET;
        }
        await dbContext.SaveChangesAsync(ct);

        return tenants;
    }
    
    
}