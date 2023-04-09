using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public sealed class TriggerUpdateAction : ITriggerAction
    {
        internal LambdaExpression UpdateFilter;
        internal LambdaExpression UpdateExpression;

        internal TriggerUpdateAction(
            LambdaExpression updateFilter,
            LambdaExpression updateExpression)
        {
            UpdateFilter = updateFilter;
            UpdateExpression = updateExpression;
        }
    }
}