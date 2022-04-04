using DataAbstraction;

namespace UnitOfWorkRepoPattern.RepositoryInterfaces
{
    public interface IRepositoryBankCards : IRepositoryCommonGeneric<CardEntity>
    {
        // some additional, not common request here
        Task<CardEntity> GetCardByNumber(string number);
    }
}
