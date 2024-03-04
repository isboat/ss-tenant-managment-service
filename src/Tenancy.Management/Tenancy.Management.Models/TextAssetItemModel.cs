using MongoDB.Bson.Serialization.Attributes;

namespace Tenancy.Management.Models
{
    [BsonIgnoreExtraElements]
    public class TextAssetItemModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? BackgroundColor { get; set; }

        public string? TextColor { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
