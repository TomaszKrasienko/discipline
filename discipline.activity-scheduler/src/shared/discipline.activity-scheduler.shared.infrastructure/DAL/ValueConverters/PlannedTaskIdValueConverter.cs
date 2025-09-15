using discipline.activity_scheduler.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.activity_scheduler.shared.infrastructure.DAL.ValueConverters;

internal sealed class PlannedTaskIdValueConverter() : ValueConverter<PlannedTaskId, string>(
    x => x.ToString(), y => PlannedTaskId.Parse(y));