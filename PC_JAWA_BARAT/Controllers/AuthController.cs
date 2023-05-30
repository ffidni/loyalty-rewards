using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PC_JAWA_BARAT.DTOS;
using PC_JAWA_BARAT.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PC_JAWA_BARAT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;
        private readonly JWTSettings _jwtSettings;

        public AuthController(EsemkaOnePlusContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtSettings = jwtsettings.Value;
        }

        [HttpPost]
        public async Task<ActionResult<String>> Auth([FromBody] AuthDTO user)
        {
            Customer queryUser = _context.Customers.Where(i => i.Email == user.Email && i.Password == user.Password).FirstOrDefault();
            if (queryUser is null) return NotFound();

            Session.Email = queryUser.Email;
            Session.Role = queryUser.Role;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
