namespace ThePlaylist.Core.Specification.Criterion;

public interface ICriteria
{
    
}

public class SimpleCriteria(string property, ComparisonType comparisonType, object value) : ICriteria
{
    public string Property { get; } = property;
    public ComparisonType ComparisonType { get; } = comparisonType;
    public object Value { get; } = value;
}