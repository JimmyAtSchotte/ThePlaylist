using Ardalis.Specification;
using NHibernate;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public static class ApplySpecificationExtensions
{
    private static ICriteria ApplySpecification<T>(this ISession session, CriteriaSpecification<T> specification) 
        where T : class
    {
        return session.CreateCriteria<T>().Apply(query => specification.GetCriteria().Invoke(query));
    }

    private static ICriteria ApplySpecification<T, TResult>(this ISession session, CriteriaSpecification<T, TResult> specification) 
        where T : class
    {
        return session.CreateCriteria<T>().Apply(query => specification.GetCriteria().Invoke(query));
    }

    private static IQueryOver<T> ApplySpecification<T>(this ISession session, QueryOverSpecification<T> specification) 
        where T : class
    {
        return session.QueryOver<T>().Apply(queryOver => specification.GetQueryOver().Invoke(queryOver));
    }
    
    private static IQueryOver<T> ApplySpecification<T, TResult>(this ISession session, QueryOverSpecification<T, TResult> specification) 
        where T : class
    {
        return session.QueryOver<T>().Apply(queryOver => specification.GetQueryOver().Invoke(queryOver));
    }
    
    private static IQuery ApplySpecification<T>(this ISession session, HqlSpecification<T> specification) 
        where T : class
    {
        return session.Apply(x => specification.GetHql().Invoke(x));
    }
    
    private static IQuery ApplySpecification<T, TResult>(this ISession session, HqlSpecification<T, TResult> specification) 
        where T : class
    {
        return session.Apply(x => specification.GetHql().Invoke(x));
    }
    
    private static IQueryable<T> ApplySpecification<T>(this ISession session, ISpecificationEvaluator evaluator,  ISpecification<T> specification) 
        where T : class
    {
        return evaluator.GetQuery(session.Query<T>().AsQueryable(), specification);
    }
    
    private static IQueryable<TResult> ApplySpecification<T, TResult>(this ISession session, ISpecificationEvaluator evaluator,  ISpecification<T, TResult> specification) 
        where T : class
    {
        return evaluator.GetQuery(session.Query<T>().AsQueryable(), specification);
    }
    
    private static T Apply<T>(this T obj, Action<T> action)
    {
        action(obj);
        return obj;
    }
    
    private static TResult Apply<T, TResult>(this T obj, Func<T, TResult> func)
    {
        return func(obj);;
    }
    
    public static ISpecificationQuery<T> ApplySpecification<T>(this ISession session, ISpecification<T> specification) 
        where T : class
    {
        return session.ApplySpecification(specification, LinqToQuerySpecificationEvaluator.Default);
    }
    
    public static ISpecificationQuery<TResult> ApplySpecification<T, TResult>(this ISession session, ISpecification<T, TResult> specification) 
        where T : class
    {
        return session.ApplySpecification(specification, LinqToQuerySpecificationEvaluator.Default);
    }
    
    public static ISpecificationQuery<T> ApplySpecification<T>(this ISession session, ISpecification<T> specification, ISpecificationEvaluator specificationEvaluator) 
        where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification => 
                new CriteriaSpecificationQuery<T>(() => session.ApplySpecification(criteriaSpecification)),
            QueryOverSpecification<T> queryOverSpecification =>
                new QueryOverSpecificationQuery<T>(() => session.ApplySpecification(queryOverSpecification)),
            HqlSpecification<T> hqlSpecification =>
                new HqlSpecificationQuery<T>(() => session.ApplySpecification(hqlSpecification)),
            _ =>
                new LinqSpecificationQuery<T>(() => session.ApplySpecification(specificationEvaluator, specification))
        };
    }
    
    public static ISpecificationQuery<TResult> ApplySpecification<T, TResult>(this ISession session, ISpecification<T, TResult> specification,  ISpecificationEvaluator specificationEvaluator) 
        where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification => 
                new CriteriaSpecificationQuery<TResult>(() => session.ApplySpecification(criteriaSpecification)),
            QueryOverSpecification<T, TResult> queryOverSpecification =>
                new QueryOverSpecificationQuery<T, TResult>(() => session.ApplySpecification(queryOverSpecification)),
            HqlSpecification<T, TResult> hqlSpecification =>
                new HqlSpecificationQuery<TResult>(() => session.ApplySpecification(hqlSpecification)),
            _ =>
                new LinqSpecificationQuery<TResult>(() => session.ApplySpecification(specificationEvaluator, specification))
        };
    }
}

