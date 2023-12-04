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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using RestSharp;
using Microsoft.Extensions.Caching.Memory;
// using MySql.Data.EntityFrameworkCore;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentMomoController : ControllerBase
{
        private readonly CinemaContext _context;
        private readonly VnpayConfig vnpayConfig;
       
      private readonly IMemoryCache _memoryCache;
        public PaymentMomoController(CinemaContext context,IOptions<VnpayConfig> VnpayConfigs,IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
            this.vnpayConfig  = VnpayConfigs.Value;
         
        }
// lưu cache
 public void SetCacheValue<T>(string key, T value, TimeSpan expirationTime)
    {
        _memoryCache.Set(key, value, expirationTime);
    }

    public T GetCacheValue<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }


//end luu cache
             

      
public class paymentrequest {
    public long vnp_Amount {get;set;}
    public string vnp_BankCode {get;set;}

    public string vnp_BankTranNo {get;set;}
     public string vnp_CardType {get;set;}
      public string vnp_OrderInfo {get;set;}
       public string vnp_PayDate {get;set;}

       public string vnp_ResponseCode {get;set;}

       public string vnp_TmnCode {get;set;}

       public string vnp_TransactionNo {get;set;}

       public string vnp_TransactionStatus {get;set;}

       public string vnp_TxnRef {get;set;}

