using System;

namespace OrderMicroService.Model.Entities
{
    public class OrderLine
    {
        public Guid Id { get; set; }
     
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }

        public Product Product { get; set; }
        public Guid ProductId { get; set; }
    }


}
