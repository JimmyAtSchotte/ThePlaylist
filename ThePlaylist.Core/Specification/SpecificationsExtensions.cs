using Ardalis.Specification;
using ThePlaylist.Core.Specification.Criterion;

namespace ThePlaylist.Core.Specification;

public static class SpecificationsExtensions
{
    private const string CRITERIA_KEY  = "Critteria";
    
    public static IList<ICriterion> Criteria<T>(this ISpecification<T> specification)
    {
        if(specification.Items.TryGetValue(CRITERIA_KEY, out var item))
            return item as IList<ICriterion> ?? new List<ICriterion>();
        
        var criteria  = new List<ICriterion>();
        specification.Items.Add(CRITERIA_KEY, criteria);
        return criteria;
    }
    
    public static void AddCriterion<T>(this ISpecification<T> specification, ICriterion criterion)
    {
        specification.Criteria().Add(criterion);
    }
    
    
    public static bool HasCriteria<T>(this ISpecification<T> specification)
    {
        return specification.Criteria().Any();
    }
    
    
    
}