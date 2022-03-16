using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestProject.Data;
using TestProject.Models;
using TestProject.Models.TestProject.Models;

namespace TestProject.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
       
        private readonly ApplicationDbContext db;
        private readonly ILogger<UsersController> _logger;
        protected readonly IHubContext<ActivityStatutHub> _messageHub;
        public UsersController(ApplicationDbContext db , ILogger<UsersController> logger , [NotNull] IHubContext<ActivityStatutHub> messageHub)
        {
            this.db = db;
            _logger = logger;
            _messageHub = messageHub;
        }


        [HttpPost]
        [Route("api/ChangeStatut")]

        public async Task SendToAll([FromBody] ActivityStatut a)
        {
            saveStatus(a.ID, a.ActivityS);
            await _messageHub.Clients.All.SendAsync("ReceiveMessage", a.ActivityS, a.ID);
        }
       
       public void saveStatus(string id,string activity)
        {
            var user = db.Users.FirstOrDefault(item => item.Id == id);

           
            if (user != null)
            {
                
                user.Activity = activity;

                db.SaveChanges();
            }
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            IEnumerable <ApplicationUser> users = db.Users.Select(w => w);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            return Ok(new { users, userId });
           

        }
    }
}
