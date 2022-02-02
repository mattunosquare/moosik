using AutoMapper;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Authentication;
using moosik.api.ViewModels.User;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;

namespace moosik.api.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        ConfigureViewModelToDto();
        ConfigureDtoToDomain();
        ConfigureDomainToDto();
        ConfigureDtoToViewModel();
    }

    private void ConfigureViewModelToDto()
    {
        CreateMap<CreateUserViewModel, CreateUserDto>();
        CreateMap<UpdateUserViewModel, UpdateUserDto>();
        CreateMap<AuthenticationRequestViewModel, AuthenticationRequestDto>();
    }

    private void ConfigureDtoToDomain()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.Active, o => o.MapFrom(x => true));
        CreateMap<UpdateUserDto, User>();
    }
    
    private void ConfigureDomainToDto()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserDetailDto>();
        CreateMap<UserRole, UserRoleDto>();
        CreateMap<User, AuthenticationResponseDto>()
            .ForMember(u => u.Role, o=> o.MapFrom(x=> x.Role.Description))
            .ForMember(u => u.AccessToken, o => o.Ignore())
            .ForMember(u => u.RefreshToken, o => o.Ignore());
    }

    private void ConfigureDtoToViewModel()
    {
        CreateMap<UserDto, UserViewModel>();
        CreateMap<UserRoleDto, UserRoleViewModel>();
        CreateMap<UserDetailDto, UserDetailViewModel>();
        CreateMap<AuthenticationResponseDto, AuthenticationResponseViewModel>();
        CreateMap<AuthenticationResponseDto, LoggedInUserViewModel>();
    }
}