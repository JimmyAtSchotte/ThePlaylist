using Ardalis.Specification;
using NHibernate;

namespace ThePlaylist.Specifications;

public abstract class CriteriaSpecification<T>(Action<ICriteria> action, params object[] args) : Specification<T>
{
    public Action<ICriteria> GetCriteria() => action;
}

public abstract class CriteriaSpecification<T, TResult>(Action<ICriteria> action, params object[] args) : Specification<T, TResult>
{
    public Action<ICriteria> GetCriteria() => action;
}