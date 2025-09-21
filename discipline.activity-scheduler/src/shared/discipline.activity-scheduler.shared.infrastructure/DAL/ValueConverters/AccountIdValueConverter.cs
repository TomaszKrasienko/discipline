using discipline.activity_scheduler.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.activity_scheduler.shared.infrastructure.DAL.ValueConverters;

internal sealed class AccountIdValueConverter() : ValueConverter<AccountId,string>(
    x => x.ToString(), y => AccountId.Parse(y));