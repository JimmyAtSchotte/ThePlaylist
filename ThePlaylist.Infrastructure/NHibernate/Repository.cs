using Ardalis.Specification;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Core.Specification;
using ThePlaylist.Specifications;
using ThePlaylist.Specifications.Track;

namespace ThePlaylist.Infrastructure.NHibernate;

public class Repository : IRepository
{
    private readonly ISession _session;
    private readonly ISpecificationEvaluator _specificationEvaluator;
    
    public Repository(ISession session)
    {
        _session = session;
        _specificationEvaluator = new LinqQuerySpecificationEvaluator();
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
       // var s1 = _session.Query<Playlist>().ToList();
        var s2 = _session.Query<Playlist>().FetchMany(x => x.Tracks).ToList();
        var s3 = _session.Query<Playlist>().FetchMany(x => x.Tracks).ThenFetchMany(x => x.Genres).ToList();
        //
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
            
            _ => _specificationEvaluator.GetQuery(_session.Query<T>().AsQueryable(), specification).ToList()
        };
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}