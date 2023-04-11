using Tenancy.Management.Models;

namespace Tenancy.Management.Web.Models
{
    public class TenantViewModel
    {
        public TenantModel Tenant { get; set; }

        public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();

        public int UserCount => Users.Count();
    }
    public class TenantListViewModel
    {
        public List<TenantViewModel> Tenants { get; set; }
    }
}
