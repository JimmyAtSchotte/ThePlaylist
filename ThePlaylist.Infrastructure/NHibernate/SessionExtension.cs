using NHibernate;
using ThePlaylist.Core.Specification;
using ThePlaylist.Infrastructure.NHibernate.Specification;

namespace ThePlaylist.Infrastructure.NHibernate;

public static class SessionExtension
{
    private static readonly SpecificationQuery SpecificationQuery = new SpecificationQuery();
    
    public static IEnumerable<T> Query<T>(this ISession session, ISpecification<T> specification)
        where T : class
    {
        return SpecificationQuery.Execute(session, specification);
    }
}