using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using MyCinema.Model;
using Microsoft.EntityFrameworkCore;

namespace MyCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class checkinController : ControllerBase
{
       private readonly CinemaContext _context;
       private readonly IMemoryCache _memmory;
        public checkinController(CinemaContext context,IMemoryCache memory)
        {
            _context = context;
            _memmory = memory;
        }

    // lưu cache
 public void SetCacheValue<T>(string key, T value, TimeSpan expirationTime)
    {
        _memmory.Set(key, value, expirationTime);
    }

    public T GetCacheValue<T>(string key)
    {
        return _memmory.Get<T>(key);
    }    
     public void UpdateCacheValue<T>(string key, T newValue, TimeSpan expirationTime)
    {
        // Kiểm tra xem khóa có tồn tại trong cache không
        if (_memmory.TryGetValue(key, out T existingValue))
        {
            // Nếu khóa đã tồn tại, cập nhật giá trị của khóa đó trong cache
            _memmory.Set(key, newValue, expirationTime);
        }
        else
        {
            // Nếu khóa không tồn tại, thêm khóa mới vào cache với giá trị mới
            _memmory.Set(key, newValue, expirationTime);
        }
    }

// API GET LIST VOUCHER
[HttpPost("checkin")]
public IActionResult checkin(checkin checkins)
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
                   var data = GetCacheValue<checkin>("infocheckcache");
                   var dataresponse = new checkin();
                   if (data == null){
                     var checkinnew = new checkin();
                     checkinnew.Idusers = checkins.Idusers;
                     checkinnew.timestart = DateTime.Now;
                     checkinnew.timeend = DateTime.Now;
                     checkinnew.checksession = 1;
                     checkinnew.Idcinema = checkins.Idcinema;
                    var datas = _context.checkin.Add(checkinnew);
                    _context.SaveChanges();
                     if (checkinnew.Idusers != null) {
                        SetCacheValue("infocheckcache",checkinnew,TimeSpan.FromDays(960));
                        dataresponse = checkinnew;
                     successApiResponse.Status = 200;
                     successApiResponse.Message = "Mở ca thành công";
                     successApiResponse.Data = dataresponse;
                    }

                    } else {
                        var dataupdatecheckin = _context.checkin.Find(data.idcheckin);
                        dataupdatecheckin.timeend = DateTime.Now;
                        dataupdatecheckin.checksession =  2;
                        _context.checkin.Update(dataupdatecheckin);
                        _context.SaveChanges();
                        UpdateCacheValue("infocheckcache",dataupdatecheckin,TimeSpan.FromDays(960));
                        dataresponse = dataupdatecheckin;
                        successApiResponse.Status = 200;
                        successApiResponse.Message = "Đóng ca thành công";
                        successApiResponse.Data = dataresponse;
                    }
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

public class checkinresponse: checkin {
   

     public string avatar {get;set;}
    
    public string nameusers {get;set;}
   

}

// API GET LIST VOUCHER
[HttpGet("getlistcheckin")]
public IActionResult getlistcheckin(long idcinema,long iduser,string timestart,string timeend,int type)
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
                  if (type == 1 ){
                   var sql = "select * from cinema.checkin where timestart >= '"+timestart+" 00:00:00' and timeend <= '"+timeend+" 23:59:59'  and Idusers =  '"+iduser+"'";
                      var data = _context.checkin.FromSqlRaw(sql).ToList();
                       List<checkinresponse> user = new List<checkinresponse>();
                      foreach (var listuser in data) {
                             var datalist = _context.Users.Where(x=>x.Idusers == listuser.Idusers).SingleOrDefault();
                             var datauser= new checkinresponse();
                             datauser.avatar = datalist.Avatar;
                             datauser.Idusers = datalist.Idusers;
                             datauser.nameusers = datalist.Fullname;
                             datauser.timestart = listuser.timestart;
                             datauser.timeend = listuser.timeend;
                             datauser.checksession = listuser.checksession;
                             datauser.idcheckin = listuser.idcheckin;
                             datauser.Idcinema = listuser.Idcinema;
                             user.Add(datauser);
                      }
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = user;
                  }else {
                        var sql = "select * from cinema.checkin where timestart >= '"+timestart+" 00:00:00' and timeend <= '"+timeend+" 23:59:59'  and Idcinema =  '"+iduser+"'";
                      var data = _context.checkin.FromSqlRaw(sql).ToList();
                     List<checkinresponse> user = new List<checkinresponse>();
                      foreach (var listuser in data) {
                             var datalist = _context.Users.Where(x=>x.Idusers == listuser.Idusers).SingleOrDefault();
                             var datauser= new checkinresponse();
                             datauser.avatar = datalist.Avatar;
                             datauser.Idusers = datalist.Idusers;
                             datauser.nameusers = datalist.Fullname;
                             datauser.timestart = listuser.timestart;
                             datauser.timeend = listuser.timeend;
                             datauser.checksession = listuser.checksession;
                             datauser.idcheckin = listuser.idcheckin;
                             datauser.Idcinema = listuser.Idcinema;
                             user.Add(datauser);
                      }
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = user;
                  }
                 
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

// api check session
[HttpGet("checksession")]
public IActionResult checksession()
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
                  
                   var datachecksession = GetCacheValue<checkin>("infocheckcache");
                   if (datachecksession != null) {
                      if (datachecksession.checksession < 2) {
                     successApiResponse.Status = 200;
                     successApiResponse.Message = "Xin vui lòng đóng ca trước khi đăng suất để hệ thống ghi nhận";
                     successApiResponse.Data = datachecksession;
                        }else if (datachecksession.checksession == 2) {
                            successApiResponse.Status = 500;
                        successApiResponse.Message = "stop";
                        successApiResponse.Data = datachecksession;
                        }
                   }else {
                     
                      successApiResponse.Status = 300;
                     successApiResponse.Message = "Xin vui lòng mở ca để thực hiện ghi nhận trên hệ thống";
                     successApiResponse.Data = datachecksession;
                   
                   }
                
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


}
