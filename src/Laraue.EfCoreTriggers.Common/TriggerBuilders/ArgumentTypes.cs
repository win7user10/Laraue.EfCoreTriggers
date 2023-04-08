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
    public ArgumentType Get(MemberExpression memberExpression)
    {
        if (memberExpression.Expression is not ParameterExpression)
        {
            return ArgumentType.Default;
        }

        if (!memberExpression.Type.IsClass)
        {
            return ArgumentType.Default;
        }

        if (memberExpression.Member.DeclaringType.TryGetNewTableRef(out _))
        {
            return ArgumentType.New;
        }
        
        return memberExpression.Member.DeclaringType.TryGetOldTableRef(out _)
            ? ArgumentType.Old
            : ArgumentType.Default;
    }
}