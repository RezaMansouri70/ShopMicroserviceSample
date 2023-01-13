using AutoMapper;
using BasketMicroService.MessageingBus.Send;
using BasketMicroService.Model.Dtos;
using BasketMicroService.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketMicroService.Infrastructure.MappingProfile
{
    public class BasketMappingProfile:Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<BasketItem, AddItemToBasketDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<AddItemToBasketDto, ProductDto>().ReverseMap();
            CreateMap<CheckoutBasketDto, BasketCheckoutMessage>()
                .ReverseMap();


        }
    }
}
