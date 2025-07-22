namespace ISTA.Portal.Application;

public record ConsumptionReportDto
(
  Guid Id,
  Guid DevicePositionUUID,
  DateTime Date,
  Boolean IsExtrapolated,
  int HeatingDegreeDays,
  double HGTPercentage,
  int? ExtrapolationFailureCause,
  double HGTAdjusted,
  int UpdateState,
  double LastValue,
  DateTime CreateDate,
  double MonthlyConsumption,
  int ActionIndex
);