using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.Builders.Visitor
{
    public interface IExpressionSqlVisitor
    {
        string GetMemberInitSql(MemberInitExpression memberInitExpression, Dictionary<string, ArgumentPrefix> argumentTypes);

        string GetMemberAssignmentSql(MemberAssignment memberAssignment, Dictionary<string, ArgumentPrefix> argumentTypes);

        string GetUnaryExpressionSql(UnaryExpression unaryExpression, Dictionary<string, ArgumentPrefix> argumentTypes);

        string GetBinaryExpressionSql(BinaryExpression binaryExpression, Dictionary<string, ArgumentPrefix> argumentTypes);

        string GetMemberExpressionSql(MemberExpression memberExpression, Dictionary<string, ArgumentPrefix> argumentTypes);

        string GetConstantExpressionSql(ConstantExpression constantExpression);

        string GetExpressionTypeSql(ExpressionType expressionType);
    }
}