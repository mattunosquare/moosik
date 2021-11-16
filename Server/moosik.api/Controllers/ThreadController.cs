using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace moosik.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetThreadById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult GetThreadsAfterDate([FromQuery] DateTime date)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateThread([FromRoute] int id, [FromBody] Thread thread)
        {
            throw new NotImplementedException();
        }

        [HttpPost()]
        public IActionResult CreateThread([FromBody] Thread thread)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteThreadById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}