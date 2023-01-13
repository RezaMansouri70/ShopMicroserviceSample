using ApiGateway.ForWeb.Models.Dtos;
using DiscountMicroservice.Proto;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net;

namespace ApiGateway.ForWeb.Models.DiscountServices
{
    public interface IDiscountService
    {
        ResultDto<DiscountDto> GetDiscountByCode(string Code);
        ResultDto<DiscountDto> GetDiscountById(Guid Id);
        ResultDto UseDiscount(Guid DiscountId);
        ResultDto AddDiscount(DiscountDto Discount);

    }
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration configuration;
        private readonly GrpcChannel channel;

        public DiscountService(IConfiguration configuration)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestVersion = HttpVersion.Version20;
            httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;


            this.configuration = configuration;
             channel = GrpcChannel.ForAddress(configuration["MicroservicAddress:Discount:Uri"],
                new GrpcChannelOptions() { 
                  
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
        }


        public ResultDto<DiscountDto> GetDiscountByCode(string Code)
        {
            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
           
            var result = grpc_discountService.GetDiscountByCode(new RequestGetDiscountByCode
            {
                Code = Code
            });

            if (result.IsSuccess)
            {
                return new ResultDto<DiscountDto>
                {
                    Data = new DiscountDto
                    {
                        Amount = result.Data.Amount,
                        Code = result.Data.Code,
                        Id = Guid.Parse(result.Data.Id),
                        Used = result.Data.Used
                    },
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                };
            }
            return new ResultDto<DiscountDto>
            {
                IsSuccess = false,
                Message = result.Message,
            };
        }

        public ResultDto<DiscountDto> GetDiscountById(Guid Id)
        {
            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
            var result = grpc_discountService.GetDiscountById(new RequestGetDiscountById
            {
                Id = Id.ToString(),
            });

            if (result.IsSuccess)
            {
                return new ResultDto<DiscountDto>
                {
                    Data = new DiscountDto
                    {
                        Amount = result.Data.Amount,
                        Code = result.Data.Code,
                        Id = Guid.Parse(result.Data.Id),
                        Used = result.Data.Used
                    },
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                };
            }
            return new ResultDto<DiscountDto>
            {
                IsSuccess = false,
                Message = result.Message,
            };
        }

        public ResultDto UseDiscount(Guid DiscountId)
        {
            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
            var result = grpc_discountService.UseDiscount(new RequestUseDiscount
            {
                Id = DiscountId.ToString(),
            });
            return new ResultDto
            {
                IsSuccess = result.IsSuccess
            };
        }

        public ResultDto AddDiscount(DiscountDto Discount)
        {
            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
            var result = grpc_discountService.AddNewDiscount(new RequestAddNewDiscount 
            {
                Amount=Discount.Amount,
                Code = Discount.Code

            });
            return new ResultDto
            {
                IsSuccess = result.IsSuccess
            };
        }
    }

    public class DiscountDto
    {
        public int Amount { get; set; }
        public string Code { get; set; }
        public Guid Id { get; set; }
        public bool Used { get; set; }
    }
}
