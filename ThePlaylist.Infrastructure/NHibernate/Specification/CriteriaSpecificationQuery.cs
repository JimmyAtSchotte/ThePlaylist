using NHibernate;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public class CriteriaSpecificationQuery<T>(Func<ICriteria> query) : ISpecificationQuery<T>
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