using System.Linq.Expressions;
using Ardalis.Specification;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;

public static class SpecificationBuilderExtensions
{
    public static SpecificationBuilder<T> Where<T>(this SpecificationBuilder<T> specificationBuilder,
        ICriterion criterion)
    {
        specificationBuilder.Specification.AddCriterion(criterion);
        return specificationBuilder;
    }


    public static SpecificationBuilder<T> Where<T>(this SpecificationBuilder<T> specificationBuilder,
        Expression<Func<T, bool>> expression)
    {
        specificationBuilder.Specification.Query.Where(expression);
        return specificationBuilder;
    }
}