using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CG4U.Core.Services.InitialSetup
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            //TODO: Change this to a ordinary user
            const string databaseName = "CG4UAuth";
            const string databaseUser = "SA";
            const string databasePass = "He1DeVencer!CG4U#";

            return $"Server=localhost,1401;" +
                   $"database={databaseName};" +
                   $"uid={databaseUser};" +
                   $"pwd={databasePass};" +
                   $"pooling=true;";
        }
    }
}
