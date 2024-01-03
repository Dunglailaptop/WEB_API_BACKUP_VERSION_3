using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.Net.Cache;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
        private readonly CinemaContext _context;
          private readonly IWebHostEnvironment _environment;
          private readonly IHubContext<OrderHub> _orderhub;

        public MovieController(CinemaContext context, IWebHostEnvironment environment,IHubContext<OrderHub> orderhub)
        {
            _orderhub = orderhub;
            _context = context;
            _environment = environment;
        }

      

  [HttpGet("GetListMovieWithTimeReset")]
public IActionResult GetListMovieWithTimeReset(int timereset)
{
    
    var result = _context.Movies.Where(x => x.Timeall <= timereset).ToList();
    var successApiResponse = new ApiResponse();

    // Retrieve specific request headers
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (result == null || result.Count == 0) // Check if the result list is empty
    {
        var apiResponse = new ApiResponse
        {
            Status = 404,
            Message = "Movies not found.",
            Data = null
        };

        return NotFound(apiResponse);
    }
    else
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            // The "Authorize" header was not found in the request
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
            }
            else
            {
                var moviesList = new List<MovieItem>();

                foreach (var item in result)
                {
                    var movie = new MovieItem
                    {
                        MovieID = item.Idmovie,
                        Namemovie = item.Namemovie,
                        Poster = item.Poster,
                        Timeall = item.Timeall
                    };

                    moviesList.Add(movie);
                }

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = moviesList;
            }
        }
    }

    return Ok(successApiResponse);
}     



[HttpGet("ListMovie")]
public IActionResult getListMovies(int offset_value, int page_size,int Status,int? Idcategory,DateTime? dateFrom,DateTime? dateTo)
{
    // string sql = "CALL cinema.getListMovieNowShow(@p0, @p1, @p2)";
    var result = new List<Movie>();
    if (Status == 3) {
     if (Idcategory > 0) {
       result = _context.Movies.Where(x=>x.Statusshow >= 1 && x.Idcategorymovie == Idcategory).ToList();
     } else if (dateFrom.ToString() != "" && dateTo.ToString() != "") {
        result = _context.Movies.Where(x=>x.Statusshow >= 1 && x.Yearbirthday >= dateFrom && x.Yearbirthday <= dateTo).ToList();
     } else {
      result = _context.Movies.Where(x=>x.Statusshow >= 1).ToList();
     }
       
      
    }else {
        if (Idcategory > 0){
           result = _context.Movies.Where(x=>x.Statusshow == Status && x.Idcategorymovie == Idcategory).ToList();
        } else if (dateFrom.ToString() != "" && dateTo.ToString() != "") {
        result = _context.Movies.Where(x=>x.Statusshow == Status && x.Yearbirthday >= dateFrom && x.Yearbirthday <= dateTo).ToList();
         }else {
            result = _context.Movies.Where(x=>x.Statusshow == Status).ToList();
        }
      
    }

    var successApiResponse = new ApiResponse();

    // Retrieve specific request headers
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

   
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            // The "Authorize" header was not found in the request
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
            }
            else
            {
                var moviesList = new List<MovieItem>();

                foreach (var item in result)
                {
                    var datacategoryMovie = _context.CategoryMovies.Where(x=>x.Idcategorymovie == item.Idcategorymovie).SingleOrDefault();
                    var movie = new MovieItem
                    {
                        MovieID = item.Idmovie,
                        Namemovie = item.Namemovie,
                        Poster = item.Poster,
                        Timeall = item.Timeall,
                        Statusshow = item.Statusshow,
                        Yearbirthday = item.Yearbirthday,
                        namecategorymovie = datacategoryMovie.Namecategorymovie
                    };

                    moviesList.Add(movie);
                }

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = moviesList;
            }
        }
    

    return Ok(successApiResponse);
}


