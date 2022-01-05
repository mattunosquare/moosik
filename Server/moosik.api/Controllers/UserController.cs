using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using moosik.api.Contexts;
using moosik.api.ViewModels;
using Thread = System.Threading.Thread;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Get list of all users
        /// </summary>
        /// <returns>A list containing all users</returns>
        /// <response code="200">Success - List has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such list exists</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            using var context = new MoosikContext();

            var user = context.Users.ToList();

            return Ok(user);
        }
        
        /// <summary>
        /// Finds the User matching a given UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User object matching the given id parameter</returns>
        /// <response code="200">Success - Thread has been successfully returned</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - Given Post does not exist</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        public IActionResult GetUserById([FromRoute]int id)
        {
            var context = new MoosikContext();

            var user = context.Users.Single(x => x.Id == id);

            return Ok(user);
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
            var context = new MoosikContext();

            var user = context.Users.Single(x => x.Username == username && x.Email == email);

            return Ok(user);
        }

        /// <summary>
        /// Updates the User object matching the provided UserId of the request body object, returns the newly updated object
        /// </summary>
        /// <param name="userViewModelDto"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///          id: 7,
        ///          username: "ringo_01",
        ///          email: "ringo_01@beatles.com"
        ///     }
        /// </remarks>
        /// <returns>User object if it has been updated, otherwise error code</returns>
        /// <response code="200">Success - User has been successfully updated</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <response code="404">Not Found - No such User exists to update</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut(Name = "UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserViewModel userViewModelDto)
        {
            var context = new MoosikContext();

            var user = context.Users.Find(userViewModelDto.Id);

            user.Username = userViewModelDto.Username;
            user.Email = userViewModelDto.Email;

            context.SaveChanges();

            return Ok(user);
        }

        /// <summary>
        /// Creates a new User using the provided User object
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Body:
        ///     {
        ///         username: "mccartney_01",
        ///         email: "mccartney_01@beatles.com"
        ///     }
        /// </remarks>
        /// <param name="userViewModelDto"></param>
        /// <returns>Newly created User provided it has been created, otherwise an error code</returns>
        /// <response code="201">Success - User has been successfully created</response>
        /// <response code="400">Bad Request - Check input values</response>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "CreateUser")]
        public IActionResult CreateUser([FromBody] UserViewModel userViewModelDto)
        {
            var context = new MoosikContext();

            context.Users.Add(new User
            {
                Username = userViewModelDto.Username,
                Email = userViewModelDto.Email,
                Active = true
            });

            context.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes the User matching the provided UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [HttpDelete("{id:int}", Name = "DeleteUserById")]
        public IActionResult DeleteUserById([FromRoute] int id)
        {
            var context = new MoosikContext();

            var user = context.Users.Find(id);

            user.Active = false;

            context.SaveChanges();
            
            return Ok();
        }
        
    }
}