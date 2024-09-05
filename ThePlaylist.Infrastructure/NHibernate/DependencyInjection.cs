using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace ThePlaylist.Infrastructure.NHibernate;

public static class DependencyInjection
{
    public static void AddNHibernate(this IServiceCollection services, string connectionString)
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
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
            });
            
            configuration.AddMappingsFromAssembly(typeof(INamespacePlaceholder).Assembly);
    
            return configuration;
        });

        services.AddSingleton<ISessionFactory>(svc => svc.GetRequiredService<Configuration>().BuildSessionFactory());
        services.AddTransient<ISession>(svc => svc.GetRequiredService<ISessionFactory>().OpenSession());

    }
}