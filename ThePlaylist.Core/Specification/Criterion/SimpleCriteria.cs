namespace ThePlaylist.Core.Specification.Criterion;

public interface ICriterion
{
    
}

public class SimpleCriterion(string property, ComparisonType comparisonType, object value) : ICriterion
{
    public string Property { get; } = property;
    public ComparisonType ComparisonType { get; } = comparisonType;
    public object Value { get; } = value;
}