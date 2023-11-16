using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore;
using MyCinema.Model;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using webapiserver.Controllers;
using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CinemaContext>(optionsAction: _=>
{
    _.UseMySQL(connectionString:"Server=localhost;Database=cinema;Uid=root;Pwd=2792001dung");
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "your_issuer_here", // Replace with your JWT issuer
            ValidAudience = "your_audience_here", // Replace with your JWT audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here")) // Replace with your JWT secret key
        };
    });


        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 104857600; // 100 MB limit (in bytes)
        });

        // ...
    
builder.Services.AddSignalR();
builder.Services.Configure<VnpayConfig>(builder.Configuration.GetSection(VnpayConfig.ConfigName));
//momo config
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
var app = builder.Build();
// app.MapHub<MiClaseSignalR>("/signalR");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}







app.UseHttpsRedirection();

// Them cau hinh goi local Image
app.UseStaticFiles();
app.UseCors();
//them Authorization
app.UseAuthentication();

app.UseAuthorization();

// app.UseWebSockets();
// app.UseMiddleware<MiClaseSignalR>();
app.MapHub<ChatHub>("/chatHub");
app.MapControllers();


app.Run();
