using Microservices.Web.Frontend.Models.Dtos;
using Newtonsoft.Json;
using RestSharp;
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
    public class PaymentService : IPaymentService
    {
        private readonly RestClient restClient;
        public PaymentService(RestClient restClient)
        {
            this.restClient = restClient;
            restClient.Timeout = -1;
        }

        public ResultDto<ReturnPaymentLinkDto> GetPaymentlink(Guid OrderId, string CallbackUrl)
        {
            var request = new RestRequest($"/api/Pay?OrderId={OrderId}&callbackUrlFront={CallbackUrl}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var orders = JsonConvert.DeserializeObject<ResultDto<ReturnPaymentLinkDto>>(response.Content);
            return orders;
        }
    }

    public class ReturnPaymentLinkDto
    {
        public string PaymentLink { get; set; }
    }
}
