using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Converters.MemberAccess
{
    /// <summary>
    /// Converter for <see cref="MethodCallExpression"/>.
    /// </summary>
    public interface IMemberAccessVisitor
    {
        /// <summary>
        /// Should this converter be used to translate a <see cref="MemberExpression"/> to a SQL.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool IsApplicable(MemberExpression expression);

        /// <summary>
        /// Build a SQL for passed <see cref="MethodCallExpression"/>.
        /// </summary>
        /// <param name="expression">Expression to parse.</param>
        /// <returns></returns>
        SqlBuilder Visit(MemberExpression expression);
    }
}