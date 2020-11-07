using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerInsertAction<TTriggerEntity, TInsertEntity> : ITriggerAction
       where TTriggerEntity : class
       where TInsertEntity : class
    {
        public LambdaExpression InsertExpression;

        public TriggerInsertAction(LambdaExpression insertExpression)
        {
            InsertExpression = insertExpression;
        }

        public virtual string BuildSql(ITriggerSqlVisitor visitor)
            => visitor.GetTriggerInsertActionSql(this);

        public abstract Dictionary<string, ArgumentPrefix> InsertExpressionPrefixes { get; }
    }
}