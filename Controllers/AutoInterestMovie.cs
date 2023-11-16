using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyCinema.Model;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using ZstdSharp.Unsafe;
using Microsoft.EntityFrameworkCore;

namespace webapiserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutointerestMovieController : ControllerBase
    {
        private readonly CinemaContext _context;

        public AutointerestMovieController(CinemaContext context)
        {
            _context = context;
        }

 
public class MovieSchedule
{
    public long? idinterest {get;set;}
    public long? idMovie { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? alltime {get;set;}
    public string? image {get;set;}

    public string? namemovie {get;set;}

    public int status {get;set;}

    public int resetTime {get;set;}
}

public class RoomAuto
{
    public long? idroom { get; set; }
    public List<MovieSchedule> Schedule { get; set; } = new List<MovieSchedule>();
}
 public class MovieRoomLists
{
   
    public DateTime dayStart {get;set;}
     public DateTime dayEnd {get;set;}
    public List<MovieSchedule> MovieList { get; set; }
    public Roomdata RoomList { get; set; }

    public int breakTime {get;set;}
    
}

public class Roomdata {
    public int idroom {get;set;}
     public long? Idcinema {get;set;}
}

public class requestresponse {
    public string data {get;set;}
    public RoomAuto list {get;set;}
}

 
// public class CinemaScheduler
// {
//     public static List<RoomAuto> GenerateSchedule(List<string> movieList, List<string> roomList, int durationInMinutes)
//     {
//         List<RoomAuto> rooms = new List<RoomAuto>();

//         foreach (string roomName in roomList)
//         {
//             RoomAuto room = new RoomAuto { Name = roomName };

//             DateTime currentDate = DateTime.Now.Date;
//             DateTime endDate = currentDate.AddDays(7);

//             TimeSpan startHour = TimeSpan.FromHours(8); // Thời gian bắt đầu (8 giờ sáng)
//             TimeSpan endHour = TimeSpan.FromHours(22);  // Thời gian kết thúc (10 giờ tối)

//             DateTime currentTime = currentDate + startHour;

//             while (currentTime.TimeOfDay <= endHour && currentDate <= endDate)
//             {
//                 foreach (string movieName in movieList)
//                 {
//                     DateTime endTime = currentTime.AddMinutes(durationInMinutes);

//                     if (endTime.TimeOfDay <= endHour)
//                     {
//                         MovieSchedule newMovie = new MovieSchedule
//                         {
//                             MovieName = movieName,
//                             StartTime = currentTime,
//                             EndTime = endTime
//                         };

//                         room.Schedule.Add(newMovie);
//                     }
//                 }

//                 currentTime = currentDate + startHour; // Reset lại thời gian bắt đầu cho ngày tiếp theo
//                 currentDate = currentDate.AddDays(1); // Chuyển qua ngày tiếp theo
//             }

//             rooms.Add(room);
//         }

//         return rooms;
//     }
// }
//    public static List<RoomAuto> GenerateSchedule(List<string> movieList, List<string> roomList, int durationInMinutes)
//     {
//         List<RoomAuto> rooms = new List<RoomAuto>();

//         foreach (string roomName in roomList)
//         {
//             RoomAuto room = new RoomAuto { Name = roomName };

//             DateTime currentDate = DateTime.Now.Date;
//             DateTime endDate = currentDate.AddDays(7);

//             TimeSpan startHour = TimeSpan.FromHours(8); // Thời gian bắt đầu (8 giờ sáng)
//             TimeSpan endHour = TimeSpan.FromHours(22);  // Thời gian kết thúc (10 giờ tối)

//             DateTime currentTime = currentDate + startHour;

//             while (currentTime.TimeOfDay <= endHour && currentDate <= endDate)
//             {
//                 foreach (string movieName in movieList)
//                 {
//                     DateTime endTime = currentTime.AddMinutes(durationInMinutes);

//                     if (endTime.TimeOfDay <= endHour)
//                     {
//                         MovieSchedule newMovie = new MovieSchedule
//                         {
//                             MovieName = movieName,
//                             StartTime = currentTime,
//                             EndTime = endTime
//                         };

//                         room.Schedule.Add(newMovie);
//                     }
//                 }

//                 currentTime = currentDate + startHour; // Reset lại thời gian bắt đầu cho ngày tiếp theo
//                 currentDate = currentDate.AddDays(1); // Chuyển qua ngày tiếp theo
//             }

//             rooms.Add(room);
//         }

//         return rooms;
//     }

// public static List<RoomAuto> GenerateSchedule(List<string> movieList, List<string> roomList, int movieDurationMinutes)
// {
//     List<RoomAuto> rooms = new List<RoomAuto>();

//     foreach (string roomName in roomList)
//     {
//         RoomAuto room = new RoomAuto { Name = roomName };

//         DateTime currentDate = DateTime.Now.Date;
//         DateTime endDate = currentDate.AddDays(7);

//         while (currentDate <= endDate)
//         {
//             DateTime startTime = currentDate.Date + TimeSpan.FromHours(8); // 8:00 AM
//             DateTime endTime = currentDate.Date + TimeSpan.FromHours(22).Add(TimeSpan.FromMinutes(30)); // 10:30 PM

//             foreach (string movieName in movieList)
//             {
//                 DateTime movieStartTime = startTime;
//                 DateTime movieEndTime = movieStartTime.AddMinutes(movieDurationMinutes);

//                 while (movieEndTime <= endTime)
//                 {
//                     MovieSchedule newMovie = new MovieSchedule
//                     {
//                         MovieName = movieName,
//                         StartTime = movieStartTime,
//                         EndTime = movieEndTime
//                     };

//                     room.Schedule.Add(newMovie);

//                     movieStartTime = movieEndTime; // Next movie starts after current movie
//                     movieEndTime = movieStartTime.AddMinutes(movieDurationMinutes);
//                 }
//             }

          

//             // Move to the next day
//             currentDate = currentDate.AddDays(1);
//         }
//           rooms.Add(room);
//     }

//     return rooms;
// }

public static RoomAuto GenerateSchedule( CinemaContext _context,DateTime dateStart,DateTime dateEnd,List<MovieSchedule> movieList, Roomdata roomList, int movieDurationMinutes,int breakTimeMinutes)
{
    Random random = new Random();
    RoomAuto rooms = new RoomAuto();

    
        rooms.idroom = roomList.idroom ;

        DateTime currentDate = dateStart;
        DateTime endDate = dateEnd;

        while (currentDate <= endDate)
        {
            DateTime startTime = currentDate.Date + TimeSpan.FromHours(8); // 8:00 AM
            DateTime endTime = currentDate.Date + TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(breakTimeMinutes)); // 10:30 PM
          
            
            while (startTime.AddMinutes(movieDurationMinutes) <= endTime)
            {
               
                  MovieSchedule randomMovie = movieList[random.Next(movieList.Count)]; // Chọn ngẫu nhiên một bộ phim
                //  string[] parts = randomMovie.Split('/'); // Tách chuỗi theo ký tự '/'
                // int movieDurationMinutess = Convert.ToInt32(parts[1]); // Chuyển đổi phần tử thứ hai thành số nguyên
               int movieDurationMinutess = Convert.ToInt32(randomMovie.alltime);
                DateTime movieEndTime = startTime.AddMinutes(movieDurationMinutess);
                 var data = _context.Movies.Where(x=>x.Idmovie == randomMovie.idMovie).SingleOrDefault();
                MovieSchedule newMovie = new MovieSchedule
                {
                    idMovie = randomMovie.idMovie,
                    namemovie = data.Namemovie,
                    image = data.Poster,
                    StartTime = startTime,
                    EndTime = movieEndTime,
                    alltime = data.Timeall,
                    resetTime = breakTimeMinutes
                };

                rooms.Schedule.Add(newMovie);

                startTime = movieEndTime.AddMinutes(breakTimeMinutes);  // Next movie starts after current movie
            }

           
            // Move to the next day
            currentDate = currentDate.AddDays(1);
        }
   

    

    return rooms;
}

// public static List<RoomAuto> GenerateSchedule( DateTime startDate, DateTime endDate,List<string> movieList, List<string> roomList)
// {
//     List<RoomAuto> rooms = new List<RoomAuto>();
//     Random random = new Random();

//     int roomIndex = 0;

//     for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
//     {
//         RoomAuto room = new RoomAuto { Name = roomList[roomIndex % roomList.Count] }; // Lặp lại các rạp
//         roomIndex++;

//         DateTime currentTime = currentDate + TimeSpan.FromHours(8); // Bắt đầu từ 8h sáng

//         while (currentTime.TimeOfDay <= TimeSpan.FromHours(22.5)) // 22h30
//         {
//             string randomMovie = movieList[random.Next(movieList.Count)];
//             string[] parts = randomMovie.Split('/');
//             int movieDurationMinutes = Convert.ToInt32(parts[1]);

//             DateTime movieEndTime = currentTime.AddMinutes(movieDurationMinutes);
//             if (movieEndTime.TimeOfDay <= TimeSpan.FromHours(22.5))
//             {
//                 MovieSchedule newMovie = new MovieSchedule
//                 {
//                     MovieName = randomMovie,
//                     StartTime = currentTime,
//                     EndTime = movieEndTime
//                 };

//                 room.Schedule.Add(newMovie);
//                 currentTime = movieEndTime;
//             }
//             else
//             {
//                 break;
//             }
//         }

//         rooms.Add(room);
//     }

//     return rooms;
// }
// class update status
public class updatestatus {
    public int idinterest {get;set;}
    public int status {get;set;}
}
// API update status interest
[HttpPost("UpdateStatusInterest")]
public IActionResult UpdateStatusInterest([FromBody] updatestatus updatestatus)
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
                    
                    var entity = _context.Cinemainterests.Find(updatestatus.idinterest);
                    entity.statusinterest = updatestatus.status;
                    _context.Cinemainterests.Update(entity);
                    _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = entity;
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



[HttpPost("InsertIntoAutoInterest")]
public IActionResult InsertIntoAutoInterest([FromBody] MovieRoomLists datas)
{
    var successApiResponse = new ApiResponse();

    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodPost);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token.");
        }
        else
        {
            try
            {
           
                 foreach(var movieList in datas.MovieList) {
                    Cinemainterest interest = new Cinemainterest {
                          Idroom = datas.RoomList.idroom,
                          Idmovie = movieList.idMovie,
                          Times = movieList.StartTime,
                          TimeEnd = movieList.EndTime,
                          Dateshow = movieList.StartTime,
                          Idcinema = datas.RoomList.Idcinema,
                          resetTime = datas.breakTime,
                          statusinterest = 0

                    };
                       _context.Cinemainterests.Add(interest);
                 }
               
            
                 _context.SaveChanges();

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = "";
            }
            catch (Exception ex)
            {
                successApiResponse.Status = 500;
                successApiResponse.Message = "Internal Server Error";
                successApiResponse.Data = ex.Message;
            }
        }
    }
    return Ok(successApiResponse);
}

