using ECommerceBE.Data;
using ECommerceBE.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ECommerceBE.Models;

namespace ECommerceBE.Controllers
{
    public class CompositeObject
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<IUser>> Login([FromBody] CompositeObject user)
        {


          var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == user.email & x.Password == user.password);
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.Email == user.email & x.Password == user.password);
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == user.email & x.Password == user.password);

            if (customer != null)           
               return Ok(customer);
            if (supplier != null)
                return Ok(supplier);
            if (admin == null)
                return Ok(admin);
            return NotFound("User not found.");
        }

    }
}
