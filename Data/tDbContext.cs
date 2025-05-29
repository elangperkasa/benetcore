using Microsoft.EntityFrameworkCore;
using BENETCORE.Model;

namespace BENETCORE.Data
{
    public class tDbContext : DbContext
    {
        public tDbContext(DbContextOptions<tDbContext> options) : base(options) { }

        // Add your DbSets here
        public DbSet<User> User { get; set; }
    }

    public class User
    {
        public string? Id { get; set; } = null;
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; } = null;
        public required string PasswordHash { get; set; }
        public required string Status { get; set; }

        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;

    }
}
