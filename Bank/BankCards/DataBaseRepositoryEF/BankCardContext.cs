using DataAbstraction;
using DataAbstraction.AuthModels;
using Microsoft.EntityFrameworkCore;

namespace DataBaseRepositoryEF
{
    public class BankCardContext : DbContext
    {
        public BankCardContext(DbContextOptions<BankCardContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CardEntity> CardEntities { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
