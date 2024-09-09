using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Specification;
using NHibernate.Linq;

namespace ThePlaylist.Infrastructure.NHibernate.Evaluators;

public class FetchEvaluator : IEvaluator
{
   private static readonly MethodInfo IncludeMethodInfo = typeof(EagerFetchingExtensionMethods)
        .GetTypeInfo().GetDeclaredMethods(nameof(EagerFetchingExtensionMethods.Fetch))
        .Single(mi => mi.GetGenericArguments().Length == 2
                      && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                      && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));
    
    
    public static FetchEvaluator Instance { get; } = new FetchEvaluator();
    
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        foreach (var includeInfo in specification.IncludeExpressions)
        {
            var result = IncludeMethodInfo.MakeGenericMethod(includeInfo.EntityType, includeInfo.PropertyType).Invoke(null, new object[] { query, includeInfo.LambdaExpression });

            _ = result ?? throw new TargetException();

            return (IQueryable<T>)result;
        }

        return query;
    }

    public bool IsCriteriaEvaluator { get; } = false;
}