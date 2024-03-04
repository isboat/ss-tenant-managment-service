using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Tenancy.Management.Web.Services
{
    public class SignalrService: ISignalrService
    {

        private readonly string _connectionString;
        private readonly ServiceTransportType _serviceTransportType;
        private ServiceHubContext _hubContext;

        public SignalrService(string connectionString, ServiceTransportType serviceTransportType)
        {
            _connectionString = connectionString;
            _serviceTransportType = serviceTransportType;

            _ = InitAsync();
        }

        private async Task InitAsync()
        {
            var serviceManager = new ServiceManagerBuilder().WithOptions(option =>
            {
                option.ConnectionString = _connectionString;
                option.ServiceTransportType = _serviceTransportType;
            })
            .BuildServiceManager();

            _hubContext = await serviceManager.CreateHubContextAsync(SignalRConstants.HubName, default);
        }

        public async Task<ServiceHubContext> GetHubContextAsync()
        {
            if(_hubContext == null) await InitAsync();
            return _hubContext!;
        }

        public Task SendMessage(ChangeMessage changeMessage)
        {
            try
            {
                var message = JsonConvert.SerializeObject(changeMessage, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                if (!string.IsNullOrEmpty(changeMessage.DeviceId))
                {
                    var userId = SignalrExtensions.ToSignalRUserId(changeMessage.DeviceId);
                    _hubContext.Clients.User(userId).SendAsync(SignalRConstants.ClientSideTargetEvent, message);
                }

                if (!string.IsNullOrEmpty(changeMessage.TenantId))
                {
                    var grpName = SignalrExtensions.ToGroupName(changeMessage.TenantId);
                    _hubContext.Clients.Group(grpName).SendAsync(SignalRConstants.ClientSideTargetEvent, message);
                }

                if (string.IsNullOrEmpty(changeMessage.TenantId) && string.IsNullOrEmpty(changeMessage.DeviceId))
                {
                    _hubContext.Clients.All.SendAsync(SignalRConstants.ClientSideTargetEvent, message);
                }
            }
            catch (Exception ex)
            {
                // log
            }

            return Task.CompletedTask;
        }
    }
}
