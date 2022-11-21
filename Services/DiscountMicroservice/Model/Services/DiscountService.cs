using AutoMapper;
using DiscountMicroservice.Infrastructure.Contexts;
using DiscountMicroservice.Model.Entities;
using MongoDB.Driver;

namespace DiscountMicroservice.Model.Services
{
    public class DiscountService : IDiscountService
    {

        private readonly IDiscountContext _context;
        private readonly IMapper mapper;

        public DiscountService(IDiscountContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this._context = context;

        }

        public bool AddNewDiscount(string Code, int Amount)
        {
            DiscountCode discountCode = new DiscountCode()
            {
                Amount = Amount,
                Code = Code,
                Used = false,
            };

            _context.DiscountCode.InsertOneAsync(discountCode);

            return true;
        }

        public DiscountDto GetDiscountByCode(string Code)
        {

            var discountCode = _context.DiscountCode.Find(p => p.Code.Equals(Code)).FirstOrDefault();

            if (discountCode == null)
                return null;
            var result = mapper.Map<DiscountDto>(discountCode);
            return result;
        }

        public DiscountDto GetDiscountById(Guid Id)
        {
            var discountCode = _context.DiscountCode.Find(p => p.Id == Id).FirstOrDefault();

            if (discountCode == null)
                return null;
            var result = mapper.Map<DiscountDto>(discountCode);
            return result;
        }

        public bool UseDiscount(Guid Id)
        {
            var discountCode = _context.DiscountCode.Find(p => p.Id == Id).FirstOrDefault();
            if (discountCode == null)
                throw new Exception("Discouint Not Found....");
            discountCode.Used = true;
            _context.DiscountCode.ReplaceOneAsync(p => p.Id == Id, discountCode);



            return true;
        }


    }
}
