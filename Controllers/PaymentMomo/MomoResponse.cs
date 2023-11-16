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
// using MySql.Data.EntityFrameworkCore;

namespace webapiserver.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentMomoController : ControllerBase
{
        private readonly CinemaContext _context;
        private readonly VnpayConfig vnpayConfig;
       
      
        public PaymentMomoController(CinemaContext context,IOptions<VnpayConfig> VnpayConfigs)
        {
            _context = context;
          
            this.vnpayConfig  = VnpayConfigs.Value;
         
        }
             

      
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

        if (vnp_TransactionStatus == "00") {
        var datapaymentvnpays = new paymentVNPAY();
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
        datapaymentvnpays.Idbills = Convert.ToInt32(vnp_TxnRef);
       var datapaymentvnpay = _context.paymentVNPAY.Add(datapaymentvnpays);
       _context.SaveChanges();
       
        successApiResponse.Status = 200;
        successApiResponse.Message = "OK";

        successApiResponse.Data = datapaymentvnpays;
        } else {
               successApiResponse.Status = 500;
        successApiResponse.Message = "error";

        successApiResponse.Data = "Thanh toan that bai";
        }
         
       

        return Ok(successApiResponse);
    }

   

   

public class responsePayment {
    public int idorder {get;set;}

    public decimal amount {get;set;}

    public string urlpayment {get;set;}
}

 [HttpGet("getidbillPaymentVnpay")]
    public IActionResult getidbillPaymentVnpay(int idbill){
        var dataidbill = _context.paymentVNPAY.Where(x=>x.Idbills == idbill).SingleOrDefault();
            var successApiResponse = new ApiResponse();
            successApiResponse.Status = 200;
            successApiResponse.Message = "OK";
            successApiResponse.Data = dataidbill;
        return Ok(successApiResponse);
    }

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
                     note,vnpayConfig.ReturnUrl,responsePayments.idorder.ToString());
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
