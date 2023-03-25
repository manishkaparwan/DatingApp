using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext :DbContext
    {
//         A provider can be configured by overriding the 'DbContext.OnConfiguring' method or by using 'AddDbContext' on the application service provider. If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext> object 
// in its constructor and passes it to the base constructor for DbContext.
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> User{get;set;}
    }
}