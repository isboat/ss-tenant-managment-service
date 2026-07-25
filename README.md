# ss-tenant-managment-service

Screen service tenant management service.

## Architecture

The solution is an ASP.NET Core MVC application targeting .NET 8. It is split into Web, Services, Mongo, and Models projects.

## Configuration

Configure secrets outside source control using environment variables, user secrets, or a secret store:

- `MongoSettings:ConnectionString`
- `EmailSettings:*`
- `AuthSettings:Username`
- `AuthSettings:Passwd`
- `AuthSettings:PasswordPepper`
- `AuthSettings:InviteTokenExpiryHours`
- `AzureSignalRConnectionString`

`AuthSettings:PasswordPepper` is required because password hashes and invite token hashes are derived with it.

## Local development

```bash
dotnet restore src/Tenancy.Management/Tenancy.Management.sln
dotnet build src/Tenancy.Management/Tenancy.Management.sln
dotnet run --project src/Tenancy.Management/Tenancy.Management.Web/Tenancy.Management.Web.csproj
```

## Health monitoring

`GET /health` returns HTTP 200 when MongoDB and Azure SignalR are reachable and the
configured SMTP server accepts the application's credentials. It returns HTTP 503
when any dependency is unhealthy.
The JSON response includes the overall status, total check duration, and the status,
description, and duration of every dependency check. The endpoint is anonymous so
that an external dashboard or container orchestrator can call it. Unlike application
routes, `/health` is not redirected to HTTPS, allowing an HTTP container probe to
receive the actual 200 or 503 result.

## Security notes

- Do not commit production secrets.
- User creation now issues a hashed invite token record instead of storing a shared temporary password.
- Destructive user actions should use POST forms with anti-forgery tokens.
- Tenant-owned resources should always be queried using both tenant ID and resource ID.
