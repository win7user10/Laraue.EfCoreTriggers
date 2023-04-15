using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;

namespace Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors
{
    public class TriggerInsertActionVisitor : ITriggerActionVisitor<TriggerInsertAction>
    {
        private readonly IInsertExpressionVisitor _visitor;
        private readonly ISqlGenerator _sqlGenerator;

        public TriggerInsertActionVisitor(
            IInsertExpressionVisitor visitor,
            ISqlGenerator sqlGenerator)
        {
            _visitor = visitor;
            _sqlGenerator = sqlGenerator;
        }
    
        /// <inheritdoc />
        public SqlBuilder Visit(TriggerInsertAction triggerAction, VisitedMembers visitedMembers)
        {
            var insertStatement = _visitor.Visit(
                triggerAction.InsertExpression,
                visitedMembers);

            var insertEntityType = triggerAction.InsertExpression.Body.Type;
        
            var sql = SqlBuilder.FromString($"INSERT INTO {_sqlGenerator.GetTableSql(insertEntityType)} ")
                .Append(insertStatement)
                .Append(";");

            return sql;
        }
    }
}