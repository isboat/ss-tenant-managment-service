namespace Tenancy.Management.Models
{
    public interface IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }
    }
}
