namespace ThePlaylist.Core.Specification.Criterion;

public class Restrictions
{
    public static SimpleCriterion Eq(string property, object value) => new SimpleCriterion(property, ComparisonType.Equals, value);
}