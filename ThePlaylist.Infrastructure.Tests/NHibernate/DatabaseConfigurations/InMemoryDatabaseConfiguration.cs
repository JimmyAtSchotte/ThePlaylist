using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.DatabaseConfigurations;



    public sealed class InMemoryDatabaseConfiguration : IDatabaseConfiguration
    {
        private static readonly Lazy<InMemoryDatabaseConfiguration> Lazy = new(() => new InMemoryDatabaseConfiguration());

        public static InMemoryDatabaseConfiguration Instance => Lazy.Value;
        public ISessionFactory SessionFactory { get; set; }
        public Configuration Configuration { get; set; }
        private InMemoryDatabaseConfiguration()
        {
            Configuration = new Configuration();
            Configuration.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                db.LogSqlInConsole = true;
            });
            Configuration.AddMappingsFromAssembly(typeof(INamespacePlaceholder).Assembly);
            SessionFactory = Configuration.BuildSessionFactory();
        }
    }
