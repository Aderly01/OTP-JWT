using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JWT_api.Modelos;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;

namespace JWT_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticationController : ControllerBase
    {
        private readonly string secretKey;

        public AutenticationController(IConfiguration config)
        {
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
        }

        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] User request)
        {
            if (request.correo == "arpd@gmail.com" && request.clave == "123456")
            {
                /*Random rnd = new Random();
                string otpCreado = "";

                for (int ctr = 1; ctr <= 6; ctr++)
                {
                    otpCreado += rnd.Next(10).ToString();
                }*/

                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.correo));
                //claims.AddClaim(new Claim("otp", otpCreado));
               

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });
            } else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new {token = ""});
            }
        }
    }
}
