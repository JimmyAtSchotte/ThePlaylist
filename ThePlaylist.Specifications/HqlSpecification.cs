using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public class HqlSpecification<T> : Specification<T>
{
    private Func<ISession, IQuery>? _action;
    public Func<ISession, IQuery> GetHql() => _action ?? throw new NotSupportedException("The hql has not been specified. Please use UseHql() to define the hql.");
    protected void UseHql(Func<ISession, IQuery> action) => _action = action;
}

public class HqlSpecification<T, TResult> : Specification<T, TResult>
{
    private Func<ISession, IQuery>? _action;
    public Func<ISession, IQuery> GetHql() => _action ?? throw new NotSupportedException("The hql has not been specified. Please use UseHql() to define the hql.");
    protected void UseHql(Func<ISession, IQuery> action) => _action = action;
}