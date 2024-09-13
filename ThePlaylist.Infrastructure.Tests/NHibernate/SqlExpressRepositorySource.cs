namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class SqlExpressRepositorySource()
    : BaseNHibernateRepositorySource("SQL Express", () => NHibernateRepositoryFactory.UseSqlExpress());