[HttpGet("DetailMovie")]
public IActionResult getDetailMovies(long Idmovie)
{
     var sql = "select * from cinema.Movie where Idmovie = '"+ Idmovie +"' ";
    var result = _context.Movies.FromSqlRaw(sql, Idmovie).AsEnumerable().FirstOrDefault();
    var successApiResponse = new ApiResponse();

    // Retrieve specific request headers
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (result == null) // Check if the result list is empty
    {
        var apiResponse = new ApiResponse
        {
            Status = 404,
            Message = "Movies not found.",
            Data = null
        };

        return NotFound(apiResponse);
    }
    else
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            // The "Authorize" header was not found in the request
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
            }
            else
            {
                 var datacategoryMovie = _context.CategoryMovies.Where(x=>x.Idcategorymovie == result.Idcategorymovie).SingleOrDefault();
                 var dataNation = _context.Nations.Where(x=>x.Idnation == result.Idnation).SingleOrDefault();
                var getvideofile = _context.Videousers.Where(x => x.Idvideo == result.Idvideo).FirstOrDefault();
                var moviedetail = new MovieItemDetail();
                moviedetail.MovieID = result.Idmovie;
                moviedetail.Namemovie = result.Namemovie;
                moviedetail.Author = result.Author;
                moviedetail.Describes = result.Describes;
                moviedetail.Poster = result.Poster;
                moviedetail.Timeall = result.Timeall;
                moviedetail.Yearbirthday = result.Yearbirthday;
                moviedetail.Idcategory = result.Idcategorymovie;
                moviedetail.Idnation = result.Idnation;
               moviedetail.namecategorymovie = datacategoryMovie.Namecategorymovie;
               moviedetail.namenation = dataNation.Namenation;
                moviedetail.Videofile = getvideofile.Videofile;
                moviedetail.Statusshow = result.Statusshow;
              moviedetail.type = getvideofile.Types;
                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = moviedetail;
            }
        }
    }

    return Ok(successApiResponse);
}
// class createMovie
public class NewMovie {
    public long Idmovie { get; set; }

    public string? Namemovie { get; set; }

    public string? Author { get; set; }

    public DateTime? Yearbirthday { get; set; }

    public int? Idcategorymovie { get; set; }

    public int? Idnation { get; set; }

    public int? Timeall { get; set; }

    public string? Describes { get; set; }

    public string? Poster { get; set; }

   //video
    public string? Videofile { get; set; }

    public long? Iduser { get; set; }

    public int? Types { get; set; }

  
}
public class notifacationresponse: Notifaction {
    public string image {get;set;}
}

[HttpPost("CreateMovieNew")]
public async Task<IActionResult> CreateMovieNew([FromBody] NewMovie newmovie)
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
                    //tao video moi
                    Videouser videousers = new Videouser();
                    videousers.Videofile = newmovie.Videofile;
                    videousers.Dateup = DateTime.Now;
                    videousers.Titlevideo = newmovie.Namemovie;
                    videousers.Describes = "none";
                    videousers.Iduser = newmovie.Iduser;
                    videousers.Imageview = "none";
                    videousers.Types = newmovie.Types;
                    _context.Videousers.Add(videousers);
                    _context.SaveChanges();
                    //
                    Movie movienew = new Movie();
                    movienew.Author = newmovie.Author;
                    movienew.Namemovie = newmovie.Namemovie;
                    movienew.Describes = newmovie.Describes;
                    movienew.Poster = newmovie.Poster;
                    movienew.Idcategorymovie = newmovie.Idcategorymovie;
                    movienew.Idnation = newmovie.Idnation;
                    movienew.Idvideo = videousers.Idvideo;
                    movienew.Statusshow = 0;
                    // movienew.Idvideo = 0;
                    movienew.Timeall = newmovie.Timeall;
                    movienew.Yearbirthday = newmovie.Yearbirthday;
                    _context.Movies.Add(movienew);
                    _context.SaveChanges();
                    // thông báo
                    var Notifactions = new Notifaction();
                    var datauser = _context.Users.Where(x=>x.Idusers == newmovie.Iduser).SingleOrDefault();
                    Notifactions.messages = "Quản lý :" + datauser.Fullname + "- vừa tạo phim mới với tên phim" + movienew.Namemovie;
                    Notifactions.datecreate = DateTime.Now;
                    Notifactions.iduser = datauser.Idusers;
                    Notifactions.image_noti = movienew.Poster;
                    _context.Notifaction.Add(Notifactions);
                    _context.SaveChanges();
                    var notifacationresponses = new notifacationresponse();
                    notifacationresponses.messages = Notifactions.messages;
                    notifacationresponses.datecreate = Notifactions.datecreate;
                    notifacationresponses.iduser = Notifactions.iduser;
                    notifacationresponses.image = movienew.Poster;
                    // socket 
                   await _orderhub.Clients.All.SendAsync("NOTIFACTIONMOVIENEW",notifacationresponses);
                    successApiResponse.Status = 200;
                    successApiResponse.Message = "OK";
                    successApiResponse.Data = movienew;
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

