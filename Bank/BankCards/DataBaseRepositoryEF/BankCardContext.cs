using DataAbstraction;
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
    }
}
