using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.Infrastructure.Tests.__TestCaseSources.RepositorySource;

public abstract class RepositorySource(Func<IRepository> repositoryProvider)
{
    public IRepository CreateRepository() => repositoryProvider() ;
    
}