//
public class timeinterest {
    public int idinterest {get;set;}

    public long idmovie {get;set;}

}

[HttpPost("UpdateInterestMovie")]
public IActionResult UpdateInterestMovie([FromBody] timeinterest times)
{
    var successApiResponse = new ApiResponse();

    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodPost);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token.");
        }
        else
        {
            try
            {
           
                var dataupdate = _context.Cinemainterests.Find(times.idinterest);
                if (dataupdate != null) {
                  dataupdate.Idmovie = times.idmovie;
                    _context.SaveChanges();
                }

                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = dataupdate;
            }
            catch (Exception ex)
            {
                successApiResponse.Status = 500;
                successApiResponse.Message = "Internal Server Error";
                successApiResponse.Data = ex.Message;
            }
        }
    }
    return Ok(successApiResponse);
}


[HttpGet("getListInterest")]
public async Task<IActionResult> getListInterest(int idcinema, int idroom,string date)
{
    var successApiResponse = new ApiResponse();

    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token.");
        }
        else
        {
            try
            {
                    var sql = "SELECT * FROM cinema.Cinemainterest s WHERE DATE_FORMAT(s.Times, '%Y-%m-%d') LIKE '" + date + "%' and Idroom = '"+ idroom +"' and Idcinema = '"+ idcinema +"'";
                    var data = _context.Cinemainterests.FromSqlRaw(sql).ToList();


                    requestresponse response = new requestresponse();

                    // Tạo danh sách RoomAuto
                   

                    RoomAuto room = new RoomAuto
                    {
                    idroom = idroom, // Lấy idroom từ tham số truyền vào
                    Schedule = new List<MovieSchedule>()
                    };

                    // Duyệt qua danh sách cinemainterest và thêm dữ liệu vào room.Schedule
                    foreach (var item in data)
                    {
                    var dataMovie = await _context.Movies.Where(x => x.Idmovie == item.Idmovie).SingleOrDefaultAsync();

                    if (dataMovie != null)
                    {
                    MovieSchedule schedule = new MovieSchedule
                    {
                    idMovie = item.Idmovie,
                    StartTime = item.Times,
                    EndTime = item.TimeEnd,
                    image = dataMovie.Poster,
                    namemovie = dataMovie.Namemovie,
                    alltime = dataMovie.Timeall,
                    status = item.statusinterest,
                    idinterest = item.Idinterest,
                    resetTime = item.resetTime
                    };

                    room.Schedule.Add(schedule);
                    }
                    }

                  

                    // Gán dữ liệu vào response
                    response.data = ""; // Điền dữ liệu tương ứng nếu cần
                    response.list = room; // Gán danh sách RoomAuto

                    successApiResponse.Status = 200;
                    successApiResponse.Message = "OK";
                    successApiResponse.Data = response; 

            }
            catch (Exception ex)
            {
                successApiResponse.Status = 500;
                successApiResponse.Message = "Internal Server Error";
                successApiResponse.Data = ex.Message;
            }
        }
    }
    return Ok(successApiResponse);
}


