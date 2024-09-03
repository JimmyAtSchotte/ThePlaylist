using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

[TestFixture]
public class RepositoryTests
{
    private Configuration _configuration;
    private Repository _repository;
    
    [OneTimeSetUp]
    public void SetupFixture()
    {
        _configuration = new Configuration();
        _configuration.DataBaseIntegration(db =>
        {
            db.ConnectionString = "Server=1337-JIMMY\\SQLEXPRESS;Database=Learn;Trusted_Connection=True;";
            db.Driver<SqlClientDriver>();
            db.Dialect<MsSql2012Dialect>();
            db.ConnectionProvider<DriverConnectionProvider>();
            db.LogSqlInConsole = true;
        });
        _configuration.AddAssembly(typeof (INamespacePlaceholder).Assembly);
        _repository = new Repository(_configuration);
    }

    [SetUp]
    public void Setup()
    {
        new SchemaExport(_configuration).Execute(true, true, false);
    }
}