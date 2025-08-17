namespace E_voting.Repository;

public interface ICommonRepository<T>
{
    Task<List<T>> GetAllAsync();
    public IQueryable<T> GetAll();
    Task<T> GetDetailsAsync(int id);
    Task<int> InsertAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(int id);
    Task<int> RemoveAsync(T entity);
}
