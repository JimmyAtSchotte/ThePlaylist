using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public abstract class HqlSpecification<T>(Func<ISession, IQuery> action) : Specification<T>
{ 
    public Func<ISession, IQuery> GetHql() => action;
}

public abstract class HqlSpecification<T, TResult>(Func<ISession, IQuery> action) : Specification<T, TResult>
{
    public Func<ISession, IQuery> GetHql() => action;
}