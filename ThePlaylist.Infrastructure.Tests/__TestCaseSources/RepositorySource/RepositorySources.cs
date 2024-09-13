using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.EfCore;
using ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

public class RepositorySources
{
    public static IEnumerable<BaseRepositorySource> RepositoryProviders => new BaseRepositorySource[]
    { 
        new NHibernateSqlExpressRepositorySource(),
        new NHibernateSqlLiteRepositorySource(),
        new EfCoreSqlExpressRepositorySource()
    }; 
    
    public static IEnumerable<BaseRepositorySource> NHibernateOnlyRepositoryProviders => new BaseRepositorySource[]
    { 
        new NHibernateSqlExpressRepositorySource(),
        new NHibernateSqlLiteRepositorySource()
    }; 
}