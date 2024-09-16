using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Specification;
using NHibernate.Linq;

namespace ThePlaylist.Infrastructure.NHibernate.Evaluators;

public class FetchEvaluator : IEvaluator
{
   private static readonly MethodInfo FetchMethodInfo = typeof(EagerFetchingExtensionMethods)
        .GetTypeInfo().GetDeclaredMethods(nameof(EagerFetchingExtensionMethods.Fetch))
        .Single();
   
   private static readonly MethodInfo FetchManyMethodInfo = typeof(EagerFetchingExtensionMethods)
       .GetTypeInfo().GetDeclaredMethods(nameof(EagerFetchingExtensionMethods.FetchMany))
       .Single();
    
   private static readonly MethodInfo ThenFetchMethodInfo
       = typeof(EagerFetchingExtensionMethods)
           .GetTypeInfo().GetDeclaredMethods(nameof(EagerFetchingExtensionMethods.ThenFetch))
           .Single();
   
   private static readonly MethodInfo ThenFetchManyMethodInfo
       = typeof(EagerFetchingExtensionMethods)
           .GetTypeInfo().GetDeclaredMethods(nameof(EagerFetchingExtensionMethods.ThenFetchMany))
           .Single();
   
    public static FetchEvaluator Instance { get; } = new FetchEvaluator();
    
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        foreach (var includeInfo in specification.IncludeExpressions)
        {
            query = includeInfo.Type switch
            {
                IncludeTypeEnum.Include => BuildInclude<T>(query, includeInfo),
                IncludeTypeEnum.ThenInclude => BuildThenInclude<T>(query, includeInfo),
                _ => query
            };
        }

        return query;
    }

    public bool IsCriteriaEvaluator { get; } = false;
    
    private IQueryable<T> BuildInclude<T>(IQueryable query, IncludeExpressionInfo includeInfo)
    {
        _ = includeInfo ?? throw new ArgumentNullException(nameof(includeInfo));

        var methodInfo = (IsGenericEnumerable(includeInfo.PropertyType, out var propertyType)
            ? FetchManyMethodInfo 
            : FetchMethodInfo);
            
       var method = methodInfo.MakeGenericMethod(includeInfo.EntityType, propertyType);
           
       var result = method.Invoke(null, new object[] { query, includeInfo.LambdaExpression });
        _ = result ?? throw new TargetException();

        return (IQueryable<T>)result;
    }

    private IQueryable<T> BuildThenInclude<T>(IQueryable query, IncludeExpressionInfo includeInfo)
    {
        _ = includeInfo ?? throw new ArgumentNullException(nameof(includeInfo));
        _ = includeInfo.PreviousPropertyType ?? throw new ArgumentNullException(nameof(includeInfo.PreviousPropertyType));

        var method = (IsGenericEnumerable(includeInfo.PreviousPropertyType, out var previousPropertyType)
            ? ThenFetchManyMethodInfo
            : ThenFetchMethodInfo);

        IsGenericEnumerable(includeInfo.PropertyType, out var propertyType);

        var result = method.MakeGenericMethod(includeInfo.EntityType, previousPropertyType, propertyType)
            .Invoke(null, new object[] { query, includeInfo.LambdaExpression });

        _ = result ?? throw new TargetException();

        return (IQueryable<T>)result;
    }
    
    private static bool IsGenericEnumerable(Type type, out Type propertyType)
    {
        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
        {
            propertyType = type.GenericTypeArguments[0];

            return true;
        }

        propertyType = type;

        return false;
    }
}