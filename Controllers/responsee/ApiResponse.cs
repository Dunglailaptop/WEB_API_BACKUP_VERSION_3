using System;
using System.Collections.Generic;

namespace MyCinema.Model;

public partial class ApiResponse
{
    public int Status { get; set; }
    public string Message { get; set; }
    public Object Data { get; set; }

}
public class ApiData
{
    public string Type { get; set; }
    public string Version { get; set; }
    public string ApiKey { get; set; }
    public string ApiDomain { get; set; }
    public string ApiChatTms { get; set; }
    public string ApiChatAloline { get; set; }
    public string ApiUpload { get; set; }
    public string ApiUploadShort { get; set; }
    public string ApiConnection { get; set; }
    public string ApiLog { get; set; }
    public string AdsDomain { get; set; }
    public string ChatDomain { get; set; }
    public string ApiOauthNode { get; set; }
}
// public class validheader {
//      string token = "YourExpectedTokenV";
//      int MethodPost = 1;
     
//       int MethodGet = 0;
    
//      int Project_id = 8097;
// }