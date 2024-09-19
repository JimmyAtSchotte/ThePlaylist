using Ardalis.Specification;
using NHibernate;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.Exceptions;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.NHibernate;

public class Repository(ISession session) : IRepository
{
    private readonly LinqToQuerySpecificationEvaluator _specificationEvaluator = LinqToQuerySpecificationEvaluator.Default;
    private bool _unitOfWorkActive = false;
    
    public T Add<T>(T entity) where T : class
    {
        if(_unitOfWorkActive)
            session.Save(entity);
        else
            ExecuteUnitOfWork(_ => session.Save(entity));
        
        return entity;
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return session.Query<T>().ToList();
    }

    public void Delete<T>(T entity) where T : class
    {
        using var transaction = session.BeginTransaction();
        try
        {
            session.Delete(entity);
            session.Flush();
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
        using var transaction = session.BeginTransaction();
        
        try
        {
            session.Update(entity);
            session.Flush();
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
        var entity = session.Get<T>(id);

        if (entity is null)
            throw new EntityNotFoundException();

        return entity;
    }

    public T Get<T>(ISpecification<T> specification) where T : class
    {
       var entity = _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToList().FirstOrDefault();

       if (entity is null)
           throw new EntityNotFoundException();

       return entity;
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification => 
                session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .List<T>(),

            QueryOverSpecification<T> queryOverSpecification => 
                session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .List<T>(),
            
            HqlSpecification<T> hqlSpecification => 
                session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .List<T>(),

            _ => _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToList()
        };
    }
    
    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
    
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification => 
                session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .List<TResult>(),

            QueryOverSpecification<T, TResult> queryOverSpecification =>
                session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .List<TResult>(),
            
            HqlSpecification<T, TResult> hqlSpecification => 
                session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .List<TResult>(),
            
            _ => _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToList()
        };
    }

    public void ExecuteUnitOfWork(Action<IRepository> action)
    {
        if (_unitOfWorkActive)
        {
            action.Invoke(this);
            return;
        }
        
        using var transaction = session.BeginTransaction();

        try
        {
            _unitOfWorkActive = true;
            
            action.Invoke(this);
            transaction.Commit();
            session.Flush();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            _unitOfWorkActive = false;
        }
    }

    public void Dispose()
    {
        session.Dispose();
    }
}