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
        [HttpGet(Name = "GetThreadsAfterDate")]
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
        /// Creates a new Thread using the provided Thread object
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Body:
        ///     {
        ///         title: "Need song for gym please",
        ///         threadTypeId: 8,
        ///         userId: 10,
        ///         createdDate: "2008-10-31T17:04:32"
        ///     }
        /// </remarks>
        /// <param name="threadViewModelDto"></param>
        /// <returns>Newly created Thread provided it has been created, otherwise an error code</returns>
        /// <response code="201">Success - Post has been successfully created</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ThreadViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "CreateThread")]
        public IActionResult CreateThread([FromBody] ThreadViewModel threadViewModelDto)
        {
            var context = new MoosikContext();

            context.Threads.Add(new Thread
            {
                Title = threadViewModelDto.Title,
                Active = true,
                CreatedDate = DateTime.UtcNow,
                ThreadTypeId = threadViewModelDto.ThreadTypeId,
                UserId = threadViewModelDto.UserId
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