using efcoreApi.Data;
using efcoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public efContext dbContext;
        public CategoryController(efContext _dbContext)
        {
            dbContext = _dbContext;
        }
        // GET: api/<CategoryController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var model=dbContext.category.Include(p=>p.Goods).ToList();
            return Ok(model);

        }
        
        [HttpGet("CategoryType")]
        public async Task<IActionResult> GetCat()
        {
            var model =await dbContext.category.ToListAsync();
            return Ok(model);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category  category)
        {
           await dbContext.AddAsync(category);
           await dbContext.SaveChangesAsync();
            return Ok();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
