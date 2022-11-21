using OrderMicroService.MessagingBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMicroService.Model.Services.RegisterOrderServices
{
    public interface IRegisterOrderService
    {
        bool Execute(BasketDto basket);
    }
}
