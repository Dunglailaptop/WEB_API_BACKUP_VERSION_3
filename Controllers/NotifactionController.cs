using System.Resources;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
// using MySql.Data.EntityFrameworkCore;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class NotifactionController : ControllerBase
{
        private readonly CinemaContext _context;
        public NotifactionController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST VOUCHER
[HttpGet("getNotifactionInUser")]
public IActionResult getNotifactionInUser(long iduser,int type)
{
    // khoi tao api response
    var successApiResponse = new ApiResponse();
    //header
       string token = Request.Headers["token"];
       string filterHeaderValue2 = Request.Headers["ProjectId"];
       string filterHeaderValue3 = Request.Headers["Method"];
       string expectedToken = ValidHeader.Token;
       string method =Convert.ToString(ValidHeader.MethodGet);
       string Pojectid = Convert.ToString(ValidHeader.Project_id);
    //check header
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
        // The "Authorize" header was not found in the request
           return BadRequest("Authorize header not found in the request.");
        }else {

            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
          {
            return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
          }else{
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
                    if (type == 1){
                     var dataget = _context.Notifaction.Where(x=>x.iduser == iduser).ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                    }else if (type == 2) {
                              // Lấy ngày hiện tại
                        DateTime currentDate = DateTime.Now.Date;

                        // Lấy dữ liệu từ 00:00:00 đến 23:59:59 của ngày hiện tại
                        DateTime startDate = currentDate.Date; // 00:00:00
                        DateTime endDate = currentDate.Date.AddDays(1).AddTicks(-1); // 23:59:59

                        var dataget = _context.Notifaction
                        .Where(x => x.datecreate >= startDate && x.datecreate <= endDate)
                        .ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                    }
             
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            // }else {
            //     return BadRequest("khong tim thay thong tin");
            // }
                 

           }

        }
 return Ok(successApiResponse);
}


}
