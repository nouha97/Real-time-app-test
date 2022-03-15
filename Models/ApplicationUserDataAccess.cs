using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    using global::TestProject.Data;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    

    namespace TestProject.Models
    {
        public class ApplicationUserDataAccess
        {

            private readonly UserManager<ApplicationUser> userManager;
            public ApplicationUserDataAccess(UserManager<ApplicationUser> userManager)
            {
                this.userManager = userManager;
            }
            public IEnumerable<ApplicationUser> Get()
            {
                try
                {
                    return userManager.Users;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}

   