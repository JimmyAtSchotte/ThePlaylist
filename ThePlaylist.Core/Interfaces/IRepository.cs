using ThePlaylist.Core.Entitites;
using ThePlaylist.Core.Specification;

namespace ThePlaylist.Core.Interfaces;

public interface IRepository
{
    T Add<T>(T entity) where T : class;
    IEnumerable<T> List<T>() where T : class;
    void Delete<T>(T entity) where T : class;
    T Update<T>(T entity) where T : class;
    T Get<T>(object id) where T : class;
    IEnumerable<T> List<T>(ISpecification<T> specification) where T : class;
}