       public string vnp_SecureHash {get;set;}
}

   [HttpGet("MakePayment")]
    public  IActionResult MakePayment(string vnp_Amount,string vnp_BankCode,string vnp_BankTranNo,string vnp_CardType,string vnp_OrderInfo ,
    string vnp_PayDate,
    string vnp_ResponseCode ,string vnp_TmnCode,string vnp_TransactionNo,string vnp_TransactionStatus,string vnp_TxnRef,string vnp_SecureHash)
    {
  
         var successApiResponse = new ApiResponse();
         long idorderfinal = 0;
        if (vnp_TransactionStatus == "00") {
           var cachedData = GetCacheValue<BillsVNPAY>("myCacheKey");
if (cachedData != null){
                 BillsVNPAY bl = new BillsVNPAY();
                  
                   
                       Bill billspay = new Bill {
                         Idcinema = cachedData.Idcinema,
                         Idinterest = cachedData.Idinterest,
                         Iduser = cachedData.Iduser,
                         Idmovie = cachedData.Idmovie,
                         Vat = cachedData.Vat,
                         Quantityticket = cachedData.Quantityticket,
                         Totalamount = cachedData.Totalamount,
                         Datebill = DateTime.Now,
                         Note = cachedData.Note,
                         Statusbill = cachedData.Statusbill,
                          Idvoucher = cachedData.Idvoucher <= 0 ? 10:cachedData.Idvoucher
                       };
                       _context.Bills.Add(billspay);
                       _context.SaveChanges();
                       bl.Idbill = billspay.Idbill;
                       bl.Totalamount = billspay.Totalamount;
                       if (billspay.Idbill != 0) {
                            foreach (var item in cachedData.ticket) {
                                Ticket TC = new Ticket {
                                Idbill =  billspay.Idbill,
                                Idchair = item.Idchair,
                                Idinterest = cachedData.Idinterest,
                                Pricechair = item.Pricechair
                                };
                                _context.Tickets.Add(TC);
                                _context.SaveChanges();
                            }
                       }
                      if (cachedData.combobill.Count != 0){
                         foreach(var item in cachedData.combobill) {
                              FoodComboWithBills foodcombowithbill = new FoodComboWithBills {
                                idcombo = item.idcombo,
                                Idbill = billspay.Idbill,
                              };
                              _context.FoodComboWithBills.Add(foodcombowithbill);
                              _context.SaveChanges();
                         };
                      };
      //tao giao dich vnpay
       var datapaymentvnpays = new paymentVNPAY();
      if (bl.Idbill != 0){
              
        datapaymentvnpays.vnp_Amount = vnp_Amount;
        datapaymentvnpays.vnp_BackTranNo = vnp_BankTranNo;
        datapaymentvnpays.vnp_BankCode = vnp_BankCode;
        datapaymentvnpays.vnp_TmnCode = vnp_TmnCode;
        datapaymentvnpays.vnp_TransactionSatus = vnp_TransactionStatus;
        datapaymentvnpays.vnp_CardType = vnp_CardType;
        datapaymentvnpays.vnp_OrderInfo = vnp_OrderInfo;
        datapaymentvnpays.vnp_PayDate = vnp_PayDate;
        datapaymentvnpays.vnp_ResponseCode = vnp_ResponseCode;
        datapaymentvnpays.vnp_TransactionNo = vnp_TransactionNo;
        datapaymentvnpays.vnp_TransactionSatus = vnp_TransactionStatus;
        datapaymentvnpays.vnp_TxnRef = vnp_TxnRef;
        datapaymentvnpays.vnp_SecureHash = vnp_SecureHash;
        datapaymentvnpays.Idbills = bl.Idbill;
       var datapaymentvnpay = _context.paymentVNPAY.Add(datapaymentvnpays);
       _context.SaveChanges();
      }
      //xu ly luu cache
         const string cacheKey = "intValue";  
      if (!_memoryCache.TryGetValue(cacheKey, out int cachedValue))
        {
    
       _memoryCache.Set(cacheKey, bl.Idbill, TimeSpan.FromMinutes(30));
         }
      //end
      // tao thong bao
        if (billspay.Idbill != null){
            var datanotifaction = new Notifaction();
            datanotifaction.messages = "Bạn có mã hoá đơn mua vé mới với mã là: " + billspay.Idbill;
            datanotifaction.iduser = billspay.Iduser;
            datanotifaction.datecreate = DateTime.Now;
            _context.Notifaction.Add(datanotifaction);
            _context.SaveChanges();
        }
      //
      idorderfinal = bl.Idbill;

      //sendemail=====
        List<Chair> chairs = new List<Chair>();
                     List<FoodCombo> foodcombo = new List<FoodCombo>();
                     foreach (var ticket in cachedData.ticket) {
                            var datachair = _context.Chairs.Where(x=>x.Idchair == ticket.Idchair).SingleOrDefault();
                             chairs.Add(datachair);
                     }
                     foreach (var foodcombos in cachedData.combobill){
                        var datacombo = _context.Foodcombo.Where(x=>x.idcombo == foodcombos.idcombo).SingleOrDefault();
                        foodcombo.Add(datacombo);
                     }
                     HashHelper.sendemailTicket("ndung983@gmail.com",chairs,billspay,foodcombo);

      //end======
       
        successApiResponse.Status = 200;
        successApiResponse.Message = "OK";

        successApiResponse.Data = datapaymentvnpays;
                    
}


       
        } else {
               successApiResponse.Status = 500;
        successApiResponse.Message = "error";

        successApiResponse.Data = "Thanh toan that bai";
        }
         
       
    var html = System.IO.File.ReadAllText(@"./Controllers/PaymentMomo/successPayment.aspx");
    html = html.Replace("{{name}}", idorderfinal.ToString());
    return base.Content(html, "text/html");
    //        var html = System.IO.File.ReadAllText(@"./Controllers/PaymentMomo/successPayment.aspx");
    // return base.Content(html, "text/html");
    
        // return Ok(successApiResponse);
    }
    //model food
    public class paymentBillFoodComboS {
            public int id { get; set; }
            public int  IdFoodcombo { get; set; }

            public int idFoodlistcombo { get; set; }

            public int numbers {get;set;}

            public DateTime datetimes {get;set;}

            public int total_price {get;set;}

            public long? iduser {get;set;}

            public long? idcinemas {get;set;}
            public int status {get;set;}
            public List<ListFoodCombo> foodComboBills {get;set;}

            public int idvoucher {get;set;}

}

     [HttpGet("MakePaymentFood")]
    public  IActionResult MakePaymentFood(string vnp_Amount,string vnp_BankCode,string vnp_BankTranNo,string vnp_CardType,string vnp_OrderInfo ,
    string vnp_PayDate,
    string vnp_ResponseCode ,string vnp_TmnCode,string vnp_TransactionNo,string vnp_TransactionStatus,string vnp_TxnRef,string vnp_SecureHash)
    {
  
         var successApiResponse = new ApiResponse();
         long idorderfinal = 0;
        if (vnp_TransactionStatus == "00") {
           var cachedData = GetCacheValue<paymentBillFoodComboS>("myCacheKeyFood");
if (cachedData != null){
                 FoodCombillPayment foodcombo = new FoodCombillPayment();
                  
                  foodcombo.idFoodlistcombo = 21;
                  foodcombo.IdFoodcombo = 1;
                  foodcombo.datetimes = DateTime.Now;
                  foodcombo.numbers = cachedData.numbers;
                  foodcombo.total_price = cachedData.total_price;
                  foodcombo.iduser = cachedData.iduser;
                  foodcombo.idcinemas = cachedData.idcinemas;
                  foodcombo.statusbillfoodcombo = 0;
                  foodcombo.idvoucher = cachedData.idvoucher;
                     _context.FoodCombillPayment.Add(foodcombo);
                     _context.SaveChanges();
                     if (foodcombo.id != null) {
                       foreach (var item in  cachedData.foodComboBills)
                        {
                        ListFoodCombo list = new ListFoodCombo();
                        list.idbillfood = foodcombo.id;
                        list.idfoodcombobill = foodcombo.id;
                        list.Idfoodcombo = item.Idfoodcombo;
                        _context.ListFoodCombo.Add(list);
                        _context.SaveChanges();       
                        }
                     } 
                     if (foodcombo.id != null){
                        var datanotifaction = new Notifaction();
                        datanotifaction.messages = "Bạn có hoá đơn đặt món ăn mới vui lòng kiểm tra thông tin với mã hoá đơn là: " + foodcombo.id;
                        datanotifaction.iduser = foodcombo.iduser;
                        datanotifaction.datecreate = DateTime.Now;
                        _context.Notifaction.Add(datanotifaction);
                        _context.SaveChanges();
                     }
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = foodcombo;
      //tao giao dich vnpay
       var datapaymentvnpays = new paymentVNPAY();
      if (foodcombo.id != 0){
              
        datapaymentvnpays.vnp_Amount = vnp_Amount;
        datapaymentvnpays.vnp_BackTranNo = vnp_BankTranNo;
        datapaymentvnpays.vnp_BankCode = vnp_BankCode;
        datapaymentvnpays.vnp_TmnCode = vnp_TmnCode;
        datapaymentvnpays.vnp_TransactionSatus = vnp_TransactionStatus;
        datapaymentvnpays.vnp_CardType = vnp_CardType;
        datapaymentvnpays.vnp_OrderInfo = vnp_OrderInfo;
        datapaymentvnpays.vnp_PayDate = vnp_PayDate;
        datapaymentvnpays.vnp_ResponseCode = vnp_ResponseCode;
        datapaymentvnpays.vnp_TransactionNo = vnp_TransactionNo;
        datapaymentvnpays.vnp_TransactionSatus = vnp_TransactionStatus;
        datapaymentvnpays.vnp_TxnRef = vnp_TxnRef;
        datapaymentvnpays.vnp_SecureHash = vnp_SecureHash;
        datapaymentvnpays.Idbills = 46;
        datapaymentvnpays.Idbill = foodcombo.id;
       var datapaymentvnpay = _context.paymentVNPAY.Add(datapaymentvnpays);
       _context.SaveChanges();
      }
      //xu ly luu cache
         const string cacheKey = "intValuefood";  
    //   if (!_memoryCache.TryGetValue(cacheKey, out int cachedValue))
    //     {
    
       _memoryCache.Set(cacheKey, foodcombo.id, TimeSpan.FromMinutes(30));
        //  }
      //end
      // tao thong bao
        if (foodcombo.id != null){
            var datanotifaction = new Notifaction();
            datanotifaction.messages = "Bạn có mã hoá đơn món ăn mới với mã là: " + foodcombo.id;
            datanotifaction.iduser = foodcombo.iduser;
            datanotifaction.datecreate = DateTime.Now;
            _context.Notifaction.Add(datanotifaction);
            _context.SaveChanges();
        }
      //
      idorderfinal = foodcombo.id;

         //send email
          var emailuser = _context.Users.Where(x=>x.Idusers == foodcombo.iduser).SingleOrDefault();
          var datalistfoodcombo = _context.ListFoodCombo.Where(x=>x.idfoodcombobill == foodcombo.id).ToList();
          List<FoodCombo> foodcombolist = new List<FoodCombo>();
          foreach (ListFoodCombo list in datalistfoodcombo) {
            var datafoodcombo = _context.Foodcombo.Where(x=>x.idcombo == list.Idfoodcombo).SingleOrDefault();
              foodcombolist.Add(datafoodcombo);
          }
        var checkbool = HashHelper.sendemail("ndung983@gmail.com",foodcombolist,foodcombo);

        successApiResponse.Status = 200;
        successApiResponse.Message = "OK";
        successApiResponse.Data = datapaymentvnpays;
                    
}


       
        } else {
               successApiResponse.Status = 500;
        successApiResponse.Message = "error";

        successApiResponse.Data = "Thanh toan that bai";
        }
         
       
    var html = System.IO.File.ReadAllText(@"./Controllers/PaymentMomo/successPayment.aspx");
    html = html.Replace("{{name}}", idorderfinal.ToString());
    return base.Content(html, "text/html");
    //        var html = System.IO.File.ReadAllText(@"./Controllers/PaymentMomo/successPayment.aspx");
    // return base.Content(html, "text/html");
    
        // return Ok(successApiResponse);
    }

   

