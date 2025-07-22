using ISTA.Portal.API.Application;
using ISTA.Portal.Application.Exceptions;
using ISTA.Portal.Application.Services.Abstractions;
using ISTA.Portal.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Enums;
using System.Linq;
using System.Net;
using SharedKernel.Domain;
using Property = SharedKernel.Domain.Property;

namespace ISTA.Portal.Application.Services;

public class PropertyService : BaseService, IPropertyService
{
    public PropertyService(VDMAdminDbContext _dbContext) : base(_dbContext)
    {
    }

    public async Task ConfirmDeleteRequest(Guid propertyUUID, CancellationToken ct)
    {
        await dbContext.Properties.Where(a => a.Id == propertyUUID).ExecuteUpdateAsync(setters => setters.SetProperty(b => b.MigrationStatus, PropertyMigrationStatus.REQUESTED_TO_BE_DELETED), ct);
    }

    public async Task<List<DeletePropertyListDto>> ListDeleteRequests(PropertyFilterParams? propertyFilterParams, CancellationToken ct)
    {
        var allowedMigrationStatus = new List<PropertyMigrationStatus>
        {
            PropertyMigrationStatus.REQUESTED_TO_BE_DELETED
        };

        return await dbContext.Properties.AsNoTracking()
            .Where(a => allowedMigrationStatus.Contains(a.MigrationStatus))
            .ApplyFilters(propertyFilterParams)
            .Select(property => DeletePropertyListDto.Create(property)).ToListAsync(ct);
    }

    public async Task<ResponseWithPagination<PropertyStatisticsDto>> ListStatisticsProperties(PropertyFilterParams? propertyFilterParams, int pageNum, bool sentToWebportal, CancellationToken ct)
    {
        var listMigrationStatus = new List<PropertyMigrationStatus>
        {
            PropertyMigrationStatus.SENT_TO_WP,
            PropertyMigrationStatus.ASSIGNED_TO_PARTNER,
            PropertyMigrationStatus.EDITED,
            PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            PropertyMigrationStatus.FAILED_T0_ASSIGN_TO_PARTNER,
            PropertyMigrationStatus.FAILED_TO_FIND_PARTNER,
            PropertyMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            PropertyMigrationStatus.FAILED_TO_DELETE_FROM_WP
        };

        if (sentToWebportal == false)
        {
            listMigrationStatus = new List<PropertyMigrationStatus>
            {
                PropertyMigrationStatus.DONE_ENRICHMENT
            };
        }

        var list = dbContext.Properties.AsNoTracking()
            .Where(a => listMigrationStatus.Contains(a.MigrationStatus))
            .Include(a => a.ConsumptionUnits!)
            .ThenInclude(a => a.Tenants)
            .Include(a => a.ConsumptionUnits!)
            .ThenInclude(a => a.Devices)
            .ApplyFilters(propertyFilterParams)
            .Select(property => new PropertyStatisticsDto
            (
                property.Id,
                property.PropertyNumber,
                property.ExternalCode,
                property.PostCode ?? "",
                property.City ?? "",
                property.Housenumber ?? "",
                property.PartnerCode,
                property.Street ?? "",
                property.ConsumptionUnits!.SelectMany(a => a.Devices).Count(),
                property.ConsumingDevicesPercentage,
                property.StartDate,
                property.MigrationStatus
            )).AsQueryable();

        return await list.AddPagination(pageNum, ct);
    }

    public async Task<List<NewPropertyDto>> ListNewProperties(CancellationToken ct)
    {
        return await dbContext.Properties.AsNoTracking()
            .Where(a => a.MigrationStatus == PropertyMigrationStatus.NOT_SET)
            .Include(a => a.ImportedFile)
            .OrderByDescending(a => a.ImportedFile.AccessDate)
            .Select(property => NewPropertyDto.Create(property)).ToListAsync(ct);
    }

    public async Task<ResponseWithPagination<PropertyListDto>> ListExistingProperties(PropertyFilterParams? propertyFilterParams, int pageNum, CancellationToken ct)
    {
        var listMigrationStatus = new List<PropertyMigrationStatus>
        {
            PropertyMigrationStatus.SENT_TO_WP,
            PropertyMigrationStatus.ASSIGNED_TO_PARTNER,
            PropertyMigrationStatus.EDITED,
            PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            PropertyMigrationStatus.FAILED_T0_ASSIGN_TO_PARTNER,
            PropertyMigrationStatus.FAILED_TO_FIND_PARTNER,
            PropertyMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            PropertyMigrationStatus.FAILED_TO_DELETE_FROM_WP
        };
        var list = dbContext.Properties.AsNoTracking()
            .Where(a => listMigrationStatus.Contains(a.MigrationStatus))
            .ApplyFilters(propertyFilterParams)
            .OrderBy(a => a.PropertyNumber)
            .Select(property => PropertyListDto.Create(property)).AsQueryable();

        return await list.AddPagination(pageNum, ct);
    }

