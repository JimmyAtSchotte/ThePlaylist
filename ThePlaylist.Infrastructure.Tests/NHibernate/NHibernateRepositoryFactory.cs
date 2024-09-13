using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class NHibernateRepositoryFactory
{
    private const string SQL_EXPRESS_CONNECTION = "Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;";
    
    public static IRepository UseSqlLite()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlLite());
        var services = serviceCollection.BuildServiceProvider();
        
        var configuration = services.GetRequiredService<Configuration>();
        var session = services.GetRequiredService<ISession>();
        
        new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);
        return new Repository(session);
    }

    public static IRepository UseSqlExpress(string connectionString = SQL_EXPRESS_CONNECTION)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlExpress("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;"));
        var services = serviceCollection.BuildServiceProvider();
        
        var configuration = services.GetRequiredService<Configuration>();
        var session = services.GetRequiredService<ISession>();
        
        new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);
        return new Repository(session);
    }
}