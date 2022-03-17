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
using TestProject.Hubs;
using TestProject.Models;


namespace TestProject.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
       
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UsersController> _logger;
        protected readonly IHubContext<ActivityStatutHub> _messageHub;
        public UsersController(ApplicationDbContext db , ILogger<UsersController> logger , [NotNull] IHubContext<ActivityStatutHub> messageHub)
        {
            _db = db;
            _logger = logger;
            _messageHub = messageHub;
        }


        [HttpPost]
        [Route("api/ChangeStatut")]
        public async Task SendToAll([FromBody] ActivityStatut a)
        {
            // this function update the activity in database 
            saveStatus(a.ID, a.ActivityS);
            //Clients methos calls a method on specific connected clients but I use "All" to specify the method on all connected clients
            await _messageHub.Clients.All.SendAsync("ReceiveMessage", a.ActivityS, a.ID);
        }
       
     

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Getting all the users from db
            IEnumerable <ApplicationUser> users = _db.Users.Select(w => w);
            // Getting the ID of the connected user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            return Ok(new { users, userId });
           

        }

        public void saveStatus(string id, string activity)
        {
            var user = _db.Users.FirstOrDefault(item => item.Id == id);


            if (user != null)
            {
                //updating the activity
                user.Activity = activity;

                _db.SaveChanges();
            }
        }
    }
}
