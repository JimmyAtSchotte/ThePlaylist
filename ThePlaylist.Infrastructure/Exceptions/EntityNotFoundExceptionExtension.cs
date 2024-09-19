namespace ThePlaylist.Infrastructure.Exceptions;

public static class EntityNotFoundExceptionExtension
{
    /// <summary>
    /// Returns entity if not null. If null: throws EntityNotFoundException
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public static T EnsureEntityFound<T>(this T entity)
    {
        return entity ?? throw new EntityNotFoundException();
    }
}