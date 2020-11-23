using Laraue.EfCoreTriggers.Common.Builders.Providers;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerDeleteAction<TTriggerEntity, TDeleteEntity> : ITriggerAction
       where TTriggerEntity : class
       where TDeleteEntity : class
    {
        internal LambdaExpression DeleteFilter;

        public TriggerDeleteAction(LambdaExpression deleteFilter)
            => DeleteFilter = deleteFilter;

        public virtual SqlBuilder BuildSql(ITriggerProvider visitor)
            => visitor.GetTriggerDeleteActionSql(this);

        internal abstract Dictionary<string, ArgumentType> DeleteFilterPrefixes { get; }
    }
}