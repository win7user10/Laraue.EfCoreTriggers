using Laraue.EfCoreTriggers.Common.SqlGeneration;

namespace Laraue.EfCoreTriggers.Common.Converters.NewExpression
{
    /// <summary>
    /// Converter for <see cref="System.Linq.Expressions.NewExpression"/>.
    /// </summary>
    public interface INewExpressionVisitor
    {
        /// <summary>
        /// Should this converter be used to translate a <see cref="System.Linq.Expressions.NewExpression"/> to a SQL.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool IsApplicable(System.Linq.Expressions.NewExpression expression);

        /// <summary>
        /// Build a SQL for passed <see cref="System.Linq.Expressions.NewExpression"/>.
        /// </summary>
        /// <param name="expression">Expression to parse.</param>
        /// <param name="visitedMembers">Visited members.</param>
        /// <returns></returns>
        SqlBuilder Visit(
            System.Linq.Expressions.NewExpression expression,
            VisitedMembers visitedMembers);
    }
}