using Core.Entities;

namespace Core.Interfaces;

//only use this repositoy with our base entity or a derived entity
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetbyIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    
    //these methods are used by our specification pattern
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

    //overload these methods to support tresult, these will return a TResult
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);


    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
    Task<int> CountAsync(ISpecification<T> spec);
}
