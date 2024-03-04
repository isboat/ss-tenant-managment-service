using MongoDB.Bson.Serialization.Attributes;

namespace Tenancy.Management.Models
{
    [BsonIgnoreExtraElements]
    public class AssetModel: IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? AssetUrl { get; set; }

        public string? FileName { get; set; }

        public AssetType? Type { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public enum AssetType
    {
        None,
        Image,
        Video
    }
}
