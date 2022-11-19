using AutoMapper;
using DiscountMicroservice.Model.Entities;
using DiscountMicroservice.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountMicroservice.Infrastructure.MappingProfile
{
    public class DiscountMappingProfile:Profile
    {
        public DiscountMappingProfile()
        {
            CreateMap<DiscountCode, DiscountDto>().ReverseMap();

        }
    }
}
