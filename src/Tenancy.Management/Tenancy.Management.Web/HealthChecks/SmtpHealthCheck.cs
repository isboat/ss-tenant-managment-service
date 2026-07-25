using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Tenancy.Management.Models;

namespace Tenancy.Management.Web.HealthChecks;

public sealed class SmtpHealthCheck(IOptions<EmailSettings> options) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var settings = options.Value;
        if (string.IsNullOrWhiteSpace(settings.Host) ||
            settings.Port is <= 0 or > 65535 ||
            string.IsNullOrWhiteSpace(settings.FromAddress) ||
            string.IsNullOrWhiteSpace(settings.Passkey))
        {
            return HealthCheckResult.Unhealthy("SMTP configuration is missing or invalid.");
        }

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(
                settings.Host,
                settings.Port,
                SecureSocketOptions.SslOnConnect,
                cancellationToken);
            await client.AuthenticateAsync(
                settings.FromAddress,
                settings.Passkey,
                cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            return HealthCheckResult.Healthy("The SMTP server accepted the configured credentials.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("The SMTP server connection or authentication failed.", exception);
        }
    }
}
