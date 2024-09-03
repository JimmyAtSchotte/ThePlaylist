using ThePlaylist.Infrastructure.Tests.NHibernate.DatabaseConfigurations;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class RepositorySqlExpressTests : RepositoryTests
{
    protected override IDatabaseConfiguration DatabaseConfiguration => SqlExpressDatabaseConfiguration.Instance;
}