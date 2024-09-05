using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.NHibernate;
using ISession = NHibernate.ISession;

namespace ThePlaylist.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddTransient<IRepository, Repository>();
        builder.Services.AddNHibernate("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist;Trusted_Connection=True;");
      
        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseRouting();
        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.Run();
        
    }
}