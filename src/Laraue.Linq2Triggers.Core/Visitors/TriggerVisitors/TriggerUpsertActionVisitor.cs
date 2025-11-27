using System.Linq;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.Visitors.SetExpressionVisitors;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors.Statements;

namespace Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors
{
    public class TriggerUpsertActionVisitor : ITriggerActionVisitor<TriggerUpsertAction>
    {
        private readonly IMemberInfoVisitorFactory _memberInfoVisitorFactory;
        private readonly IUpdateExpressionVisitor _updateExpressionVisitor;
        private readonly IInsertExpressionVisitor _insertExpressionVisitor;
        private readonly ISqlGenerator _sqlGenerator;

        public TriggerUpsertActionVisitor(
            IMemberInfoVisitorFactory memberInfoVisitorFactory,
            IUpdateExpressionVisitor updateExpressionVisitor,
            IInsertExpressionVisitor insertExpressionVisitor,
            ISqlGenerator sqlGenerator)
        {
            _memberInfoVisitorFactory = memberInfoVisitorFactory;
            _updateExpressionVisitor = updateExpressionVisitor;
            _insertExpressionVisitor = insertExpressionVisitor;
            _sqlGenerator = sqlGenerator;
        }

        /// <inheritdoc />
        public virtual SqlBuilder Visit(TriggerUpsertAction triggerAction, VisitedMembers visitedMembers)
        {
            var matchExpressionParts = _memberInfoVisitorFactory.Visit(
                triggerAction.MatchExpression,
                visitedMembers);

            var updateEntityType = triggerAction.InsertExpression.Body.Type;

            var insertStatementSql = _insertExpressionVisitor.Visit(
                triggerAction.InsertExpression,
                visitedMembers);
            
            var sqlBuilder = SqlBuilder.FromString($"INSERT INTO {_sqlGenerator.GetTableSql(updateEntityType)} ")
                .Append(insertStatementSql)
                .Append(" ON CONFLICT (")
                .AppendJoin(", ", matchExpressionParts
                    .Select(x =>
                        _sqlGenerator.GetColumnSql(updateEntityType, x.Key, ArgumentType.None)))
                .Append(")");

            if (triggerAction.UpdateExpression is null)
            {
                sqlBuilder.Append(" DO NOTHING;");
            }
            else
            {
                var updateStatementSql = _updateExpressionVisitor.Visit(
                    triggerAction.UpdateExpression,
                    visitedMembers);
            
                sqlBuilder.Append(" DO UPDATE SET ")
                    .Append(updateStatementSql)
                    .Append(";");
            }

            return sqlBuilder;
        }
    }
}