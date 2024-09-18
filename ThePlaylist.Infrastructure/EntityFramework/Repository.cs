using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.Exceptions;

namespace ThePlaylist.Infrastructure.EntityFramework;

public class Repository(Context context) : IRepository, IAsyncDisposable
{
    private readonly SpecificationEvaluator _specificationEvaluator = SpecificationEvaluator.Default;

    public T Add<T>(T entity) where T : class
    {
        context.Add(entity);
        context.SaveChanges();
        return entity;
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return context.Set<T>().ToList();
    }

    public void Delete<T>(T entity) where T : class
    {
        context.Set<T>().Remove(entity);
        context.SaveChanges();
        
    }

    public T Update<T>(T entity) where T : class
    {
        context.Set<T>().Update(entity);
        context.SaveChanges();
        return entity;
    }

    public T Get<T>(object id) where T : class
    {
        var entity = context.Set<T>().Find(id);

        if (entity is null)
            throw new EntityNotFoundException();

        return entity;
    }

    public T Get<T>(ISpecification<T> specification) where T : class
    {
        var entity = _specificationEvaluator.GetQuery(context.Set<T>().AsQueryable(), specification).FirstOrDefault();

        if (entity is null)
            throw new EntityNotFoundException();

        return entity;
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator.GetQuery(context.Set<T>().AsQueryable(), specification);
    }

    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return _specificationEvaluator.GetQuery(context.Set<T>().AsQueryable(), specification);
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}