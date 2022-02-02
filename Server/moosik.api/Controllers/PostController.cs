using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Authorization;
using moosik.api.ViewModels;
using moosik.services.Dtos;
using moosik.services.Exceptions;
using moosik.services.Interfaces;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;
        private readonly IMapper _mapper; 
        public PostController(IPostService service, IMapper mapper) => (_service, _mapper) = (service, mapper);
        
        
        /// <summary>
        /// Returns a list of all post object. Filters posts matching threadId if provided.
        /// <param name="threadId"></param>
        /// </summary>
        /// <returns>A list of PostViewModels</returns>
        /// <response code="200">Success - List has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostViewModel[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetAllPost([FromQuery]int? threadId = null)
        {
            var posts = _service.GetAllPosts(threadId);
            return Ok(_mapper.Map<PostViewModel[]>(posts));
        }
        
        /// <summary>
        /// Finds the post matching a given postId
        /// </summary>
        /// <param name="postId" example = "2"></param>
        /// <remarks>
        /// Sample request:
        ///     URL: www.moosik.com/post/2
        /// </remarks>
        /// <returns>Post object matching the given id parameter</returns>
        /// <response code="200">Success - Post has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - Given Post does not exist</response>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{postId:int}", Name = "GetPostById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPostById([FromRoute] int postId)
        {
            var post = _service.GetPostById(postId);
            return Ok(_mapper.Map<PostViewModel>(post));
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostViewModel[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAfterDate")]
        public IActionResult GetPostsAfterDate([FromQuery] DateTime date)
        {
            var posts = _service.GetPostsAfterDate(date);
            return Ok(_mapper.Map<PostViewModel[]>(posts));
        }

        /// <summary>
        /// Updates the Post object matching the provided postId with the data provided in the body.
        /// </summary>
        /// <param name="updatePostViewModel"></param>
        /// <param name="postId"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///         description: "Cool Song"
        ///     }
        /// </remarks>
        /// <returns>No content returned</returns>
        /// <response code="204">No content returned</response>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        [Route("{postId:int:min(1)}")]
        public IActionResult UpdatePost(int postId, [FromBody] UpdatePostViewModel updatePostViewModel)
        {
            var updatePostDto = _mapper.Map<UpdatePostDto>(updatePostViewModel);
            updatePostDto.Id = postId;
            
            _service.UpdatePost(updatePostDto);
            
            return NoContent();
        }

        /// <summary>
        /// Creates a new Post on the provided threadId in the route.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Body:
        ///     {
        ///         UserId: 7,
        ///         Description: "Check this song out",
        ///         Resource:{
        ///             Title: "Hey Jude",
        ///             Value: "youtube.com/heyjude",
        ///             TypeId: 1
        ///         }       
        ///     }
        /// </remarks>
        /// <param name="createPostViewModel"></param>
        /// <param name="threadId" example="1" ></param>
        /// <returns>Newly created Post, provided it has been created, otherwise an error code</returns>
        /// <response code="204">No content returned</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost]
        [Route("{threadId:int:min(1)}")]
        public IActionResult CreatePost([FromBody] CreatePostViewModel createPostViewModel, int threadId)
        {
            var createPostDto = _mapper.Map<CreatePostDto>(createPostViewModel);
            createPostDto.ThreadId = threadId;
            
            _service.CreatePost(createPostDto);
            return NoContent();
        }
        
        /// <summary>
        /// Deletes(sets 'Active' property to false) the Post matching the provided postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>No content returned</returns>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [HttpDelete("{postId:int:min(1)}")]
        public IActionResult DeletePost([FromRoute] int postId)
        {
            _service.DeletePost(postId);
            return NoContent();
        }

        /// <summary>
        /// Updates the PostResource object matching the provided postResourceId with the data provided in the body.
        /// </summary>
        /// <param name="updatePostResourceViewModel"></param>
        /// <param name="postResourceId"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///         Title: "Enter Sandman",
        ///         Value: "youtube.com/enterSandman"
        ///     }
        /// </remarks>
        /// <returns>No content returned</returns>
        /// <response code="204">No content returned</response>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        [Route("resources/{postResourceId:int:min(1)}")]
        public IActionResult UpdatePostResource(int postResourceId, [FromBody] UpdatePostResourceViewModel updatePostResourceViewModel)
        {
            var updatePostResourceDto = _mapper.Map<UpdatePostResourceDto>(updatePostResourceViewModel);
            updatePostResourceDto.Id = postResourceId;
            
            _service.UpdatePostResource(updatePostResourceDto);
            return NoContent();
        }
        
        /// <summary>
        /// Get all ResourceTypes
        /// </summary>
        /// <returns>An array of all ResourceTypeViewModels</returns>
        /// <response code="200">Success - Array of all ResourceTypeViewModels returned</response>
        /// <response code="404">Not Found - No such list exists</response>
        [HttpGet("resourceTypes")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceTypeViewModel[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllResourceTypes()
        {
            var resourceTypesDto = _service.GetAllResourceTypes();
            return Ok(_mapper.Map<ResourceTypeViewModel[]>(resourceTypesDto));
        }
    }
}