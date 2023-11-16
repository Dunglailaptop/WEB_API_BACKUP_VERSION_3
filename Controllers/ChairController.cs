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
public class ChairController : ControllerBase
{
        private readonly CinemaContext _context;
        public ChairController(CinemaContext context)
        {
            _context = context;
            
        }
public class categoryinfo {
      public int Idcategorychair { get; set; }

    public string? Namecategorychair { get; set; }

    public string? Colorchair { get; set; }

    public int? Price { get; set; }

    public long? Idroom {get;set;}
}

// API GET LIST CATEGORY IN ROOM
[HttpGet("GetListCategoryInRoom")]
public IActionResult GetListCategoryInRoom(long Idroom)
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
            if (Idroom != null){
                
                  
               try
                 {
                  List<categoryinfo> chairdata = new List<categoryinfo>(); 
                      string sql = "select distinct(ch.Idcategorychair),ch.Colorchair,ch.price,ch.Idroom,ch.Namecategorychair from cinema.chair cs inner join cinema.categorychair ch on ch.Idcategorychair = cs.Idcategorychair where cs.Idroom = '"+Idroom+"' ";
                   var dataget = _context.Categorychairs.FromSqlRaw(sql).AsEnumerable().ToList();
                 foreach (var item in dataget) {
                  var datagetinfoCategory = _context.Categorychairs.Where(x=>x.Idcategorychair == item.Idcategorychair).SingleOrDefault();
                  var categoryinfos = new categoryinfo();
                  categoryinfos.Colorchair = datagetinfoCategory.Colorchair;
                  categoryinfos.Namecategorychair = datagetinfoCategory.Namecategorychair;
                  categoryinfos.Price = datagetinfoCategory.Price;
                  chairdata.Add(categoryinfos);
                 }
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = chairdata;
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

// API GET LIST CHAIR
[HttpGet("getlistchair")]
public IActionResult getListVoucher(long Idroom,long idcinema,int idinterest)
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
            if (idcinema != null && Idroom != null && idinterest != null){
                
                  
               try
                 {
                  List<CHAIRNEW> chairdata = new List<CHAIRNEW>(); 
                      string sql = "call cinema.getListChairRoomCinema(@p0,@p1,@p2)";
                   var dataget = _context.CHAIRS.FromSqlRaw(sql,Idroom,idcinema,idinterest).AsEnumerable().ToList();
                   foreach (var item in dataget) {
                     var datagetprice = _context.Categorychairs.Where(x=>x.Idcategorychair == item.Idcategorychair).SingleOrDefault();
                     CHAIRNEW chairs = new CHAIRNEW(){
                        Idchair = item.Idchair,
                        RowChar = item.RowChar,
                        NumberChair = item.NumberChair,
                        bill = item.bill,
                        price = datagetprice.Price,
                        Idcategorychair = item.Idcategorychair,
                        colorchair = datagetprice.Colorchair
                     };
                     chairdata.Add(chairs);
                   }
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = chairdata;
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
public class chairresponse {
       public int Idchair { get; set; }

    public int? NumberChair { get; set; }

    public int? Roww { get; set; }

    public long? Idroom { get; set; }

    public int? StatusChair { get; set; }

    public int? Idcategorychair { get; set; }

    public string? RowChar { get; set; }
    public string colorchair {get;set;}
}

// API GET LIST CHAIR in room
[HttpGet("getlistchairRoom")]
public IActionResult getlistchairRoom(long Idroom)
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
            if ( Idroom != null ){
                
                  
               try
                 {
                    List<chairresponse>  chairresponse = new List<chairresponse>();
                   var dataget = _context.Chairs.Where(x=> x.Idroom == Idroom).ToList();
                   foreach (var item in dataget) {
                     var chairitem = new chairresponse();
                     var datagetchair = _context.Categorychairs.Where(x=>x.Idcategorychair == item.Idcategorychair).SingleOrDefault();
                     chairitem.Idchair = item.Idchair;
                     chairitem.Idroom = item.Idroom;
                      chairitem.NumberChair = item.NumberChair;
                      chairitem.RowChar = item.RowChar;
                      chairitem.StatusChair = item.StatusChair;
                      chairitem.Idcategorychair = item.Idcategorychair;
                      chairitem.colorchair = datagetchair.Colorchair;
                      chairresponse.Add(chairitem);
                   }
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = chairresponse;
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

// API CREATE ROOM AND CHAIR
[HttpPost("CreateChairInRoom")]
public IActionResult CreateChairInRoom([FromBody] RoomREsponse DATA)
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
            if ( DATA != null ){
                
                  
               try
                 {
                  string text = "";
                     Room room = new Room {
                        Allchair = DATA.allschair,
                        Nameroom = DATA.nameroom,
                        Idcinema = DATA.Idcinema,
                        Chairinrow = DATA.numberChair,
                        Statusroom = 1
                     };
                   _context.Rooms.Add(room);
                   _context.SaveChanges();
                   long newRoom = room.Idroom;
                   if (newRoom > 0){ 
                        string sql = "CALL cinema.insert_values(@p0,@p1,@p2)";
                        _context.Database.ExecuteSqlRaw(sql,DATA.numberChair,newRoom,DATA.allschair);
                        _context.SaveChanges();
                        text = "Thêm phòng thành công";
                   }else 
                   {
                     text = "Thêm phòng thất bại";
                   }
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = text;
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

public class RoomREsponse {
   public long Idcinema {get;set;}
   public int numberChair   {get;set;}
      public int allschair   {get;set;}
         public string nameroom   {get;set;}
}

public partial class CHAIRNEW
{
    public int Idchair { get; set; }
    
    public int NumberChair { get; set; }

     public string RowChar { get; set; }

     public long? bill {get;set;} 
     public int Idcategorychair {get;set;}
     public int? price {get;set;}

     public string colorchair {get;set;}
    
}

}
