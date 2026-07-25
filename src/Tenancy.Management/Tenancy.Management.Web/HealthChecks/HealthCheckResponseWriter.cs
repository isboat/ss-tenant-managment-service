using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Tenancy.Management.Web.HealthChecks;

public static class HealthCheckResponseWriter
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static Task WriteResponseAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            components = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    duration = entry.Value.Duration.TotalMilliseconds
                })
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
    }
}
