using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using moosik.dal.Contexts;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Exceptions;
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

    public PostDto[] GetAllPosts(int? threadId)
    {
        Expression<Func<Post, bool>> returnAll = p => true;
        Expression<Func<Post, bool>> returnSingle = p => p.Id == threadId;

        var filterPostByThreadId = threadId >= 0 ? returnSingle : returnAll;

        return _mapper.ProjectTo<PostDto>(
                _database.Get<Post>()
                    .Where(filterPostByThreadId))
            .ToArray();
    }

    public PostDto GetPostById(int postId)
    {
        return _mapper.ProjectTo<PostDto>(
                _database.Get<Post>()
                    .Where(post => post.Id == postId))
            .SingleOrDefault();
    }

    public ICollection<PostDto> GetPostsAfterDate(DateTime date)
    {
        return _mapper.ProjectTo<PostDto>(
                _database.Get<Post>()
                    .Where(p => p.CreatedDate > date))
            .ToArray();
    }

    public void UpdatePost(UpdatePostDto updatePostDto)
    {
        var existingPost = RetrievePostForId(updatePostDto.Id).SingleOrDefault();

        if (existingPost == null)
        {
            throw new NotFoundException($"No post found for id: {updatePostDto.Id}");
        }

        _mapper.Map(updatePostDto, existingPost);
        _database.SaveChanges();
    }

    public void CreatePost(CreatePostDto createPost)
    {
        var post = _mapper.Map<Post>(createPost);
        _database.Add(post);
        _database.SaveChanges();
    }

    public void DeletePost(int postId)
    {
        var post = RetrievePostForId(postId).SingleOrDefault();

        if (post == null)
        {
            throw new NotFoundException($"No post found for postId: {postId}");
        }

        post.Active = false;
        _database.SaveChanges();
    }

    public void UpdatePostResource(UpdatePostResourceDto updatePostResourceDto)
    {
        var existingPostResource = RetrievePostResourceForId(updatePostResourceDto.Id).SingleOrDefault();

        if (existingPostResource == null)
        {
            throw new NotFoundException($"Cannot find post resource for id: {updatePostResourceDto.Id}");
        }

        _mapper.Map(updatePostResourceDto, existingPostResource);
        _database.SaveChanges();
    }

    public ResourceTypeDto[] GetAllResourceTypes()
    {
        return _mapper.ProjectTo<ResourceTypeDto>(
                _database.Get<ResourceType>())
            .ToArray();
    }

    public IQueryable<Post> RetrievePostForId(int postId)
    {
        return _database
            .Get<Post>()
            .Where(p => p.Id == postId);
    }

    public IQueryable<PostResource> RetrievePostResourceForId(int postResourceId)
    {
        return _database
            .Get<PostResource>()
            .Where(p => p.Id == postResourceId);
    }
}