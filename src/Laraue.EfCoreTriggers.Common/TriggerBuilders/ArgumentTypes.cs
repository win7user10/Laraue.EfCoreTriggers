using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders;

/// <summary>
/// Dictionary where describes the argument type
/// of each member of <see cref="LambdaExpression"/>.
/// </summary>
public class ArgumentTypes
{
    public ArgumentType Get(MemberExpression expression)
    {
        return expression.Expression switch
        {
            MemberExpression memberExpression => TryGetTableRefParameter(memberExpression),
            _ => ArgumentType.Default,
        };
    }
    
    public ArgumentType TryGetTableRefParameter(MemberExpression memberExpression)
    {
        if (memberExpression.Expression is not ParameterExpression)
        {
            return ArgumentType.Default;
        }
        
        var newRefType = typeof(INewTableRef<>).MakeGenericType(memberExpression.Type);

        if (newRefType.IsAssignableFrom(memberExpression.Member.DeclaringType))
        {
            return ArgumentType.New;
        }
        
        var oldRef = typeof(IOldTableRef<>).MakeGenericType(memberExpression.Type);
        return oldRef.IsAssignableFrom(memberExpression.Member.DeclaringType)
            ? ArgumentType.Old
            : ArgumentType.Default;
    }
}