using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace TestProject
{
    public class ActivityStatutHub : Hub
    {
        public async Task SendMessage(ActivityStatut message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

    }
}
