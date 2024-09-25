using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.EfCore;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

public class RepositorySources
{
    public static IEnumerable<IRepositorySource> RepositoryProviders => new IRepositorySource[]
    { 
        new NHibernateSqlExpressRepositorySource(),
        new NHibernateSqlLiteRepositorySource(),
        new EfCoreSqlExpressRepositorySource()
    }; 
    
    public static IEnumerable<IRepositorySource> NHibernateOnlyRepositoryProviders => new IRepositorySource[]
    { 
        new NHibernateSqlExpressRepositorySource(),
        new NHibernateSqlLiteRepositorySource()
    }; 
}