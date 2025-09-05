using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Data;

namespace RefractoryApp.DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> SelectAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> SelectByIdAsync(int? id)
    {
        if (id == null) return null;
        return await _dbSet.FindAsync(id);
    }

    public async Task InsertAsync(T obj)
    {
        await _dbSet.AddAsync(obj);
    }

    public Task UpdateAsync(T obj)
    {
        _dbSet.Update(obj);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int? id)
    {
        if (id == null) return;
        
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> SelectAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}