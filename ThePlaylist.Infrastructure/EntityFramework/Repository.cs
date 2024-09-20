using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.Exceptions;

namespace ThePlaylist.Infrastructure.EntityFramework;

public class Repository(Context context) : IRepository
{
    private readonly SpecificationEvaluator _specificationEvaluator = SpecificationEvaluator.Default;
    private bool _unitOfWorkActive = false;

    public T Get<T>(object id) where T : class
    {
        return context.Set<T>().Find(id).EnsureEntityFound()!;
    }

    public async Task<T> GetAsync<T>(object id, CancellationToken cancellationToken) where T : class
    {
        return (await context.Set<T>().FindAsync([id], cancellationToken).EnsureEntityFound())!;
    }

    public T Get<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .FirstOrDefault()
            .EnsureEntityFound()!;
    }

    public async Task<T> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : class
    {
        return (await _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .FirstOrDefaultAsync(cancellationToken)
            .EnsureEntityFound())!;
    }

    public T Add<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() =>  context.Add(entity));
        return entity;
    }

    public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await context.AddAsync(entity, cancellationToken), cancellationToken);
        return entity;
    }

    public T Update<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => context.Set<T>().Update(entity));
        
        return entity;
    }

    public async Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        
        await ExecuteUnitOfWorkAsync(async () => await Task.Run(() => context.Set<T>().Update(entity), cancellationToken), cancellationToken);
        
        return entity;
    }

    public void Delete<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => context.Set<T>().Remove(entity));
    }

    public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
       await ExecuteUnitOfWorkAsync(async () => await Task.Run(() => context.Set<T>().Remove(entity), cancellationToken), cancellationToken);
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> ListAsync<T>(CancellationToken cancellationToken) where T : class
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator.GetQuery(context.Set<T>().AsQueryable(), specification).ToList();
    }

    public async Task<IEnumerable<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : class
    {
        return await _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .ToListAsync(cancellationToken);
    }

    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .ToList();
    }

    public async Task<IEnumerable<TResult>> ListAsync<T, TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken) where T : class
    {
        return await _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .ToListAsync(cancellationToken);
    }

    public void ExecuteUnitOfWork(Action<IRepository> action)
    {
        ExecuteUnitOfWork(() => action.Invoke(this));
    }
    

    public async Task ExecuteUnitOfWorkAsync(Func<IRepository, Task> action, CancellationToken cancellationToken)
    {
        await ExecuteUnitOfWorkAsync(async () => await action.Invoke(this), cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }

    private void ExecuteUnitOfWork(Action action)
    {
        if (_unitOfWorkActive)
        {
            action.Invoke();
            return;
        }
        
        using var transaction = context.Database.BeginTransaction();
        _unitOfWorkActive = true;

        try
        {
            action.Invoke();
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
    
    
    private async Task ExecuteUnitOfWorkAsync(Func<Task> action, CancellationToken cancellationToken)
    {
        if (_unitOfWorkActive)
        {
            await action.Invoke();
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        _unitOfWorkActive = true;

        try
        {
            await action.Invoke();
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            _unitOfWorkActive = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}