public class responsePayment {
    public int idorder {get;set;}

    public decimal amount {get;set;}

    public string urlpayment {get;set;}
}

[HttpGet("getidbillPaymentVnpayFood")]
    public IActionResult getidbillPaymentVnpayFood(int idbill){
        //   var cachedData = GetCacheValue<BillsVNPAY>("myCacheKey");
          const string cacheKey = "intValuefood";
         var successApiResponse = new ApiResponse();
    if (_memoryCache.TryGetValue("intValuefood", out int cachedValue)){
          var dataidbill = _context.paymentVNPAY.Where(x=>x.Idbill == cachedValue).SingleOrDefault();
          
              if (dataidbill != null) {
                  successApiResponse.Status = 200;
                  successApiResponse.Message = "Thanh toán thành công vui lòng kiểm tra tin nhấn trong app";
                  successApiResponse.Data = dataidbill;
            }else {
                    successApiResponse.Status = 500;
            successApiResponse.Message = "Thanh toán thất bại vui lòng kiểm tra lại";
            successApiResponse.Data = dataidbill;
            }
    }else {
         successApiResponse.Status = 500;
            successApiResponse.Message = "Thanh toán thất bại vui lòng kiểm tra lại";
            successApiResponse.Data = "null";
    }
         

          
        
        return Ok(successApiResponse);
    }

 [HttpGet("getidbillPaymentVnpay")]
    public IActionResult getidbillPaymentVnpay(int idbill){
        //   var cachedData = GetCacheValue<BillsVNPAY>("myCacheKey");
          const string cacheKey = "intValue";
         var successApiResponse = new ApiResponse();
    if (_memoryCache.TryGetValue("intValue", out long cachedValue)){
          var dataidbill = _context.paymentVNPAY.Where(x=>x.Idbills == cachedValue).SingleOrDefault();
          
              if (dataidbill != null) {
                  successApiResponse.Status = 200;
                  successApiResponse.Message = "Thanh toán thành công vui lòng kiểm tra tin nhấn trong app";
                  successApiResponse.Data = dataidbill;
            }else {
                    successApiResponse.Status = 500;
            successApiResponse.Message = "Thanh toán thất bại vui lòng kiểm tra lại";
            successApiResponse.Data = dataidbill;
            }
    }else {
         successApiResponse.Status = 500;
            successApiResponse.Message = "Thanh toán thất bại vui lòng kiểm tra lại";
            successApiResponse.Data = "null";
    }
         

          
        
        return Ok(successApiResponse);
    }


