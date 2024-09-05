using NHibernate;
using NHibernate.Cfg;
using ThePlaylist.Core.Interfaces;

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
}