[HttpGet("getInfoInterestMovie")]
public async Task<IActionResult> getInfoInterestMovie(int idmoive,int idinterest,int idroom,int idcinema)
{
    var successApiResponse = new ApiResponse();

    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodGet);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token.");
        }
        else
        {
            try
            {
              
                        var datainterest = _context.Cinemainterests.Where(x=>x.Idinterest == idinterest).SingleOrDefault();
                        var datamovie = _context.Movies.Where(x=>x.Idmovie == idmoive).SingleOrDefault();
                        var dataroom = _context.Rooms.Where(x=>x.Idroom == idroom).SingleOrDefault();
                        var datacinema = _context.Cinemas.Where(x=>x.Idcinema == idcinema).SingleOrDefault();
                        MovieInterestShowInfo dataget = new MovieInterestShowInfo{
                                nameroom = dataroom.Nameroom,
                                namecinema = datacinema.Namecinema,
                                startstime = datainterest.Times,
                                endtime = datainterest.TimeEnd,
                                dateshow = datainterest.Dateshow,
                                namemovie = datamovie.Namemovie,
                                poster = datamovie.Poster,
                                idcinema = datacinema.Idcinema,
                                idmovie = datamovie.Idmovie,
                                idinterest = datainterest.Idinterest
                        };
                        successApiResponse.Status = 200;
                        successApiResponse.Message = "OK";
                        successApiResponse.Data =  dataget; 

            }
            catch (Exception ex)
            {
                successApiResponse.Status = 500;
                successApiResponse.Message = "Internal Server Error";
                successApiResponse.Data = ex.Message;
            }
        }
    }
    return Ok(successApiResponse);
}



