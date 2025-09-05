using AutoMapper;
using RefractoryApp.Api.Models.Account;
using RefractoryApp.Api.Models.Comment;
using RefractoryApp.Api.Models.Refractory;
using RefractoryApp.Api.Models.User;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.Api.Infrastructure;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // Account ViewModels
        CreateMap<RegisterViewModel, DTOUser>();

        // User ViewModels
        CreateMap<DTOUser, ProfileViewModel>();
        CreateMap<ChangeProfileViewModel, DTOUser>();

        // Refractory ViewModels
        CreateMap<DTORefractory, RefractoryViewModel>();
        CreateMap<RefractoryEditViewModel, DTORefractory>();

        // Comment ViewModels
        CreateMap<DTOComment, CommentViewModel>();
        
        // Direct DAL to API mappings
        CreateMap<RefractoryApp.DAL.Models.Refractory, RefractoryViewModel>();
        CreateMap<RefractoryApp.DAL.Models.Comment, CommentViewModel>();
        CreateMap<RefractoryApp.DAL.Models.UserInfo, ProfileViewModel>();
    }
}