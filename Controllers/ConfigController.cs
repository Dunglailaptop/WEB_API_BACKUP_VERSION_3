using System;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Data;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    //    [HttpGet("configuration")]
    //     [Authorize] 
    // public IActionResult GetConfiguration()
    // {
    //     var response = new ApiResponse
    //     {
    //         Status = 200,
    //         Message = "OK",
    //         Data = new ApiData
    //         {
    //             Type = "Basic",
    //             Version = "1",
    //             ApiKey = "Z3BxdGx2MTo3ODdhMzg3NC1jYzczLTExZWQtODM3YS0wMDBjMjk3OTY5NWY=",
    //             ApiDomain = "https://api.tms.techres.vn",
    //             ApiChatTms = "http://172.16.2.245:1484",
    //             ApiChatAloline = "http://172.16.2.245:1485",
    //             ApiUpload = "http://172.16.10.114:9007",
    //             ApiUploadShort = "http://172.16.2.243:7080/short/",
    //             ApiConnection = "http://172.16.10.97:9013",
    //             ApiLog = "http://172.16.2.255:1487",
    //             AdsDomain = "http://172.16.10.114:9007",
    //             ChatDomain = "http://172.16.2.245:1484",
    //             ApiOauthNode = "http://172.16.2.245:9999"
    //         }
    //     };

    //     return Ok(response);
    // }
    private readonly CinemaContext _context;
    private readonly AppSettings _appSettings;

      IConfiguration _configuration;


    public ConfigController(CinemaContext context,IOptionsMonitor<AppSettings> optionMonitor,IConfiguration configuration)
    {
        _context = context;
        _appSettings = optionMonitor.CurrentValue;
         _configuration = configuration;
    }


     [HttpGet("Config")]
     public IActionResult GetConfig(string username){
          var UserConfig = _context.Accounts.SingleOrDefault(x => x.Username == username);

          if (UserConfig == null){
            return Ok(new ApiResponse
                {
                   Status = 404,
                   Message = "Không tìm thấy tài khoản trong hệ thống",
                   Data = null
                });
          }

          //cap token
          return Ok(new ApiResponse {
            Status = 200,
            Message = "Success",
            Data = new ApiData
            {
                 Type = "Basic",
                Version = "1",
                ApiKey = "YourExpectedTokenValue",
                ApiDomain = "https://api.tms.techres.vn",
                ApiChatTms = "http://172.16.2.245:1484",
                ApiChatAloline = "http://172.16.2.245:1485",
                ApiUpload = "http://172.16.10.114:9007",
                ApiUploadShort = "http://172.16.2.243:7080/short/",
                ApiConnection = "http://172.16.10.97:9013",
                ApiLog = "http://172.16.2.255:1487",
                AdsDomain = "http://172.16.10.114:9007",
                ChatDomain = "http://172.16.2.245:1484",
                ApiOauthNode = "http://172.16.2.245:9999"

            }
          });

     }

     [HttpGet("Session")]
     public IActionResult getSession()
     {
    var secretKey = "your_secret_key_here";
    var issuer = "your_issuer_here";
    var audience = "your_audience_here";
   var userId = "your_user_id_here";

var token = GenerateJwtToken(secretKey, issuer, audience, userId);

// 'token' now contains the JWT token, which you can use as needed.

                return  Ok(token);
     }



[HttpGet("testgetheader")]
public IActionResult Get()
{
    // Retrieve a specific request header
     string token = Request.Headers["token"];
      string filterHeaderValue2 = Request.Headers["ProjectId"];
  string filterHeaderValue3 = Request.Headers["Method"];
    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        // The "Authorize" header was not found in the request
        return BadRequest("Authorize header not found in the request.");
    }

    // Your code logic here (optional)

    // Check if the provided token matches the expected token
    string expectedToken = ValidHeader.Token;
     string method =Convert.ToString(ValidHeader.MethodGet);
      string Pojectid = Convert.ToString(ValidHeader.Project_id);
    if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
    {
        return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
    }

    // Set a custom response header
    Response.Headers.Add("Custom-Header", "Custom Value");

    return Ok("Header value: " + filterHeaderValue2 + filterHeaderValue3);
}


private string GenerateJwtToken(string secretKey, string issuer, string audience, string userId, double expirationMinutes = 60)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
        },
        notBefore: DateTime.UtcNow,
        expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
        signingCredentials: credentials
    );

    var tokenHandler = new JwtSecurityTokenHandler();
    var encodedToken = tokenHandler.WriteToken(token);

    return encodedToken;
}



    // private string GenerateToken(Account api){
    //     var jwtoken = new JwtSecurityTokenHandler();

    //     var secretkeyByte = Encoding.UTF8.GetBytes(_appSettings.Secretkey);
        
    //     var tokenDescription = new SecurityTokenDescriptor{
    //          Subject = new ClaimsIdentity(new[]
    //          {
    //              new Claim("TokenId", Guid.NewGuid().ToString())
    //          }),
    //           SigningCredentials = new SigningCredentials(new SymmetricSecurityKey (secretkeyByte),SecurityAlgorithms.HmacSha256Signature)
    //     };
    //      var token = jwtoken.CreateToken(tokenDescription);
    //     return jwtoken.WriteToken(token);
    // }
  // public string gettoken(Account account)
  // {
  //      var claims = new[] {
  //                       new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
  //                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
  //                       new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
  //                       new Claim("username", account.Username.ToString()),
  //                       new Claim("password", account.Password.ToString()),
  //                       new Claim("EmployeeId", account.EmployeeId.ToString())
                     
  //                   };


  //                   var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
  //                   var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
  //                   var token = new JwtSecurityToken(
  //                       _configuration["Jwt:Issuer"],
  //                       _configuration["Jwt:Audience"],
  //                       claims,
  //                       expires: DateTime.UtcNow.AddMinutes(10),
  //                       signingCredentials: signIn);


  //                   string tokenn = new JwtSecurityTokenHandler().WriteToken(token);

  //                   return tokenn;
  // }

}
