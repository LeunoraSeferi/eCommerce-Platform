using Microsoft.AspNetCore.Identity;

namespace AuthenticationApi.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Adress { get; set; }
        public string? Password { get; set; }

        public string? Role { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
    }
}
