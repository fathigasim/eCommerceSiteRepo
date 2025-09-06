using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Models.VM;
using efcoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        efContext dbContext;
        JwtService jwtService;
        IConfiguration configuration;
        public SignUpController(efContext _dbContext, JwtService _jwtService, IConfiguration _configuration)
        {
            dbContext = _dbContext;
            jwtService = _jwtService;
            configuration = _configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm][Bind("Username","Password")] RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("username or password error");
            }
            
            var signup = new Register()
            {
                Name = register.Username,
                Password = EncDec.EncryptString(configuration["EnkDec:key"], register.Password),
                role="1"
            };
            dbContext.Register.Add(signup);
            dbContext.SaveChanges();

            return Ok();
        }
       
    }

}
