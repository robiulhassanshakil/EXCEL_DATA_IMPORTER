using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Membership.Entities;

namespace DataImporter.Membership.Seeds
{
    public static class DataSeed
    {
        public static Role[] Roles
        {
            get
            {
                return new Role[]
                {
                    new Role { Id = Guid.NewGuid(), Name = "Importer", NormalizedName = "IMPORTER", ConcurrencyStamp = Guid.NewGuid().ToString() },
                };
            }
        }
    }
}
