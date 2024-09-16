using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public class QueryOverSpecification<T> : Specification<T>
{
    private Action<IQueryOver<T, T>>? _action;
    public Action<IQueryOver<T, T>> GetQueryOver() => _action ?? throw new NotSupportedException("The Query over has not been specified. Please use the UseQueryOver() to define the query over.");
    protected void UseQueryOver(Action<IQueryOver<T, T>> action) => _action = action;
}

public class QueryOverSpecification<T, TResult> : Specification<T, TResult>
{
    private Func<IQueryOver<T, T>, IQueryOver<T, T>>? _action;
    public Func<IQueryOver<T, T>, IQueryOver<T, T>> GetQueryOver() => _action ??  throw new NotSupportedException("The Query over has not been specified. Please use the UseQueryOver() to define the query over.");
    protected void UseQueryOver(Func<IQueryOver<T, T>, IQueryOver<T, T>> action) => _action = action;
}