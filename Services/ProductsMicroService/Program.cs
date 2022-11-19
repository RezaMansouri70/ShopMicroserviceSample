using Config.AuthenticationHeaders;
using Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using ProductsMicroService.Infrastructure;
using ProductsMicroService.Service;
using RabbitMqMessageBus.MessagingBus;

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
builder.Services.AddScoped<AutenticationFilter>();



var cnn = builder.Configuration["ProductConnection"];
builder.Services.AddDbContext<ProductDatabaseContext>(p => p.UseSqlServer(cnn));

builder.Services.AddTransient<IProductService, ProductsMicroService.Service.ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IMessageBus, RabbitMQMessageBus>();
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

var app = builder.Build();

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
