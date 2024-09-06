using ThePlaylist.Core.Specification.Criterion;
using ICriterion = ThePlaylist.Core.Specification.Criterion.ICriterion;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace ThePlaylist.Infrastructure.EntityFramework.Specification;


public class SimpleEqualsFactory : ICriterionFactory
{
    public bool AppliesTo(ICriterion criterion) => criterion is SimpleCriterion { ComparisonType: ComparisonType.Equals };

    public ICriterionProvider Create(ICriterion criterion) => new SimpleEqualsProvider(criterion as SimpleCriterion);
}

public class SimpleEqualsProvider(SimpleCriterion? simpleCriterion) : ICriterionProvider
{
    public void ApplyTo<T>(IQueryable<T> queryable)
    {
        if (simpleCriterion == null)
            return;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, simpleCriterion.Property);
        var value = Expression.Constant(simpleCriterion.Value);

        Expression comparison = Expression.Equal(property, value);

        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        queryable = queryable.Where(lambda);
    }
}