using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.shared.infrastructure.IdentityContext.Abstractions;

public interface IIdentityContext
{
    public bool IsAuthenticated { get; }
    public string? Status { get; }
    public UserId? GetUser();
}