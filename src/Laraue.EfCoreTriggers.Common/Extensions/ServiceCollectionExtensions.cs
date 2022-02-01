using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriggerActionVisitor<TVisitorType, TImpl>(this IServiceCollection services)
        where TVisitorType : ITriggerAction
        where TImpl : class, ITriggerActionVisitor<TVisitorType>
    {
        return services.AddSingleton<ITriggerActionVisitor<TVisitorType>, TImpl>();
    }
    
    public static IServiceCollection AddMethodCallConverter<TImpl>(this IServiceCollection services)
        where TImpl : class, IMethodCallVisitor
    {
        return services.AddSingleton<IMethodCallVisitor, TImpl>();
    }
    
    public static IServiceCollection AddExpressionVisitor<TVisitorType, TImpl>(this IServiceCollection services)
        where TImpl : class, IExpressionVisitor<TVisitorType>
        where TVisitorType : Expression
    {
        return services.AddSingleton<IExpressionVisitor<TVisitorType>, TImpl>();
    }
}