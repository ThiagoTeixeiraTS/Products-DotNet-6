using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Products.Api.Data;
using Products.Api.Models;
using Products.Api.Models.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ProductsContext _context;
        public AuthController(ProductsContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/auth/account")]
        public async Task<IActionResult> CreateAccount(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user = new
                {
                    name = user.Name,
                    email = user.Email,
                    id = user.Id
                },
                token = GenerateToken()
            });


        }

        [HttpPost]
        [Route("/auth/login")]
        public async Task<IActionResult> Login (Login login)
        {
            var user = await _context.Users.Where(l => l.Email == login.Email && l.Password == login.Password ).FirstOrDefaultAsync();
            if (user == null)
                return Unauthorized();
            


            return Ok(new
            {
                user = new
                {
                    name = user.Name,
                    email = user.Email,
                    id = user.Id
                },
                token = GenerateToken()
            });
        }

        #region PrivateMethods
      
            public static string GenerateToken()
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("Renato&AugustoFilhosAmados");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                   // Subject = new ClaimsIdentity(new Claim[]
                   //{
                   //  new Claim(ClaimTypes.Name, user.Email),
                   // new Claim(ClaimTypes.Role, user.Password)
                   //}),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        #endregion
    }

}
