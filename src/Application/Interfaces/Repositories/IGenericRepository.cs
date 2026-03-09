namespace Application.Interfaces.Repositories;

public interface IGenericRepository<TEntity, TId>
    where TEntity : class
{
    Task<List<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}