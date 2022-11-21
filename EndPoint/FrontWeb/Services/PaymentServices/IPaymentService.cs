using Microservices.Web.Frontend.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Services.PaymentServices
{
    public interface IPaymentService
    {
        ResultDto<ReturnPaymentLinkDto> GetPaymentlink(Guid OrderId,
            string CallbackUrl);

    }
}
