using UnitOfWorkRepoPattern.RepositoryInterfaces;

namespace UnitOfWorkRepoPattern
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBankCards BankCards { get; }
        IRepositoryUser AuthUser { get; }
        void Save();
    }
}
