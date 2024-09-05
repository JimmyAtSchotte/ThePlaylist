using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.NHibernate.DatabaseConfigurations;

public sealed class SqlExpressDatabaseConfiguration : IDatabaseConfiguration
    {
        private static readonly Lazy<SqlExpressDatabaseConfiguration> Lazy = new(() => new SqlExpressDatabaseConfiguration());

        public static SqlExpressDatabaseConfiguration Instance => Lazy.Value;
        public ISessionFactory SessionFactory { get; set; }
        public Configuration Configuration { get; set; }
        
        private SqlExpressDatabaseConfiguration()
        {
            Configuration = new Configuration();
            Configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = "Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;";
                db.Driver<SqlClientDriver>();
                db.Dialect<MsSql2012Dialect>();
                db.ConnectionProvider<DriverConnectionProvider>();
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
            });
            
            Configuration.AddMappingsFromAssembly(typeof(INamespacePlaceholder).Assembly);
            SessionFactory = Configuration.BuildSessionFactory();
        }
    }
