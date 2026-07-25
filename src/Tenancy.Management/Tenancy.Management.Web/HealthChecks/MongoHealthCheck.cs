using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Tenancy.Management.Models;

namespace Tenancy.Management.Web.HealthChecks;

public sealed class MongoHealthCheck(IOptions<MongoSettings> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var settings = options.Value;
        if (string.IsNullOrWhiteSpace(settings.ConnectionString) ||
            string.IsNullOrWhiteSpace(settings.DatabaseName))
        {
            return HealthCheckResult.Unhealthy("MongoDB configuration is missing.");
        }

        try
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            await database.RunCommandAsync<BsonDocument>(
                new BsonDocument("ping", 1),
                cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy("MongoDB accepted a ping command.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("MongoDB did not respond to a ping command.", exception);
        }
    }
}
