using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UsersController: ControllerBase
    {
        private readonly DataContext context;

        public UsersController(DataContext context)
        {
            this.context =context;
        }

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