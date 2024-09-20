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

    public Task<T> GetAsync<T>(object id) where T : class
    {
        return session.GetAsync<T>(id).EnsureEntityFound();
    }
    
    public T Get<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator
            .GetQuery(session.Query<T>().AsQueryable(), specification)
            .FirstOrDefault()
            .EnsureEntityFound()!;
    }

    public Task<T> GetAsync<T>(ISpecification<T> specification) where T : class
    {
        return _specificationEvaluator
            .GetQuery(session.Query<T>().AsQueryable(), specification)
            .FirstOrDefaultAsync()
            .EnsureEntityFound()!;
    }

    public T Add<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => session.Save(entity));
        return entity;
    }

    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await session.SaveAsync(entity));
        return entity;
    }

    public T Update<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => session.Update(entity));
        return entity;
    }

    public async Task<T> UpdateAsync<T>(T entity) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await session.UpdateAsync(entity));
        return entity;
    }

    public void Delete<T>(T entity) where T : class
    {
        ExecuteUnitOfWork(() => session.Delete(entity));
    }

    public async Task DeleteAsync<T>(T entity) where T : class
    {
        await ExecuteUnitOfWorkAsync(async () => await session.DeleteAsync(entity));

    }

    public IEnumerable<T> List<T>() where T : class
    {
        return session.Query<T>().ToList();
    }

    public async Task<IEnumerable<T>> ListAsync<T>() where T : class
    {
        return await session.Query<T>().ToListAsync();
    }



    public IEnumerable<T> List<T>(ISpecification<T> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification =>
                session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .List<T>(),

            QueryOverSpecification<T> queryOverSpecification =>
                session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .List<T>(),

            HqlSpecification<T> hqlSpecification =>
                session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .List<T>(),

            _ => _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToList()
        };
    }

    public async Task<IEnumerable<T>> ListAsync<T>(ISpecification<T> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T> criteriaSpecification =>
                await session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .ListAsync<T>(),

            QueryOverSpecification<T> queryOverSpecification =>
                await session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .ListAsync<T>(),

            HqlSpecification<T> hqlSpecification =>
                await session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .ListAsync<T>(),

            _ => await _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToListAsync()
        };
    }

    public IEnumerable<TResult> List<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification =>
                session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .List<TResult>(),

            QueryOverSpecification<T, TResult> queryOverSpecification =>
                session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .List<TResult>(),

            HqlSpecification<T, TResult> hqlSpecification =>
                session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .List<TResult>(),

            _ => _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToList()
        };
    }

    public async Task<IEnumerable<TResult>> ListAsync<T, TResult>(ISpecification<T, TResult> specification) where T : class
    {
        return specification switch
        {
            CriteriaSpecification<T, TResult> criteriaSpecification =>
                await session.CreateCriteria<T>()
                    .Apply(query => criteriaSpecification.GetCriteria().Invoke(query))
                    .ListAsync<TResult>(),

            QueryOverSpecification<T, TResult> queryOverSpecification =>
                await session.QueryOver<T>()
                    .Apply(queryOver => queryOverSpecification.GetQueryOver().Invoke(queryOver))
                    .ListAsync<TResult>(),

            HqlSpecification<T, TResult> hqlSpecification =>
                await session.Apply(hql => hqlSpecification.GetHql().Invoke(hql))
                    .ListAsync<TResult>(),

            _ => await _specificationEvaluator.GetQuery(session.Query<T>().AsQueryable(), specification).ToListAsync()
        };
    }

    public void ExecuteUnitOfWork(Action<IRepository> action)
    {
        ExecuteUnitOfWork(() => action.Invoke(this));
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
    
    private async Task ExecuteUnitOfWorkAsync(Func<Task> action)
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
            await transaction.CommitAsync();
            await session.FlushAsync();
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
}