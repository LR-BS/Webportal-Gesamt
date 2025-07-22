using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Application;

public record DeviceListDto
(
    Guid Id,
    string DeviceNumber,
    string ArticleNumber,
    string DeviceSerialNumber,
    Guid DevicePositionUUID,
    Guid ConsumptionUnitId,
    Boolean Active,
    DeviceMigrationStatus? MigrationStatus)
{
    public static DeviceListDto Create(Device device)
    {
        return new DeviceListDto
                       (
                         device.Id,
                         device.DeviceNumber!,
                         device.ArticleNumber,
                         device.DeviceSerialNumber!,
                         (Guid)device.DevicePositionUUID!,
                         device.ConsumptionUnitId,
                         device.Active,
                         device.MigrationStatus
                       );
    }
}