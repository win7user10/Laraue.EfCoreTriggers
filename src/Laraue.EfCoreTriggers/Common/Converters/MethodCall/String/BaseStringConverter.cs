using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Converters.ExpressionCall.String
{
    public abstract class BaseStringConverter : ExpressionCallConverter
    {
        public abstract string MethodName { get; }

        /// <inheritdoc />
        public override bool IsApplicable(MethodCallExpression expression)
        {
            return expression.Type == typeof(string) && MethodName == expression.Method.Name;
        }
    }
}