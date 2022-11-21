using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMicroService.Model.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; private set; }
        public DateTime OrderPlaced { get; private set; }
        public bool OrderPaid { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public int TotalPrice { get; set; }

        public ICollection<OrderLine> OrderLines { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }

        public Order(string UserId,
            string FirstName,
            string LastName,
            string Address,
            string PhoneNumber,
            int TotalPrice,
            List<OrderLine> OrderLines)
        {
            this.UserId = UserId;
            this.OrderPaid = false;
            this.OrderPlaced = DateTime.Now;
            this.OrderLines = OrderLines;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Address = Address;
            this.PhoneNumber = PhoneNumber;
            this.TotalPrice = TotalPrice;
            this.PaymentStatus = PaymentStatus.unPaid;
        }
        public Order() { }

        public void RequestPayment()
        {
            PaymentStatus = PaymentStatus.RequestPayment;
        }

        public void PaymentIsDone()
        {
            OrderPaid = true;
            PaymentStatus = PaymentStatus.isPaid;
        }
    }


    public enum PaymentStatus
    {

        unPaid = 0,

        RequestPayment = 1,

        isPaid = 2,
    }
}
