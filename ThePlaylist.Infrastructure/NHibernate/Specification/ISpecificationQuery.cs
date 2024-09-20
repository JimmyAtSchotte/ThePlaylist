namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public interface ISpecificationQuery<T>
{
    Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken);
    IEnumerable<T> ToList();
    T? FirstOrDefault();
    Task<T?> FirstOrDefaultAsync(CancellationToken cancellationToken);
}