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

    public async Task<T> GetAsync<T>(object id) where T : class
    {
        return (await context.Set<T>().FindAsync(id).EnsureEntityFound())!;
    }

    public T Get<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .FirstOrDefault()
            .EnsureEntityFound()!;
    }

    public async Task<T> GetAsync<T>(ISpecification<T> specification) where T : class
    {
        return (await _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .FirstOrDefaultAsync()
            .EnsureEntityFound())!;
    }

    public T Add<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() =>  context.Add(entity));
        return entity;
    }

    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await context.AddAsync(entity));
        return entity;
    }

    public T Update<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => context.Set<T>().Update(entity));
        
        return entity;
    }

    public async Task<T> UpdateAsync<T>(T entity) where T : class
    {
        
        await ExecuteUnitOfWorkAsync(async () => await Task.Run(() => context.Set<T>().Update(entity)));
        
        return entity;
    }

    public void Delete<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => context.Set<T>().Remove(entity));
    }

    public async Task DeleteAsync<T>(T entity) where T : class
    {
       await ExecuteUnitOfWorkAsync(async () => await Task.Run(() => context.Set<T>().Remove(entity)));
    }

    public IEnumerable<T> List<T>() where T : class
    {
        return context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> ListAsync<T>() where T : class
    {
        return await EntityFrameworkQueryableExtensions.ToListAsync(context.Set<T>());
    }

    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator.GetQuery(context.Set<T>().AsQueryable(), specification).ToList();
    }

    public async Task<IEnumerable<T>> ListAsync<T>(ISpecification<T> specification) where T : class
    {
        return await _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .ToListAsync();
    }

    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .ToList();
    }

    public async Task<IEnumerable<TResult>> ListAsync<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return await _specificationEvaluator
            .GetQuery(context.Set<T>().AsQueryable(), specification)
            .ToListAsync();
    }

    public void ExecuteUnitOfWork(Action<IRepository> action)
    {
        ExecuteUnitOfWork(() => action.Invoke(this));
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
    
    
    private async Task ExecuteUnitOfWorkAsync(Func<Task> action)
    {
        if (_unitOfWorkActive)
        {
            await action.Invoke();
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        _unitOfWorkActive = true;

        try
        {
            await action.Invoke();
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
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