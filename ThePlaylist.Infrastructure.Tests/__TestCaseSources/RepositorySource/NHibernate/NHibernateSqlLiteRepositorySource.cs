namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

public class NHibernateSqlLiteRepositorySource()
    : BaseRepositorySource(() => NHibernateRepositoryFactory.UseSqlLite());