using System.Linq.Expressions;
using NHibernate;

namespace ThePlaylist.Infrastructure.EntityFramework.Specification;

public interface ICriterionProvider
{
    Expression<Func<T, bool>>? CreateExpression<T>();
}