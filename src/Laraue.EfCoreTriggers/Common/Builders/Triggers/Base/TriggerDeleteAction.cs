using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerDeleteAction<TTriggerEntity, TDeleteEntity> : ITriggerAction
       where TTriggerEntity : class
       where TDeleteEntity : class
    {
        public LambdaExpression DeleteFilter;

        public TriggerDeleteAction(LambdaExpression deleteFilter)
            => DeleteFilter = deleteFilter;

        public virtual string BuildSql(ITriggerSqlVisitor visitor)
            => visitor.GetTriggerDeleteActionSql(this);

        public abstract Dictionary<string, ArgumentPrefix> DeleteFilterPrefixes { get; }
    }
}