using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public abstract class QueryOverSpecification<T>(Action<IQueryOver<T, T>> action) : Specification<T>
{
    public Action<IQueryOver<T, T>> GetQueryOver() => action;
}

public abstract class QueryOverSpecification<T, TResult>(Func<IQueryOver<T, T>, IQueryOver<T, T>> action)
    : Specification<T, TResult>
{
    public Func<IQueryOver<T, T>, IQueryOver<T, T>> GetQueryOver() => action;
}