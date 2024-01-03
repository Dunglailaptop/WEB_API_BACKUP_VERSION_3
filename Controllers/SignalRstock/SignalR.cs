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
using Microsoft.EntityFrameworkCore;
using MyCinema.Model;
using Org.BouncyCastle.Asn1.Ocsp;
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


public class responseNotifacation {
    public int numbernottifacation {get;set;}

    public int numberbill {get;set;} 
  
    public int numberbillfood {get;set;}
}
        public async Task SendNotifaction(string message,int role) {
              var responsedata = new responseNotifacation();
            if (role == 1) {
                var getmessage = _context.Notifaction.Where(x=>x.iduser == Convert.ToInt32(message)).ToList();
                var databill = _context.Bills.Where(x=>x.Iduser == Convert.ToInt32(message)).ToList();
                var databillfood = _context.FoodCombillPayment.Where(x=>x.iduser == Convert.ToInt32(message)).ToList();
                responsedata.numberbill = databill.Count;
                responsedata.numbernottifacation = getmessage.Count();
                responsedata.numberbillfood = databillfood.Count();
            }else if(role == 2) {
                // Lấy ngày hiện tại
                DateTime currentDate = DateTime.Now.Date;

                // Lấy dữ liệu từ 00:00:00 đến 23:59:59 của ngày hiện tại
                DateTime startDate = currentDate.Date; // 00:00:00
                DateTime endDate = currentDate.Date.AddDays(1).AddTicks(-1); // 23:59:59

                var dataget = _context.Notifaction
                .Where(x => x.datecreate >= startDate && x.datecreate <= endDate)
                        .ToList();
                           responsedata.numberbill = 0;
                responsedata.numbernottifacation = dataget.Count();
                responsedata.numberbillfood = 0;
            } else if (role == 3) {

            }
      
          
           
            await Clients.All.SendAsync("MESSAGENOTIFACTION",responsedata);
        }
        

        public async Task SendMessage(string message) {
            var datagetlist = _context.Accounts.Where(x=>x.Idusers == 1).SingleOrDefault();
            await Clients.All.SendAsync("RecevieMessage",datagetlist);
        }
    }
}