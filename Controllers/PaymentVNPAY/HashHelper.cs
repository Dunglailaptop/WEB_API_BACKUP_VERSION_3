using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net.Mail;
using System.Text.RegularExpressions;
using MyCinema.Model;


namespace webapiserver.Controllers
{
   public class HashHelper
   {
     public static String HmacSHA512(string key, string inputData) {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
          byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
          using (var hmac = new HMACSHA512(keyBytes)){
            byte[] hashvalue = hmac.ComputeHash(inputBytes);
            foreach(var theByte in hashvalue){
                hash.Append(theByte.ToString("x2"));
            }
          }
          return hash.ToString();
     }


     /// <summary>
     /// 
     /// </summary>
     /// <param name="email"></param>
     /// <returns></returns>
      public static bool IsValidEmail(string email)
        {
          // Regular expression for email validation
          string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
          Regex regex = new Regex(emailPattern);

          return regex.IsMatch(email);
       }
       private static readonly SmtpClient _smtp = new SmtpClient("smtp.gmail.com")
{
    EnableSsl = true,
    Port = 587,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    Credentials = new NetworkCredential("0850080012@sv.hcmunre.edu.vn", "2792001dung")
};
    public static bool sendemail(string emails,List<FoodCombo> numberchairs,FoodCombillPayment foodcombobill){
             // Validate email address
                if (!IsValidEmail(emails))
                {
                    return false;
                }
            try {
 // string otp = GenerateOTP();
                MailMessage message = new MailMessage();
                var to = emails;
                var from = "0850080012@sv.hcmunre.edu.vn";
                var pass = "2792001dung";
                var messageBody = "Your OTP for creating a new account: " ;

                // Set up the email message
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.IsBodyHtml = true;
                            // Build the HTML content for the table with numbers
                string emailContent = "<html>" +
                                    "<head>" +
                                    "<title>Email with HTML Content</title>" +
                                    "<style>" +
                                    "body { text-align: center; }" + // Canh giữa văn bản ngoài
                                    ".styled-table { border-collapse: collapse; width: 50%; margin: 0 auto; }" + // Kích thước và căn giữa bảng
                                    ".styled-table th, .styled-table td { border: 1px solid #dddddd; padding: 8px; }" + // Kẻ khung cho bảng
                                    ".styled-table td { font-size: small; }" + // Thu nhỏ kích thước của phông chữ bên trong bảng
                                    "</style>" +
                                    "</head>" +
                                    "<body>" +
                                    $"<h1>Hoá đơn thanh toán: {foodcombobill.id}</h1>" +
                                    $"<h3>Ngày thanh toán: {foodcombobill.datetimes}</h3>" +
                                    $"<h3>Tổng tiền: {foodcombobill.total_price}</h3>" +
                                    "<table class='styled-table'>" +
                                    "<thead>" +
                                    "<tr>" +
                                    "<th>Mã món ăn</th>" +
                                    "<th>Hình ảnh</th>" +
                                    "<th>Giá tiền</th>" +
                                    "<th>Tên món</th>" +
                                    "</tr>" +
                                    "</thead>" +
                                    "<tbody>";

                foreach (FoodCombo number in numberchairs)
                {
                    var hostimage = "http://localhost:5062/Uploads/Movie/" + number.picture;
                emailContent += "<tr>" +
                                $"<td>{number.idcombo}</td>" +
                                $"<td><img src='{hostimage}' alt='Hình ảnh'></td>" + // Đường dẫn ảnh
                                $"<td>{number.priceCombo}</td>" +
                                $"<td>{number.nametittle}</td>" +
                                "</tr>";
                }

                emailContent += "</tbody>" +
                            "</table>" +
                            "</body>" +
                            "</html>";

                 message.Body = emailContent;
                message.Subject = "OTP SEND EMAIL";
                 _smtp.SendMailAsync(message);
                return true;
            }catch {
                  return false;
            }
               
                 
       }
      
