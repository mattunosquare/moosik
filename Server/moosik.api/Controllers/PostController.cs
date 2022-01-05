using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Contexts;
using moosik.api.ViewModels;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        /// <summary>
        /// Returns a list of all post object
        /// </summary>
        /// <returns>A list of all post objects</returns>
        /// <response code="200">Success - List has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetAllPost([FromQuery]int? threadId = null)
        {
            var context = new MoosikContext();

            var posts = context.Posts.AsQueryable();
            
            //Begin filtering provided a valid query has been provided

            if (threadId != null)
            {
                posts = posts.Where(x => x.ThreadId == threadId);
            }
            
            return Ok(posts.ToList());
        }
        
        /// <summary>
        /// Finds the post matching a given PostId
        /// </summary>
        /// <param name="id" example = "2"></param>
        /// <remarks>
        /// Sample request:
        ///     URL: www.moosik.com/post/2
        /// </remarks>
        /// <returns>Post object matching the given id parameter</returns>
        /// <response code="200">Success - Post has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - Given Post does not exist</response>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{id:int}", Name = "GetPostById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostById([FromRoute] int id)
        {
            var context = new MoosikContext();

            var post = context.Posts.Find(id);

            return Ok(post);
        }
        /// <summary>
        /// Returns all Post objects that occur after the provided date
        /// </summary>
        /// <param name="date" example="2008-10-31T17:04:32"></param>
        /// <remarks>
        /// Sample request:
        ///     URL: www.moosik.com/post?date=2008-10-31T17:04:32
        /// </remarks>
        /// <returns>List of Post objects containing all Posts that occur after a provided date</returns>
        /// <response code="200">Success - List of Posts has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAfterDate")]
        public IActionResult GetPostsAfterDate([FromQuery] DateTime date)
        {
            var context = new MoosikContext();

            var posts = context.Posts.Where(x => x.CreatedDate > date).ToList();

            return Ok(posts);
        }

        /// <summary>
        /// Updates the Post object matching the provided PostId of the request body object, returns the newly updated object
        /// </summary>
        /// <param name="postViewModelDto"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///         id: 1,
        ///         description: "Cool Song",
        ///         userId: 4,
        ///         threadId: 7,
        ///         createdDate: "2008-10-31T17:04:32"
        ///     }
        /// </remarks>
        /// <returns>Post object if it has been updated, otherwise error code</returns>
        /// <response code="200">Success - Post has been successfully updated</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such Post exists to update</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(Name = "UpdatePost")]
        public IActionResult UpdatePost([FromBody] PostViewModel postViewModelDto)
        {
            var context = new MoosikContext();

            var post = context.Posts.Find(postViewModelDto.Id);

            post.Description = postViewModelDto.Description;
            post.UserId = postViewModelDto.UserId;
            post.ThreadId = postViewModelDto.ThreadId;

            context.SaveChanges();

            return Ok(post);
        }

        /// <summary>
        /// Creates a new Post using the provided Post object
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Body:
        ///     {
        ///            "ThreadId": 1,
        ///            "UserId": 7,
        ///            "Description": "REPLY FROM POSTMAN",
        ///            "PostResourceTitle": "MY PR TITLE",
        ///            "PostResourceValue": "MY PR VALUE",
        ///            "ResourceTypeId": 1
        ///     }
        /// </remarks>
        /// <param name="createPostViewModel"></param>
        /// <returns>Newly created Post provided it has been created, otherwise an error code</returns>
        /// <response code="201">Success - Post has been successfully created</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostViewModel createPostViewModel)
        {
            using var context = new MoosikContext();

            var postResource = context.PostResources.Add(new PostResource()
            {
                Title = createPostViewModel.PostResourceTitle,
                Value = createPostViewModel.PostResourceValue,
                ResourceTypeId = createPostViewModel.ResourceTypeId
            }).Entity;

            var post = context.Posts.Add(new Post()
            {
                Description = createPostViewModel.Description,
                CreatedDate = DateTime.UtcNow,
                Active = true,
                UserId = createPostViewModel.UserId,
                ThreadId = createPostViewModel.ThreadId,
                PostResources = new List<PostResource>() {postResource}
            });

            context.SaveChanges();

            return Ok();
        }
        
        /// <summary>
        /// Deletes the Post matching the provided PostId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [HttpDelete("{id:int}")]
        public IActionResult DeletePost([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all ResourceTypes
        /// </summary>
        /// <returns>List of all ResourceTypes</returns>
        /// <response code="200">Success - List of ResourceTypes successfully return</response>
        /// <response code="404">Not Found - No such list exists</response>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("resourcetypes")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ResourceTypeViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllResourceTypes()
        {
            using var context = new MoosikContext();

            var types = context.ResourceTypes.Distinct();

            return Ok(types.ToList());
        }
    }
}