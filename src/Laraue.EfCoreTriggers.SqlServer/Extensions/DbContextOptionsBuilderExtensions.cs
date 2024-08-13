using System;
using System.Linq.Expressions;
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
using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Actions;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.Visitors.TriggerVisitors.Statements;
using Laraue.EfCoreTriggers.SqlServer.Converters.MethodCalls.Guid.NewGuid;
using Laraue.EfCoreTriggers.SqlServer.Converters.NewExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.EfCoreTriggers.SqlServer.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Add EF Core triggers SQL Server provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UseSqlServerTriggers<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
            where TContext : DbContext
        {
            return optionsBuilder.UseEfCoreTriggers(AddSqlServerServices, modifyServices);
        }

        /// <summary>
        /// Add EF Core triggers SQL Server provider.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="modifyServices"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseSqlServerTriggers(
            this DbContextOptionsBuilder optionsBuilder,
            Action<IServiceCollection> modifyServices = null)
        {
            return optionsBuilder.UseEfCoreTriggers(AddSqlServerServices, modifyServices);
        }
        
        /// <summary>
        /// Add EF Core triggers SQL Server provider services.
        /// </summary>
        /// <param name="services"></param>
        public static void AddSqlServerServices(this IServiceCollection services)
        {
            services.AddDefaultServices()
                .AddScoped<SqlTypeMappings, SqlServerTypeMappings>()
                .AddExpressionVisitor<UnaryExpression, SqlServerUnaryExpressionVisitor>()
                .AddScoped<IInsertExpressionVisitor, InsertExpressionVisitor>()
                .AddTriggerActionVisitor<TriggerUpsertAction, SqlServerTriggerUpsertActionVisitor>()
                .AddScoped<ISqlGenerator, SqlServerSqlGenerator>()
                .AddScoped<ITriggerVisitor, SqlServerTriggerVisitor>()
                .AddTriggerActionVisitor<TriggerActionsGroup, SqlServerTriggerActionsGroupVisitor>()
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
                .AddMethodCallConverter<MathFloorVisitor>()
                .AddMethodCallConverter<NewGuidVisitor>()
                .AddMemberAccessConverter<Converters.MemberAccess.DateTime.UtcNowVisitor>()
                .AddMemberAccessConverter<Converters.MemberAccess.DateTime.NowVisitor>()
                .AddMemberAccessConverter<Converters.MemberAccess.DateTimeOffset.UtcNowVisitor>()
                .AddMemberAccessConverter<Converters.MemberAccess.DateTimeOffset.NowVisitor>()
                .AddNewExpressionConverter<NewDateTimeExpressionVisitor>()
                .AddNewExpressionConverter<NewDateTimeOffsetExpressionVisitor>();
        }
    }
}