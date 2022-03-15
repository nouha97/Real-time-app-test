using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Data;
using TestProject.Models;
using TestProject.Models.TestProject.Models;

namespace TestProject.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ApplicationDbContext db;
        private readonly ILogger<WeatherForecastController> _logger;
        protected readonly IHubContext<ActivityStatutHub> _messageHub;
        public WeatherForecastController(ApplicationDbContext db , ILogger<WeatherForecastController> logger , [NotNull] IHubContext<ActivityStatutHub> messageHub)
        {
            this.db = db;
            _logger = logger;
            _messageHub = messageHub;
        }

     
        [HttpPost]
        [Route("api/ChangeStatut")]
        public async Task<IActionResult> Create(ActivityStatut messagePost)
            {

                await _messageHub.Clients.All.SendAsync("sendToReact", "The message '" +
                messagePost.ActivityS + "' has been received");
                return Ok();
            }
        

        [HttpGet]

        public IEnumerable<ApplicationUser> Get()
        {

            return db.Users.Select(w => w);

        }
    }
}
