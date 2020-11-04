using Laraue.EfCoreTriggers.Common.Builders.Visitor;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Triggers.Base
{
    public abstract class TriggerUpsertAction<TTriggerEntity, TUpsertEntity> : ISqlConvertible
       where TTriggerEntity : class
       where TUpsertEntity : class
    {
        public LambdaExpression MatchExpression;
        public LambdaExpression InsertExpression;
        public LambdaExpression OnMatchExpression;

        public TriggerUpsertAction(
            LambdaExpression matchExpression,
            LambdaExpression insertExpression,
            LambdaExpression onMatchExpression)
        {
            MatchExpression = matchExpression;
            InsertExpression = insertExpression;
            OnMatchExpression = onMatchExpression;
        }

        public virtual string BuildSql(ITriggerSqlVisitor visitor)
        {
            return visitor.GetTriggerUpsertActionSql(this);
        }

        public abstract Dictionary<string, ArgumentPrefix> InsertExpressionPrefixes { get; }

        public abstract Dictionary<string, ArgumentPrefix> OnMatchExpressionPrefixes { get; }
    }
}