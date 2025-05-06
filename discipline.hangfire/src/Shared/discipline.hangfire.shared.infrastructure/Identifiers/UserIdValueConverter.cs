using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.hangfire.infrastructure.Identifiers;

public sealed class UserIdValueConverter() : ValueConverter<UserId, string>(
    id => id.ToString(), val => UserId.Parse(val));