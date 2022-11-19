using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketMicroService.Model.Dtos
{
    public class DiscountDto
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public bool Used { get; set; }
    }
}
