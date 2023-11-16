using System.Net.Http.Headers;
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
// using MySql.Data.EntityFrameworkCore;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodComboController : ControllerBase
{
        private readonly CinemaContext _context;
        public FoodComboController(CinemaContext context)
        {
            _context = context;
            
        }

// API GET LIST VOUCHER
[HttpPost("CreateFoodCombo")]
public IActionResult CreateFoodCombo([FromBody] FoodComboNew combofood )
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
                    FoodCombo combo = new FoodCombo {
                        nametittle = combofood.nametittle,
                        descriptions = combofood.discription,
                        priceCombo = combofood.priceCombo,
                        picture = combofood.picture
                    };
                    _context.Foodcombo.Add(combo);
                    _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = combo;
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

// API GET LIST VOUCHER
[HttpPost("AddFoodComboFood")]
public IActionResult AddFoodComboFood([FromBody] FoodComboNew combofood )
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
                List<FoodComboBill> data  = new List<FoodComboBill>();
                FoodCombo foodcomboo = new FoodCombo {
                  nametittle = combofood.nametittle,
                  descriptions = combofood.discription,
                  priceCombo = combofood.priceCombo,
                  picture = combofood.picture
                };
                   _context.Foodcombo.Add(foodcomboo);
                    _context.SaveChanges();
                    if (foodcomboo.idcombo != 0) {
                          foreach (var foods in combofood.foodCreates) {
                          FoodComboBill combo = new FoodComboBill {
                          idcombo = foodcomboo.idcombo,
                          Idfood = foods.Idfood
                          };
                          data.Add(combo);
                          _context.FoodComboBill.Add(combo);
                          _context.SaveChanges();
                          }
                    }
                 
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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

// API GET LIST VOUCHER
[HttpPost("CreateFoodNew")]
public IActionResult CreateFoodNew([FromBody] Foodnew combofood )
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
              
                   Food fd = new Food {
                    Namefood = combofood.Namefood,
                    Quantityfood = combofood.Quantityfood,
                    Pricefood = combofood.Pricefood,
                    Picture = combofood.Picture,
                    Idcategoryfood = combofood.Idcategoryfood,
                    datecreate =  DateTime.Now
                   };


                 
                    _context.Foods.Add(fd);
                    _context.SaveChanges();
                  
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = fd;
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


[HttpGet("getListFood")]
public IActionResult getListFood(int Idcategoryfood)
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
              
                 

                    List<Food> data = new List<Food>();
                 if (Idcategoryfood == 0) {
                      data  = _context.Foods.ToList();
                 }else {
                   data  = _context.Foods.Where(x=>x.Idcategoryfood == Idcategoryfood).ToList();
                 }
                   
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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

[HttpGet("getListComboFood")]
public IActionResult getListComboFood()
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
              
                   var data  = _context.Foodcombo.ToList();
                   List<foodCombowithfood> combodata = new List<foodCombowithfood>();
                   foreach (var itemcombo in data) {
                     var dataFood = _context.FoodComboBill.Where(x=>x.idcombo == itemcombo.idcombo).ToList();
                     foodCombowithfood foodcombowithfood = new foodCombowithfood {
                        idcombo = itemcombo.idcombo,
                        nametittle = itemcombo.nametittle,
                        descriptions = itemcombo.descriptions,
                        priceCombo = itemcombo.priceCombo,
                        picture = itemcombo.picture,
                        foods = dataFood
                     };
                 
                     
                        combodata.Add(foodcombowithfood);
                   }
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = combodata;
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

// [HttpGet("getListFoods")]
// public IActionResult getListFoods(int Idcategoryfood)
// {
//     // khoi tao api response
//     var successApiResponse = new ApiResponse();
//     //header
//        string token = Request.Headers["token"];
//        string filterHeaderValue2 = Request.Headers["ProjectId"];
//        string filterHeaderValue3 = Request.Headers["Method"];
//        string expectedToken = ValidHeader.Token;
//        string method =Convert.ToString(ValidHeader.MethodGet);
//        string Pojectid = Convert.ToString(ValidHeader.Project_id);
//     //check header
//         if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(filterHeaderValue2) || string.IsNullOrEmpty(filterHeaderValue3))
//         {
//         // The "Authorize" header was not found in the request
//            return BadRequest("Authorize header not found in the request.");
//         }else {

//             if (token != expectedToken || filterHeaderValue2 != Pojectid || filterHeaderValue3 != method)
//           {
//             return Unauthorized("Invalid token."); // Return an error response if the tokens don't match
//           }else{
//             // if (date != null && Idmovie != null){
                
                  
//                try
//                  {
              
                 


                 
//                      var data  = _context.Categoryfoods.ToList();
                 
//                       successApiResponse.Status = 200;
//                      successApiResponse.Message = "OK";
//                      successApiResponse.Data = data;
//                  }
//                  catch (IndexOutOfRangeException ex)
//                   {
    
//                   }     
//             // }else {
//             //     return BadRequest("khong tim thay thong tin");
//             // }
                 

//            }

//         }
//  return Ok(successApiResponse);
// }

[HttpGet("getListCategoryFood")]
public IActionResult getListCategoryFood()
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
              
                 


                 
                     var data  = _context.Categoryfoods.ToList();
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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

[HttpPost("UpdateInfoCombofood")]
public IActionResult UpdateInfoCombofood([FromBody] FoodComboNew FOODS)
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
              
                     var dataFoodCombo = _context.Foodcombo.Find(FOODS.idcombo);
                     dataFoodCombo.nametittle = FOODS.nametittle;
                     dataFoodCombo.descriptions = FOODS.discription;
                     dataFoodCombo.picture = FOODS.picture;
                     dataFoodCombo.priceCombo = FOODS.priceCombo;

                     var ComboWithFood = _context.FoodComboBill.Where(x => x.idcombo == dataFoodCombo.idcombo).ToList();

                     foreach (var item in ComboWithFood) 
                     {
                     
                           _context.FoodComboBill.Remove(item);
                     // Save changes for each FoodComboBill inside the inner loop
                     _context.SaveChanges(); 
                     }
                        foreach (var item2 in FOODS.foodCreates) 
                        {
                           FoodComboBill foodadd = new FoodComboBill{
                              idcombo = FOODS.idcombo,
                              Idfood = item2.Idfood
                           };
                              _context.FoodComboBill.Add(foodadd);
                            _context.SaveChanges();
                        }
                     // Update dataFoodCombo after updating FoodComboBill entities
                     _context.Update(dataFoodCombo);

                     // Save changes for dataFoodCombo
                     _context.SaveChanges();

                 
                     var data  = dataFoodCombo.idcombo;
                 
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = data;
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



[HttpGet("getDetailFood")]
public IActionResult getDetailFood(int idfood)
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
              
                   var datafood = _context.Foods.Where(x=>x.Idfood == idfood).SingleOrDefault();
                   var datacategoryfood = _context.Categoryfoods.Where(x=>x.Idcategoryfood == datafood.Idcategoryfood).SingleOrDefault();
                   Foodnew foods = new Foodnew();
                   foods.Idfood = datafood.Idfood;
                   foods.Namefood = datafood.Namefood;
                   foods.Pricefood = datafood.Pricefood;
                   foods.Picture = datafood.Picture;
                   foods.Idcategoryfood = datafood.Idcategoryfood;
                   foods.Quantityfood = datafood.Quantityfood;
                    foods.namecategoryfood = datacategoryfood.Namecategoryfood;
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = foods;
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

[HttpPost("UpdateFood")]
public IActionResult UpdateFood([FromBody] Foodnew food)
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
              
                   var foods = _context.Foods.Find(food.Idfood);
                 
                   foods.Namefood = food.Namefood;
                   foods.Pricefood = food.Pricefood;
                   foods.Picture = food.Picture;
                   foods.Idcategoryfood = food.Idcategoryfood;
                   foods.Quantityfood = food.Quantityfood;
                  _context.Foods.Update(foods);
                  _context.SaveChanges();
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = foods;
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



public class foodCombowithfood {
     public int idcombo {get;set;}

        public string descriptions {get;set;}

        public string nametittle {get;set;}

        public int priceCombo {get;set;}

        public string picture {get;set;}
   public List<FoodComboBill> foods {get;set;}
}

public class FoodComboNew {

  public int idcombo {get;set;}
    public string nametittle {get;set;}
    public string discription {get;set;}

    public int priceCombo {get;set;}

    public string picture {get;set;} 

   public List<FoodCreate> foodCreates {get;set;} = new List<FoodCreate>();

}

public class FoodCreate {
   public int idcombo {get;set;}
    public int Idfood {get;set;}
    public int Id {get;set;}
}
public partial class Foodnew
{
    public int Idfood { get; set; }

    public string? Namefood { get; set; }

    public int? Quantityfood { get; set; }

    public string? Picture { get; set; }

    public int? Pricefood { get; set; }

    public int? Idcategoryfood { get; set; }

    public DateTime datecreate {get;set;}
public string namecategoryfood {get;set;}
  
}


}
