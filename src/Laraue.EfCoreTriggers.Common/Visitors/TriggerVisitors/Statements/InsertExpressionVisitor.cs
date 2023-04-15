using System.Linq;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;
using Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors;

namespace Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements
{
    /// <inheritdoc />
    public class InsertExpressionVisitor : IInsertExpressionVisitor
    {
        private readonly IMemberInfoVisitorFactory _factory;
        private readonly ISqlGenerator _sqlGenerator;

        /// <summary>
        /// Initializes a new instance of <see cref="InsertExpressionVisitor"/>.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="sqlGenerator"></param>
        public InsertExpressionVisitor(
            IMemberInfoVisitorFactory factory,
            ISqlGenerator sqlGenerator)
        {
            _factory = factory;
            _sqlGenerator = sqlGenerator;
        }

        /// <inheritdoc />
        public SqlBuilder Visit(LambdaExpression expression, VisitedMembers visitedMembers)
        {
            var insertType = expression.Body.Type;
        
            var assignmentParts = _factory.Visit(
                expression,
                visitedMembers);
        
            var sqlResult = new SqlBuilder();

            if (assignmentParts.Any())
            {
                sqlResult.Append("(")
                    .AppendJoin(", ", assignmentParts
                        .Select(x =>
                            _sqlGenerator.GetColumnSql(insertType, x.Key, ArgumentType.None)))
                    .Append(") SELECT ");

                sqlResult.AppendViaNewLine(", ", assignmentParts
                    .Select(x => x.Value));
            }
            else
            {
                sqlResult.Append(VisitEmptyInsertBody(expression));
            }
            
            return sqlResult;
        }
    
        /// <summary>
        /// Get SQL for the empty insert statement.
        /// </summary>
        /// <param name="insertExpression"></param>
        /// <returns></returns>
        protected virtual SqlBuilder VisitEmptyInsertBody(LambdaExpression insertExpression)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Append("DEFAULT VALUES");
            return sqlBuilder;
        }
    }
}