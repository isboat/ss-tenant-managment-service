# OnScreenSync tenant management service

This service is the central administration portal for OnScreenSync operators. A tenant is an organisation with its own users, signage content, and displays. The portal supports onboarding and support across organisations and partners, while tenant users manage their day-to-day signage in the dashboard.

## What operators can do

- Create, view, update, and remove tenants, and manage their users.
- Maintain partner records and see which tenants are associated with a partner.
- Review registration records and device activation requests. The portal also supports invitations and related email workflows.
- Inspect a tenant's TV apps and send supported remote actions such as displaying an operator message, restarting or terminating the app, or requesting an upgrade.
- Check the availability of the database, email, and messaging dependencies through the health endpoint.

## How it is organised

The [Tenancy Management solution](../src/Tenancy.Management) is a server-rendered ASP.NET Core MVC application. Tenancy.Management.Web contains the pages, views, and controllers. Tenancy.Management.Services contains business workflows, Tenancy.Management.Mongo handles storage, and Tenancy.Management.Models defines the records displayed and edited in the portal. A separate test project covers service behavior.

## What it relies on

- **MongoDB** holds tenant, partner, user, device, and registration records. These records support the workflows used by the dashboard, partner portal, and display services.
- **SMTP**, through MailKit, sends invitations and related mail. **Azure SignalR Service** delivers operator actions to connected TV players.
- **ASP.NET Core MVC on .NET 8** provides the portal and authentication. The [README](../README.md) describes the service configuration.
