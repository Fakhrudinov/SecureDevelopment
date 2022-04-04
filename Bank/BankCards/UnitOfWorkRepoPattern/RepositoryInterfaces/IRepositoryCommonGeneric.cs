namespace UnitOfWorkRepoPattern.RepositoryInterfaces
{
    public interface IRepositoryCommonGeneric<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        //Task<T> Get(int id);
        //void Create(T item);
        //void Update(T item);
        //void Delete(int id);
    }
}
