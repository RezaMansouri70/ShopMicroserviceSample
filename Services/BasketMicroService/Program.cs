using BasketMicroService.Infrastructure.Contexts;
using BasketMicroService.Infrastructure.MappingProfile;
using BasketMicroService.MessageingBus;
using BasketMicroService.Model.Services.DiscountServices;
using BasketMicroService.Model.Services.ProductServices;
using BasketMicroService.Model.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMqMessageBus.MessagingBus;
using Config.AuthenticationHeaders;
using BasketMicroService.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AuthenticationHeadersFilter>();
});






builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AutenticationFilter>();

builder.Services.AddDbContext<BasketDataBaseContext>(o => o.UseSqlServer
(builder.Configuration["BasketConnection"]), ServiceLifetime.Singleton);

builder.Services.AddAutoMapper(typeof(BasketMappingProfile));

builder.Services.AddTransient<IBasketService, BasketMicroService.Model.Services.BasketService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IDiscountService, BasketMicroService.Model.Services.DiscountServices.DiscountService>();
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));


builder.Services.AddTransient<IMessageBus, RabbitMQMessageBus>();

builder.Services.AddHostedService<ReceivedUpdateProductNameMessage>();


var app = builder.Build();


// migrate any database changes on startup 
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<BasketDataBaseContext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
