namespace ThePlaylist.Core.Specification.Criterion;

public class Restrictions
{
    public static SimpleCriteria Eq(string property, object value) => new SimpleCriteria(property, ComparisonType.Equals, value);
}