[HttpPost("SaveCachePaymentBill")] 
 public IActionResult SaveCachePaymentBill([FromBody] BillsVNPAY billpayment) {
    var successApiResponse = new ApiResponse();
    if (billpayment != null){
       SetCacheValue("myCacheKey", billpayment, TimeSpan.FromMinutes(90)); 
        successApiResponse.Status = 200;
        successApiResponse.Message = "OK";
        successApiResponse.Data = billpayment;
    }else {
        successApiResponse.Status = 500;
        successApiResponse.Message = "Lưu hoá đơn không thành công";
        successApiResponse.Data = "null";
    }

   
     return Ok(successApiResponse);
 }


[HttpPost("SaveCacheBillFoods")] 
 public IActionResult SaveCacheBillFoods([FromBody] paymentBillFoodComboS billpayment) {
    var successApiResponse = new ApiResponse();
    if (billpayment != null){
       SetCacheValue("myCacheKeyFood", billpayment, TimeSpan.FromMinutes(90)); 
        successApiResponse.Status = 200;
        successApiResponse.Message = "OK";
        successApiResponse.Data = billpayment;
    }else {
        successApiResponse.Status = 500;
        successApiResponse.Message = "Lưu hoá đơn không thành công";
        successApiResponse.Data = "null";
    }

   
     return Ok(successApiResponse);
}



 //MODEL BILL
