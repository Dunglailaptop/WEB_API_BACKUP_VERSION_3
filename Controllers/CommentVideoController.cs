using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyCinema.Model;
using Microsoft.AspNetCore.SignalR;
using webapiserver.Controllers;


namespace MyCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentVideoController : ControllerBase
{
   private readonly CinemaContext _context;
   private readonly IHubContext<OrderHub> _orderhub;

   public  CommentVideoController(CinemaContext context, IHubContext<OrderHub> orderhub) {
          _context = context;
          _orderhub = orderhub;
   }
   
public class messagevideo {
  public string image {get;set;}
  public string nameuser {get;set;}
  public string message {get;set;}
  
}

// API GET LIST VOUCHER
[HttpPost("postCommentVideoController")]
public async Task<IActionResult> postCommentVideoController(CommentVideo commentVideos)
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
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
                   var messagevideos = new messagevideo();
                        CommentVideo commentVideo = new CommentVideo();
                        commentVideo.message = commentVideos.message;
                        commentVideo.Iduser = commentVideos.Iduser;
                        commentVideo.Idvideo = commentVideos.Idvideo;
                        commentVideo.likes = commentVideos.likes;
                        commentVideo.heart = commentVideos.heart;
                        _context.CommentVideo.Add(commentVideo);
                        _context.SaveChanges();
                        var dataccount = _context.Accounts.Where(x=>x.Idusers == commentVideo.Iduser).SingleOrDefault();
                        var dataUser = _context.Users.Where(x=>x.Idusers == commentVideo.Iduser).SingleOrDefault();
                     messagevideos.message = commentVideo.message;
                     messagevideos.nameuser = dataccount.Username;
                     messagevideos.image = dataUser.Avatar;
                    await _orderhub.Clients.All.SendAsync("MESSAGE",messagevideos);
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = commentVideo;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
                      successApiResponse.Status = 500;
                     successApiResponse.Message = "some thing went wrong";
                     successApiResponse.Data = "null";
                  }     
            // }else {
            //     return BadRequest("khong tim thay thong tin");
            // }
                 

           }

        }
 return Ok(successApiResponse);
}

public class responselikeandcomment: likeandcomment {
   public int numberlike {get;set;}
   public int numberheart {get;set;}
}

// API GET LIST VOUCHER
[HttpPost("liekandcomment")]
public async Task<IActionResult> liekandcomment(likeandcomment heartandlike)
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
            // if (date != null && Idmovie != null){
                
                  
               try
                 {
                    var datafind = _context.likeandcomment.Where(x=>x.idvideo == heartandlike.idvideo && x.Idusers == heartandlike.Idusers).SingleOrDefault();
                     likeandcomment likedata = new likeandcomment();
                     responselikeandcomment responselikeandcomments = new responselikeandcomment();
                    if (datafind != null) {
                       var datalikeandcomment = _context.likeandcomment.Find(datafind.idliekandcomment);
                       if (heartandlike.likes == 1 && heartandlike.comments == 0) {
                           datalikeandcomment.likes = heartandlike.likes;
                           datalikeandcomment.comments = datalikeandcomment.comments;
                           _context.likeandcomment.Update(datalikeandcomment);
                           _context.SaveChanges();
                       }else if (heartandlike.likes == 0 && heartandlike.comments == 1) {
                              datalikeandcomment.likes = datalikeandcomment.likes;
                              datalikeandcomment.comments = heartandlike.comments;
                              _context.likeandcomment.Update(datalikeandcomment);
                              _context.SaveChanges();
                       }
                    
                     var datalistnumber = _context.likeandcomment.Where(x=>x.idvideo == datafind.idvideo).ToList();

                      responselikeandcomments.likes = datalikeandcomment.likes;
                      responselikeandcomments.comments = datalikeandcomment.comments;
                      responselikeandcomments.numberlike = datalistnumber.Sum(x=>x.likes);
                      responselikeandcomments.numberheart = datalistnumber.Sum(x=>x.comments);
                      responselikeandcomments.idliekandcomment = datalikeandcomment.idliekandcomment;
                    } else {
                           likeandcomment commentVideo = new likeandcomment();
                           commentVideo.likes = heartandlike.likes;
                           commentVideo.comments = heartandlike.comments;
                           commentVideo.idvideo = heartandlike.idvideo;
                           commentVideo.Idusers = heartandlike.Idusers;
                           _context.likeandcomment.Add(commentVideo);
                           _context.SaveChanges();
                           
                            var datalistnumber = _context.likeandcomment.Where(x=>x.idvideo == datafind.idvideo).ToList();

                      responselikeandcomments.likes = commentVideo.likes;
                      responselikeandcomments.comments = commentVideo.comments;
                      responselikeandcomments.numberlike = datalistnumber.Sum(x=>x.likes);
                      responselikeandcomments.numberheart = datalistnumber.Sum(x=>x.comments);
                      responselikeandcomments.idliekandcomment = commentVideo.idliekandcomment;
                    }
                   
                    
                    await _orderhub.Clients.All.SendAsync("LIKEANDCOMMENT",responselikeandcomments);
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = likedata;
                 }
                 catch (IndexOutOfRangeException ex)
                  {
                      successApiResponse.Status = 500;
                     successApiResponse.Message = "some thing went wrong";
                     successApiResponse.Data = "null";
                  }     
            // }else {
            //     return BadRequest("khong tim thay thong tin");
            // }
                 

           }

        }
 return Ok(successApiResponse);
}
  
}
