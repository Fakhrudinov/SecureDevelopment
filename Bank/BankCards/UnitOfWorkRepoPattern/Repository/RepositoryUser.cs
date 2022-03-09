using DataAbstraction.AuthModels;
using DataBaseRepositoryEF;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkRepoPattern.RepositoryInterfaces;

namespace UnitOfWorkRepoPattern.Repository
{
    public class RepositoryUser : IRepositoryUser
    {
        private BankCardContext _context;

        public RepositoryUser(BankCardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
