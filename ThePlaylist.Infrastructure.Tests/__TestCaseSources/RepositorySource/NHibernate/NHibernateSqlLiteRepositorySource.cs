using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.NHibernate;

public class NHibernateSqlLiteRepositorySource : IRepositorySource
{
    private bool _initialized;

    private IRepository _repository;
    private ISession _session;

    private void Initialize()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlLite());
        serviceCollection.AddTransient<IRepository, Infrastructure.NHibernate.Repository>();
        
        var services = serviceCollection.BuildServiceProvider();
        var configuration = services.GetRequiredService<Configuration>();
        _session = services.GetRequiredService<ISession>();
        _repository = services.GetRequiredService<IRepository>();
        
        new SchemaExport(configuration).Execute(true, true, false, _session.Connection, null);
        
        _initialized = true;
    }

    public IRepository CreateRepository()
    {
        if(!_initialized)
            Initialize();
        
        _session.Clear();
        
        return _repository;
    }
}