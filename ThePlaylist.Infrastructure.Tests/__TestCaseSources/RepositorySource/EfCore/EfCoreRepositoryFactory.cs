using Microsoft.EntityFrameworkCore;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.EntityFramework;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource.EfCore;

public class EfCoreRepositoryFactory
{
    private const string SQL_EXPRESS_CONNECTION = "Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;TrustServerCertificate=True;";
    public static IRepository UseSqlExpress(string connectionString = SQL_EXPRESS_CONNECTION)
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;TrustServerCertificate=True;");
        optionsBuilder.LogTo(Console.WriteLine);
        return new EntityFramework.Repository(new Context(optionsBuilder.Options));
    }
}