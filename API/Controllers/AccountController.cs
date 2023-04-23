using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        public DataContext Context { get; set; }
        public ITokenService TokenService { get; }
        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.TokenService = tokenService;
            this.Context = context;
            
        }
        #region Register using query strings
        /* [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string userName, string password)
        {
            //Storing password using hash and salt
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = userName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
            Context.User.Add(user);
            await Context.SaveChangesAsync();
            return user;
        }*/
        #endregion

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExits(registerDto.UserName.ToLower())) 
            {
                return BadRequest("User already exists");
            }
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            Context.User.Add(user);
            await Context.SaveChangesAsync();
            return new UserDto{
                UserName = registerDto.UserName,
                Token = TokenService.CreateToken(user)
            };
        }        
        private async Task<bool> UserExits(string userName)
        {
            return await Context.User.AnyAsync(x=>x.UserName == userName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //Fetching user
            var user = await Context.User.SingleOrDefaultAsync(x=>x.UserName == loginDto.UserName);
            if(user==null)
            {
                return BadRequest("User not found");
            }
            //Matching password
            var hmac = new HMACSHA512(user.PasswordSalt);
            var currentHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i = 0; i<currentHash.Length;i++)
            {
                if(currentHash[i]!= user.PasswordHash[i])
                {
                return Unauthorized("Invalid Password");
                }
            }
            return new UserDto
            {
                UserName = loginDto.UserName,
                Token = TokenService.CreateToken(user)
            };
        }
    }
}