

using E_voting.DB_CONTEXT;
using Microsoft.EntityFrameworkCore;

namespace E_voting.Repository;
public class CommonRepository<T> : ICommonRepository<T> where T : class
{
    private readonly EVotingDbContext _dbContext;

    //injecting the instance of DBContext into the constructor

    public CommonRepository(EVotingDbContext dbContext)
    {
        _dbContext = dbContext;
        
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public IQueryable<T> GetAll()
    {
        return _dbContext.Set<T>();
    }


    public async Task<T> GetDetailsAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

     public async Task<int> InsertAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync();
    }
    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return 0;
        }
        _dbContext.Set<T>().Remove(entity);
        return await _dbContext.SaveChangesAsync();
    }


    public async Task<int> RemoveAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return await _dbContext.SaveChangesAsync();
    }


}
