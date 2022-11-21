using Microservices.Web.Frontend.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrders();
        Task<OrderDetailDto> OrderDetail(Guid OrderId);

    } 
}
