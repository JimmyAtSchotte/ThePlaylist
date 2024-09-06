using Microsoft.EntityFrameworkCore;
using ThePlaylist.Core.Interfaces;
using ThePlaylist.Infrastructure.EntityFramework;
namespace ThePlaylist.Infrastructure.Tests.EntityFramework;

[TestFixture]
public class RepositorySqlExpressTests : BaseRepositoryTests
{
    private IRepository _repository;
    protected override IRepository Repository => _repository;
    
    public override void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer("Server=1337-JIMMY\\SQLEXPRESS;Database=ThePlaylist_Test;Trusted_Connection=True;TrustServerCertificate=True;");
        _repository = new Repository(new Context(optionsBuilder.Options));
    }

    public override void TearDown()
    {
    }
}