using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Security.Cryptography;

namespace webapiserver.Controllers
{
  public class MomoHelper {

       public static IHttpContextAccessor httpContextAccessor;
            public static string HmacSHA256(string input, string key) {
                    byte[] keybyte = Encoding.UTF8.GetBytes(key);
                    byte[] messageBytes = Encoding.UTF8.GetBytes(input);
                    using (var hmac256 = new HMACSHA256(keybyte)){
                    byte[] hashmessage =  hmac256.ComputeHash(messageBytes);
                    string hex = BitConverter.ToString(hashmessage);
                    hex = hex.Replace("-","").ToLower();
                    return hex;
                }  

            }
             public class MomoResponse {

         


        public string partnerCode {get;set;}

        public string requestId {get;set;}
         
          public string orderId {get;set;}
           public long amount {get;set;}
            public long responseTime {get;set;}
             public string message {get;set;}
              public string resultCode {get;set;}
               public string pauUrl {get;set;}
                public string deeplink {get;set;}
                 public string qrCodeUrl {get;set;}

    }

    public static string? IpAddress => httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();

public static string? GetIpAddress()
{
          string ipAddress;
          try
            {
        ipAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

        if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
        ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
             }
           catch (Exception ex)
          {
        ipAddress = "Invalid IP:" + ex.Message;
          }

        return ipAddress;
 }
   
  }

   
}