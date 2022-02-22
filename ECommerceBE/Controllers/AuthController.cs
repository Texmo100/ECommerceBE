using ECommerceBE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ECommerceBE.Models;
using ECommerceBE.Controllers.Utilities;
using Microsoft.Extensions.Configuration;

namespace ECommerceBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] User user)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == user.email & x.Password == UserUtilities.hashPassword(user.password));
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.Email == user.email & x.Password == UserUtilities.hashPassword(user.password));
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == user.email & x.Password == UserUtilities.hashPassword(user.password));
           
            if (customer != null)           
               return Ok(UserUtilities.CreateToken(user, _configuration));
            if (supplier != null)
                return Ok(UserUtilities.CreateToken(user, _configuration));
            if (admin != null)
                return Ok(UserUtilities.CreateToken(user, _configuration));

            return NotFound("User not found.");
        }

    

    }
}
