using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Abs;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Acos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Asin;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Atan2;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Ceiling;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Cos;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Exp;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math.Floor;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Concat;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Contains;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.EndsWith;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.IsNullOrEmpty;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToLower;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.ToUpper;
using Laraue.EfCoreTriggers.Common.Converters.MethodCall.String.Trim;
using Laraue.EfCoreTriggers.Common.Extensions;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Laraue.EfCoreTriggers.Common.v2;
using Laraue.EfCoreTriggers.Common.v2.Impl;
using Laraue.EfCoreTriggers.Common.v2.Impl.ExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.SetExpressionVisitors;
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.MySql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseMySqlTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            TriggerExtensions.Services.AddMySqlServices();
            
            return optionsBuilder.UseTriggers();
        }
        
        public static DbContextOptionsBuilder<TContext> UseMySqlTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            TriggerExtensions.Services.AddMySqlServices();
            
            return optionsBuilder.UseTriggers();
        }

        public static IServiceCollection AddMySqlServices(this IServiceCollection services)
        {
            services.AddSingleton<ITriggerActionVisitorFactory, TriggerActionVisitorFactory>();
            
            return services.AddTriggerActionVisitor<TriggerCondition, TriggerConditionVisitor>()
                .AddTriggerActionVisitor<TriggerDeleteAction, TriggerDeleteActionVisitor>()
                .AddTriggerActionVisitor<TriggerInsertAction, TriggerInsertActionVisitor>()
                .AddTriggerActionVisitor<TriggerRawAction, TriggerRawActionVisitor>()
                .AddTriggerActionVisitor<TriggerUpdateAction, TriggerUpdateActionVisitor>()
                .AddTriggerActionVisitor<TriggerUpsertAction, MySqlTriggerUpsertActionVisitor>()
                
                .AddSingleton<IInsertExpressionVisitor, MySqlInsertExpressionVIsitor>()
                .AddSingleton<IUpdateExpressionVisitor, UpdateExpressionVisitor>()
                    
                .AddSingleton<SqlTypeMappings, MySqlTypeMappings>()
                    
                .AddSingleton<ISqlGenerator, MySqlSqlGenerator>()
                
                .AddSingleton<IExpressionVisitorFactory, ExpressionVisitorFactory>()
                .AddExpressionVisitor<BinaryExpression, BinaryExpressionVisitor>()
                .AddExpressionVisitor<UnaryExpression, UnaryExpressionVisitor>()
                .AddExpressionVisitor<MemberExpression, MemberExpressionVisitor>()
                .AddExpressionVisitor<ConstantExpression, ConstantExpressionVisitor>()
                .AddExpressionVisitor<MethodCallExpression, MethodCallExpressionVisitor>()
                .AddExpressionVisitor<NewExpression, MySqlNewExpressionVisitor>()
                
                .AddSingleton<IEfCoreMetadataRetriever, EfCoreMetadataRetriever>()
                
                .AddSingleton<ITriggerVisitor, MySqlTriggerVisitor>()
                
                .AddSingleton<ISetExpressionVisitorFactory, SetExpressionVisitorFactory>()
                
                .AddSingleton<ISetExpressionVisitor<LambdaExpression>, SetLambdaExpressionVisitor>()
                .AddSingleton<ISetExpressionVisitor<MemberInitExpression>, SetMemberInitExpressionVisitor>()
                .AddSingleton<ISetExpressionVisitor<NewExpression>, SetNewExpressionVisitor>()
                    
                .AddMethodCallConverter<ConcatStringViaConcatFuncVisitor>()
                .AddMethodCallConverter<StringToUpperViaUpperFuncVisitor>()
                .AddMethodCallConverter<StringToLowerViaLowerFuncVisitor>()
                .AddMethodCallConverter<StringTrimViaTrimFuncVisitor>()
                .AddMethodCallConverter<StringContainsViaInstrFuncVisitor>()
                .AddMethodCallConverter<StringEndsWithViaConcatFuncVisitor>()
                .AddMethodCallConverter<StringIsNullOrEmptyVisitor>()
                .AddMethodCallConverter<MathAbsVisitor>()
                .AddMethodCallConverter<MathAcosVisitor>()
                .AddMethodCallConverter<MathAsinVisitor>()
                .AddMethodCallConverter<MathAtanVisitor>()
                .AddMethodCallConverter<MathAtan2Visitor>()
                .AddMethodCallConverter<MathCeilingVisitor>()
                .AddMethodCallConverter<MathCosVisitor>()
                .AddMethodCallConverter<MathExpVisitor>()
                .AddMethodCallConverter<MathFloorVisitor>();
            
        }
    }
}