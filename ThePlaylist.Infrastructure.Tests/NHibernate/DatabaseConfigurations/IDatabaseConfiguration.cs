using NHibernate;
using NHibernate.Cfg;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.DatabaseConfigurations;

public interface IDatabaseConfiguration
{
    public ISessionFactory SessionFactory { get; set; }
    public Configuration Configuration { get; set; }
}