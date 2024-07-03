namespace Tenancy.Management.Models
{
    public class TenantAuthorization
    {
        public const string RequiredPolicy = "TenancyManagement";
        public const string RequiredScope = "tenancy.management.content";


        public const string RequiredCorsPolicy = "allowSpecificOrigins";
    }
}
