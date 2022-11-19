using AuthMicroservice.Filter;
using Microservices.Web.Frontend.Services.BasketServices;
using Microservices.Web.Frontend.Services.DiscountServices;
using Microservices.Web.Frontend.Services.OrderServices;
using Microservices.Web.Frontend.Services.PaymentServices;
using Microservices.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




builder.Services.AddHttpContextAccessor();


IHttpContextAccessor htpContextAccessor = new HttpContextAccessor() ;

builder.Services.AddHttpClient<IOrderService, OrderService>(p =>
{
    p.BaseAddress = new Uri(builder.Configuration["MicroservicAddress:ApiGatewayForWeb:Uri"]);
});


builder.Services.AddScoped<IDiscountService>(p =>
{
    return new DiscountServiceRestful(new RestClient(
        builder.Configuration["MicroservicAddress:ApiGatewayForWeb:Uri"]));
});

builder.Services.AddScoped<IProductService>(p =>
{
    return new ProductService(
        new RestClient(builder.Configuration["MicroservicAddress:ApiGatewayForWeb:Uri"]));
});

builder.Services.AddScoped<IBasketService>(p =>
{
    return new BasketService(
        new RestClient(builder.Configuration["MicroservicAddress:ApiGatewayForWeb:Uri"]), htpContextAccessor);
});


builder.Services.AddScoped<IPaymentService>(p =>
{
    return new PaymentService(
        new RestClient(builder.Configuration["MicroservicAddress:ApiGatewayForWeb:Uri"]));
});

builder.Services.AddScoped<AutenticationFilter>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
