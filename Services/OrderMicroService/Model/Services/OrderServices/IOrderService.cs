using OrderMicroService.MessagingBus;
using OrderMicroService.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMicroService.Model.Services
{
    public interface IOrderService
    {

        List<OrderDto> GetOrdersForUser(string UserId);
        OrderDetailDto GetOrderById(Guid Id);

    }
}
