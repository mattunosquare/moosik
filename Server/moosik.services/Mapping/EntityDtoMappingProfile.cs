using System.Collections.Generic;
using moosik.dal.Contexts;
using moosik.services.Dtos;

namespace moosik.services.Mapping;
using AutoMapper;

public class EntityDtoMappingProfile : Profile
{
    public EntityDtoMappingProfile()
    {
        CreateMap<Post, PostDto>();
        CreateMap<PostResource, PostResourceDto>();
        CreateMap<ResourceType, ResourceTypeDto>();
        CreateMap<Thread, ThreadDto>();
        CreateMap<ICollection<Thread>, ICollection<ThreadDto>>();
        CreateMap<ThreadType, ThreadTypeDto>();
        CreateMap<ICollection<ThreadType>, ICollection<ThreadTypeDto>>();
        CreateMap<User, UserDto>();
    }
}