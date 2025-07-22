using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services.Interfaces;

public interface ITenantService
{
    Task<List<TenantDto>> ListTenantsWithErrors(CancellationToken ct);
    Task<List<Tenant>> FixMigrationErrors(CancellationToken ct);
}