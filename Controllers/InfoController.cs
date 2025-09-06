using efcoreApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
     readonly   efContext dbContext;
        public InfoController(efContext _dbContext)
        {
            dbContext = _dbContext;
        }
        // GET: api/<InfoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model= dbContext.category.Include(p=>p.Goods).ToList();
            return Ok(model);
        }

        // GET api/<InfoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InfoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // Get api/<InfoController>
        [HttpGet]
        public void About([FromBody] string value)
        {
        }

        // PUT api/<InfoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InfoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
