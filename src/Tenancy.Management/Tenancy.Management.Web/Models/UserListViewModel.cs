using Tenancy.Management.Models;

namespace Tenancy.Management.Web.Models
{
    public class UserListViewModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public string TenantId { get; set; }
    }
}
