using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.TriggerBuilders;

namespace Laraue.EfCoreTriggers.SqlServer;

public class SqlServerMemberExpressionVisitor : MemberExpressionVisitor
{
    private readonly ISqlGenerator _sqlGenerator;
    
    public SqlServerMemberExpressionVisitor(ISqlGenerator sqlGenerator) 
        : base(sqlGenerator)
    {
        _sqlGenerator = sqlGenerator;
    }

    protected override string Visit(MemberExpression memberExpression, ArgumentType argumentType)
    {
        var memberInfo = memberExpression.Member;
        
        return argumentType switch
        {
            ArgumentType.New => _sqlGenerator.GetVariableSql(null, memberInfo, argumentType),
            ArgumentType.Old => _sqlGenerator.GetVariableSql(null, memberInfo, argumentType),
            _ => base.Visit(memberExpression, argumentType)
        };
    }
}