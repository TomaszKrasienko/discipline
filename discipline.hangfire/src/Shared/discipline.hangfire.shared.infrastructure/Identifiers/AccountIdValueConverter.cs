using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.hangfire.infrastructure.Identifiers;

public sealed class AccountIdValueConverter() : ValueConverter<AccountId, string>(
    id => id.ToString(), val => AccountId.Parse(val));