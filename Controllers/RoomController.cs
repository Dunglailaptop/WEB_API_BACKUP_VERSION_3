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
public class RoomController : ControllerBase
{
        private readonly CinemaContext _context;
        public RoomController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST room
[HttpGet("getlistRoom")]
public IActionResult getListRoom(int idCinema)
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
            if (idCinema != null){
                
                  
               try
                 {
                     
                     var data = _context.Rooms.Where(x=>x.Idcinema == idCinema).ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("Vui Lòngg truyền Id rạp chiếu phim");
            }
                 

           }

        }
 return Ok(successApiResponse);
}



}
