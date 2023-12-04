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
public class interestMovieController : ControllerBase
{
        private readonly CinemaContext _context;
        public interestMovieController(CinemaContext context)
        {
            _context = context;
            
        }
//API GET LIST ROOM
[HttpGet("getlistInterestWithRoom")]
public IActionResult getlistInterestWithRoom(string date,int idroom)
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
            if (date != null && idroom != null){
                
                   INTERESTCINEMA us = new INTERESTCINEMA();
               try
                 {
                      string sql = "select DISTINCT m.Namemovie as Namecinema,m.Idmovie as Idcinema from cinema.Cinema c inner join cinema.Room r on r.Idcinema = c.Idcinema inner join cinema.cinemainterest t on t.Idroom = r.Idroom inner join cinema.Movie m on m.Idmovie = t.Idmovie where t.Dateshow = '"+date+"' and t.Idroom = '"+idroom+"';";
                   var dataget = _context.LISTCINEMA.FromSqlRaw(sql).AsEnumerable().ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API GET LIST INTEREST OF ROOM
[HttpGet("getListInterestRoomMovie")]
public IActionResult getListInterestRoomMovie(string date,int idroom)
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
            if (date != null && idroom != null){
                
                   INTERESTCINEMA us = new INTERESTCINEMA();
               try
                 {
                      string sql = "select m.Namemovie as Namecinema,DATE_FORMAT(t.Times, '%H:%i') AS times,t.Dateshow,t.Idinterest,m.Idmovie,t.Idroom,ci.Idcinema from cinema.cinemainterest t inner join cinema.Movie m on m.Idmovie = t.Idmovie inner join cinema.Room r on r.Idroom = t.Idroom inner join cinema.Cinema ci on ci.Idcinema = r.Idcinema where t.Dateshow = '"+date+"' and t.Idroom = '"+idroom+"';";
                   var dataget = _context.INTERESTCINEMA.FromSqlRaw(sql).AsEnumerable().ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}

// API GET LIST INTEREST OF CINEMA
[HttpGet("getListInterestCinema")]
public IActionResult getListInterestCinema(string date,int Idmovie)
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
            if (date != null && Idmovie != null){
                
                   INTERESTCINEMA us = new INTERESTCINEMA();
               try
                 {
                      string sql = "call cinema.getListInterestMovie(@p0,@p1)";
                   var dataget = _context.INTERESTCINEMA.FromSqlRaw(sql, date,Idmovie).AsEnumerable().ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API GET LIST CINEMA 
[HttpGet("getlistcinema")]
public IActionResult getListCinema(string date,int Idmovie)
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
            if (date != null && Idmovie != null){
                
                   INTERESTCINEMA us = new INTERESTCINEMA();
               try
                 {
                      string sql = "CALL cinema.getListCinema(@p0,@p1)";
                   var dataget = _context.LISTCINEMA.FromSqlRaw(sql, date,Idmovie).AsEnumerable().ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("khong tim thay thong tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}



public class INTERESTCINEMA
{
    public string Namecinema { get; set; }
    public string Times { get; set; }
    public long Idmovie { get; set; }
    public long Idcinema { get; set; }
    public long Idroom { get; set; }
    public int Idinterest { get; set; }
    
}


}
