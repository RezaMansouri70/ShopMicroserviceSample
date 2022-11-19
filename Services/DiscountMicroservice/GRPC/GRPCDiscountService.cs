using DiscountMicroservice.Model.Services;
using DiscountMicroservice.Proto;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountMicroservice.GRPC
{
    public class GRPCDiscountService : DiscountServiceProto.DiscountServiceProtoBase
    {
        private readonly IDiscountService discountService;

        public GRPCDiscountService(IDiscountService discountService)
        {
            this.discountService = discountService;
        }
       
        public override Task<ResultGetDiscountByCode> GetDiscountByCode(RequestGetDiscountByCode request, ServerCallContext context)
        {
            var data = discountService.GetDiscountByCode(request.Code);

            if (data == null)
            {
                return Task.FromResult(new ResultGetDiscountByCode
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "The entered discount code was not found",
                });
            }

            return Task.FromResult(new ResultGetDiscountByCode
            {
                IsSuccess = true,
                Message = "discount info",
                Data = new DiscountInfo
                {
                    Amount = data.Amount,
                    Code = data.Code,
                    Id = data.Id.ToString(),
                    Used = data.Used,
                }

            });
        }


        public override Task<ResultGetDiscountByCode> GetDiscountById(RequestGetDiscountById request, ServerCallContext context)
        {
            var data = discountService.GetDiscountById(Guid.Parse(request.Id));

            if (data == null)
            {
                return Task.FromResult(new ResultGetDiscountByCode
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "The entered discount code was not found",
                });
            }

            return Task.FromResult(new ResultGetDiscountByCode
            {
                IsSuccess = true,
                Message = "اطلاعات تخفیف",
                Data = new DiscountInfo
                {
                    Amount = data.Amount,
                    Code = data.Code,
                    Id = data.Id.ToString(),
                    Used = data.Used,
                }

            });
        }
        public override Task<ResultUseDiscount> UseDiscount(RequestUseDiscount request, ServerCallContext context)
        {
            var result = discountService.UseDiscount(Guid.Parse(request.Id));
            return Task.FromResult(new ResultUseDiscount
            {
                IsSuccess = result,
            });

        }

        public override Task<ResultAddNewDiscount> AddNewDiscount(RequestAddNewDiscount request, ServerCallContext context)
        {
            var result = discountService.AddNewDiscount(request.Code, request.Amount);
            return Task.FromResult(new ResultAddNewDiscount
            {
                IsSuccess = result,
            });
        }
    }
}
