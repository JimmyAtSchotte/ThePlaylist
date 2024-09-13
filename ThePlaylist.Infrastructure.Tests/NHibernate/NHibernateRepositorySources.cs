namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class NHibernateRepositorySources
{
    public static IEnumerable<BaseNHibernateRepositorySource> RepositoryProviders => new BaseNHibernateRepositorySource[]
    { 
        new SqlExpressRepositorySource(),
        new SqlLiteRepositorySource()
    }; 
}