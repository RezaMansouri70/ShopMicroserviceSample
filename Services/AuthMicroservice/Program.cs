using ApplicationServices.Services.UserService;
using AuthMicroservice.Filter;
using DataLayer.SqlServer.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
});




var cnnString = builder.Configuration.GetConnectionString("AppCnn");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(cnnString));



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AutenticationFilter>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
