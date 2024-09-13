using ThePlaylist.Core.Interfaces;

namespace ThePlaylist.Infrastructure.Tests.NHibernate;

public abstract class BaseNHibernateRepositorySource(string name, Func<IRepository> repositoryProvider)
{
    public IRepository CreateRepository() => repositoryProvider() ;
    
}