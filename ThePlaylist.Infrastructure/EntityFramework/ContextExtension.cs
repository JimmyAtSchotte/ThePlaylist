
using NHibernate;
using ThePlaylist.Core.Specification;
using ThePlaylist.Infrastructure.EntityFramework.Specification;

namespace ThePlaylist.Infrastructure.EntityFramework;

public static class ContextExtension
{
    private static readonly SpecificationQuery SpecificationQuery = new SpecificationQuery();
    
    public static IEnumerable<T> Query<T>(this IQueryable<T> queryable, ISpecification<T> specification)
        where T : class
    {
        return SpecificationQuery.Execute(queryable, specification);
    }
}