namespace Tenancy.Management.Web.Services
{
    public class SignalRConstants
    {
        public const string ClientSideTargetEvent = "ReceiveChangeMessage";

        public const string AzureSignalRConnectionStringName = "AzureSignalRConnectionString";

        public const string HubName = "changelistenerhub";
    }

    public class SignalRMessageType
    {
        public const string DeviceInfoUpdate = "device.info.update";
        public const string ContentPublish = "content.publish";
        public const string AppRestart = "app.restart";
        public const string AppTerminate = "app.terminate";
        public const string AppUpgradeInfo = "app.upgrade.info";
        public const string OperatorInfo = "operator.info";
        public const string AppUpgradeForce = "app.upgrade.force";
    }

    public class SignalrExtensions
    {
        public static string ToSignalRUserId(string deviceId)
        {
            return $"device_id_{deviceId}";
        }

        public static string ToGroupName(string tenantId)
        {
            return $"grp-{tenantId}";
        }
    }
}
