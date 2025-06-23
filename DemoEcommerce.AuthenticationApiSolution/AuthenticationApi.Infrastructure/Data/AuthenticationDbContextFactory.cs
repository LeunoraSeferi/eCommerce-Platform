using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AuthenticationApi.Infrastructure.Data
{
    public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AuthenticationApi.Presentation"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            var ConnectionStrings = config.GetConnectionString("eCommerceConnection");
            optionsBuilder.UseSqlServer(ConnectionStrings);

            return new AuthenticationDbContext(optionsBuilder.Options);
        }
    }
}
