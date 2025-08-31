using AutoMapper;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.BLL.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Bidirectional mapping for basic entities
        CreateMap<Comment, DTOComment>().ReverseMap();
        CreateMap<Refractory, DTORefractory>().ReverseMap();
        CreateMap<UserInfo, DTOUser>().ReverseMap();
        
        // ApplicationUser to DTOUser mapping
        CreateMap<ApplicationUser, DTOUser>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.Name : null))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.LastName : null))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.Position : null))
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.UserAvatar : null))
            .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.IsBlocked : false))
            .ForMember(dest => dest.DateRegistration, opt => opt.MapFrom(src => src.UserInfo != null ? src.UserInfo.DateRegistration : null))
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be populated separately

        // DTOUser to ApplicationUser mapping
        CreateMap<DTOUser, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserInfo, opt => opt.Ignore()) // UserInfo will be created separately
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}