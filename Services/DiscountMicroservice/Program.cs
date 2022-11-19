using DiscountMicroservice.Infrastructure.Contexts;
using DiscountMicroservice.Infrastructure.MappingProfile;
using DiscountMicroservice.Model.Services;
using DiscountMicroservice.GRPC;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiscountService", Version = "v1" });
});




builder.Services.AddScoped<IDiscountContext, DiscountContext>();


builder.Services.AddAutoMapper(typeof(DiscountMappingProfile));
builder.Services.AddScoped<IDiscountService, DiscountMicroservice.Model.Services.DiscountService>();

var app = builder.Build();



app.UseRouting();
app.UseAuthorization();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true, });
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GRPCDiscountService>();
});
app.MapGet("/", () => "Discount GRPC Service");

app.Run();



