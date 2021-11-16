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
    public class PostController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet()]
        public IActionResult GetPostsAfterDate([FromQuery] DateTime date)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePost([FromRoute] int id, [FromBody] Post post)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] Post post)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePost([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}