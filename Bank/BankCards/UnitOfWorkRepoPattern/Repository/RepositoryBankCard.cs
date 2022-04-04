using DataAbstraction;
using DataBaseRepositoryEF;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkRepoPattern.RepositoryInterfaces;

namespace UnitOfWorkRepoPattern.Repository
{
    public class RepositoryBankCard : IRepositoryBankCards
    {
        private BankCardContext _context;

        public RepositoryBankCard(BankCardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardEntity>> GetAll()
        {
            return await _context.CardEntities.ToListAsync();
        }

        public async Task<CardEntity> GetCardByNumber(string number)
        {
            return await _context.CardEntities
                .Where(card => card.Number.Equals(number))
                .FirstOrDefaultAsync();
        }
    }
}
