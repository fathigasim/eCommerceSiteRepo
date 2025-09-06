using efcoreApi.Data;
using efcoreApi.Models;
using efcoreApi.Models.VM;
using efcoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace efcoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        efContext dbContext;
        JwtService jwtService;
        IConfiguration configuration;
        PasswordResetService _resetService;
        IEmailSender _emailSender;
        public LoginController(efContext _dbContext, JwtService _jwtService, IConfiguration _configuration, PasswordResetService resetService, IEmailSender emailSender)
        {
            dbContext = _dbContext;
            jwtService = _jwtService;
            configuration = _configuration;
            _resetService = resetService;
            _emailSender = emailSender;
        }
        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Authenticate authenticate)
        {
            try
            {
                //if(authenticate.Password==EncDec.DecryptString)
                var userdb = await dbContext.Register.Where(p => p.Name.Equals(authenticate.Username)).FirstOrDefaultAsync();
                var decryptpass = EncDec.DecryptString(configuration["EnkDec:key"], userdb.Password);
                if (userdb == null || authenticate.Password != decryptpass)
                {
                    return NotFound("User Not exist or wrong pass");
                }
                else if (authenticate.Password == decryptpass)
                {
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[]
                            {
                                new Claim(ClaimTypes.NameIdentifier,userdb.Id),
                         new Claim(ClaimTypes.Name, userdb.Name),
                         new Claim(ClaimTypes.Role, userdb.role)
                    }),
                            Expires = DateTime.Now.Add(TimeSpan.FromMinutes(20)),//DateTime.UtcNow.AddHours(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var jwt = tokenHandler.WriteToken(token);
                        //   var reactLoginUrl = "https://localhost:5000";
                        // return url after successfull authentication


                        //return Ok(new { token = jwt ,user=authenticate.Username});
                        return Ok(new { token = jwt });
                        // Add other claims as needed

                    }
                }
                else
                {
                    return Unauthorized("Unauthorized to access this resource");
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _resetService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);

            if (!result)
                return BadRequest(new { Message = "Invalid or expired token." });

            return Ok(new { Message = "Password has been reset successfully." });
        }

     

            [HttpPost("forgot-password")]
            public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
            {
                var user = await dbContext.Register.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                    return Ok(new { Message = "If the email exists, a reset link has been sent." }); // don't leak user existence

                var token = await _resetService.GeneratePasswordResetTokenAsync(user.Id);
            var frontendUrl = configuration["Frontend:BaseUrl"];

            //var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";
            var resetLink = $"{frontendUrl}/resetPassowrd?token={Uri.EscapeDataString(token)}&email={user.Email}";
            await _emailSender.SendEmailAsync(user.Email, "Reset Password",
                    $"Click <a href='{resetLink}'>here</a> to reset your password.");

                return Ok(new { Message = "If the email exists, a reset link has been sent." });
            }
        


        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
