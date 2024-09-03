using NHibernate;
using NHibernate.Cfg;
using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.Infrastructure.NHibernate;

public class Repository : IRepository
{
    private readonly ISession _session;
    
    public Repository(Configuration configuration)
    {
        var sessionFactory = configuration.BuildSessionFactory();
        _session = sessionFactory.OpenSession();
    }
}