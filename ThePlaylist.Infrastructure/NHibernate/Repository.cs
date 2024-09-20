using Ardalis.Specification;
using NHibernate;
using NHibernate.Linq;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.Exceptions;
using ThePlaylist.Specifications;

namespace ThePlaylist.Infrastructure.NHibernate;

public class Repository(ISession session) : IRepository
{
    private readonly LinqToQuerySpecificationEvaluator _specificationEvaluator =
        LinqToQuerySpecificationEvaluator.Default;

    private bool _unitOfWorkActive;

    public T Get<T>(object id) where T : class
    {
        return session.Get<T>(id).EnsureEntityFound();
    }

    public Task<T> GetAsync<T>(object id, CancellationToken cancellationToken) where T : class
    {
        return session.GetAsync<T>(id, cancellationToken).EnsureEntityFound();
    }
    
    public T Get<T>(ISpecification<T> specification) where T : class
    {
        return session.ApplySpecification(_specificationEvaluator, specification)
            .FirstOrDefault()
            .EnsureEntityFound()!;
    }

    public Task<T> GetAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : class
    {
        return session.ApplySpecification(_specificationEvaluator, specification)
            .FirstOrDefaultAsync(cancellationToken)
            .EnsureEntityFound()!;
    }

    public T Add<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => session.Save(entity));
        return entity;
    }

    public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await session.SaveAsync(entity, cancellationToken), cancellationToken);
        return entity;
    }

    public T Update<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => session.Update(entity));
        return entity;
    }

    public async Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await session.UpdateAsync(entity, cancellationToken), cancellationToken);
        return entity;
    }

    public void Delete<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => session.Delete(entity));
    }

    public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await session.DeleteAsync(entity, cancellationToken), cancellationToken);

    }

    public IEnumerable<T> List<T>() where T : class
    {
        return session.Query<T>().ToList();
    }

    public async Task<IEnumerable<T>> ListAsync<T>(CancellationToken cancellationToken) where T : class
    {
        return await session.Query<T>().ToListAsync(cancellationToken);
    }



    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification => 
                session.ApplySpecification(criteriaSpecification).List<T>(),
            QueryOverSpecification<T> queryOverSpecification =>
                session.ApplySpecification(queryOverSpecification).List<T>(),
            HqlSpecification<T> hqlSpecification =>
                session.ApplySpecification(hqlSpecification).List<T>(),
            _ =>
                session.ApplySpecification(_specificationEvaluator, specification).ToList()
        };
    }

    public async Task<IEnumerable<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification => 
                await session.ApplySpecification(criteriaSpecification).ListAsync<T>(cancellationToken),
            QueryOverSpecification<T> queryOverSpecification =>
                await session.ApplySpecification(queryOverSpecification).ListAsync<T>(cancellationToken),
            HqlSpecification<T> hqlSpecification =>
                await session.ApplySpecification(hqlSpecification).ListAsync<T>(cancellationToken),
            _ => 
                await session.ApplySpecification(_specificationEvaluator, specification).ToListAsync(cancellationToken)
        };
    }

    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification => 
                session.ApplySpecification(criteriaSpecification).List<TResult>(),
            QueryOverSpecification<T, TResult> queryOverSpecification =>
                session.ApplySpecification(queryOverSpecification).List<TResult>(),
            HqlSpecification<T, TResult> hqlSpecification =>
                session.ApplySpecification(hqlSpecification).List<TResult>(),
            _ => 
                session.ApplySpecification(_specificationEvaluator, specification).ToList()
        };
    }

    public async Task<IEnumerable<TResult>> ListAsync<T, TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification => 
                await session.ApplySpecification(criteriaSpecification).ListAsync<TResult>(cancellationToken),
            QueryOverSpecification<T, TResult> queryOverSpecification =>
                await session.ApplySpecification(queryOverSpecification).ListAsync<TResult>(cancellationToken),
            HqlSpecification<T, TResult> hqlSpecification =>
                await session.ApplySpecification(hqlSpecification).ListAsync<TResult>(cancellationToken),
           _ => 
               await session.ApplySpecification(_specificationEvaluator, specification).ToListAsync(cancellationToken)
        };
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
        session.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (session is IAsyncDisposable sessionAsyncDisposable)
            await sessionAsyncDisposable.DisposeAsync();
        else
            session.Dispose();
    }

    private void ExecuteUnitOfWork(Action action)
    {
        if (_unitOfWorkActive)
        {
            action.Invoke();
            return;
        }

        using var transaction = session.BeginTransaction();

        try
        {
            _unitOfWorkActive = true;

            action.Invoke();
            transaction.Commit();
            session.Flush();
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

        using var transaction = session.BeginTransaction();

        try
        {
            _unitOfWorkActive = true;

            await action.Invoke();
            await transaction.CommitAsync(cancellationToken);
            await session.FlushAsync(cancellationToken);
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
}