using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters.MemberAccess;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.CSharpMethods;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Enumerable.Count;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Functions;
using Laraue.EfCoreTriggers.Common.Converters.NewExpression;
using Laraue.EfCoreTriggers.Common.Migrations;
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Abstractions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.Visitors.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.Common.Extensions
{
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
            return services.AddScoped<ITriggerActionVisitor<TVisitorType>, TImpl>();
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
            return services.AddScoped<IMethodCallVisitor, TImpl>();
        }
        
        /// <summary>
        /// Register new <see cref="IMemberAccessVisitor"/> into container.
        /// All visitors are applied in reverse order. 
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <typeparam name="TImpl">Implementation of visitor.</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddMemberAccessConverter<TImpl>(this IServiceCollection services)
            where TImpl : class, IMemberAccessVisitor
        {
            return services.AddScoped<IMemberAccessVisitor, TImpl>();
        }
        
        /// <summary>
        /// Register new <see cref="INewExpressionVisitor"/> into container.
        /// All visitors are applied in reverse order.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <typeparam name="TImpl">Implementation of visitor.</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddNewExpressionConverter<TImpl>(this IServiceCollection services)
            where TImpl : class, INewExpressionVisitor
        {
            return services.AddScoped<INewExpressionVisitor, TImpl>();
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
            return services.AddScoped<IExpressionVisitor<TVisitorType>, TImpl>();
        }
    
        /// <summary>
        /// Add the most used EFCore triggers services into container. 
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultServices(this IServiceCollection services)
        {
            return services.AddScoped<ITriggerActionVisitorFactory, TriggerActionVisitorFactory>()
                .AddScoped<IExpressionVisitorFactory, ExpressionVisitorFactory>()
                .AddScoped<IMemberInfoVisitorFactory, MemberInfoVisitorFactory>()
            
                .AddTriggerActionVisitor<TriggerDeleteAction, TriggerDeleteActionVisitor>()
                .AddTriggerActionVisitor<TriggerInsertAction, TriggerInsertActionVisitor>()
                .AddTriggerActionVisitor<TriggerRawAction, TriggerRawActionVisitor>()
                .AddTriggerActionVisitor<TriggerUpdateAction, TriggerUpdateActionVisitor>()
                .AddTriggerActionVisitor<TriggerCondition, TriggerConditionVisitor>()
                
                .AddScoped<IMemberInfoVisitor<LambdaExpression>, SetLambdaExpressionVisitor>()
                .AddScoped<IMemberInfoVisitor<MemberInitExpression>, SetMemberInitExpressionVisitor>()
                .AddScoped<IMemberInfoVisitor<NewExpression>, SetNewExpressionVisitor>()
                .AddScoped<IMemberInfoVisitor<BinaryExpression>, SetBinaryExpressionVisitor>()
            
                .AddScoped<IDbSchemaRetriever, EfCoreDbSchemaRetriever>()
            
                .AddExpressionVisitor<BinaryExpression, BinaryExpressionVisitor>()
                .AddExpressionVisitor<UnaryExpression, UnaryExpressionVisitor>()
                .AddExpressionVisitor<MemberExpression, MemberExpressionVisitor>()
                .AddExpressionVisitor<ConstantExpression, ConstantExpressionVisitor>()
                .AddExpressionVisitor<MethodCallExpression, MethodCallExpressionVisitor>()
                .AddExpressionVisitor<LambdaExpression, LambdaExpressionVisitor>()
                .AddExpressionVisitor<ParameterExpression, ParameterExpressionVisitor>()
                .AddExpressionVisitor<NewExpression, NewExpressionVisitor>()
            
                .AddMethodCallConverter<CountVisitor>()
                .AddMethodCallConverter<CoalesceVisitor>()
                .AddMethodCallConverter<GetTableNameVisitor>()
                .AddMethodCallConverter<GetColumnNameVisitor>()
            
                .AddScoped<VisitingInfo>()

                .AddScoped<ITriggerModelDiffer, TriggerModelDiffer>()

                .AddScoped<IUpdateExpressionVisitor, UpdateExpressionVisitor>();
        }
    }
}