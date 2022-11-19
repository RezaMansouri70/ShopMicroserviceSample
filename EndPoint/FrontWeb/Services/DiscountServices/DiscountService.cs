
//using DiscountService.Proto;
//using Grpc.Net.Client;
//using Microservices.Web.Frontend.Models.Dtos;
//using Microsoft.Extensions.Configuration;
//using System;

//namespace Microservices.Web.Frontend.Services.DiscountServices
//{
//    public class DiscountService : IDiscountService
//    {
//        private readonly GrpcChannel channel;
//        private readonly IConfiguration configuration;

//        public DiscountService(IConfiguration configuration)
//        {
//            this.configuration = configuration;
//            string discountServer = configuration["MicroservicAddress:Discount:Uri"];

//            channel = GrpcChannel.ForAddress(discountServer);
//        }

//        public ResultDto<DiscountDto> GetDiscountByCode(string Code)
//        {
//            var grpc_discountService = new
//                DiscountServiceProto.DiscountServiceProtoClient(channel);
//            var result=  grpc_discountService.GetDiscountByCode(new RequestGetDiscountByCode
//            {
//                Code = Code,
//            });


//            if (result.IsSuccess)
//            {
//                return new ResultDto<DiscountDto>
//                {
//                    Data = new DiscountDto
//                    {
//                        Amount = result.Data.Amount,
//                        Code = result.Data.Code,
//                        Id = Guid.Parse(result.Data.Id),
//                        Used = result.Data.Used
//                    },
//                    IsSuccess = result.IsSuccess,
//                    Message = result.Message,
//                };
//            }
//            return new ResultDto<DiscountDto>
//            {
//                IsSuccess = false,
//                Message = result.Message,
//            };

//        }

//        public ResultDto<DiscountDto> GetDiscountById(Guid Id)
//        {
//            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
//            var result = grpc_discountService.GetDiscountById(new RequestGetDiscountById
//            {
//                Id = Id.ToString(),
//            });

//            if (result.IsSuccess)
//            {
//                return new ResultDto<DiscountDto>
//                {
//                    Data = new DiscountDto
//                    {
//                        Amount = result.Data.Amount,
//                        Code = result.Data.Code,
//                        Id = Guid.Parse(result.Data.Id),
//                        Used = result.Data.Used
//                    },
//                    IsSuccess = result.IsSuccess,
//                    Message = result.Message,
//                };
//            }
//            return new ResultDto<DiscountDto>
//            {
//                IsSuccess = false,
//                Message = result.Message,
//            };
//        }

//        public ResultDto UseDiscount(Guid DiscountId)
//        {
//            var grpc_discountService = new DiscountServiceProto.DiscountServiceProtoClient(channel);
//            var result = grpc_discountService.UseDiscount(new RequestUseDiscount
//            {
//                Id = DiscountId.ToString(),
//            });
//            return new ResultDto
//            {
//                IsSuccess = result.IsSuccess
//            };
//        }
//    }
//}
