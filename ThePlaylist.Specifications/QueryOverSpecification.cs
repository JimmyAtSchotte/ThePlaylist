using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public class QueryOverSpecification<T> : Specification<T>
{
    private Action<IQueryOver<T, T>>? _action;
    public Action<IQueryOver<T, T>> GetQueryOver() => _action ?? (_ => {});
    protected void UseQueryOver(Action<IQueryOver<T, T>> action) => _action = action;
}

public class QueryOverSpecification<T, TResult> : Specification<T, TResult>
{
    private Func<IQueryOver<T, T>, IQueryOver<T, T>>? _action;
    public Func<IQueryOver<T, T>, IQueryOver<T, T>> GetQueryOver() => _action ??  throw new NotSupportedException();
    protected void UseQueryOver(Func<IQueryOver<T, T>, IQueryOver<T, T>> action) => _action = action;
}