using System.Net.Cache;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class PhimController : ControllerBase
{
        // private readonly CinemaContext _context;
          private readonly IWebHostEnvironment _environment;

        public PhimController(IWebHostEnvironment environment)
        {
            // _context = context;
            _environment = environment;
        }

        // // GET: api/Products
        // [HttpGet]
        // // [Authorize]
        // public  IEnumerable<Account> GetAccount()
        // {
        //     return  _context.Accounts.ToList();
        // }

       
// [HttpGet("Movie")]
// public IActionResult getMovieList()
// {
//      var Movie = _context.Movies.ToList();
//      var successApiResponse = new ApiResponse();
//          // Retrieve a specific request header
//        string token = Request.Headers["token"];
//        string filterHeaderValue2 = Request.Headers["ProjectId"];
//        string filterHeaderValue3 = Request.Headers["Method"];
//        string expectedToken = ValidHeader.Token;
//        string method =Convert.ToString(ValidHeader.MethodGet);
//        string Pojectid = Convert.ToString(ValidHeader.Project_id);
   

//    if (Movie == null)
//     {
//         var apiResponse = new ApiResponse
//         {
//             Status = 404,
//             Message = "Movie not found.",
//             Data = null
//         };

//         return NotFound(apiResponse);
//     }else {
    
//         if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
//         {
//         // The "Authorize" header was not found in the request
         
//            return BadRequest("Authorize header not found in the request.");
//         }else {

//             if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
//           {
//             return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
//           }else{
//                     // accountDto.EmployeeId = account.EmployeeId;
//                     // accountDto.Username = account.Username;
//                     //    var employeeInfo = _context.EmployeeInfos.SingleOrDefault(x=>x.EmployeeId == accountDto.EmployeeId);
//                      successApiResponse.Status = 200;
//                      successApiResponse.Message = "OK";
//                      successApiResponse.Data = Movie;
//            }

//         }

  
     
//     // var hashedPassword = HashPassword("123");
//     }


//     return Ok(successApiResponse);
// }
// up load image test


// upload file Image
[HttpPost("UploadImage")]
public async Task<ActionResult> UploadImage()
{
    bool Results = false;
      var uploadedFileNames = new List<string>();
      var apiresponse = new ApiResponse();
    try
    {
        var _uploadedfiles = Request.Form.Files;
        foreach (IFormFile source in _uploadedfiles)
        {
            string Filename = source.FileName;
            string Filepath = GetFilePath(Filename);

            if (!System.IO.Directory.Exists(Filepath))
            {
                System.IO.Directory.CreateDirectory(Filepath);
            }

            string imagepath = Path.Combine(Filepath, Filename);
            
            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }
            using (FileStream stream = System.IO.File.Create(imagepath))
            {
                await source.CopyToAsync(stream);
                uploadedFileNames.Add(Filename + "/" + Filename);
                apiresponse.Status = 200;
                apiresponse.Message = Filename + "/" + Filename;
                apiresponse.Data = uploadedFileNames;
               
                Results = true;
            }
        }
    }
    catch (Exception ex)
    {
        // Handle or log the exception
         Console.WriteLine($"Exception: {ex.Message}");
          return BadRequest(); 
    }
    return Ok(apiresponse);
}

  [NonAction]
    private string GetFilePath(string ProductCode)
    {
        // Use Path.Combine to ensure platform-independent path construction
   return this._environment.WebRootPath + "/Uploads/Movie/" + ProductCode;
    }
 [NonAction]
    private string GetImagebyProduct(string productcode)
    {
        string ImageUrl = string.Empty;
        string HostUrl = "http://localhost:5062";
        string Filepath = GetFilePath(productcode);
        string Imagepath = Filepath;
        if (!System.IO.File.Exists(Imagepath))
        {
            ImageUrl = Imagepath;
        }
        else
        {
            ImageUrl = HostUrl + "/Uploads/Movie/" + productcode;
        }
        return ImageUrl;

    }
    //get file image
    [HttpGet("getImage")]
    public string getImage(){
        return GetImagebyProduct("videotest.mp4/videotest.mp4");
    }

}
