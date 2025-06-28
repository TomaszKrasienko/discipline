using Hangfire;

namespace discipline.hangfire.server.Hangfire;

internal static class HangfireApplicationConfigurationExtensions
{
    internal static WebApplication UseDisciplineHangfireServer(this WebApplication app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        {
            DashboardTitle = "Discipline hangfire",
            AppPath = "/hangfire",
            DarkModeEnabled = true
        });
        return app;
    }
}