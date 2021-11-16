using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using moosik.api.ViewModels;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetUserById([FromRoute]int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{username}/{email}")]
        public IActionResult GetIdByUsernameAndEmail([FromQuery]string username, [FromQuery]string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUserById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
    }
}