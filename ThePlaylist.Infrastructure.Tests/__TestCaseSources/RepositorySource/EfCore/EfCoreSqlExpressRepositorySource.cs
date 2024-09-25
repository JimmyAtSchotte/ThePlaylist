using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.EntityFramework;
using ThePlaylist.Infrastructure.NHibernate;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.EfCore;

public class EfCoreSqlExpressRepositorySource : IRepositorySource
{
    private IServiceProvider? _services;
    private IServiceProvider Services => _services ??= Initialize();

    private const string CONNECTION_STRING = "Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;TrustServerCertificate=True;";

    private static IServiceProvider Initialize()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(CONNECTION_STRING);
            options.LogTo(Console.WriteLine);
        });
        serviceCollection.AddScoped<IRepository, EntityFramework.Repository>();
        
        var services = serviceCollection.BuildServiceProvider();
        var context = services.GetRequiredService<Context>();
        context.Database.ExecuteSqlRaw(@"DELETE FROM TrackGenres
                                         DELETE FROM PlaylistTracks
                                         DELETE FROM Genres
                                         DELETE FROM Playlists
                                         DELETE FROM Tracks");
        
        return services;
    }
    
    public IRepository CreateRepository()
    {
        Services.GetRequiredService<Context>().ChangeTracker.Clear();
        
        return Services.GetRequiredService<IRepository>();
    }
}