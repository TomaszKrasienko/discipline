// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class AuthWebAppConfigurationExtensions
{
    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}