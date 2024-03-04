namespace Tenancy.Management.Web.Services
{
    public class ChangeMessage
    {
        public string? DeviceId { get; set; }
        public string? TenantId { get; set; }

        public string? MessageType { get; set; }

        public string? MessageData { get; set; }

        public string? MessageStatus { get; set; }
    }
}
