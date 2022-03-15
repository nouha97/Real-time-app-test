using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Data;

namespace TestProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Activity { get; set; }
    }
}


