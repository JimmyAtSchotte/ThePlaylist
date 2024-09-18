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

        try
        {
            _session.Save(entity);
            _session.Flush();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }

        return entity;
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return _session.Query<T>().ToList();
    }

    public void Delete<T>(T entity) where T : class
    {
        using var transaction = _session.BeginTransaction();
        try
        {
            _session.Delete(entity);
            _session.Flush();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public T Update<T>(T entity) where T : class
    {
        using var transaction = _session.BeginTransaction();
        
        try
        {
            _session.Update(entity);
            _session.Flush();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        
        return entity;
    }
    
    public T Get<T>(object id) where T : class
    {
        return _session.Get<T>(id);
    }

    public T? Get<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator.GetQuery(_session.Query<T>().AsQueryable(), specification).ToList().FirstOrDefault();
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