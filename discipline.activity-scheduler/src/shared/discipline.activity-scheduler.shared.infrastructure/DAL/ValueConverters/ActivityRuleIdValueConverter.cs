using discipline.activity_scheduler.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.activity_scheduler.shared.infrastructure.DAL.ValueConverters;

internal sealed class ActivityRuleIdValueConverter() : ValueConverter<ActivityRuleId, string>
    (id => id.ToString(), val => ActivityRuleId.Parse(val));  
