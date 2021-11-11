using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace DataImporter.Membership.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