public class statsusUpdate {

  public long? Idmovie {get;set;}
  public int status {get;set;}

}

[HttpPost("UpdateSatusMovie")]
public IActionResult UpdateSatusMovie([FromBody] statsusUpdate updatestatus)
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
                    var movieneedUp = _context.Movies.Find(updatestatus.Idmovie);
                    movieneedUp.Statusshow = updatestatus.status;
                    _context.Movies.Update(movieneedUp);
                    _context.SaveChanges();
                    
                    successApiResponse.Status = 200;
                    successApiResponse.Message = "OK";
                    successApiResponse.Data = movieneedUp;
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



[HttpPost("UpdateMovies")]
public IActionResult UpdateMovies([FromBody] NewMovie newmovie)
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
                       long bl = 0 ;
                    //luu videouser
                    if (newmovie.Videofile != null){
                     Videouser user = new Videouser();
                        user.Videofile = newmovie.Videofile;
                        user.Dateup = DateTime.Now;
                        user.Describes = newmovie.Describes;
                        user.Iduser = newmovie.Iduser;
                        user.Titlevideo = newmovie.Namemovie;
                        user.Types = newmovie.Types;
                        _context.Videousers.Add(user);
                        _context.SaveChanges();
                        bl = user.Idvideo;
                    }
                
                   var dataUpdate = _context.Movies.Find(newmovie.Idmovie);
                    dataUpdate.Idnation = newmovie.Idnation;
                    dataUpdate.Idcategorymovie = newmovie.Idcategorymovie;
                    dataUpdate.Author = newmovie.Author;
                    dataUpdate.Namemovie = newmovie.Namemovie;
                    dataUpdate.Describes = newmovie.Describes;
                    dataUpdate.Poster = newmovie.Poster;
                    dataUpdate.Yearbirthday = newmovie.Yearbirthday;
                    dataUpdate.Timeall = newmovie.Timeall;
                    if (bl != 0){
                       dataUpdate.Idvideo = bl;
                    }
                   
                    _context.Movies.Update(dataUpdate);
                   
                   _context.SaveChanges();

                  

                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = dataUpdate;
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



