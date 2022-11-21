using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountMicroservice.Model.Services
{
    public interface IDiscountService
    {
        DiscountDto GetDiscountByCode(string Code);
        DiscountDto GetDiscountById(Guid Id);

        bool UseDiscount(Guid Id);
        bool AddNewDiscount(string Code, int Amount);
    }
}
