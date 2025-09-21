using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace discipline.centre.shared.infrastructure.Messaging.Outbox.DAL;

internal sealed class UlidTypeConverter() : ValueConverter<Ulid, string>(
    x => x.ToString(), y => Ulid.Parse(y));