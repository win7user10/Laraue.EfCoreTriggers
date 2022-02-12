﻿using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.Services;
using Laraue.EfCoreTriggers.Common.Services.Impl;
using Laraue.EfCoreTriggers.Common.Services.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions;

/// <summary>
/// Extension methods for EFCore triggers container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register new <see cref="ITriggerActionVisitorFactory"/> into container.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <typeparam name="TVisitorType">Trigger action to visit.</typeparam>
    /// <typeparam name="TImpl">Trigger visitor implementation.</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddTriggerActionVisitor<TVisitorType, TImpl>(this IServiceCollection services)
        where TVisitorType : ITriggerAction
        where TImpl : class, ITriggerActionVisitor<TVisitorType>
    {
        return services.AddSingleton<ITriggerActionVisitor<TVisitorType>, TImpl>();
    }
    
    /// <summary>
    /// Register new <see cref="IMethodCallVisitor"/> into container.
    /// All visitors are applied in reverse to register order. 
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <typeparam name="TImpl">Implementation of visitor.</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddMethodCallConverter<TImpl>(this IServiceCollection services)
        where TImpl : class, IMethodCallVisitor
    {
        return services.AddSingleton<IMethodCallVisitor, TImpl>();
    }
    
    /// <summary>
    /// Register new <see cref="IExpressionVisitor{T}"/> into container.
    /// Such visitors parses <see cref="Expression"/> of specific type and generates SQL.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <typeparam name="TVisitorType">Expression to visit type</typeparam>
    /// <typeparam name="TImpl">Implementation of visitor.</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddExpressionVisitor<TVisitorType, TImpl>(this IServiceCollection services)
        where TImpl : class, IExpressionVisitor<TVisitorType>
        where TVisitorType : Expression
    {
        return services.AddSingleton<IExpressionVisitor<TVisitorType>, TImpl>();
    }
    
    /// <summary>
    /// Add the most used EFCore triggers services into container. 
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultServices(this IServiceCollection services)
    {
        return services.AddSingleton<ITriggerActionVisitorFactory, TriggerActionVisitorFactory>()
            .AddSingleton<IExpressionVisitorFactory, ExpressionVisitorFactory>()
            .AddSingleton<IMemberInfoVisitorFactory, MemberInfoVisitorFactory>()
            
            .AddTriggerActionVisitor<TriggerDeleteAction, TriggerDeleteActionVisitor>()
            .AddTriggerActionVisitor<TriggerInsertAction, TriggerInsertActionVisitor>()
            .AddTriggerActionVisitor<TriggerRawAction, TriggerRawActionVisitor>()
            .AddTriggerActionVisitor<TriggerUpdateAction, TriggerUpdateActionVisitor>()
            .AddTriggerActionVisitor<TriggerCondition, TriggerConditionVisitor>()
                
            .AddSingleton<IMemberInfoVisitor<LambdaExpression>, SetLambdaExpressionVisitor>()
            .AddSingleton<IMemberInfoVisitor<MemberInitExpression>, SetMemberInitExpressionVisitor>()
            .AddSingleton<IMemberInfoVisitor<NewExpression>, SetNewExpressionVisitor>()
            
            .AddSingleton<IDbSchemaRetriever, EfCoreDbSchemaRetriever>()
            
            .AddExpressionVisitor<BinaryExpression, BinaryExpressionVisitor>()
            .AddExpressionVisitor<UnaryExpression, UnaryExpressionVisitor>()
            .AddExpressionVisitor<MemberExpression, MemberExpressionVisitor>()
            .AddExpressionVisitor<ConstantExpression, ConstantExpressionVisitor>()
            .AddExpressionVisitor<MethodCallExpression, MethodCallExpressionVisitor>()
            
            .AddSingleton<IUpdateExpressionVisitor, UpdateExpressionVisitor>();
    }
}