using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.hangfire.infrastructure.DAL.ValueConverters;

internal sealed class AccountIdValueConverter() : ValueConverter<AccountId,string>(
    x => x.ToString(), y => AccountId.Parse(y));