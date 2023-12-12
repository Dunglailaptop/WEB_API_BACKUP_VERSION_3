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

                  var userDto = new UserDtoResponse();

        // Set properties based on the retrieved user entity
                  userDto.Idusers = result.Idusers;
                  var datausername = _context.Accounts.Where(x=>x.Idusers == result.Idusers).SingleOrDefault();
                  userDto.username = datausername.Username;
                  userDto.Fullname = result.Fullname;
                  userDto.Email = result.Email;
                  userDto.Phone = result.Phone;
                  userDto.Birthday = result.Birthday.ToString();
                  userDto.Idrole = result.Idrole;
                  userDto.Avatar = result.Avatar;
                  userDto.statuss = result.statuss;
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
                       string sqlaccupdate = "CALL cinema.getInfoAccount(@p0)";
                   var dataget = _context.Users.FromSqlRaw(sqlaccupdate, account.Idusers).AsEnumerable().FirstOrDefault();
                    successApiResponse.Status = 200;
                     successApiResponse.Message = "cập nhật tài khoản thành công";
                     successApiResponse.Data = dataget;
                      
            }else {
                return BadRequest("Vui long nhap day du thong tin tai khoan");
            }
                 

           }

        }
 return Ok(successApiResponse);
}
// API GET INFO ACCOUNT
[HttpGet("getInfoAccount")]
public IActionResult getInfoAccount(long id)
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

[HttpGet("checkaccount")]
public IActionResult checkaccount(string username){
    var successApiResponse = new ApiResponse();
    var dataccountcheck = _context.Accounts.Where(x=>x.Username == username).SingleOrDefault();
    if (dataccountcheck != null){
            successApiResponse.Status = 500;
            successApiResponse.Message = "Tài khoản đã tồn tại";
            successApiResponse.Data = "Error";
    }else {
            successApiResponse.Status = 200;
            successApiResponse.Message = "OK";
            successApiResponse.Data = "ACCOUNT SUCCESS";
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
    // var dataaccount = _context.Accounts.Where(x=>x.Username == Username).SingleOrDefault();
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
            // if (dataaccount == null) {
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
                    successApiResponse.Message = "Gửi mã OTP Email thành công";
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
            // }else {
            //         successApiResponse.Status = 500; // Internal Server Error
            //         successApiResponse.Message = "Tên tài khoản đã tồn tại";
            //         successApiResponse.Data = "error";
            // }
          
        }
    }

    return Ok(successApiResponse);
}

public class iduser: userAdd {
    public long? id {get;set;}

    
}

public class userAdd {
    public string username {get;set;}

    public string passwords {get;set;}

    public string fullname {get;set;}

    public string emails {get;set;}

    public string enteredOTP {get;set;}
}

[HttpGet("ForgotPasssword")]
public IActionResult FindAccount(string username) {
     var successApiResponse = new ApiResponse();
     var dataaccount = _context.Accounts.Where(x=>x.Username == username).SingleOrDefault();
     var userresponse = new iduser();
     if (dataaccount != null) {
        var dataemails = _context.Users.Where(x=>x.Idusers == dataaccount.Idusers).SingleOrDefault();
        userresponse.id = dataemails.Idusers;
        userresponse.username = dataaccount.Username;
        userresponse.emails = dataemails.Email;
          successApiResponse.Status = 200; // Internal Server Error
                successApiResponse.Message = "OK";
                successApiResponse.Data = userresponse;
     }else {
           successApiResponse.Status = 500; // Internal Server Error
                successApiResponse.Message = "Tài khoản không tồn tại";
                successApiResponse.Data = "Tài khoản không tồn tại";
     }
     return Ok(successApiResponse);
}

// [HttpPost("CreateAccounts")]
// public IActionResult createAccounts(userAdd user){
  
//    var successApiResponse = new ApiResponse();
//      var dataccountcheck = _context.Accounts.Where(x=>x.Username == user.username).SingleOrDefault();
//      if (dataccountcheck != null) {
//              successApiResponse.Status = 500; // Internal Server Error
//                 successApiResponse.Message = "Tài khoản đã tồn tại ";
//                 successApiResponse.Data = "Tạo tài khoản thất bại";
//      }else{
//                 var users = new User();
//                 users.Fullname = user.fullname;
//                 users.Email = user.email;
//                 users.Idrole = 1;
//                 _context.Users.Add(users);
//                 _context.SaveChanges();

