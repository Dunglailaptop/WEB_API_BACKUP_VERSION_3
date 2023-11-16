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
public class EmployeeController : ControllerBase
{
        private readonly CinemaContext _context;
        public EmployeeController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST EMPLOYEE
[HttpGet("getlistemployee")]
public IActionResult getListemployee(int iduser,string? keysearch,int idcinema)
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
            if (iduser != null && idcinema != null){
                
                  
               try
                 {
                  string keysearc = keysearch == null ? "":keysearch;
                      string sql = "call cinema.GetListEmployee(@p0,@p1,@p2)";
                   var dataget = _context.USERS.FromSqlRaw(sql,iduser,keysearc,idcinema).AsEnumerable().ToList();
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
// API UPDATE EMPLOYEE
[HttpPost("Createnewemployee")]
public IActionResult CreateNewemployee([FromBody] USERS us)
{
    // khoi tao api response
    var successApiResponse = new ApiResponse();
    //header
       string token = Request.Headers["token"];
       string filterHeaderValue2 = Request.Headers["ProjectId"];
       string filterHeaderValue3 = Request.Headers["Method"];
       string expectedToken = ValidHeader.Token;
       string method =Convert.ToString(ValidHeader.MethodPost);
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
            if (us.Fullname != null && us.Email != null && us.phone != null && us.Birthday != null && us.Idrole != null){
                
                  
               try
                 {
                      string sql = "call cinema.insertIntoEmployee(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
                   _context.Database.ExecuteSqlRaw(sql,us.Fullname,us.Email,us.phone,us.Birthday,us.Idrole,us.avatar,us.gender,us.Idcinema,us.address);
                      _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = "null";
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("Vui lòng nhập đầy đủ thông tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API LOCK EMPLOYEE
[HttpGet("Lockemployee")]
public IActionResult Lockemployee( int idemployee, int statuss)
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
            if (idemployee != null && statuss != null){
                
                  
               try
                 {
                      string sql = "call cinema.LockEmployee(@p0,@p1)";
                   _context.Database.ExecuteSqlRaw(sql,idemployee,statuss);
                   _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = "";
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("Vui lòng nhập đầy đủ thông tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API RESET PASSWORD EMPLOYEE
[HttpGet("resetpasswordemployee")]
public IActionResult resetpasswordemployee(int idemployee)
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
            if (idemployee != null){
                
                  
               try
                 {
                      string sql = "call cinema.resetPasswordEmployee(@p0)";
                   _context.Database.ExecuteSqlRaw(sql,idemployee);
                   _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = "null";
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            }else {
                return BadRequest("Vui lòng nhập đầy đủ thông tin");
            }
                 

           }

        }
 return Ok(successApiResponse);
}

// API get list role
[HttpGet("getlistrole")]
public IActionResult getlistrole()
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
            // if (idemployee != null){
                
                  
               try
                 {
                    var datarole = _context.Roles.ToList();
                  
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = datarole;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
            // }else {
            //     return BadRequest("Vui lòng nhập đầy đủ thông tin");
            // }
                 

           }

        }
 return Ok(successApiResponse);
}

}
