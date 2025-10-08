namespace discipline.daily_trackers.infrastructure.IdentityContext.Abstractions;

public interface IIdentityContextFactory
{
    IIdentityContext Create();
}