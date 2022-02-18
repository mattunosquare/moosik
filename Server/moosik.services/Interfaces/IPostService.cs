using System;
using System.Collections.Generic;
using System.Linq;
using moosik.dal.Models;
using moosik.services.Dtos;
using moosik.services.Dtos.Post;

namespace moosik.services.Interfaces;

public interface IPostService
{
    PostDto[] GetAllPosts(int? threadId = null);
    PostDto GetPostById(int postId);
    ICollection<PostDto> GetPostsAfterDate(DateTime date);
    void UpdatePost(UpdatePostDto updatePostDto);
    void CreatePost(CreatePostDto post);
    void DeletePost(int postId);
    void UpdatePostResource(UpdatePostResourceDto updatePostResourceDto);
    ResourceTypeDto[] GetAllResourceTypes();
    public IQueryable<Post> RetrievePostForId(int postId);
    public IQueryable<PostResource> RetrievePostResourceForId(int postResourceId);
}