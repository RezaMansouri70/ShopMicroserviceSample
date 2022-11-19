using AuthMicroservice.Filter;
using Microservices.Web.Frontend.Services.OrderServices;
using Microservices.Web.Frontend.Services.PaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IPaymentService paymentService;
        private readonly IConfiguration configuration;


        public OrdersController(IOrderService orderService,
            IPaymentService paymentService,
            IConfiguration configuration)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {

            var orders = orderService.GetOrders().Result;
            return View(orders);
        }

        public IActionResult Detail(Guid Id)
        {
            var order = orderService.OrderDetail(Id).Result;
            return View(order);
        }

        public IActionResult Pay(Guid OrderId)
        {
            var order = orderService.OrderDetail(OrderId).Result;
            if (order.PaymentStatus == PaymentStatus.isPaid)
            {
                return RedirectToAction(nameof(Detail), new { Id = OrderId });
            }
            if (order.PaymentStatus == PaymentStatus.unPaid)
            {
                return Redirect(configuration["MicroservicAddress:ApiGatewayForWeb:Uri"] + "api/Pay/Verify?OrderID=" + OrderId.ToString());
            }
            return NotFound();
            

        }
    }
}
