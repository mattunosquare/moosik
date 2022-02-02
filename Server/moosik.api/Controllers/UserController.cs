using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Authorization;
using moosik.api.ViewModels.User;
using moosik.services.Dtos;
using moosik.services.Exceptions;
using moosik.services.Interfaces;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuthorization(TokenTypes.ValidAccessToken)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper) => (_service, _mapper) = (service, mapper);

        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns>A list containing all users</returns>
        /// <response code="200">Success - List has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RoleAuthorization(MoosikRoles.Admin)]
        [HttpGet]
        public IActionResult GetAllUsers([FromQuery]int? userId = null)
        {
            var users = _service.GetAllUsers(userId); 
            return Ok(_mapper.Map<UserViewModel[]>(users));
        }
        
        /// <summary>
        /// Finds the User matching a given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User object matching the given userId parameter</returns>
        /// <response code="200">Success - User has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - Given Post does not exist</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDetailViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId:int}")]
        public IActionResult GetUserById([FromRoute]int userId)
        {
            var user = _service.GetUserById(userId);
            return Ok(_mapper.Map<UserDetailViewModel>(user));
        }
        
        /// <summary>
        /// Returns all User objects that matches the given parameters
        /// </summary>
        /// <param name="username" example="lennon_01"></param>
        /// <param name="email" example="j_lennon@beatles.com"></param>
        /// <remarks>Sample request:
        /// URL: www.moosik.com/user?username=lennon_01email=j_lennon@beatles.com
        /// </remarks>
        /// <returns>User object matching the provided username and email address</returns>
        /// <response code="200">Success - User has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such user matches the provided arguments</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserByUsernameAndEmail",Name = "GetUserByUsernameAndEmail")]
        public IActionResult GetUserByUsernameAndEmail([FromQuery]string username, [FromQuery]string email)
        {
            var user = _service.GetUserByUsernameAndEmail(username, email);
            return Ok(_mapper.Map<UserViewModel>(user));
        }

        /// <summary>
        /// Updates the User object matching the provided UserId with the data provided in the body.
        /// </summary>
        /// <param name="updateUserViewModel"></param>
        /// <param name="userId"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///          username: "ringo_01",
        ///          email: "ringo_01@beatles.com"
        ///     }
        /// </remarks>
        /// <returns>No content returned.</returns>
        /// <response code="204">No content returned.</response>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut]
        [Route("{userId:int:min(1)}")]
        public IActionResult UpdateUser(int userId, [FromBody] UpdateUserViewModel updateUserViewModel)
        {
            var updateUserDto = _mapper.Map<UpdateUserDto>(updateUserViewModel);
            updateUserDto.Id = userId;
            
            _service.UpdateUser(updateUserDto);
            return NoContent();
        }

        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///         username: "mccartney_01",
        ///         password: "Password123",
        ///         email: "mccartney_01@beatles.com"
        ///     }
        /// </remarks>
        /// <param name="createUserViewModel"></param>
        /// <returns>No content returned</returns>
        /// <response code="204">No content returned.</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserViewModel createUserViewModel)
        {
            _service.CreateUser(_mapper.Map<CreateUserDto>(createUserViewModel));
            return NoContent();
        }

        /// <summary>
        /// Deletes(sets 'Active' property to false) the User matching the provided userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>No content returned.</returns>
        /// <exception cref="NotFoundException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [HttpDelete("{userId:int:min(1)}")]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            _service.DeleteUser(userId);
            return NoContent();
        }
        
    }
}