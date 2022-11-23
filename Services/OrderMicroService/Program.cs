using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using OrderMicroService.Model.Services.RegisterOrderServices;
using OrderMicroService.Model.Services;
using OrderMicroService.Model.Services.ProductServices;
using OrderMicroService.MessagingBus;
using Microsoft.EntityFrameworkCore;
using OrderMicroService.Infrastructure.Context;
using RabbitMqMessageBus.MessagingBus;
using Config.AuthenticationHeaders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AuthenticationHeadersFilter>();
});






builder.Services.AddHttpContextAccessor();



builder.Services.AddDbContext<OrderDataBaseContext>(o => o.UseSqlServer
    (builder.Configuration["OrderConnection"]), ServiceLifetime.Singleton);

builder.Services.AddTransient<IOrderService, OrderMicroService.Model.Services.OrderService>();

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));


builder.Services.AddHostedService<RecievedPaymentOfOrderService>();

builder.Services.AddHostedService<RecievedOrderCreatedMessage>();
builder.Services.AddHostedService<ReceivedUpdateProductNameMessage>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IRegisterOrderService, RegisterOrderService>();
builder.Services.AddTransient<IMessageBus, RabbitMQMessageBus>();


builder.Services.AddTransient<IVerifyProductService>(p =>
{
    return new VerifyProductService(new RestSharp.RestClient(builder.Configuration["ProductService"]));
});




var app = builder.Build();




// migrate any database changes on startup 
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<OrderDataBaseContext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
