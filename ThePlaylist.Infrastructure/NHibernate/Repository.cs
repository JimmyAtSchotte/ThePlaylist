using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using NHibernate;
using NHibernate.Criterion;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Core.Specification;
using ThePlaylist.Infrastructure.NHibernate.Specification;

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
        if (specification.HasCriteria())
        {
            var criteria = specification.Criteria();
            var query = _session.CreateCriteria<T>();
            var criteriaStrategy = new CriterionStrategy(new ICriterionFactory[]
            {
                new SimpleEqualsFactory()
            });

            foreach (var criterion in criteria)
                criteriaStrategy.Apply(query, criterion);

            return query.List<T>();
        }
        
        return _specificationEvaluator.GetQuery(_session.Query<T>().AsQueryable(), specification);

    }
}