[HttpPost("AutoGetListInterest")]
public IActionResult AutoGetListInterest([FromBody] MovieRoomLists datas)
{
    var successApiResponse = new ApiResponse();

    string token = Request.Headers["token"];
    string filterHeaderValue2 = Request.Headers["ProjectId"];
    string filterHeaderValue3 = Request.Headers["Method"];
    string expectedToken = ValidHeader.Token;
    string method = Convert.ToString(ValidHeader.MethodPost);
    string Pojectid = Convert.ToString(ValidHeader.Project_id);

    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
    {
        return BadRequest("Authorize header not found in the request.");
    }
    else
    {
        if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
        {
            return Unauthorized("Invalid token.");
        }
        else
        {
            try
            {
    //   List<string> movieList = new List<string>
    //     {
    //         "Movie 1",
    //         "Movie 2",
    //         "Movie 3"
    //     };
        

    //     List<string> roomList = new List<string>
    //     {
    //         "Room A",
    //         "Room B",
    //         "Room C"
    //     };

        int durationInMinutes = 120; // Độ dài mỗi buổi phim (2 giờ)

         List<MovieSchedule> movieList = datas.MovieList;
        Roomdata roomList = datas.RoomList;
        requestresponse list = new requestresponse();
        DateTime dayStart = datas.dayStart;
        DateTime dayEnd = datas.dayEnd;
       var schedule = GenerateSchedule(_context,dayStart,dayEnd,movieList, roomList,durationInMinutes,datas.breakTime);
       list.list = schedule;
///////
// List<MovieSchedule> movieList = new List<MovieSchedule>
// {
//     new MovieSchedule { MovieName = "Movie 1", StartTime = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(3) },
//     new MovieSchedule { MovieName = "Movie 2", StartTime = DateTime.Now.AddHours(4), EndTime = DateTime.Now.AddHours(6) }
// };

// List<string> roomList = new List<string>
// {
//     "Room A",
//     "Room B"
// };

// List<RoomAuto> schedule = GenerateSchedule(movieList, roomList,120);


                successApiResponse.Status = 200;
                successApiResponse.Message = "OK";
                successApiResponse.Data = list;
            }
            catch (Exception ex)
            {
                successApiResponse.Status = 500;
                successApiResponse.Message = "Internal Server Error";
                successApiResponse.Data = ex.Message;
            }
        }
    }
    return Ok(successApiResponse);
}



    }
public class MovieInterestShowInfo {
   public string namemovie {get;set;}
   public DateTime? startstime {get;set;}
   public DateTime? endtime {get;set;}
   public DateTime? dateshow {get;set;}

   public string nameroom {get;set;}

   public string namecinema {get;set;}
   public string poster {get;set;}

   public long? idcinema {get;set;}
   public long? idmovie {get;set;}
   public long? idinterest {get;set;}
}
 
}