[HttpGet("getListMovieWithBooking")]
public IActionResult getListMovieWithBooking(int statusshow)
{
     
     var successApiResponse = new ApiResponse();
         // Retrieve a specific request header
       string token = Request.Headers["token"];
       string filterHeaderValue2 = Request.Headers["ProjectId"];
       string filterHeaderValue3 = Request.Headers["Method"];
       string expectedToken = ValidHeader.Token;
       string method =Convert.ToString(ValidHeader.MethodGet);
       string Pojectid = Convert.ToString(ValidHeader.Project_id);
   

  
    
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
        // The "Authorize" header was not found in the request
         
           return BadRequest("Authorize header not found in the request.");
        }else {

            if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
          {
            return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
          }else{
                  try {
                    if (statusshow == 1) {
                        var datetime = DateTime.Now;
                        string sql = "select DISTINCT m.Namemovie,m.Idmovie as movieID,nt.Namecategorymovie,m.Yearbirthday,m.poster,m.Timeall from cinema.Movie m inner join cinema.Cinemainterest cn on cn.Idmovie = m.Idmovie inner join cinema.categorymovie nt on nt.Idcategorymovie = m.Idcategorymovie where cn.Dateshow >= '"+datetime.ToString("yyyy-MM-dd")+"' and m.Statusshow = '"+statusshow+"'";
                        var datamovie = _context.moviesBookings.FromSqlRaw(sql).AsEnumerable().ToList();
                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data = datamovie;
                    }else {
                            var datetime = DateTime.Now;
                             string sql = "select DISTINCT m.Namemovie,m.Idmovie as movieID,nt.Namecategorymovie,m.Yearbirthday,m.poster,m.Timeall from cinema.Movie m  inner join cinema.categorymovie nt on nt.Idcategorymovie = m.Idcategorymovie where  m.Statusshow = 0";
                            var datamovie = _context.moviesBookings.FromSqlRaw(sql).AsEnumerable().ToList();
                            successApiResponse.Status = 200;
                            successApiResponse.Message = "OK";
                            successApiResponse.Data = datamovie;
                    }
                      
                  }catch {
                         successApiResponse.Status = 404;
                        successApiResponse.Message = "error";
                        successApiResponse.Data = "null";
                  }
           }

        }

  
     
    // var hashedPassword = HashPassword("123");
    


    return Ok(successApiResponse);
}

// // upload file Image
// [HttpPost("UploadImage")]
// public async Task<ActionResult> UploadImage()
// {
//     bool Results = false;
//     try
//     {
//         var _uploadedfiles = Request.Form.Files;
//         foreach (IFormFile source in _uploadedfiles)
//         {
//             string Filename = source.FileName;
//             string Filepath = GetFilePath(Filename);

//             if (!System.IO.Directory.Exists(Filepath))
//             {
//                 System.IO.Directory.CreateDirectory(Filepath);
//             }

//             string imagepath = Path.Combine(Filepath, Filename);
            
//             if (System.IO.File.Exists(imagepath))
//             {
//                 System.IO.File.Delete(imagepath);
//             }
//             using (FileStream stream = System.IO.File.Create(imagepath))
//             {
//                 await source.CopyToAsync(stream);
//                 Results = true;
//             }
//         }
//     }
//     catch (Exception ex)
//     {
//         // Handle or log the exception
//          Console.WriteLine($"Exception: {ex.Message}");
//     }
//     return Ok(Results);
// }

//   [NonAction]
//     private string GetFilePath(string ProductCode)
//     {
//         // Use Path.Combine to ensure platform-independent path construction
//    return this._environment.WebRootPath + "/Uploads/Movie/" + ProductCode;
//     }
//  [NonAction]
//     private string GetImagebyProduct(string productcode)
//     {
//         string ImageUrl = string.Empty;
//         string HostUrl = "https://localhost:7118/";
//         string Filepath = GetFilePath(productcode);
//         string Imagepath = Filepath;
//         if (!System.IO.File.Exists(Imagepath))
//         {
//             ImageUrl = HostUrl + "/Uploads/common/noimage.png";
//         }
//         else
//         {
//             ImageUrl = HostUrl + "/Uploads/Movie/" + productcode;
//         }
//         return ImageUrl;

//     }
//     //get file image
//     [HttpGet("getImage")]
//     public string getImage(){
//         return GetImagebyProduct("age.png");
//     }
public class MovieItemDetail: MovieItem {
    public int? type {get;set;}

}
     
public class MovieItem
{
    public long? MovieID { get; set; }
    public string Namemovie { get; set; }
    public string Author { get; set; }
    public DateTime? Yearbirthday { get; set; }
    public int? Timeall { get; set; }
    public string Describes { get; set; }
    public string Poster { get; set; }
    public int? Statusshow { get; set; }
    public string Videofile {get; set;}

    public long? Idvideo {get; set;}

    public int? Idcategory {get; set;}

    public string namecategorymovie {get;set;}

    public string namenation {get;set;}

    public int? Idnation {get;set;}
}

    
}