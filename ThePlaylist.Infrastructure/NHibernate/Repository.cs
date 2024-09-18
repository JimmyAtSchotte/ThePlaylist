using Ardalis.Specification;
using NHibernate;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.NHibernate;

public class Repository : IRepository
{
    private readonly ISession _session;
    private readonly ISpecificationEvaluator _specificationEvaluator;
    
    public Repository(ISession session)
    {
        _session = session;
        _specificationEvaluator = new LinqToQuerySpecificationEvaluator();
    }
    
    public T Add<T>(T entity) where T : class
    {
        using var transaction = _session.BeginTransaction();
        _session.Save(entity);
        transaction.Commit();
        
        _session.Clear();
        
        return entity;
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return _session.Query<T>().ToList();
    }

    public void Delete<T>(T entity) where T : class
    {
        using var transaction = _session.BeginTransaction();
        _session.Delete(entity);
        transaction.Commit();
        _session.Clear();
    }
    
    public T Update<T>(T entity) where T : class
    {
        using var transaction = _session.BeginTransaction();
        _session.Update(entity);
        transaction.Commit();
        
        _session.Clear();
        
        return entity;
    }
    
    public T Get<T>(object id) where T : class
    {
        return _session.Get<T>(id);
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification => 
                _session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .List<T>(),

            QueryOverSpecification<T> queryOverSpecification => 
                _session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .List<T>(),
            
            HqlSpecification<T> hqlSpecification => 
                _session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .List<T>(),

            _ => _specificationEvaluator.GetQuery(_session.Query<T>().AsQueryable(), specification).ToList()
        };
    }
    
    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
    
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification => 
                _session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .List<TResult>(),

            QueryOverSpecification<T, TResult> queryOverSpecification =>
                _session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .List<TResult>(),
            
            HqlSpecification<T, TResult> hqlSpecification => 
                _session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .List<TResult>(),
            
            _ => _specificationEvaluator.GetQuery(_session.Query<T>().AsQueryable(), specification).ToList()
        };
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}