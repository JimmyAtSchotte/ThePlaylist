using NHibernate;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public interface ICriterionProvider
{
    void ApplyTo<T>(IQueryable<T> queryable);
}