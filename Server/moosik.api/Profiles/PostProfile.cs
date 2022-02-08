using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Post;
using moosik.dal.Models;
using moosik.services.Dtos;

namespace moosik.api.Profiles;

[ExcludeFromCodeCoverage]
public class PostProfile : Profile
{
    public PostProfile()
    {
        ConfigureViewModelToDto();
        ConfigureDtoToDomain();
        ConfigureDomainToDto();
        ConfigureDtoToViewModel();
    }

    private void ConfigureViewModelToDto()
    {
        CreateMap<CreatePostViewModel, CreatePostDto>();
        CreateMap<UpdatePostViewModel, UpdatePostDto>();
        CreateMap<CreatePostResourceViewModel, CreatePostResourceDto>();
        CreateMap<UpdatePostResourceViewModel, UpdatePostResourceDto>();
    }

    private void ConfigureDtoToDomain()
    {
        CreateMap<CreatePostDto, Post>()
            .ForMember(d => d.PostResources, o => o.MapFrom(
                x => x.Resource != null ? new List<CreatePostResourceDto> {x.Resource} : new List<CreatePostResourceDto>()))
            .ForMember(d => d.Active, o => o.MapFrom(x => true))
            .ForMember(d => d.CreatedDate, o => o.MapFrom(x => DateTime.UtcNow));
        CreateMap<UpdatePostDto, Post>();
        CreateMap<CreatePostResourceDto, PostResource>()
            .ForMember(d => d.ResourceTypeId, o => o.MapFrom(x => x.TypeId));
        CreateMap<UpdatePostResourceDto, PostResource>();
    }
    
    private void ConfigureDomainToDto()
    {
        CreateMap<Post, PostDto>()
            .ForMember(d => d.Resources, o => o.MapFrom(x => x.PostResources));
        CreateMap<PostResource, PostResourceDto>();
        CreateMap<ResourceType, ResourceTypeDto>();
    }
    
    private void ConfigureDtoToViewModel()
    {
        CreateMap<PostDto, PostViewModel>();
        CreateMap<PostResourceDto, PostResourceViewModel>();
        CreateMap<ResourceTypeDto, ResourceTypeViewModel>();
    }
    
}