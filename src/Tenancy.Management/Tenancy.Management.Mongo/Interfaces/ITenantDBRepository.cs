using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenancy.Management.Models;

namespace Tenancy.Management.Mongo.Interfaces
{
    public interface ITenantDBRepository<T> : IRepository<T>
    {
        public void CreateDB(string dbName);
    }
}
