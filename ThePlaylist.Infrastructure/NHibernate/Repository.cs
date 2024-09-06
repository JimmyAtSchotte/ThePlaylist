using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Core.Specification;
using ThePlaylist.Core.Specification.Criterion;
using Restrictions = NHibernate.Criterion.Restrictions;

namespace ThePlaylist.Infrastructure.NHibernate;

public class Repository(ISession session) : IRepository
{
    public T Add<T>(T entity) where T : class
    {
        using var transaction = session.BeginTransaction();
        session.Save(entity);
        transaction.Commit();
        
        session.Clear();
        
        return entity;
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return session.Query<T>().ToList();
    }

    public void Delete<T>(T entity) where T : class
    {
        using var transaction = session.BeginTransaction();
        session.Delete(entity);
        transaction.Commit();
        session.Clear();
    }
    
    public T Update<T>(T entity) where T : class
    {
        using var transaction = session.BeginTransaction();
        session.Update(entity);
        transaction.Commit();
        
        session.Clear();
        
        return entity;
    }
    
    public T Get<T>(object id) where T : class
    {
        return session.Get<T>(id);
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        var query = session.CreateCriteria<T>();
        
        foreach (var criteria in specification.Criterias)
        {
            if (criteria is SimpleCriterion simpleCriteria)
            {
                switch (simpleCriteria.ComparisonType)
                {
                    case ComparisonType.Equals:
                        query.Add(Restrictions.Eq(simpleCriteria.Property, simpleCriteria.Value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
        }

        return query.List<T>();

    }
}