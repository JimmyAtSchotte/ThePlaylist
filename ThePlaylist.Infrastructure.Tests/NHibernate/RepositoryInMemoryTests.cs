using ThePlaylist.Infrastructure.Tests.NHibernate.DatabaseConfigurations;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class RepositoryInMemoryTests : RepositoryTests
{
    protected override IDatabaseConfiguration DatabaseConfiguration => InMemoryDatabaseConfiguration.Instance;
}