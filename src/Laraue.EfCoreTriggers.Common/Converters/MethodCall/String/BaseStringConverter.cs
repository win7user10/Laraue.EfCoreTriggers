using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String
{
    public abstract class BaseStringConverter : MethodCallConverter
    {
        public abstract string MethodName { get; }

        /// <inheritdoc />
        public override bool IsApplicable(MethodCallExpression expression)
        {
            return expression.Method.ReflectedType == typeof(string) && MethodName == expression.Method.Name;
        }
    }
}