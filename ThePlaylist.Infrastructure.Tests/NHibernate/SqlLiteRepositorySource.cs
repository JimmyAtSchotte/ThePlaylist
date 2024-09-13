namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class SqlLiteRepositorySource()
    : BaseNHibernateRepositorySource("SQL Lite", () => NHibernateRepositoryFactory.UseSqlLite());