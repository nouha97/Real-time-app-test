using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
   
        public class Element
        {
        [Key]
        public string ID { get; set; }
        public string Activity { get; set; }
        public string Name { get; set; }
    }
    }


   