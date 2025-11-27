using System.Linq.Expressions;
using Laraue.Linq2Triggers.Core.SqlGeneration;
using Laraue.Linq2Triggers.Core.TriggerBuilders.Actions;
using Laraue.Linq2Triggers.Core.Visitors.ExpressionVisitors;
using Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors.Statements;

namespace Laraue.Linq2Triggers.Core.Visitors.TriggerVisitors
{
    public class TriggerUpdateActionVisitor : ITriggerActionVisitor<TriggerUpdateAction>
    {
        private readonly ISqlGenerator _sqlGenerator;
        private readonly IExpressionVisitorFactory _expressionVisitorFactory;
        private readonly IUpdateExpressionVisitor _updateExpressionVisitor;

        public TriggerUpdateActionVisitor(
            ISqlGenerator sqlGenerator,
            IExpressionVisitorFactory expressionVisitorFactory, 
            IUpdateExpressionVisitor updateExpressionVisitor)
        {
            _sqlGenerator = sqlGenerator;
            _expressionVisitorFactory = expressionVisitorFactory;
            _updateExpressionVisitor = updateExpressionVisitor;
        }

        /// <inheritdoc />
        public SqlBuilder Visit(TriggerUpdateAction triggerAction, VisitedMembers visitedMembers)
        {
            var updateStatement = _updateExpressionVisitor.Visit(
                triggerAction.UpdateExpression,
                visitedMembers);
        
            var binaryExpressionSql = _expressionVisitorFactory.Visit(
                (BinaryExpression)triggerAction.Predicate.Body,
                visitedMembers);

            var updateEntity = triggerAction.UpdateExpression.Body.Type;

            return new SqlBuilder()
                .Append($"UPDATE {_sqlGenerator.GetTableSql(updateEntity)}")
                .AppendNewLine("SET ")
                .Append(updateStatement)
                .AppendNewLine("WHERE ")
                .Append(binaryExpressionSql)
                .Append(";");
        }
    
    
    }
}