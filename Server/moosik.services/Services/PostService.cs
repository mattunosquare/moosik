using System;
using System.Collections.Generic;
using AutoMapper;
using moosik.dal.Contexts;
using moosik.services.Dtos;
using moosik.services.Interfaces;

namespace moosik.services.Services;

public class PostService : IPostService
{
    private readonly MoosikContext _database;
    private readonly IMapper _mapper;

    public PostService(MoosikContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public ICollection<PostDto> GetAllPosts(int? threadId)
    {
        throw new NotImplementedException();
    }

    public PostDto GetPostById(int postId)
    {
        throw new NotImplementedException();
    }

    public ICollection<PostDto> GetPostsAfterDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public void UpdatePost(PostDto post)
    {
        throw new NotImplementedException();
    }

    public void CreatePost(PostDto post)
    {
        throw new NotImplementedException();
    }

    public void DeletePost(int postId)
    {
        throw new NotImplementedException();
    }

    public ICollection<ResourceType> GetAllResourceTypes()
    {
        throw new NotImplementedException();
    }
}