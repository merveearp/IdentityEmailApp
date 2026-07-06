using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Context
{
    public class EmailContext :IdentityDbContext<AppUser>
    {
        public EmailContext(DbContextOptions options):base(options)
        {
            
        }
    }
}
