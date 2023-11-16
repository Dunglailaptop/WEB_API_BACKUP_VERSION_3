using Microsoft.AspNetCore.Http;

public struct ValidHeader
{
    // Define constant values for the header tokens
    public const string Token = "YourExpectedTokenValue";
    public const int MethodPost = 1;
    public const int MethodGet = 0;
    public const int Project_id = 8097;
}
public class TokenHelper
{
  public static bool IsValidTokenHeader(HttpRequest request, string expectedToken, int expectedMethod, int expectedProjectId)
    {
        // Retrieve the header values from the request
        string tokenValue = request.Headers["Authorization"];
        string methodValue = request.Headers["Method"];
        string projectIdValue = request.Headers["ProjectId"];

        // Check if the provided header values match the expected values
        return tokenValue == expectedToken &&
               methodValue == Convert.ToString(expectedMethod) &&
               projectIdValue == Convert.ToString(expectedProjectId);
    }
}
