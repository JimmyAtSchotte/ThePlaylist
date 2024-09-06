using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public class RepositorySqlExpressTests : RepositoryTests
{
    private IRepository _repository;
    private ISession _session;

    protected override IRepository Repository => _repository;

    public override void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNHibernate(options => options.UseSqlExpress("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;"));
        var services = serviceCollection.BuildServiceProvider();
        
        var configuration = services.GetRequiredService<Configuration>();
        _session = services.GetRequiredService<ISession>();
        
        new SchemaExport(configuration).Execute(true, true, false, _session.Connection, null);
        _repository = new Repository(_session);
    }

    public override void TearDown()
    {
        _session.Close();
    }
}