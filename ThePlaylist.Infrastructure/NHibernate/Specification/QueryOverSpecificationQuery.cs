using NHibernate;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public class QueryOverSpecificationQuery<T>(Func<IQueryOver<T>> query) : ISpecificationQuery<T>
{
    public async Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken)
    {
        return await query.Invoke().ListAsync<T>(cancellationToken);
    }
    
    public IEnumerable<T> ToList()
    {
        return query.Invoke().List<T>();
    }
    
    public T? FirstOrDefault()
    {
        return query.Invoke().List<T>().FirstOrDefault();
    }
    
    public async Task<T?> FirstOrDefaultAsync(CancellationToken cancellationToken)
    {
        return (await query.Invoke().ListAsync<T>(cancellationToken)).FirstOrDefault();
    }
}

public class QueryOverSpecificationQuery<T, TResult>(Func<IQueryOver<T>> query) : ISpecificationQuery<TResult>
{
    public async Task<IEnumerable<TResult>> ToListAsync(CancellationToken cancellationToken)
    {
        return await query.Invoke().ListAsync<TResult>(cancellationToken);
    }
    
    public IEnumerable<TResult> ToList()
    {
        return query.Invoke().List<TResult>();
    }
    
    public TResult? FirstOrDefault()
    {
        return query.Invoke().List<TResult>().FirstOrDefault();
    }
    
    public async Task<TResult?> FirstOrDefaultAsync(CancellationToken cancellationToken)
    {
        return (await query.Invoke().ListAsync<TResult>(cancellationToken)).FirstOrDefault();
    }
}