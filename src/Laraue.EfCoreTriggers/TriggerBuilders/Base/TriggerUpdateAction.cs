using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.SqlGeneration;

namespace Laraue.EfCoreTriggers.TriggerBuilders.Base
{
    public abstract class TriggerUpdateAction<TTriggerEntity, TUpdateEntity> : ITriggerAction
       where TTriggerEntity : class
       where TUpdateEntity : class
    {
        internal LambdaExpression UpdateFilter;
        internal LambdaExpression UpdateExpression;

        public TriggerUpdateAction(
            LambdaExpression updateFilter,
            LambdaExpression updateExpression)
        {
            UpdateFilter = updateFilter;
            UpdateExpression = updateExpression;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerUpdateActionSql(this);

        internal abstract Dictionary<string, ArgumentType> UpdateFilterPrefixes { get; }

        internal abstract Dictionary<string, ArgumentType> UpdateExpressionPrefixes { get; }
    }
}