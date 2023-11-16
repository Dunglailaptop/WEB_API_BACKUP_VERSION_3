using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using webapiserver.Controllers;

namespace MyCinema.Model;

public partial class INTERESTCINEMA
{
    public string Namecinema { get; set; }
    public string Times { get; set; }
    public long Idmovie { get; set; }
    public long Idcinema { get; set; }
    public long Idroom { get; set; }
    public int Idinterest { get; set; }
    
}

public partial class LISTCINEMA
{
    public string Namecinema { get; set; }
    
    public long Idcinema { get; set; }
    
    
}

public partial class CHAIR
{
    public int Idchair { get; set; }
    
    public int NumberChair { get; set; }

     public string RowChar { get; set; }

     public long? bill {get;set;} 
     public int Idcategorychair {get;set;}
   
    
}
public partial class CHAIRDETAILBILL
{
    public int Idchair { get; set; }
    
    public int NumberChair { get; set; }

     public string RowChar { get; set; }

    //  public long? bill {get;set;} 
     public int Idcategorychair {get;set;}
   
    
}

public partial class USERS
{
    public string Fullname { get; set; }
    
    public string Email { get; set; }

     public string phone { get; set; }

     public DateTime Birthday {get;set;} 

     public string avatar {get;set;} 
       public int gender {get;set;} 
      public int Idrole {get;set;} 
       public int Idusers {get;set;} 
        public int Idcinema {get;set;} 
         public int statuss {get;set;} 

         public string? address {get;set;}
    
}

public partial class CINEMA
{
    public int Idcinema {get;set;}
}

public partial class ReportTicket {
// public DateTime? DateHour {get;set;}

  public DateTime Datebill {get;set;}

  public int Total {get;set;}

  public int pricefood {get;set;} 
  public int statusbill {get;set;}
    
}


public partial class ReportFoodCombo {
// public DateTime? DateHour {get;set;}

  public DateTime datetimes {get;set;}

  public int totals {get;set;}

public int statusbillfoodcombo {get;set;}
    
}
//LAY HET ALL YEAR
public partial class ReportTicketALL {
// public DateTime? DateHour {get;set;}

  public int Datebill {get;set;}

  public int Total {get;set;}

  public int pricefood {get;set;} 
  public int statusbill {get;set;}
    
}


public partial class ReportFoodComboALL {
// public DateTime? DateHour {get;set;}

  public int datetimes {get;set;}

  public int totals {get;set;}

public int statusbillfoodcombo {get;set;}
    
}

public partial class ReportMovie {
public int stt {get;set;}

 public int totals {get;set;}

 public long? Idmovie {get;set;}

//  public string poster {get;set;}

//  public string Datebill {get;set;}

}
public partial class ReportFood {
  public int Idfood {get;set;}

  public int idcombo {get;set;}

  public int totals {get;set;}

  public string datetimes {get;set;}
  public int stt {get;set;}

}

public partial class totalfoodcombowithbill {
  public int totals {get;set;}
}

public partial class MoviesBooking {
   public long movieID { get; set; }

    public string? Namemovie { get; set; }

  

    public DateTime? Yearbirthday { get; set; }

    public string? Namecategorymovie { get; set; }

  

    public int? Timeall { get; set; }



    public string? Poster { get; set; }

 

}

public partial class PaymentMomoRequest {
    public string PartnerCode {get;set;}

    public string requestId {get;set;}

    public long amount {get;set;}

    public string orderId {get;set;}
    public string orderInfo {get;set;}
    public string redirectUrl {get;set;}

    public string ipnUrl {get;set;}
public string requestType {get;set;}
public string extraData {get;set;}
public string lang {get;set;}
public string signature {get;set;}
  
  public void MakeSignature(string accessKey,string sceretkey)
{
      var rawHash = "accessKey=" + accessKey +
          "&amount=" + this.amount +
          "&extraData=" + this.extraData +
          "&ipnUrl=" + this.ipnUrl +
          "&orderId=" + this.orderId +
          "&orderInfo=" + this.orderInfo +
          "&partnerCode=" + this.PartnerCode +
          "&redirectUrl=" + this.redirectUrl + 
          "&requestId=" + this.requestId +
          "&requestType=" + this.requestType;
          this.signature = MomoHelper.HmacSHA256(rawHash,sceretkey);
}
public (bool, string?) getLink(string payment) {
        using HttpClient client =  new HttpClient();
        var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings() {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
                    });
                    var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");
                    var createPaymentLinkRes = client.PostAsync(payment, requestContent).Result;
                 if (createPaymentLinkRes.IsSuccessStatusCode) {
                          var responseContent = createPaymentLinkRes.Content.ReadAsStringAsync().Result;
                          var responseData = JsonConvert.DeserializeObject<MomoHelper.MomoResponse>(responseContent);
                          if (responseData.resultCode == "0") {
                              return (true, responseData.pauUrl);
                          }else {
                            return (false, responseData.message);
                          }
                 }else {

                  return (false, createPaymentLinkRes.ReasonPhrase);
                 }
   }
  


}

