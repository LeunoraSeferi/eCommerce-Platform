using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthenticationApi.Domain.Entities;


public class AuthenticationDbContext : IdentityDbContext<AppUser>
{
    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
        : base(options)
    {
    }

    public new DbSet<AppUser> Users { get; set; } = null!;
}
