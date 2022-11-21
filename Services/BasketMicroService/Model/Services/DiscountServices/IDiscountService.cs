
using BasketMicroService.Model.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketMicroService.Model.Services.DiscountServices
{
    public interface IDiscountService
    {
        DiscountDto GetDiscountById(Guid DiscountId);
        ResultDto<DiscountDto> GetDiscountByCode(string Code);

    }
}
