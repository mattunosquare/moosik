using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using moosik.api.ViewModels.Authentication;
using moosik.api.ViewModels.User;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Authentication;
using moosik.services.Dtos.User;
using BC = BCrypt.Net.BCrypt;

namespace moosik.api.Profiles;

[ExcludeFromCodeCoverage]
public class UserProfile : Profile
{
    public UserProfile()
    {
        ConfigureViewModelToDto();
        ConfigureDtoToDomain();
        ConfigureDomainToDto();
        ConfigureDtoToViewModel();
        DtoToDto();
    }

    private void ConfigureViewModelToDto()
    {
        CreateMap<CreateUserViewModel, CreateUserDto>()
            .ForMember(x => x.Password, o => o.MapFrom(x => BC.HashPassword(x.Password)));
        CreateMap<UpdateUserViewModel, UpdateUserDto>();
        CreateMap<AuthenticationRequestViewModel, AuthenticationRequestDto>();
    }

    private void ConfigureDtoToDomain()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.Active, o => o.MapFrom(x => true))
            .ForMember(d => d.UserRoleId, o=> o.MapFrom(x => x.RoleId));
        CreateMap<UpdateUserDto, User>();
    }
    private void DtoToDto()
    {
        CreateMap<UserDto, AuthenticationResponseDto>()
            .ForMember(x => x.AccessToken, o => o.Ignore())
            .ForMember(x => x.RefreshToken, o => o.Ignore());

    }
    
    private void ConfigureDomainToDto()
    {
        CreateMap<User, UserDto>()
            .ForMember(u => u.Role, o=> o.MapFrom(x => x.Role.Description));
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