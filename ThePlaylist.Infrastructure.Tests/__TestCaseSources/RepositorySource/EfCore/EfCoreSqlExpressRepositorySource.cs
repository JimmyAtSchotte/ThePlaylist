namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.EfCore;

public class EfCoreSqlExpressRepositorySource()
    : RepositorySource(() => EfCoreRepositoryFactory.UseSqlExpress());