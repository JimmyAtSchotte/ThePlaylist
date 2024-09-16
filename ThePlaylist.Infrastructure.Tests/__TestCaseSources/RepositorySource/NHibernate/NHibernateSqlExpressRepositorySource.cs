namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

public class NHibernateSqlExpressRepositorySource()
    : RepositorySource(() => NHibernateRepositoryFactory.UseSqlExpress());