    public async Task<PropertyDetailsDto> GetProperty(string PropertyNumber, CancellationToken ct)
    {
        var property = await dbContext.Properties.AsNoTracking()
            .Where(a => a.PropertyNumber == PropertyNumber)
            .Include(a => a.ConsumptionUnits!)
            .ThenInclude(a => a.Tenants)
            .Include(a => a.ConsumptionUnits!)
            .ThenInclude(a => a.Devices)
            .Join(dbContext.Partners,
            a => a.PartnerCode,
            b => b.PartnerCode,
            (a, b) => new { property = a, partner = b }
            ).FirstOrDefaultAsync(ct);

        if (property is null) throw new GeneralException("Property not found", "PropertyNumber", HttpStatusCode.NotFound);

        var propertyConsumptionUnits = property.property.ConsumptionUnits!
            .Where(a => a.IsMainMeter == false)
            .OrderBy(a => a.ConsumptionUnitNumber)
            .Select(a => ConsumptionUnitListDto.Create(a)).ToList()
            .AsReadOnly()!;

        return new PropertyDetailsDto
        {
            Id = property!.property.Id,
            PropertyNumber = property.property.PropertyNumber,
            ExternalCode = property.property.ExternalCode!,
            PartnerCode = property.property.PartnerCode!,
            City = property.property.City ?? "",
            HouseNumber = property.property.Housenumber ?? "",
            PostCode = property.property.PostCode ?? "",
            Street = property.property.Street ?? "",
            StartDate = property.property.StartDate!,
            EndDate = property.property.EndDate,
            MainMeters = property.property.ConsumptionUnits!.Where(a => a.IsMainMeter == true).SelectMany(a => a.Devices).Select(a => DeviceListDto.Create(a)).ToList().AsReadOnly()!,
            IstaSpecialistId = property.property.IstaSpecialistId,
            MigrationStatus = property.property.MigrationStatus,
            ContractNumber = property.property.ContractNumber ?? "",
            DueDateDay = property.property.DueDateDay,
            DueDateMonth = property.property.DueDateMonth,
            GPSLatitude = property.property.GPSLatitude,
            GPSLongitude = property.property.GPSLongitude,
            Partner = PartnerDto.Create(property.partner)!,
            ConsumptionUnits = propertyConsumptionUnits,
        };
    }

    public async Task<PropertyEditResponseDto> EnrichProperties(PropertyEnrichmenForm propertyEnrichmenForm, CancellationToken ct)
    {
        var property = await dbContext.Properties.FindAsync(propertyEnrichmenForm.PropertyId, ct);

        if (property is null) throw new GeneralException("Property not found", "PropertyId", HttpStatusCode.NotFound);

        ValidateDueDate(propertyEnrichmenForm);

        /*if (PropertyInformationUpdated(property, propertyEnrichmenForm))
        {
            property.ManuallyUpdated = true;
        }*/
        property.City = propertyEnrichmenForm.City;
        property.PostCode = propertyEnrichmenForm.PostCode;
        property.Street = propertyEnrichmenForm.Street;
        property.Housenumber = propertyEnrichmenForm.HouseNumber;
        property.StartDate = propertyEnrichmenForm.StartDate;
        property.EndDate = propertyEnrichmenForm.EndDate;
        property.DueDateMonth = propertyEnrichmenForm.DueDateMonth;
        property.DueDateDay = propertyEnrichmenForm.DueDateDay;
        property.GPSLatitude = propertyEnrichmenForm.GPSLatitude;
        property.GPSLongitude = propertyEnrichmenForm.GPSLongitude;
        property.ContractNumber = propertyEnrichmenForm.ContractNumber.ToString();
        property.MigrationStatus = PropertyMigrationStatus.DONE_ENRICHMENT;

        await dbContext.SaveChangesAsync(ct);

        return new PropertyEditResponseDto
        {
            Id = property.Id,
            PropertyNumber = property.PropertyNumber,
            PostCode = property.PostCode,
            City = property.City,
            Street = property.Street,
            HouseNumber = property.Housenumber,
            ExternalCode = property.ExternalCode,
            PartnerCode = property.PartnerCode,
            StartDate = (DateTime)property.StartDate,
            MigrationStatus = property.MigrationStatus,
            GPSLatitude = property.GPSLatitude,
            GPSLongitude = property.GPSLongitude,
            ContractNumber = property.ContractNumber,
            IstaSpecialistId = property.IstaSpecialistId
        };
    }

    private Boolean PropertyInformationUpdated(Property property, PropertyEnrichmenForm propertyEnrichmenForm)
    {
        if (property.PostCode != propertyEnrichmenForm.PostCode) return true;
        if (property.City != propertyEnrichmenForm.City) return true;
        if (property.Street != propertyEnrichmenForm.Street) return true;
        if (property.Housenumber != propertyEnrichmenForm.HouseNumber) return true;

        return false;
    }

    private void ValidateDueDate(PropertyEnrichmenForm property)
    {
        try
        {
            new DateOnly(2022, property.DueDateMonth, property.DueDateDay);
        }
        catch (Exception e)
        {
            throw new GeneralException("Invalid DueDate", "DueDay,DueMonth", HttpStatusCode.BadRequest);
        }
    }

