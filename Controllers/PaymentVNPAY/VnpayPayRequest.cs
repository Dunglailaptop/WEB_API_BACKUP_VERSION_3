using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace webapiserver.Controllers
{
            public class VnpayPayRequest {

           public SortedList<string,string> requestData = new SortedList<string, string>(new VnpayCompare());




            public VnpayPayRequest(string version,string tmCode,DateTime createDate, string ipAddress,decimal? amount,string currCode,string orderType,string orderInfo, string returnUrl, string txnRef)
          {
            this.vnp_Locale = "vn";
            this.vnp_IpAddr = ipAddress;
            this.vnp_Version = version;
            this.vnp_CurrCode = "VND";
            this.vnp_CreateDate = createDate.ToString("yyyyMMddHHmmss");
            this.vnp_Amount = (int)amount * 100;
             this.vnp_OrderType = orderType;
             this.vnp_OrderInfo = orderInfo;
             this.vnp_ReturnUrl = returnUrl;
             this.vnp_TxnRef = txnRef;
             this.vnp_Command = "pay";
             this.vnp_TmnCode = tmCode;
             this.vnp_ReturnUrl = returnUrl;
          }

          public string GetLink(string baseUrl, string secretKey){
            MakeRequestData();
            StringBuilder data = new StringBuilder();
            foreach(KeyValuePair<string,string> kv in requestData) {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string result = baseUrl + "?" + data.ToString();
            var secureHash = HashHelper.HmacSHA512(secretKey, data.ToString().Remove(data.Length -1 ,1));
            return result += "vnp_SecureHash=" + secureHash;
          }

                 public void MakeRequestData() {
                    if (vnp_Amount != null) 
                    requestData.Add("vnp_Amount", vnp_Amount.ToString() ?? string.Empty);
                     if (vnp_Command != null) 
                    requestData.Add("vnp_Command", vnp_Command.ToString() ?? string.Empty);
                     if (vnp_CreateDate != null) 
                    requestData.Add("vnp_CreateDate", vnp_CreateDate.ToString() ?? string.Empty);
                      if (vnp_CurrCode != null) 
                    requestData.Add("vnp_CurrCode", vnp_CurrCode.ToString() ?? string.Empty);
                     if (vnp_IpAddr != null) 
                    requestData.Add("vnp_IpAddr", vnp_IpAddr.ToString() ?? string.Empty);
                     if (vnp_Locale != null) 
                    requestData.Add("vnp_Locale", vnp_Locale.ToString() ?? string.Empty);
                     if (vnp_OrderInfo != null) 
                    requestData.Add("vnp_OrderInfo", vnp_OrderInfo.ToString() ?? string.Empty);
                     if (vnp_OrderType != null) 
                    requestData.Add("vnp_OrderType", vnp_OrderType.ToString() ?? string.Empty);
                     if (vnp_ReturnUrl != null) 
                    requestData.Add("vnp_ReturnUrl", vnp_ReturnUrl.ToString() ?? string.Empty);
                     if (vnp_TmnCode != null) 
                    requestData.Add("vnp_TmnCode", vnp_TmnCode.ToString() ?? string.Empty);
                     if (vnp_TxnRef != null) 
                    requestData.Add("vnp_TxnRef", vnp_TxnRef.ToString() ?? string.Empty);
                     if (vnp_Version != null) 
                    requestData.Add("vnp_Version", vnp_Version.ToString() ?? string.Empty);
                     if (vnp_SecureHash != null) 
                    requestData.Add("vnp_SecureHash", vnp_SecureHash.ToString() ?? string.Empty);
                   
                 }

            public string? vnp_Version {get;set;} 
            public string? vnp_Command {get;set;} 
            public string? vnp_TmnCode {get;set;} 
            public decimal? vnp_Amount {get;set;} 
            public string? vnp_BankCode {get;set;} 
            public string? vnp_CreateDate {get;set;} 
            public string? vnp_CurrCode {get;set;} 

            public string? vnp_IpAddr {get;set;} 

            public string? vnp_Locale {get;set;} 

            public string? vnp_OrderInfo {get;set;} 

            public string? vnp_OrderType {get;set;} 

            public string? vnp_ReturnUrl {get;set;} 
            public string? vnp_TxnRef {get;set;} 

            public string? vnp_SecureHash {get;set;} 
    }
}