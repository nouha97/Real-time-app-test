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
            Debug.WriteLine("rrrrrdefzg" + a.ActivityS + "eeee");
            await _messageHub.Clients.All.SendAsync("ReceiveMessage", a.ActivityS, a.ActivityS);
        }
       
       /* [HttpPost]
        [Route("api/ChangeStatut")]
        public async Task<IActionResult> Create(ActivityStatut messagePost)
            {

                await _messageHub.Clients.All.SendAsync("sendToReact", "The message '" +
                messagePost.ActivityS + "' has been received");
                return Ok();
            }
        */

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            IEnumerable <ApplicationUser> users = db.Users.Select(w => w);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            return Ok(new { users, userId });
           

        }
    }
}
