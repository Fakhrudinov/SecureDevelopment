namespace DataAbstraction.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<CardEntity>> GetAllCards(CancellationTokenSource cts);
        Task<CardEntity> GetCardById(int id, CancellationTokenSource cts);
        Task<CardEntity> GetCardByNumber(string number, CancellationTokenSource cts);
        Task<bool> CheckCardIdExist(int id, CancellationTokenSource cts);
        Task EditCardEntity(CardEntity cardEntity, CancellationTokenSource cts);
        Task<CardEntity> CreateNewCard(CardEntityToPost cardEntity, CancellationTokenSource cts);
        Task<CardEntity> CreateNewCardAutoField(CardEntityToPostAutoField cardEntity, CancellationTokenSource cts);
        Task DeleteCardEntity(int id, CancellationTokenSource cts);
    }
}
