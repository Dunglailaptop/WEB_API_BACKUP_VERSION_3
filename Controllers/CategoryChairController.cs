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
public class CategoryChairController : ControllerBase
{
        private readonly CinemaContext _context;
        public CategoryChairController(CinemaContext context)
        {
            _context = context;
            
        }


// API GET LIST CHAIR in room
[HttpGet("getDetailCategoryChair")]
public IActionResult getDetailCategoryChair(int idcategory)
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
           
                
                  
               try
                 {
                     categorychairs category = new categorychairs();
                     var dataget = _context.Categorychairs.Where(x=>x.Idcategorychair == idcategory).SingleOrDefault();
                     category.colorchair = dataget.Colorchair;
                     category.idcategoryChair = dataget.Idcategorychair;
                     category.namecategory = dataget.Namecategorychair;
                     category.price = dataget.Price;
                     category.idroom = dataget.Idroom;
                     var dataroom = _context.Rooms.Where(x=>x.Idroom == dataget.Idroom).SingleOrDefault();
                     category.nameroom = dataroom.Nameroom;
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = category;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
           
                 

           }

        }
 return Ok(successApiResponse);
}

public class listidchair {
   public int idchair {get;set;}
}

public class categorychairofroom {
   public int idcategory {get;set;}
    public List<listidchair> listchair {get;set;}
}

// API GET LIST CHAIR in room
[HttpPost("updateCategoryChairInChairWithRoom")]
public IActionResult updateCategoryChairInChairWithRoom([FromBody] categorychairofroom cate)
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
           
                
                  
               try
                 {
                  List<Chair> chairlist = new List<Chair>();
                     foreach (var item in  cate.listchair ) {
                        var datachair = _context.Chairs.Find(item.idchair);
                        datachair.Idcategorychair = cate.idcategory;
                        _context.Chairs.Update(datachair);
                        _context.SaveChanges();
                        chairlist.Add(datachair);
                     }
                     successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = chairlist;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
           
                 

           }

        }
 return Ok(successApiResponse);
}

// API GET LIST CHAIR in room
[HttpPost("updateCategoryChairInfo")]
public IActionResult updateCategoryChairInfo([FromBody] categorychairs cate)
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
           
                
                  
               try
                 {
                     var dataupdate = _context.Categorychairs.Find(cate.idcategoryChair);
                     dataupdate.Colorchair = cate.colorchair;
                     dataupdate.Idroom = cate.idroom;
                     dataupdate.Namecategorychair = cate.namecategory;
                      dataupdate.Price = cate.price;
                      successApiResponse.Status = 200;
                      _context.Categorychairs.Update(dataupdate);
                      _context.SaveChanges();
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataupdate;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
           
                 

           }

        }
 return Ok(successApiResponse);
}

// API GET LIST CHAIR in room
[HttpGet("getlistCategoryChairByRoom")]
public IActionResult getlistCategoryChairByRoom()
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
           
                
                  
               try
                 {
                     
                   var dataget = _context.Categorychairs.ToList();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataget;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
           
                 

           }

        }
 return Ok(successApiResponse);
}
public class categorychairs {
   public int? idcategoryChair {get;set;}
   public int? price {get;set;}
   public string namecategory {get;set;}
   
   public string colorchair {get;set;}

   public long? idroom {get;set;}

   public string nameroom {get;set;}
}

// API GET LIST CHAIR in room
[HttpPost("createCategoryChair")]
public IActionResult createCategoryChair([FromBody] categorychairs categorychairinfo)
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
           
                
                  
               try
                 {
                  var categorychair = new Categorychair();
                  categorychair.Colorchair = categorychairinfo.colorchair;
                  categorychair.Namecategorychair = categorychairinfo.namecategory;
                  categorychair.Price = categorychairinfo.price;
                  categorychair.Idroom = categorychairinfo.idroom;
                     _context.Categorychairs.Add(categorychair);
                     _context.SaveChanges();
                  
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = categorychair;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
    
                  }     
           
                 

           }

        }
 return Ok(successApiResponse);
}

}
