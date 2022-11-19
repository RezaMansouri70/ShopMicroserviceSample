using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PaymentMicroService.MessagingBus.Send;
using RabbitMqMessageBus.MessagingBus;
using System.Transactions;

namespace PaymentMicroService.Controllers
{
    [Route("api/[controller]")]
    public class PayController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IMessageBus messageBus;
        private readonly string QueueName_PaymentDone;

        public PayController(
            IConfiguration configuration, IMessageBus messageBus)
        {
            this.configuration = configuration;
            this.messageBus = messageBus;
            QueueName_PaymentDone = configuration["RabbitMq:QueueName_PaymentDone"];
        }

        [AllowAnonymous]
        [HttpGet("Verify")]
        public IActionResult Verify(Guid orderid)
        {
            PaymentIsDoneMessage paymentIsDoneMessage = new PaymentIsDoneMessage
            {
                Creationtime = DateTime.Now,
                MessageId = Guid.NewGuid(),
                OrderId = orderid,
            };
            messageBus.SendMessage(paymentIsDoneMessage,"", QueueName_PaymentDone);

            return Ok("PaymentDone");
        }
     

    }
}
