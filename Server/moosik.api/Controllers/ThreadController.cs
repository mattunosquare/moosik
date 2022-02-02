using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using moosik.api.ViewModels;
using moosik.services.Interfaces;
using AutoMapper;
using moosik.api.ViewModels.Thread;
using moosik.services.Dtos;
using moosik.services.Exceptions;


namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        private readonly IThreadService _service;
        private readonly IMapper _mapper;
        
        public ThreadController(IThreadService service, IMapper mapper) => (_service, _mapper) = (service, mapper);

        /// <summary>
        /// Returns list of all ThreadViewModels
        /// </summary>
        /// <returns>An array of all ThreadViewModels. Filters by userId if parameter is provided</returns>
        /// <response code="200">Success - List of ThreadViewModels has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ThreadViewModel[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetAllThreads([FromQuery] int? userId = null)
        {
            var threads = _service.GetAllThreads(userId);
            return Ok(_mapper.Map<ThreadViewModel[]>(threads));
        }

        /// <summary>
        /// Finds the Thread matching a given threadId
        /// </summary>
        /// <param name="threadId"></param>
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
        [HttpGet("{threadId:int}", Name = "GetThreadById")]
        public IActionResult GetThreadById([FromRoute] int threadId)
        {
            var thread = _service.GetThreadById(threadId);
            return Ok(_mapper.Map<ThreadViewModel>(thread));
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ThreadViewModel[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAfterDate", Name = "GetThreadsAfterDate")]
        public IActionResult GetThreadsAfterDate([FromQuery] DateTime date)
        {
            var threads = _service.GetThreadsAfterDate(date);
            return Ok(_mapper.Map<ThreadViewModel[]>(threads));
        }

        /// <summary>
        /// Updates the Thread object matching the provided threadId using the data from the body 
        /// </summary>
        /// <param name="updateThreadViewModel"></param>
        /// <param name="threadId"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Body:
        ///     {
        ///         title: "Need a good song please"
        ///     }
        /// </remarks>
        /// <returns>No content returned</returns>
        /// <response code="200">No content returned</response>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        [Route("{threadId:int:min(1)}")]
        public IActionResult UpdateThread(int threadId, [FromBody] UpdateThreadViewModel updateThreadViewModel)
        {
            var updateThreadDto = _mapper.Map<UpdateThreadDto>(updateThreadViewModel);
            updateThreadDto.Id = threadId;
            
            _service.UpdateThread(updateThreadDto);
            return NoContent();
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
        ///         UserId: 1,
        ///         ThreadTypeId: 2,
        ///         Post: {
        ///             UserId: 1,
        ///             Description: "Doing cardio this evening, need music please.",
        ///             Resource: {
        ///                 Title: "Hey Jude",
        ///                 Value: "youtube.com/heyjude"
        ///                 TypeId: 1
        ///             }
        ///         }
        ///     }
        /// </remarks>
        /// <param name="createThreadViewModel"></param>
        /// <returns>Newly created Thread provided it has been created, otherwise an error code</returns>
        /// <response code="201">Success - Post has been successfully created</response>
        /// <response code="400">Bad Request - Check input values</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult CreateThread([FromBody] CreateThreadViewModel createThreadViewModel)
        {
            _service.CreateThread(_mapper.Map<CreateThreadDto>(createThreadViewModel));
            return NoContent();
        }

        /// <summary>
        /// Deletes(sets 'Active' property to false) the Thread matching the provided threadId
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns>No content returned.</returns>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [HttpDelete("{threadId:int:min(1)}")]
        public IActionResult DeleteThread([FromRoute] int threadId)
        {
            _service.DeleteThread(threadId);
            return NoContent();
        }

        /// <summary>
        /// Gets all ThreadTypes
        /// </summary>
        /// <returns>An array of all ThreadTypeViewModels</returns>
        /// <response code="200">Success - Array of all ThreadTypeViewModels returned</response>
        /// <response code="404">Not Found - No such array exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ThreadTypeViewModel[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("types")]
        public IActionResult GetAllThreadTypes()
        {
            var threadTypes = _service.GetAllThreadTypes();
            return Ok(_mapper.Map<ThreadTypeViewModel[]>(threadTypes));
        }
    }
}