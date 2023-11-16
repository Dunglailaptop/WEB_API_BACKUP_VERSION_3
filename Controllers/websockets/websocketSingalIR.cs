using System.Drawing.Printing;
using Microsoft.AspNetCore.SignalR;

namespace webapiserver.Controllers
{
    public class ChatHub : Hub
    {
        public async Task EnviarMensaje(string user, string message)
        {
            
        await Clients.All.SendAsync("Respuesta de SignalR: ", user, message);
        }

           public async Task SendFromServer(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
public string GetMessage(string message)
{
    return "You sent: " + message;
}

    }
}