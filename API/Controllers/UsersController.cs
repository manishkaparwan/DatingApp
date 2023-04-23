using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController: BaseApiController
    {
        private readonly DataContext context;

        public UsersController(DataContext context)
        {
            this.context =context;
        }

        [AllowAnonymous]
        //Endpoint to get all users in the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await context.User.ToListAsync();
        }
        //Endpoint to get specific user in the database
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           return await context.User.FindAsync(id);
        }
    }
}