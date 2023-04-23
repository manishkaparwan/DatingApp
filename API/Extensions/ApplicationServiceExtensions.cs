using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        
        public static IServiceCollection AddApplicationService(this IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<DataContext>(opt=> 
            {
                opt.UseSqlite(
                config["DefaultConnection"]);
            });
            service.AddCors();
            service.AddScoped<ITokenService,TokenService>();           
            return service;
        }
    }
}