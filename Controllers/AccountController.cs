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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;


// using MySql.Data.EntityFrameworkCore;
//testcommit cai nha
namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
        private readonly CinemaContext _context;
       
         private readonly IMemoryCache _memoryCache;
        public AccountController(CinemaContext context,IMemoryCache memoryCache)
        {
            _context = context;
         _memoryCache = memoryCache;

            
        }
// API DANG NHAP VAO TAI KHOAN **
[HttpGet("Logins")]
public IActionResult getdata(string username, string password)
{
        var successApiResponse = new ApiResponse();
    
    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string ProjectId = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        return BadRequest("Account data is invalid.");
    }
    try {
     string sql = "CALL cinema.login(@p0, @p1)";
    var result = _context.Users.FromSqlRaw(sql, username, password).AsEnumerable().FirstOrDefault();


    if (result == null)
    {
            successApiResponse.Status = 404;
            successApiResponse.Message = "Tài khoản không tồn tại trong hệ thống";
            successApiResponse.Data = "null";

     
    }
    else
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
        {
            return BadRequest("Authorize header not found in the request.");
        }
        else
        {
            if (token != expectedToken || filterHeaderValue2 != ProjectId || filterHeaderValue3 != method)
            {
                return Unauthorized("Invalid token.");
            }
            else
            {

                  var userDto = new UserDto();

        // Set properties based on the retrieved user entity
                  userDto.Idusers = result.Idusers;
                  userDto.Fullname = result.Fullname;
                  userDto.Email = result.Email;
                  userDto.Phone = result.Phone;
                  userDto.Birthday = result.Birthday.ToString();
                  userDto.Idrole = result.Idrole;
                  userDto.Avatar = result.Avatar;

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = userDto;

             
            }
        }
    }
    }catch {
        successApiResponse.Status = 404;
        successApiResponse.Message = "Sai mật khẩu xin vui lòng nhập lại";
        successApiResponse.Data = "null";
    }
       return Ok(successApiResponse);

}

[HttpGet("getrole")]
public IActionResult getdatarole(){
    var account = _context.Roles.ToList();
    var succapi = new ApiResponse();
    succapi.Status = 200;
    succapi.Message = "OK";
    succapi.Data = account;
    return Ok(succapi);
}


// API CAP NHAT THONG TIN TAI KHOAN *
[HttpPost("UpdateAccount")]
public IActionResult UpdateAccount([FromBody] UserDto account)
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
            if (account.Fullname != null && account.Email != null && account.Idusers != null && account.Phone != null){
                   string sql = "CALL cinema.updateAccount(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
                   _context.Database.ExecuteSqlRaw(sql, account.Idusers,account.Fullname,account.Email,account.Phone,account.Birthday,account.Avatar,account.gender,account.address,account.Idrole);
                   _context.SaveChanges();
                    successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = "cậpp nhậtt tàii khoảnn thànhh công";
                      
            }else {
                return BadRequest("Vui long nhap day du thong tin tai khoan");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API GET INFO ACCOUNT
[HttpGet("getInfoAccount")]
public IActionResult UpdateAccount(long id)
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
            if (id != null){
                   string sql = "CALL cinema.getInfoAccount(@p0)";
                   var dataget = _context.Users.FromSqlRaw(sql, id).AsEnumerable().FirstOrDefault();
                   UserDto us = new UserDto();
                   us.Idusers = dataget.Idusers;
                   us.Email = dataget.Email;
                   us.Birthday = dataget.Birthday.ToString();
                   us.Fullname = dataget.Fullname;
                   us.Phone = dataget.Phone;
                   us.Idrole = dataget.Idrole;
                   us.gender = dataget.gender;
                   us.address = dataget.address;
                   var role = _context.Roles.SingleOrDefault(x => x.Idrole == dataget.Idrole);
                   us.Avatar = dataget.Avatar;
                   us.idrolename = role.IdName;
                    successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = us;
                      
            }else {
                return BadRequest("khong tim thay thong tin tai khoan");
            }
                 

           }

        }
 return Ok(successApiResponse);
}

// API GET INFO POINTS
[HttpGet("getInfoPointAccount")]
public IActionResult getInfoPointAccount(long? iduser)
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
                     points pointes = new points();
                     
                   var dataget = _context.Accounts.Where(x=>x.Idusers == iduser).SingleOrDefault();
                     pointes.point = dataget.points;
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = pointes;
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

// API CREATE ACCOUNT
// Create a single instance of SmtpClient and reuse it
private static readonly SmtpClient _smtp = new SmtpClient("smtp.gmail.com")
{
    EnableSsl = true,
    Port = 587,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    Credentials = new NetworkCredential("0850080012@sv.hcmunre.edu.vn", "2792001dung")
};

