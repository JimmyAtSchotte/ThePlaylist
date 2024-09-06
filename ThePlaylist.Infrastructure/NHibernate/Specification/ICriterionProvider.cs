using NHibernate;

namespace ThePlaylist.Infrastructure.NHibernate.Specification;

public interface ICriterionProvider
{
    void ApplyTo(ICriteria criteria);
}