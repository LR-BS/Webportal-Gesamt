using SharedKernel.Domain;
using SharedKernel.Enums;

namespace ISTA.Portal.API.Application;

public record DeviceErrorsDto(
    Guid Id,
    string DeviceNumber,
    string ArticleNumber,
    string DeviceSerialNumber,
    Guid DevicePositionUUID,
    Guid ConsumptionUnitId,
    Boolean Active,
    DeviceMigrationStatus? MigrationStatus,
    string PropertyNumber
)
{
public static DeviceErrorsDto Create(Device device)
    {
        return new DeviceErrorsDto
        (
            device.Id,
            device.DeviceNumber!,
            device.ArticleNumber,
            device.DeviceSerialNumber!,
            (Guid)device.DevicePositionUUID!,
            device.ConsumptionUnitId,
            device.Active,
            device.MigrationStatus,
            device.ConsumptionUnit.Property.PropertyNumber
        );
    }
}