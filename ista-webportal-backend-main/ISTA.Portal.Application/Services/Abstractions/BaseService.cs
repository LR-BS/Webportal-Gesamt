using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;

namespace ISTA.Portal.Application.Services.Abstractions;

public abstract class BaseService
{
    protected readonly VDMAdminDbContext dbContext;

    public BaseService(VDMAdminDbContext _dbContext)
    {
        dbContext = _dbContext;
        dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
    }
}