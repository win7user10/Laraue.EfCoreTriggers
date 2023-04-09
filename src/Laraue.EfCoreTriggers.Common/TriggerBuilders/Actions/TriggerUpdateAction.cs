using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions
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