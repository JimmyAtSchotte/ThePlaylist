using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public class CriteriaSpecification<T> : Specification<T>
{
    private Action<ICriteria>? _action;
    public Action<ICriteria> GetCriteria() => _action ?? (_ => {});
    protected void UseCriteria(Action<ICriteria> action) => _action = action;
    
}

public class CriteriaSpecification<T, TResult> : Specification<T, TResult>
{
    private Action<ICriteria>? _action;
    public Action<ICriteria> GetCriteria() => _action ?? (_ => {});
    protected void UseCriteria(Action<ICriteria> action) => _action = action;
}