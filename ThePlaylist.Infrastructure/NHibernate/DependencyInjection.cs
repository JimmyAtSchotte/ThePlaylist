using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Projections;
using Environment = NHibernate.Cfg.Environment;

namespace ThePlaylist.Infrastructure.NHibernate;

public static class DependencyInjection
{
    public static void AddNHibernate(this IServiceCollection services, Action<NHibernateConfiguration> configure)
    {
        var conf = new NHibernateConfiguration(services);
        configure(conf);
    }
}

public class NHibernateConfiguration(IServiceCollection services)
{
    public void UseSqlExpress(string connectionString, bool logSqlInConsole = true, bool logFormattedSql = true )
    {
        services.AddSingleton<Configuration>(_ =>
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.Driver<SqlClientDriver>();
                db.Dialect<MsSql2012Dialect>();
                db.ConnectionProvider<DriverConnectionProvider>(); 
                db.LogSqlInConsole = logSqlInConsole;
                db.LogFormattedSql = logFormattedSql;
                
            });
            
            
            configuration.AddMappingsFromAssembly(typeof(IAmMappingsNamespace).Assembly);
            configuration.AddProjectionsFromAssemblyNamespace(typeof(IAmProjectionsNamespace).Assembly, typeof(IAmProjectionsNamespace).Namespace);
            
            return configuration;
        });

        services.AddSingleton<ISessionFactory>(svc => svc.GetRequiredService<Configuration>().BuildSessionFactory());
        services.AddTransient<ISession>(svc => svc.GetRequiredService<ISessionFactory>().OpenSession());
    }

    public void UseSqlLite()
    {
        services.AddSingleton<Configuration>(_ =>
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                db.LogSqlInConsole = true;
            });
            
            configuration.AddMappingsFromAssembly(typeof(IAmMappingsNamespace).Assembly);
            configuration.AddProjectionsFromAssemblyNamespace(typeof(IAmProjectionsNamespace).Assembly, typeof(IAmProjectionsNamespace).Namespace);
    
            return configuration;
        });

        services.AddSingleton<ISessionFactory>(svc => svc.GetRequiredService<Configuration>().BuildSessionFactory());
        services.AddTransient<ISession>(svc =>
        {
            var session =  svc.GetRequiredService<ISessionFactory>().OpenSession();
            var configuration = svc.GetRequiredService<Configuration>();
        
            new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);
            return session;
        });
    }
    
    
}