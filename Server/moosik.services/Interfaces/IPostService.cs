using System;
using System.Collections.Generic;
using moosik.dal.Contexts;
using moosik.services.Dtos;

namespace moosik.services.Interfaces;

public interface IPostService
{
    ICollection<PostDto> GetAllPosts(int? threadId);
    PostDto GetPostById(int postId);
    ICollection<PostDto> GetPostsAfterDate(DateTime date);
    void UpdatePost(PostDto post);
    void CreatePost(PostDto post);
    void DeletePost(int postId);
    ICollection<ResourceType> GetAllResourceTypes();
}