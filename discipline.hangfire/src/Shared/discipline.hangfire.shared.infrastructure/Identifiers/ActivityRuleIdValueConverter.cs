using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace discipline.hangfire.infrastructure.Identifiers;

public sealed class ActivityRuleIdValueConverter() : ValueConverter<ActivityRuleId, string>
    (id => id.ToString(), val => ActivityRuleId.Parse(val));  
