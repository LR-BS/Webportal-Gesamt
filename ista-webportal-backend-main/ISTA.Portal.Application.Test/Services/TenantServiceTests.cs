namespace ISTA.Portal.Application.Test.Services;

using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Domain;
using Xunit;
using System.Threading;
using SharedKernel.Enums;

public class TenantServiceTests
{
    private readonly TenantService tenantService;
    private readonly VDMAdminDbContext dbContext;

    public TenantServiceTests()
    {
        var options = new DbContextOptionsBuilder<VDMAdminDbContext>().EnableSensitiveDataLogging()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        Guid consumptionUnitId = Guid.NewGuid();
        Guid propertyId = Guid.NewGuid();
        dbContext = new VDMAdminDbContext(options, null);
        dbContext.ConsumptionUnits.Add(new ConsumptionUnit { Id = consumptionUnitId, PropertyId = propertyId, ImportFileID = new Guid()});
        dbContext.Properties.Add(new Property { Id = propertyId, PropertyNumber = "123" , PartnerCode = "123", ImportFileID = new Guid()});
        dbContext.Tenants.Add(new Tenant { MigrationStatus = TenantMigrationStatus.FAILED_TO_UPATE_IN_WP, ExternalTenantId = "test1", Name = "tochange", ConsumptionUnitId = consumptionUnitId});
        dbContext.Tenants.Add(new Tenant { MigrationStatus = TenantMigrationStatus.SENT_TO_WP, ExternalTenantId = "test1", Name = "name", ConsumptionUnitId = consumptionUnitId});

        dbContext.SaveChanges();
        tenantService = new TenantService(dbContext);
    }

    [Fact]
    public async void TestListTenantsWithErrors()
    {
        // Arrange
        // Add test data to in-memory database
        
        await dbContext.SaveChangesAsync();

        var result = tenantService.ListTenantsWithErrors(CancellationToken.None);

        // Assert
        Assert.Single(result.Result); // Assert that there is one tenant with errors
    }

    [Fact]
    public async void TestChangeMigrationStatus()
    {
        //test
        // Act
        var result = await tenantService.ChangeMigrationStatus(TenantMigrationStatus.PREPARED_FOR_WP, CancellationToken.None);

        // Assert
        var tenant = dbContext.Tenants.First(t => t.Name == "tochange");
        Assert.Equal(TenantMigrationStatus.PREPARED_FOR_WP, tenant.MigrationStatus); // Assert that the tenant's status has been updated
    }
}