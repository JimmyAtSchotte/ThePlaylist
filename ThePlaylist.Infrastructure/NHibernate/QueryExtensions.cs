namespace ThePlaylist.Infrastructure.NHibernate;

public static class QueryExtensions
{
    public static T Apply<T>(this T obj, Action<T> action)
    {
        action(obj);
        return obj;
    }
    
    public static TResult Apply<T, TResult>(this T obj, Func<T, TResult> func)
    {
        return func(obj);;
    }
}