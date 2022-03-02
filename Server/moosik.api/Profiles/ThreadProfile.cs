using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using moosik.api.ViewModels;
using moosik.api.ViewModels.Thread;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Post;
using moosik.services.Dtos.Thread;
using Thread = moosik.dal.Models.Thread;

namespace moosik.api.Profiles;

[ExcludeFromCodeCoverage]
public class ThreadProfile : Profile
{
    public ThreadProfile()
    {
        ConfigureViewModelToDto();
        ConfigureDtoToDomain();
        ConfigureDomainToDto();
        ConfigureDtoToViewModel();
    }

    private void ConfigureViewModelToDto()
    {
        CreateMap<CreateThreadViewModel, CreateThreadDto>();
        CreateMap<UpdateThreadViewModel, UpdateThreadDto>();
    }
    
    private void ConfigureDtoToDomain()
    {
        CreateMap<CreateThreadDto, Thread>()
            .ForMember(d => d.Active, o => o.MapFrom(x => true))
            .ForMember(d => d.CreatedDate, o => o.MapFrom(x => DateTime.UtcNow))
            .ForMember(d => d.Posts, o => o.MapFrom(x => new List<CreatePostDto>{x.Post}));
        CreateMap<UpdateThreadDto, Thread>();
    }
    
    private void ConfigureDomainToDto()
    {
        CreateMap<Thread, ThreadDto>();
        CreateMap<ThreadType, ThreadTypeDto>();
    }

    private void ConfigureDtoToViewModel()
    {
        CreateMap<ThreadDto, ThreadViewModel>();
        CreateMap<ThreadTypeDto, ThreadTypeViewModel>();
    }

    


}