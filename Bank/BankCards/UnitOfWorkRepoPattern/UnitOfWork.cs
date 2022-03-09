using DataBaseRepositoryEF;
using UnitOfWorkRepoPattern.RepositoryInterfaces;

namespace UnitOfWorkRepoPattern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankCardContext _context;

        public IRepositoryBankCards BankCards { get; }
        public IRepositoryUser AuthUser { get; }


        public UnitOfWork(BankCardContext context,    IRepositoryBankCards bankCarsRepository,    IRepositoryUser userRepository)
        {
            _context = context;

            BankCards = bankCarsRepository;
            AuthUser = userRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
