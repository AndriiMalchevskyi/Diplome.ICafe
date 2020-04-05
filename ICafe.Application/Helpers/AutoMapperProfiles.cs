using AutoMapper;
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

            CreateMap<User, UserForListDto>();
            //.ForMember(dest => dest.Age, opt => {
            //     opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            // });
            CreateMap<User, UserForDetailedDto>();
              //.ForMember(dest => dest.Age, opt => {
             //     opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
             // });

            //CreateMap<EstatePhoto, PhotosForDetailedDto>();
           // CreateMap<EstatePhoto, PhotoForReturnDto>();
            //CreateMap<PhotoForCreationDto, EstatePhoto>();
            CreateMap<UserForRegisterDto, User>();

            //CreateMap<MessageForCreationDto, Message>().ReverseMap();
           // CreateMap<Message, MessageToReturnDto>();
            /*
            .ForMember(m => m.SenderPhotoUrl, )//, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(m => m.RecipientPhotoUrl, );//, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        */
        }
    }
}
