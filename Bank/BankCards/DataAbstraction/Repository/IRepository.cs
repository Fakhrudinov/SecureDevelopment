namespace DataAbstraction.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<CardEntity>> GetAllCards();
        Task<CardEntity> GetCardById(int id);
        Task<CardEntity> GetCardByNumber(string number);
        Task<bool> CheckCardIdExist(int id);
        Task EditCardEntity(CardEntity cardEntity);
        Task<CardEntity> CreateNewCard(CardEntityToPost cardEntity);
        Task<CardEntity> CreateNewCardAutoField(CardEntityToPostAutoField cardEntity);
        Task DeleteCardEntity(int id);
    }
}
