using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moosik.api.Contexts;
using moosik.api.ViewModels;
using Thread = moosik.api.Contexts.Thread;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        /// <summary>
        /// Returns list of all ThreadViewModels
        /// </summary>
        /// <returns>A list of all ThreadViewModels</returns>
        /// <response code="200">Success - List of ThreadViewModels has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ThreadViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetAllThreads([FromQuery]int? userId = null)
        {
            using var context = new MoosikContext();
            
                var threads = context.Threads.Select(x => new ThreadViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    ThreadTypeId = x.ThreadTypeId,
                    ThreadType = new ThreadTypeViewModel()
                    {
                        Id = x.ThreadType.Id,
                        Description = x.ThreadType.Description,
                    },
                    UserId = x.UserId,
                    User = new UserViewModel()
                    {
                        Id = x.User.Id,
                        Username = x.User.Username,
                        Email = x.User.Email,
                        Active = x.Active
                    },
                    CreatedDate = x.CreatedDate,
                    Active = x.Active,
                    Posts = x.Posts.OrderBy(d => d.CreatedDate).Select(p => new PostViewModel()
                    {
                        Id = p.Id,
                        Description = p.Description,
                        UserId = p.UserId,
                        User = new UserViewModel()
                        {
                            Id = p.User.Id,
                            Username = p.User.Username,
                            Email = p.User.Email,
                            Active = p.User.Active
                        },
                        ThreadId = p.ThreadId,
                        CreatedDate = p.CreatedDate,
                        Active = p.Active,
                        PostResources = p.PostResources.Select(pr => new PostResourceViewModel()
                        {
                            Id = pr.Id,
                            Title = pr.Title,
                            Value = pr.Value,
                            ResourceTypeId = pr.ResourceTypeId,
                            ResourceType = new ResourceTypeViewModel()
                            {
                                Id = pr.ResourceType.Id,
                                Description = pr.ResourceType.Description
                            }
                        })
                    }),
                });

                //Begin filtering provided a valid query has been provided
                if (userId != null)
                {
                    threads = threads.Where(x => x.UserId == userId);
                }
                
            return Ok(threads.ToList());
        }
        
        /// <summary>
        /// Finds the Thread matching a given ThreadId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Thread object matching the given id parameter</returns>
        /// <response code="200">Success - Thread has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - Given Post does not exist</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ThreadViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}", Name = "GetThreadById")]
        public IActionResult GetThreadById([FromRoute] int id)
        {
            using var context = new MoosikContext();

            var thread = context.Threads.Single(x => x.Id == id);
            return Ok(thread);
        }
        
        /// <summary>
        /// Returns all Thread objects that occur after the provided date
        /// </summary>
        /// <param name="date" example="2008-10-31T17:04:32"></param>
        /// <remarks>
        /// Sample request:
        ///     URL: www.moosik.com/thread?date=2008-10-31T17:04:32
        /// </remarks>
        /// <returns>List of Thread objects containing all Threads that occur after a provided date</returns>
        /// <response code="200">Success - List of Threads has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ThreadViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAfterDate",Name = "GetThreadsAfterDate")]
        public IActionResult GetThreadsAfterDate([FromQuery] DateTime date)
        {
            
            using var context = new MoosikContext();
            
            var threads = context.Threads.Where(x => x.CreatedDate > date ).AsNoTracking().ToList();

            return Ok(threads);
        }

        /// <summary>
        /// Updates the Thread object matching the provided ThreadId of the request body object, returns the newly updated object
        /// </summary>
        /// <param name="threadViewModelDto"></param>-
        /// <remarks>
        /// Sample request:
        ///
        ///     Body:
        ///     {
        ///         id: 8,
        ///         title: "Need a good song please",
        ///         threadTypeId: 4,
        ///         userId: 7,
        ///         createdDate: "2008-10-31T17:04:32"
        ///     }
        /// </remarks>
        /// <returns>Thread object if it has been updated, otherwise error code</returns>
        /// <response code="200">Success - Thread has been successfully updated</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such Thread exists to update</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ThreadViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(Name = "UpdateThread")]
        public IActionResult UpdateThread([FromBody] ThreadViewModel threadViewModelDto)
        {
            using var context = new MoosikContext();

            //Find matching thread object in DB
            var thread = context.Threads.Find(threadViewModelDto.Id);
            
            //Update values
            thread.Title = threadViewModelDto.Title;
            thread.ThreadTypeId = threadViewModelDto.ThreadTypeId;
            thread.UserId = threadViewModelDto.UserId;
            
            //Save values
            context.SaveChanges();

            return Ok(thread);
        }

        /// <summary>
        /// Creates a new Thread using the provided CreateThreadViewModel
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Body:
        ///     {
        ///         Title: "Need song for gym please",
        ///         PostDescription: "Going to gym, please help with song"
        ///         ThreadTypeId: 2,
        ///         UserId: 1,
        ///         PostResourceTitle: "Beatles - Ticket to Ride"
        ///         PostResourceValue: "htp://youtube.com/tickettoride"
        ///         ResourceTypeId: 1
        ///     }
        /// </remarks>
        /// <param name="createThreadViewModel"></param>
        /// <returns>Newly created Thread provided it has been created, otherwise an error code</returns>
        /// <response code="201">Success - Post has been successfully created</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult CreateThread([FromBody] CreateThreadViewModel createThreadViewModel)
        {
            using var context = new MoosikContext();

            //Todo: Make optional by null checking if incoming data contains a postResource, if so skip this chunk and deal with line 234
            var postResource = context.PostResources.Add(new PostResource()
            {
                Title = createThreadViewModel.PostResourceTitle,
                Value = createThreadViewModel.PostResourceValue,
                ResourceTypeId = createThreadViewModel.ResourceTypeId
            }).Entity;
            
            var post = context.Posts.Add(new Post()
            {
                Description = createThreadViewModel.PostDescription,
                CreatedDate = DateTime.UtcNow,
                Active = true,
                UserId = createThreadViewModel.UserId,
                PostResources = new List<PostResource>()
                {
                    postResource
                }
            }).Entity;

            var thread = context.Threads.Add(new Thread()
            {
                Title = createThreadViewModel.Title,
                CreatedDate = DateTime.UtcNow,
                Active = true,
                UserId = createThreadViewModel.UserId,
                ThreadTypeId = createThreadViewModel.ThreadTypeId,
                Posts = new List<Post>()
                {
                    post
                }
            });
            
            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes the Thread matching the provided ThreadId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [HttpDelete("{id:int}", Name = "DeleteThreadById")]
        public IActionResult DeleteThreadById([FromRoute] int id)
        {
            var context = new MoosikContext();

            var thread = context.Threads.Single(x => x.Id == id);

            thread.Active = false;

            context.SaveChanges();

            return Ok();
        }
        
        /// <summary>
        /// Gets all ThreadTypes
        /// </summary>
        /// <returns>A list of all ThreadTypes within the system</returns>
        /// <response code="200">Success - List of ThreadTypes successfully return</response>
        /// <response code="404">Not Found - No such list exists</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ThreadTypeViewModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("threadtypes", Name = "GetAllThreadTypes")]
        public IActionResult GetAllThreadTypes()
        {
            using var context = new MoosikContext();
            
            return Ok(context.ThreadTypes.ToList());
        }
        
    }
}