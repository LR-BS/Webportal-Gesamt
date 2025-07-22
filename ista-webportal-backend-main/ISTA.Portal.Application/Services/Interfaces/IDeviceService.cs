using ISTA.Portal.API.Application;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.Application.Services.Interfaces;

    public interface IDeviceService
    {
        public Task<List<Device>> GetDevices(CancellationToken ct);
        Task<List<Device>> FixMigrationErrors(CancellationToken ct);
        Task<List<DeviceErrorsDto>> ListDevicesWithErrors(CancellationToken ct);
    }
