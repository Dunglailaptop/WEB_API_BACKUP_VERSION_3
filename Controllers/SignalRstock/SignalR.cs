// using Microsoft.AspNetCore.SignalR;

// namespace webapiserver.Controllers.stockHub
// {
//    public class stockHub: Hub
//    {
//     public Task OnConnectedAsync() {
//         return Clients.All.SendAsync("RecevieMessage",$"{Context.ConnectionId} has connected");
//     }
//    }

// }
using Microsoft.AspNetCore.SignalR;
using MyCinema.Model;
namespace webapiserver.Controllers
{
    public class OrderHub : Hub
    {
        // public async Task TestMe(string someRandomText)
        // {
        //     await Clients.All.SendAsync(
        //         $"{this.Context.User.Identity.Name} : {someRandomText}", 
        //         CancellationToken.None);
        // }
        private readonly CinemaContext _context;
        public OrderHub(CinemaContext context)
         {
              _context = context;
        }
            public async Task TestMe(string someRandomText)
            {
            await Clients.All.SendAsync(
            $"{this.Context.User.Identity.Name} : {someRandomText}",
            CancellationToken.None);
            }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("RecevieMessage",$"{Context.ConnectionId} has connected");
        }

        public async Task SendMessage() {
            var datagetlist = _context.Accounts.Where(x=>x.Idusers == 1).SingleOrDefault();
            await Clients.All.SendAsync("RecevieMessage",datagetlist);
        }
    }
}