using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace TestProject.Hubs

{
    public class ActivityStatutHub : Hub
    {
        // Hubs API (from SignalR) enables us to call methods on connected clients from the server.
        // I moved this function to the controller (endpoints) in order to run some logic there (saving the changes in database)
      
        /* public async Task SendToAll(string user, string message)
         {
             await Clients.All.SendAsync("ReceiveMessage", user, message);
         }*/

    }
}
