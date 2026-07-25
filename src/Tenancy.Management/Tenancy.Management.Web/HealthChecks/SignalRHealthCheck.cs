using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Tenancy.Management.Web.Services;

namespace Tenancy.Management.Web.HealthChecks;

public sealed class SignalRHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var connectionString = configuration[SignalRConstants.AzureSignalRConnectionStringName];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return HealthCheckResult.Unhealthy("Azure SignalR configuration is missing.");
        }

        try
        {
            using var serviceManager = (IServiceManager)new ServiceManagerBuilder()
                .WithOptions(options => options.ConnectionString = connectionString)
                .BuildServiceManager();

            var isHealthy = await serviceManager.IsServiceHealthy(cancellationToken);
            return isHealthy
                ? HealthCheckResult.Healthy("Azure SignalR reported that the service is healthy.")
                : HealthCheckResult.Unhealthy("Azure SignalR reported that the service is unhealthy.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("Azure SignalR could not be reached.", exception);
        }
    }
}
