using Ardalis.Specification;

namespace ThePlaylist.Core.Interfaces;

public interface IRepository : IDisposable, IAsyncDisposable
{
    T Get<T>(object id) where T : class;
    Task<T> GetAsync<T>(object id, CancellationToken cancellationToken) where T : class;
    T Get<T>(ISpecification<T> specification) where T : class;
    Task<T> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : class;
    T Add<T>(T entity) where T : class;
    Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    void Delete<T>(T entity) where T : class;
    Task DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    T Update<T>(T entity) where T : class;
    Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    IEnumerable<T> List<T>() where T : class;
    Task<IEnumerable<T>> ListAsync<T>(CancellationToken cancellationToken) where T : class;
    IEnumerable<T> List<T>(ISpecification<T> specification) where T : class;
    Task<IEnumerable<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : class;
    IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class;
    Task<IEnumerable<TResult>> ListAsync<T, TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken) where T : class;
    void ExecuteUnitOfWork(Action<IRepository> action);
    Task ExecuteUnitOfWorkAsync(Func<IRepository, Task> action, CancellationToken cancellationToken);
}