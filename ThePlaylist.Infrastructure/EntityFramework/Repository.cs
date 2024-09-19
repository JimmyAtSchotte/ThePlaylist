using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.Exceptions;

namespace ThePlaylist.Infrastructure.EntityFramework;

public class Repository(Context context) : IRepository, IAsyncDisposable
{
    private readonly SpecificationEvaluator _specificationEvaluator = SpecificationEvaluator.Default;
    private bool _unitOfWorkActive = false;
    
    public T Add<T>(T entity) where T : class
    {
        if(_unitOfWorkActive)
            context.Add(entity);
        else
            ExecuteUnitOfWork(_ => Add(entity));
        
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

    public void ExecuteUnitOfWork(Action<IRepository> action)
    {
        if (_unitOfWorkActive)
        {
            action.Invoke(this);
            return;
        }
        
        using var transaction = context.Database.BeginTransaction();
        _unitOfWorkActive = true;

        try
        {
            action.Invoke(this);
            context.SaveChanges();
            transaction.Commit();
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
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}