        public static bool sendemailTicket(string emails,List<Chair> numberchairs,Bill foodcombobill,List<FoodCombo> FOODCOMBOLIST){
             // Validate email address
                if (!IsValidEmail(emails))
                {
                    return false;
                }
            try {
 // string otp = GenerateOTP();
                MailMessage message = new MailMessage();
                var to = emails;
                var from = "0850080012@sv.hcmunre.edu.vn";
                var pass = "2792001dung";
                var messageBody = "Your OTP for creating a new account: " ;

                // Set up the email message
                message.To.Add(to);
                message.From = new MailAddress(from);
                message.IsBodyHtml = true;
                            // Build the HTML content for the table with numbers
                string emailContent = "<html>" +
                                    "<head>" +
                                    "<title>Email with HTML Content</title>" +
                                    "<style>" +
                                    "body { text-align: center; }" + // Canh giữa văn bản ngoài
                                    ".styled-table { border-collapse: collapse; width: 50%; margin: 0 auto; }" + // Kích thước và căn giữa bảng
                                    ".styled-table th, .styled-table td { border: 1px solid #dddddd; padding: 8px; }" + // Kẻ khung cho bảng
                                    ".styled-table td { font-size: small; }" + // Thu nhỏ kích thước của phông chữ bên trong bảng
                                    "</style>" +
                                    "</head>" +
                                    "<body>" +
                                    $"<h1>Hoá đơn thanh toán: {foodcombobill.Idbill}</h1>" +
                                    $"<h3>Ngày thanh toán: {foodcombobill.Datebill}</h3>" +
                                    $"<h3>Tổng tiền: {foodcombobill.Totalamount}</h3>" +
                                    "<table class='styled-table'>" +
                                    "<thead>" +
                                    "<tr>" +
                                    "<th>Mã hoá đơn</th>" +
                                    "<th>Mã số ghế</th>" +
                                    "<th>Giá tiền</th>" +
                                    "</tr>" +
                                    "</thead>" +
                                    "<tbody>";

                foreach (Chair number in numberchairs)
                {
                    var hostimage = number.RowChar + number.NumberChair;
                emailContent += "<tr>" +
                                $"<td>{number.Idroom}</td>" +
                                $"<td>{hostimage}</td>" + // Đường dẫn ảnh
                                $"<td>{number.Idchair}</td>" +
                                "</tr>";
                }

                emailContent += "</tbody>" +
                            "</table>" +
                            "<table class='styled-table'>" +
                            "<thead>" +
                            "<tr>" +
                            "<th>Mã món</th>" +
                            "<th>Hinh ảnh</th>" +
                            "<th>Giá tiền</th>" +
                            "<th>Tên món</th>" +
                            "</tr>" +
                            "</thead>" +
                            "<tbody>";

            foreach (FoodCombo numbers in FOODCOMBOLIST)
                {
                    var hostimage = "http://localhost:5062/Uploads/Movie/" + numbers.picture;
                emailContent += "<tr>" +
                                $"<td>{numbers.idcombo}</td>" +
                                $"<td>{hostimage}</td>" + // Đường dẫn ảnh
                                $"<td>{numbers.priceCombo}</td>" + // Đường dẫn ảnh
                                $"<td>{numbers.nametittle}</td>" +
                                "</tr>";
                }  
                   emailContent += "</body>" +
                                   "</html>";

                

                 message.Body = emailContent;
                message.Subject = "OTP SEND EMAIL";
                 _smtp.SendMailAsync(message);
                return true;
            }catch {
                  return false;
            }
               
                 
       }



    //     public static bool SendEmailWithHTMLContent(string receiverEmail)
    // {
    //     // Validate email address
    //     if (!IsValidEmail(receiverEmail))
    //     {
    //         return false; // Invalid email address
    //     }

    //     try
    //     {
    //         // SMTP configuration
    //         SmtpClient smtpClient = new SmtpClient("your_smtp_server_here");
    //         smtpClient.Port = 587; // Update port number if necessary
    //         smtpClient.EnableSsl = true; // Update based on your SMTP configuration
    //         smtpClient.Credentials = new NetworkCredential("0850080012@sv.hcmunre.edu.vn", "2792001dung");

    //         // Create the email message
    //         MailMessage mailMessage = new MailMessage();
    //         mailMessage.From = new MailAddress("0850080012@sv.hcmunre.edu.vn");
    //         mailMessage.To.Add(receiverEmail);
    //         mailMessage.Subject = "HTML Email";

    //         // Set email body as HTML content
    //         mailMessage.IsBodyHtml = true;
    //         mailMessage.Body = @"
    //             <html>
    //                 <head>
    //                     <title>Email with HTML Content</title>
    //                 </head>
    //                 <body>
    //                     <h1>This is an HTML email</h1>
    //                     <p>This is a paragraph in the email.</p>
    //                     <!-- Add your HTML content here -->
    //                 </body>
    //             </html>";

    //         // Send the email
    //         smtpClient.Send(mailMessage);
           
    //         return true; // Email sent successfully
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Error sending email: " + ex.Message);
    //         return false; // Failed to send email
    //     }
    // }


    }
   }

