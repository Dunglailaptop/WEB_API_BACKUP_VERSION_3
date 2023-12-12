using Microsoft.AspNetCore.SignalR;

namespace webapiserver.Controllers.stockHub
{
   public class stockHub: Hub
   {
    public override async Task OnConnectedAsync() {
        await Clients.All.SendAsync("RecevieMessage",$"{Context.ConnectionId} has connected");
    }
   }

}
