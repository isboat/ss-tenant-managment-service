using MongoDB.Bson.Serialization.Attributes;

namespace Tenancy.Management.Models
{

    [BsonIgnoreExtraElements]
    public class MenuModel: IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; } 

        public string? Title { get; set; }

        public string? Currency { get; set; }

        public string? IconUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
