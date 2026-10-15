using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Models
{
    public class NewContext : IdentityDbContext<ApplicationUser>
    {
        public NewContext(DbContextOptions<NewContext> options) : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Teamnumbers> Teamnumbers { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }
        public DbSet<Readers> Readers { get; set; }
    }
}