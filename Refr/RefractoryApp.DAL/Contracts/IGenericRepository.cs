using System.Linq.Expressions;

namespace RefractoryApp.DAL.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> SelectAllAsync();
    Task<T?> SelectByIdAsync(int? id);
    Task InsertAsync(T obj);
    Task UpdateAsync(T obj);
    Task DeleteAsync(int? id);
    Task SaveAsync();
    Task<IEnumerable<T>> SelectAllAsync(Expression<Func<T, bool>> predicate);
}