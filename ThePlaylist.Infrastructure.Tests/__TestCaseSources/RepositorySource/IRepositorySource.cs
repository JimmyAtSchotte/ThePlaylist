using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

public interface IRepositorySource
{
    IRepository CreateRepository();
}