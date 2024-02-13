using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenancy.Management.Models
{
    [BsonIgnoreExtraElements]
    public class DeviceAuthModel
    {
        public DateTime? RegisteredDatetime { get; set; }

        public DateTime? ApprovedDatetime { get; set; }

        public string? TenantId { get; set; }
        public string? Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? UserCode { get; set; }
        public int? ExpiresIn { get; set; }
        public int? Interval { get; set; }
    }
}
