using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Hql.Ast;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

public class NHibernateSqlExpressRepositorySource : IRepositorySource
{
    private IServiceProvider? _services;
    private IServiceProvider Services => _services ??= Initialize();

    private static IServiceProvider Initialize()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlExpress("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;"));
        serviceCollection.AddTransient<IRepository, Infrastructure.NHibernate.Repository>();
        
        var services = serviceCollection.BuildServiceProvider();
        var configuration = services.GetRequiredService<Configuration>();
        var session = services.GetRequiredService<ISession>();
        new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);

        return services;
    }

    public IRepository CreateRepository()
    {
        return Services.GetRequiredService<IRepository>();
    }
}