using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.Infrastructure.EntityFramework;

public class Repository : IRepository, IAsyncDisposable
{
    private readonly Context db;
    private readonly ISpecificationEvaluator _specificationEvaluator;
    
    public Repository(Context context)
    {
        db = context;
        _specificationEvaluator = new SpecificationEvaluator(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            Ardalis.Specification.EntityFrameworkCore.SearchEvaluator.Instance,
            IncludeEvaluator.Default,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance,
            AsTrackingEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            AsSplitQueryEvaluator.Instance
        });
    }
    
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
        return _specificationEvaluator.GetQuery(db.Set<T>().AsQueryable(), specification);
    }

    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return _specificationEvaluator.GetQuery(db.Set<T>().AsQueryable(), specification);
    }

    public void Dispose()
    {
        db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await db.DisposeAsync();
    }
}