    public async Task<List<DeviceListDto>> ListMainMeters(Guid propertyUUID, CancellationToken ct)
    {
        return await dbContext.ConsumptionUnits.AsNoTracking()
                  .Include(a => a.Tenants)
                  .Where(a => a.PropertyId == propertyUUID)
                  .Where(a => a.IsMainMeter == true)
                  .Include(a => a.Devices)
                  .SelectMany(a => a.Devices)
                  .Select(device => DeviceListDto.Create(device))
                  .ToListAsync(ct);
    }

    public async Task SendProperty(string propertyNumber, CancellationToken ct)
    {
        var property = await dbContext.Properties.Where(a => a.PropertyNumber == propertyNumber).SingleOrDefaultAsync();

        if (property == null)
        {
            throw new GeneralException("Property not found", "propertyNumber", HttpStatusCode.BadRequest);
        }

        property.MigrationStatus = PropertyMigrationStatus.PREPARED_FOR_WP;
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task<List<PropertyListDto>> ListPropertiesWithErrors(CancellationToken ct)
    {
        var allowedMigrationStatus = new List<PropertyMigrationStatus>
        {
            //PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            PropertyMigrationStatus.FAILED_TO_DELETE_FROM_WP,
            //PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            //PropertyMigrationStatus.CONFIRMED_TO_BE_DELTETED,
            //PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            PropertyMigrationStatus.FAILED_T0_ASSIGN_TO_PARTNER,
            PropertyMigrationStatus.FAILED_TO_FIND_PARTNER,
            PropertyMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            PropertyMigrationStatus.FAILED_TO_DELETE_FROM_WP,
            PropertyMigrationStatus.FAILED_TO_SEND_TO_WP
        };

        return await dbContext.Properties.AsNoTracking()
            .Where(a => allowedMigrationStatus.Contains(a.MigrationStatus))
            .Select(property => PropertyListDto.Create(property)).ToListAsync(ct);
    }
    
    public async Task<List<Property>> FixMigrationErrors(CancellationToken ct)
    {
        dbContext.ChangeTracker.Clear();
        var allowedMigrationStatus = new List<PropertyMigrationStatus>
        {
            //PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            PropertyMigrationStatus.FAILED_TO_DELETE_FROM_WP,
            //PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            //PropertyMigrationStatus.CONFIRMED_TO_BE_DELTETED,
            //PropertyMigrationStatus.REQUESTED_TO_BE_DELETED,
            PropertyMigrationStatus.FAILED_T0_ASSIGN_TO_PARTNER,
            PropertyMigrationStatus.FAILED_TO_FIND_PARTNER,
            PropertyMigrationStatus.FAILED_TO_UPDATE_IN_WP,
            PropertyMigrationStatus.FAILED_TO_DELETE_FROM_WP,
            PropertyMigrationStatus.FAILED_TO_SEND_TO_WP
        };

        var properties = await dbContext.Properties.Where(a => allowedMigrationStatus.Contains(a.MigrationStatus)).ToListAsync(ct);
        List<Partner> partners = new List<Partner>();
        
        foreach (Property property in properties)
        {
           
            var temp = await dbContext.Partners.Where(a => a.PartnerCode == property.PartnerCode).ToListAsync(ct);
            partners.AddRange(temp);

            if (property.MigrationStatus == PropertyMigrationStatus.FAILED_TO_UPDATE_IN_WP)
            {
                property.MigrationStatus = PropertyMigrationStatus.EDITED;
            }
            else
            {
                property.MigrationStatus = PropertyMigrationStatus.NOT_SET;
            }
        }
        //change the migrationstatus of all partners
        foreach (Partner partner in partners)
        {
            partner.MigrationStatus = PartnerMigrationStatus.PREPARED_FOR_WP;
        }
        await dbContext.SaveChangesAsync(ct);
        return properties;
    }
}

internal static class PropertyExtenstionMethods
{
    public static IQueryable<Property> ApplyFilters(this IQueryable<Property> properties, PropertyFilterParams? propertyFilterParams)
    {
        if (propertyFilterParams is null) return properties;

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.Name))
        {
            properties = properties.Where(a => a.ConsumptionUnits!.SelectMany(a => a.Name).ToString()!.Contains(propertyFilterParams.Name));
        }

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.Propertynumber))
        {
            properties = properties.Where(a => a.PropertyNumber.Contains(propertyFilterParams.Propertynumber));
        }

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.Street))
        {
            properties = properties.Where(a => a.Street!.Contains(propertyFilterParams.Street));
        }

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.City))
        {
            properties = properties.Where(a => a.City!.Contains(propertyFilterParams.City));
        }

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.PostCode))
        {
            properties = properties.Where(a => a.PostCode!.Contains(propertyFilterParams.PostCode));
        }

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.HouseNumber))
        {
            properties = properties.Where(a => a.Housenumber!.Contains(propertyFilterParams.HouseNumber));
        }

        if (!string.IsNullOrWhiteSpace(propertyFilterParams.PartnerCode))
        {
            properties = properties.Where(a => a.PartnerCode.Contains(propertyFilterParams.PartnerCode));
        }

        return properties;
    }
}