using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.EfCore;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

public class RepositorySources
{
    public static IEnumerable<RepositorySource> RepositoryProviders => new RepositorySource[]
    { 
        new NHibernateSqlExpressRepositorySource(),
        new NHibernateSqlLiteRepositorySource(),
        new EfCoreSqlExpressRepositorySource()
    }; 
    
    public static IEnumerable<RepositorySource> NHibernateOnlyRepositoryProviders => new RepositorySource[]
    { 
        new NHibernateSqlExpressRepositorySource(),
        new NHibernateSqlLiteRepositorySource()
    }; 
}