using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAuthenticationAspNetCore2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthenticationAspNetCore2.API
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private AppDbContext db;

        public UsersController(AppDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Users.ToList());
        }
    }
}