using System;
using System.Linq.Expressions;
using Laraue.EfCoreTriggers.Common.Converters;
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
using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.SqlServer.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder<TContext> UseSqlServerTriggers<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            TriggerExtensions.Services.AddSqlServerServices();
            
            return optionsBuilder.UseTriggers();
        }

        public static DbContextOptionsBuilder UseSqlServerTriggers(this DbContextOptionsBuilder optionsBuilder)
        {
            TriggerExtensions.Services.AddSqlServerServices();
            
            return optionsBuilder.UseTriggers();
        }
        
        public static IServiceCollection AddSqlServerServices(this IServiceCollection services)
        {
            return services.AddDefaultServices()
                .AddSingleton<SqlTypeMappings, SqlServerTypeMappings>()
                .AddExpressionVisitor<UnaryExpression, SqlServerUnaryExpressionVisitor>()
                .AddExpressionVisitor<NewExpression, SqlServerNewExpressionVisitor>()
                .AddExpressionVisitor<MemberExpression, SqlServerMemberExpressionVisitor>()
                .AddSingleton<IInsertExpressionVisitor, InsertExpressionVisitor>()
                .AddTriggerActionVisitor<TriggerUpsertAction, SqlServerTriggerUpsertActionVisitor>()
                .AddSingleton<ISqlGenerator, SqlServerSqlGenerator>()
                .AddSingleton<ITriggerVisitor, SqlServerTriggerVisitor>()
                .AddMethodCallConverter<ConcatStringViaPlusVisitor>()
                .AddMethodCallConverter<StringToUpperViaUpperFuncVisitor>()
                .AddMethodCallConverter<StringToLowerViaLowerFuncVisitor>()
                .AddMethodCallConverter<StringTrimViaLtrimRtrimFuncVisitor>()
                .AddMethodCallConverter<StringContainsViaCharindexFuncVisitor>()
                .AddMethodCallConverter<StringEndsWithViaPlusVisitor>()
                .AddMethodCallConverter<StringIsNullOrEmptyVisitor>()
                .AddMethodCallConverter<MathAbsVisitor>()
                .AddMethodCallConverter<MathAcosVisitor>()
                .AddMethodCallConverter<MathAsinVisitor>()
                .AddMethodCallConverter<MathAtanVisitor>()
                .AddMethodCallConverter<MathAtn2Visitor>()
                .AddMethodCallConverter<MathCeilingVisitor>()
                .AddMethodCallConverter<MathCosVisitor>()
                .AddMethodCallConverter<MathExpVisitor>()
                .AddMethodCallConverter<MathFloorVisitor>();
        }
    }
}