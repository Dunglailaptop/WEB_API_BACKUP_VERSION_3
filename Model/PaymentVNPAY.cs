using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using webapiserver.Controllers;

namespace MyCinema.Model;

public partial class paymentVNPAY {
  public int idpaymentvnpay {get;set;}
public string vnp_Amount {get;set;}
public string vnp_BankCode {get;set;}
public string vnp_BackTranNo {get;set;}
public string vnp_CardType {get;set;} 
public string vnp_OrderInfo {get;set;} 
public string vnp_PayDate {get;set;} 
public string vnp_ResponseCode {get;set;} 
public string vnp_TmnCode {get;set;}
public string vnp_TransactionNo {get;set;} 
public string vnp_TransactionSatus {get;set;}
public string vnp_TxnRef {get;set;}
public string vnp_SecureHash {get;set;} 

public long Idbills {get;set;}
}