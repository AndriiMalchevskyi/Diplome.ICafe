using AutoMapper;
using ICafe.Application.Models.Filter;
using ICafe.Application.Models.Order;
using ICafe.Application.Models.Photo;
using ICafe.Application.Models.Product;
using ICafe.Application.Models.User;
using ICafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<User, UserForListDto>()
                .ForMember(m => m.PhotoUrl, opt => opt.MapFrom(src => src.Photo.Url));
            //.ForMember(dest => dest.Age, opt => {
            //     opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            // });
            CreateMap<User, UserForDetailedDto>();
            //.ForMember(dest => dest.Age, opt => {
            //     opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            // });
            CreateMap<User, UserForRegisterDto>();



            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            
            CreateMap<UserForRegisterDto, User>();

            CreateMap<User, UserForDetailedDto>()
                .ForMember(m => m.Roles, opt => opt.MapFrom(src => src.UserRoles))
                .ForMember(m => m.PhotoUrl, opt => opt.MapFrom(src => src.Photo.Url));

            CreateMap<Filter, FilterDto>();
            CreateMap<FilterDto, Filter>();

            CreateMap<ProductToCreateDto, Product>();
            CreateMap<ProductToUpdateDto, Product>();
            CreateMap<Product, ProductToDetailDto>()
                .ForMember(m => m.PhotoUrl, opt => opt.MapFrom(src => src.Photo.Url));

            CreateMap<OrderToCreateDto, Order>();
            CreateMap<OrderToUpdateDto, Order>();
            //CreateMap<MessageForCreationDto, Message>().ReverseMap();
            // CreateMap<Message, MessageToReturnDto>();
            /*
            .ForMember(m => m.SenderPhotoUrl, )//, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(m => m.RecipientPhotoUrl, );//, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        */
        }
    }
}
