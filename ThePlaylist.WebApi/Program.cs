using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;
using ISession = NHibernate.ISession;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddTransient<IRepository, Repository>();

builder.Services.AddSingleton<Configuration>(_ =>
{
    var configuration = new Configuration();
    configuration.DataBaseIntegration(db =>
    {
        db.ConnectionString = "Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist;Trusted_Connection=True;";
        db.Driver<SqlClientDriver>();
        db.Dialect<MsSql2012Dialect>();
        db.ConnectionProvider<DriverConnectionProvider>();
        db.LogSqlInConsole = true;
        db.LogFormattedSql = true;
    });
            
    configuration.AddMappingsFromAssembly(typeof(INamespacePlaceholder).Assembly);
    
    return configuration;
});

builder.Services.AddSingleton<ISessionFactory>(services => services.GetRequiredService<Configuration>().BuildSessionFactory());
builder.Services.AddTransient<ISession>(services => services.GetRequiredService<ISessionFactory>().OpenSession());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

