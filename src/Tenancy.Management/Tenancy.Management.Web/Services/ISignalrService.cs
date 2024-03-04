using Microsoft.Azure.SignalR.Management;

namespace Tenancy.Management.Web.Services
{
    public interface ISignalrService
    {
        Task<ServiceHubContext> GetHubContextAsync();

        Task SendMessage(ChangeMessage changeMessage);
    }
}
