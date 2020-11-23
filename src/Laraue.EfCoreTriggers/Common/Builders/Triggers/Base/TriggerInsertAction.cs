using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerInsertAction<TTriggerEntity, TInsertEntity> : ITriggerAction
       where TTriggerEntity : class
       where TInsertEntity : class
    {
        internal LambdaExpression InsertExpression;

        public TriggerInsertAction(LambdaExpression insertExpression)
        {
            InsertExpression = insertExpression;
        }

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerInsertActionSql(this);

        internal abstract Dictionary<string, ArgumentType> InsertExpressionPrefixes { get; }
    }
}