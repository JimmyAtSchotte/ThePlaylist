using Ardalis.Specification;
using NHibernate;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.NHibernate;

public static class ApplySpecificationExtensions
{
    public static ICriteria ApplySpecification<T>(this ISession session, CriteriaSpecification<T> specification) 
        where T : class
    {
        return session.CreateCriteria<T>()
            .Apply(query => specification.GetCriteria().Invoke(query));
    }
    
    public static ICriteria ApplySpecification<T, TResult>(this ISession session, CriteriaSpecification<T, TResult> specification) 
        where T : class
    {
        return session.CreateCriteria<T>()
            .Apply(query => specification.GetCriteria().Invoke(query));
    }
    
    public static IQueryOver<T> ApplySpecification<T>(this ISession session, QueryOverSpecification<T> specification) 
        where T : class
    {
        return session.QueryOver<T>().Apply(queryOver => specification.GetQueryOver().Invoke(queryOver));
    }
    
    public static IQueryOver<T> ApplySpecification<T, TResult>(this ISession session, QueryOverSpecification<T, TResult> specification) 
        where T : class
    {
        return session.QueryOver<T>().Apply(queryOver => specification.GetQueryOver().Invoke(queryOver));
    }
    
    public static IQuery ApplySpecification<T>(this ISession session, HqlSpecification<T> specification) 
        where T : class
    {
        return session.Apply(x => specification.GetHql().Invoke(x));
    }
    
    public static IQuery ApplySpecification<T, TResult>(this ISession session, HqlSpecification<T, TResult> specification) 
        where T : class
    {
        return session.Apply(x => specification.GetHql().Invoke(x));
    }
    
    
    public static IQueryable<T> ApplySpecification<T>(this ISession session, LinqToQuerySpecificationEvaluator evaluator,  ISpecification<T> specification) 
        where T : class
    {
        return evaluator.GetQuery(session.Query<T>().AsQueryable(), specification);
    }
    
    public static IQueryable<TResult> ApplySpecification<T, TResult>(this ISession session, LinqToQuerySpecificationEvaluator evaluator,  ISpecification<T, TResult> specification) 
        where T : class
    {
        return evaluator.GetQuery(session.Query<T>().AsQueryable(), specification);
    }
    
}