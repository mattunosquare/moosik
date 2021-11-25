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
        [HttpGet(Name = "GetPostsAfterDate")]
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
        ///         description: "Cool Song",
        ///         userId: 4,
        ///         threadId: 7,
        ///         createdDate: "2008-10-31T17:04:32"
        ///     }
        /// </remarks>
        /// <param name="postViewModel"></param>
        /// <returns>Newly created Post provided it has been created, otherwise an error code</returns>
        /// <response code="201">Success - Post has been successfully created</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "CreatePost")]
        public IActionResult CreatePost([FromBody] PostViewModel postViewModel)
        {
            throw new NotImplementedException();
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
        [HttpGet("/resourcetypes")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ResourceTypeViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllResourceTypes()
        {
            throw new NotImplementedException();
        }
    }
}