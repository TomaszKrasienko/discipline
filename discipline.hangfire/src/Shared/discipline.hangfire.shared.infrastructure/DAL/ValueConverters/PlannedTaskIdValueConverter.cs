using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.hangfire.infrastructure.DAL.ValueConverters;

internal sealed class PlannedTaskIdValueConverter() : ValueConverter<PlannedTaskId, string>(
    x => x.ToString(), y => PlannedTaskId.Parse(y));