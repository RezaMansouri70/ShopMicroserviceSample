using System;

namespace Microservices.Web.Frontend.Services.OrderServices
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

    public enum PaymentStatus
    {
        /// <summary>
        /// پرداخت نشده
        /// </summary>
        unPaid = 0,

        /// <summary>
        /// درخواست پرداخت ثبت شده
        /// </summary>
        RequestPayment = 1,

        /// <summary>
        /// پرداخت شده است
        /// </summary>
        isPaid = 2,
    }
}
