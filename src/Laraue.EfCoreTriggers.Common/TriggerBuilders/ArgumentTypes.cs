using System.Collections.Generic;
using System.Linq.Expressions;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders;

/// <summary>
/// Dictionary where describes the argument type
/// of each member of <see cref="LambdaExpression"/>.
/// </summary>
public class ArgumentTypes : Dictionary<string, ArgumentType>
{
}