using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public  static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration config)
        {
             service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>
            {
                opt.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuerSigningKey= false,
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(config["SecretKey"])),
                ValidateIssuer = false,
                ValidateAudience =false            
                };
            });
            return service;
        }

    }
}