public class BillsVNPAY {

    
     public long Idbill { get; set; }

    public long? Idmovie { get; set; }

    public int? Idvoucher { get; set; }

    public long? Iduser { get; set; }

    public int? Idinterest { get; set; }

    public long? Idcinema { get; set; }

    public int? Quantityticket { get; set; }

    public int? Vat { get; set; }

    public int? Totalamount { get; set; }

    public DateTime? Datebill { get; set; }

    public string? Note { get; set; }

    public int? Statusbill { get; set; }

    public List<ticketess> ticket {get;set;}

    public List<combobillss> combobill {get;set;} 
}

public class ticketess {
    public long Idticket { get; set; }

    public int? Idchair { get; set; }

    public int? Idinterest { get; set; }

    public int? Pricechair { get; set; }

    public long? Idbill { get; set; }
}

public class combobillss {
  public int IdBillfoodCombo {get;set;}

      public int idcombo {get;set;}

      public long? Idbill {get;set;}
}



 // END MODEL BILL

    // API GET LIST VOUCHER
[HttpPost("CreateLinkVNPAY")]
public IActionResult CreateLinkVNPAY([FromBody] responsePayment responsePayments)
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
                    var reponsepaymentvnpay = new responsePayment();
                    var orderid = DateTime.Now.Ticks;
                    var note =   "Thanh toan don hang:" +orderid.ToString();
                        var vnpayRequest = new VnpayPayRequest(vnpayConfig.Version,
                       vnpayConfig.TmnCode,DateTime.Now, 
                       MomoHelper.GetIpAddress(),responsePayments.amount,
                      "VND","other",
                     note,vnpayConfig.ReturnUrl,orderid.ToString());
                      var paymentUrlvn = string.Empty;
                      paymentUrlvn = vnpayRequest.GetLink(vnpayConfig.PaymentUrl,vnpayConfig.HashSecret);
                      reponsepaymentvnpay.amount = responsePayments.amount;
                      reponsepaymentvnpay.idorder = responsePayments.idorder;
                      reponsepaymentvnpay.urlpayment = paymentUrlvn; 
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = reponsepaymentvnpay;
                
                
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
[HttpPost("CreateLinkVNPAYFood")]
public IActionResult CreateLinkVNPAYFood([FromBody] responsePayment responsePayments)
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
                    var reponsepaymentvnpay = new responsePayment();
                    var orderid = DateTime.Now.Ticks;
                    var note =   "Thanh toan don hang:" +orderid.ToString();
                        var vnpayRequest = new VnpayPayRequest(vnpayConfig.Version,
                       vnpayConfig.TmnCode,DateTime.Now, 
                       MomoHelper.GetIpAddress(),responsePayments.amount,
                      "VND","other",
                     note,"http://localhost:5062/PaymentMomo/MakePaymentFood",orderid.ToString());
                      var paymentUrlvn = string.Empty;
                      paymentUrlvn = vnpayRequest.GetLink(vnpayConfig.PaymentUrl,vnpayConfig.HashSecret);
                      reponsepaymentvnpay.amount = responsePayments.amount;
                      reponsepaymentvnpay.idorder = responsePayments.idorder;
                      reponsepaymentvnpay.urlpayment = paymentUrlvn; 
                   
                      successApiResponse.Status = 200;
                     successApiResponse.Message = "OK";
                     successApiResponse.Data = reponsepaymentvnpay;
                
                
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
