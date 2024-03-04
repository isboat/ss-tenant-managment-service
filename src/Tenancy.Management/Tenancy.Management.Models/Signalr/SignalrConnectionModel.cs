namespace Tenancy.Management.Models.Signalr
{
    public class SignalrConnectionModel: IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public DateTime? ConnectionDateTime { get; set; }
    }

    public class SignalrInfoMessage
    {
        public string? Message { get; set; }
    }
}
