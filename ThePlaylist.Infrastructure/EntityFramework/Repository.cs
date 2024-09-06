using NHibernate.Linq;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Core.Specification;

namespace ThePlaylist.Infrastructure.EntityFramework;

public class Repository(Context db) : IRepository
{
    public T Add<T>(T entity) where T : class
    {
        db.Add(entity);
        db.SaveChanges();
        return entity;
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return db.Set<T>().ToList();
    }

    public void Delete<T>(T entity) where T : class
    {
        db.Set<T>().Remove(entity);
        db.SaveChanges();
        
    }

    public T Update<T>(T entity) where T : class
    {
        db.Set<T>().Update(entity);
        db.SaveChanges();
        return entity;
    }

    public T Get<T>(object id) where T : class
    {
        return db.Set<T>().Find(id);
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return db.Set<T>().Query(specification);
    }
}