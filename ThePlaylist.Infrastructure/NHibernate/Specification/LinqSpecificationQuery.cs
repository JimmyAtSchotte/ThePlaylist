using NHibernate.Linq;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public class LinqSpecificationQuery<T>(Func<IQueryable<T>> query) : ISpecificationQuery<T>
{
    public async Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken)
    {
        return await query.Invoke().ToListAsync<T>(cancellationToken);
    }
    
    public IEnumerable<T> ToList()
    {
        return query.Invoke().ToList();
    }
    
    public T? FirstOrDefault()
    {
        return query.Invoke().FirstOrDefault();
    }
    
    public async Task<T?> FirstOrDefaultAsync(CancellationToken cancellationToken)
    {
        return await query.Invoke().FirstOrDefaultAsync(cancellationToken);
    }
}