[HttpGet("SendOTPInEmail")]  // Changed to [HttpPost] for consistency with the asynchronous changes
public async Task<IActionResult> SendOTPInEmail(string emails)
{
    // // Initialize API response
    var successApiResponse = new ApiResponse();

    // Header
    // string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string projectId = Convert.ToString(ValidHeader.Project_id);

    // Check header
    if ( //string.IsNullOrEmpty(token) ||
      string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        // The "Authorize" header was not found in the request
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (//token != expectedToken || 
        filterHeaderValue2 != projectId || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
        }
        else
        {
            try
            {
                // Validate email address
                if (!IsValidEmail(emails))
                {
                    return BadRequest("Invalid email address format.");
                }

                string otp = GenerateOTP();
                MailMessage message = new MailMessage();
                var to = emails;
                var from = "0850080012@sv.hcmunre.edu.vn";
                var pass = "2792001dung";
                var messageBody = "Your OTP for creating a new account: " + otp;

                // Set up the email message
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.Body = messageBody;
                message.Subject = "OTP SEND EMAIL";

                // Set up the SMTP client

                // Generate and send OTP
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) // Expiration time for OTP in cache
                };

                // Save OTP to cache
                _memoryCache.Set(emails, otp, cacheEntryOptions);

                try
                {
                    // Send the email asynchronously using the shared SmtpClient instance
                    Task.Run(() => _smtp.SendMailAsync(message));


                    successApiResponse.Status = 200;
                    successApiResponse.Message = "OK";
                    successApiResponse.Data = "Code sent successfully";
                }
                catch (SmtpException ex)
                {
                    // Handle SMTP exception
                    successApiResponse.Status = 500; // Internal Server Error
                    successApiResponse.Message = "Error sending email";
                    successApiResponse.Data = ex.Message;
                }
            }
            catch (FormatException)
            {
                // Handle invalid email format exception
                return BadRequest("Invalid email address format.");
            }
        }
    }

    return Ok(successApiResponse);
}

public class userAdd {
    public string username {get;set;}

    public string passwords {get;set;}

    public string fullname {get;set;}

    public string email {get;set;}
}

[HttpPost("CreateAccounts")]
public IActionResult createAccounts(userAdd user){
  
   var successApiResponse = new ApiResponse();

   var users = new User();
   users.Fullname = user.fullname;
   users.Email = user.email;
   users.Idrole = 1;
   _context.Users.Add(users);
   _context.SaveChanges();

   if (users.Idusers != 0) {
     var account  = new Account();
     account.Idusers = users.Idusers;
     account.Username = user.username;
     account.Password = user.passwords;
     account.points = 0;
     _context.Accounts.Add(account);
     _context.SaveChanges();
        successApiResponse.Status = 200; // Internal Server Error
        successApiResponse.Message = "OK";
        successApiResponse.Data = account;

   }else {
        successApiResponse.Status = 500; // Internal Server Error
        successApiResponse.Message = "error";
        successApiResponse.Data = "Tạo tài khoản thất bại";
   }
   return Ok(successApiResponse);
}


// Validate email address using a regular expression
private bool IsValidEmail(string email)
{
    try
    {
        var mailAddress = new MailAddress(email);
        return true;
    }
    catch (FormatException)
    {
        return false;
    }
}
[HttpGet("ConfirmAccount")]
public IActionResult ConfirmAccount(string emails, string enteredOTP, [FromServices] IMemoryCache memoryCache)
{
     var successApiResponse = new ApiResponse();
    // Kiểm tra xem email có tồn tại trong bộ nhớ đệm không
    if (memoryCache.TryGetValue(emails, out string storedOTP))
    {
        // So sánh mã OTP nhập vào với mã OTP trong bộ nhớ đệm
        if (enteredOTP == storedOTP)
        {
            // Mã OTP hợp lệ, thực hiện các hành động cần thiết
         

            successApiResponse.Status = 200; // Internal Server Error
            successApiResponse.Message = "Ok";
            successApiResponse.Data = "OTP hợp lệ tạo tài khoản thành công";
        }
        else
        {
             successApiResponse.Status = 500; // Internal Server Error
            successApiResponse.Message = "error";
            successApiResponse.Data = "OTP không hợp lệ";
        }
    }
    else
    {
        successApiResponse.Status = 500; // Internal Server Error
            successApiResponse.Message = "error";
            successApiResponse.Data = "otp hết thời gian chấp nhận";
    }
    return Ok(successApiResponse);
}


    private string GenerateOTP()
    {
        // Logic để sinh mã OTP, ví dụ:
        Random random = new Random();
        return random.Next(1000, 9999).ToString();
    }


public class points {
    public int? point {get;set;}
}

public class AccountDto
{
    // Define the properties you want to return in the response
    public long? EmployeeId { get; set; }
    public string Username { get; set; }
    // Other properties you want to expose
}
public class UserDto
{
    public long? Idusers { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Birthday { get; set; }
    public int? Idrole { get; set; }
    public string Avatar { get; set; }
    public int? gender {get;set;}
    public string? address {get;set;}
    public string? idrolename {get;set;}
}


}
