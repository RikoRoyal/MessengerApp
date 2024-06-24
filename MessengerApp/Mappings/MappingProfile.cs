using AutoMapper;
using MessengerApp.Entities;
using MessengerApp.ViewModels;
using System.Linq;

namespace MessengerApp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName)) 
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Chat, ChatViewModel>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.ChatUsers.Select(cu => cu.User)));

            CreateMap<ChatUser, UserViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name));
        }
    }
}
