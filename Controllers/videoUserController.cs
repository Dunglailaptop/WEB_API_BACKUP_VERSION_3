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
public class videoUserController : ControllerBase
{
        private readonly CinemaContext _context;
        public videoUserController(CinemaContext context)
        {
            _context = context;
            
        }

public class responsevideo: Videouser {
  public int like {get;set;}
  public int heart {get;set;}
  public int statuslike {get;set;}
  public int statusheart {get;set;}
  public int commentscount {get;set;}
  public List<messagevideo> messagevideos {get;set;} = new List<messagevideo>();
}
public class messagevideo {
  public string image {get;set;}
  public string nameuser {get;set;}
  public string message {get;set;}
  
}


// API GET LIST VIDEO && TRAILLER
[HttpGet("getlistvideoTrailler")]
public IActionResult getListvideoUserTrailler(int TYPE)
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
            if (TYPE != null){
                
                  
               try
                 {
                      string sql = "call cinema.getListVideoUser(@p0)";
                      List<responsevideo> responsevideousers= new List<responsevideo>();
                   var dataget = _context.Videousers.ToList();
                   foreach (var itemvideo in dataget) {
                    var responsedata = new responsevideo();
                  responsedata.Imageview  = itemvideo.Imageview;
                  responsedata.Describes = itemvideo.Describes;
                  responsedata.Dateup = itemvideo.Dateup;
                  responsedata.Titlevideo = itemvideo.Titlevideo;
                  responsedata.Iduser = itemvideo.Iduser;
                  responsedata.Types = itemvideo.Types;
                  responsedata.Idvideo = itemvideo.Idvideo;
                  responsedata.Videofile = itemvideo.Videofile;
                         var datamessvideo = _context.CommentVideo.Where(x=>x.Idvideo == itemvideo.Idvideo).ToList();
                         var datalikeandcomments = _context.likeandcomment.Where(x=>x.idvideo == itemvideo.Idvideo).ToList();
                         var datauserlikecomments = _context.likeandcomment.Where(x=>x.Idusers == itemvideo.Iduser && x.idvideo == itemvideo.Idvideo).SingleOrDefault();
                  responsedata.commentscount = datamessvideo.Count();
                         // xu ly data comment tong
                         if (datalikeandcomments != null ) {
                            responsedata.like = datalikeandcomments.Sum(x=>x.likes);
                          responsedata.heart = datalikeandcomments.Sum(x=>x.comments);
                         }else {
                                responsedata.like = 0;
                                responsedata.heart = 0;
                         }
                         // xu ly data like rieng cho user
                         if (datauserlikecomments != null) {
                              responsedata.statuslike = 1;
                              responsedata.statusheart = 1;
                         } else {
                              responsedata.statuslike = 0;
                              responsedata.statusheart = 0;
                         }
                             
                         foreach(var itemmessinvideo in datamessvideo) {
                          var dataresponsemessage = new messagevideo();
                          dataresponsemessage.message = itemmessinvideo.message;
                    
                          var dataccount = _context.Users.Where(x=>x.Idusers==itemmessinvideo.Iduser).SingleOrDefault();
                          var datausername = _context.Accounts.Where(x=>x.Idusers == itemmessinvideo.Iduser).SingleOrDefault();
                          dataresponsemessage.nameuser = datausername.Username;
                          dataresponsemessage.image = dataccount.Avatar;
                           responsedata.messagevideos.Add(dataresponsemessage);
                         }
                         responsevideousers.Add(responsedata);
                   }
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = responsevideousers;
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



}
