using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace webapiserver.Controllers
{
   public class HashHelper
   {
     public static String HmacSHA512(string key, string inputData) {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
          byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
          using (var hmac = new HMACSHA512(keyBytes)){
            byte[] hashvalue = hmac.ComputeHash(inputBytes);
            foreach(var theByte in hashvalue){
                hash.Append(theByte.ToString("x2"));
            }
          }
          return hash.ToString();
     }

    
    }
   }