//                 if (users.Idusers != 0) {
//                 var account  = new Account();
//                 //  account.Idusers = users.Idusers;
//                 account.Username = user.username;
//                 account.Password = user.passwords;
//                 account.points = 0;
//                 _context.Accounts.Add(account);
//                 _context.SaveChanges();
//                 successApiResponse.Status = 200; // Internal Server Error
//                 successApiResponse.Message = "OK";
//                 successApiResponse.Data = account.Username;

//                 }else {
//                 successApiResponse.Status = 500; // Internal Server Error
//                 successApiResponse.Message = "error";
//                 successApiResponse.Data = "Tạo tài khoản thất bại";
//                 }
//      }
  
//    return Ok(successApiResponse);
// }


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

public class requestUpdate {
    public string username {get;set;}
    public string newpassword {get;set;}
}

[HttpPost("ChangePassWord")]
public IActionResult ChangePassword([FromBody] requestUpdate requests) {
    var successApiResponse = new ApiResponse();
    try{
           // Assuming iduser is a long, convert it to string if needed
           
            var dataup = _context.Accounts.Find(requests.username);
         
            if (dataup != null){
                   dataup.Password = requests.newpassword;
            _context.Accounts.Update(dataup);
            _context.SaveChanges();
            }

            successApiResponse.Status = 200; // Internal Server Error
            successApiResponse.Message = "Cập nhật tài khoản thành công";
            successApiResponse.Data = dataup.Username;
    }catch {
            successApiResponse.Status = 500; // Internal Server Error
            successApiResponse.Message = "Cập nhật tài khoản thất bại";
            successApiResponse.Data = "error";
    }
   
     return Ok(successApiResponse);

}

[HttpGet("ConfirmAccountForgotPassword")]
public IActionResult ConfirmAccountForgotPassword([FromServices] IMemoryCache memoryCache,string emails,string enteredOTP,long iduser)
{
     var successApiResponse = new ApiResponse();
    // Kiểm tra xem email có tồn tại trong bộ nhớ đệm không
    if (memoryCache.TryGetValue(emails, out string storedOTP))
    {
        // So sánh mã OTP nhập vào với mã OTP trong bộ nhớ đệm
        if (enteredOTP == storedOTP)
        {
            var datagetaccount = _context.Accounts.Where(x=>x.Idusers == iduser).SingleOrDefault();
            // Mã OTP hợp lệ, thực hiện các hành động cần thiết
             var user = new iduser();
             user.id = datagetaccount.Idusers;
             user.username = datagetaccount.Username;
            successApiResponse.Status = 200; // Internal Server Error
            successApiResponse.Message = "OTP hợp lệ";
            successApiResponse.Data = user;
                       
                         
                        
        
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

[HttpPost("ConfirmAccount")]
public IActionResult ConfirmAccount([FromServices] IMemoryCache memoryCache,[FromBody] userAdd user)
{
     var successApiResponse = new ApiResponse();
    // Kiểm tra xem email có tồn tại trong bộ nhớ đệm không
    if (memoryCache.TryGetValue(user.emails, out string storedOTP))
    {
        // So sánh mã OTP nhập vào với mã OTP trong bộ nhớ đệm
        if (user.enteredOTP == storedOTP)
        {
            // Mã OTP hợp lệ, thực hiện các hành động cần thiết
         
 
                       
                            var users = new User();
                            users.Fullname = user.fullname;
                            users.Email = user.emails;
                            users.Idrole = 1;
                            _context.Users.Add(users);
                            _context.SaveChanges();

                            if (users.Idusers != 0) {
                                    var account  = new Account();
                                     account.Idusers = users.Idusers;
                                    account.Username = user.username;
                                    account.Password = user.passwords;
                                    account.points = 5000000; // 5000000vnd 
                                    _context.Accounts.Add(account);
                                    _context.SaveChanges();
                                    successApiResponse.Status = 200; // Internal Server Error
                                    successApiResponse.Message = "OTP hợp lệ tạo tài khoản thành công";
                                    successApiResponse.Data = account.Username;

                                    }else {
                                    successApiResponse.Status = 500; // Internal Server Error
                                    successApiResponse.Message = "Tạo tài khoản thất bại";
                                    successApiResponse.Data = "Tạo tài khoản thất bại";
                            }
                        
        
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

public class UserDtoResponse: UserDto {
    public string username {get;set;}

    public int statuss {get;set;}
}


}
