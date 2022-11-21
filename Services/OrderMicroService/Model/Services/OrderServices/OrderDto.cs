using OrderMicroService.Model.Entities;

namespace OrderMicroService.Model.Services
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int ItemCount { get; set; }
        public int TotalPrice { get; set; }
        public bool OrderPaid { get; set; }
        public DateTime OrderPlaced { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

    }
}
