using Tenancy.Management.Models;

namespace Tenancy.Management.Web.Models
{
    public class TenantViewModel
    {
        public TenantModel Tenant { get; set; }

        public IEnumerable<UserModel> Users { get; set; } = new List<UserModel>();

        public int UserCount => Users.Count();

        public IEnumerable<AssetModel> Assets { get; set; } = new List<AssetModel>();

        public int AssetCount => Assets.Count();

        public IEnumerable<MenuModel> Menus { get; set; } = new  List<MenuModel>();
        public IEnumerable<TextAssetItemModel> TextAssets { get; set; } = new List<TextAssetItemModel>();
    }

    public class TenantListViewModel
    {
        public List<TenantViewModel> Tenants { get; set; }
    }
}
