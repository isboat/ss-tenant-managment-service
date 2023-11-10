using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenancy.Management.Models
{
    public class TenantModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? Telephone { get; set; }

        public string? Country { get; set; }

        public string? Postcode { get; set; }

        public string? Note { get; set; }

        public int TvAppsLimit { get; set; } = 3;

        public int UsersLimit { get; set; } = 5;

        public DateTime Created { get; set; }
    }
}
