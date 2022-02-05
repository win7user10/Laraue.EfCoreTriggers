﻿using Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.PostgreSql;
using Laraue.EfCoreTriggers.PostgreSql.Extensions;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Infrastructure;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;

namespace Laraue.EfCoreTriggers.PostgreSqlTests.Unit
{
    [Collection(CollectionNames.PostgreSql)]
    public class PostgreSqlUnitStringFunctionTests : UnitStringFunctionsTests
    {
        public PostgreSqlUnitStringFunctionTests() : base(
            Helper.GetTriggerActionFactory(
                new ContextFactory().CreateDbContext().Model, 
                collection => collection.AddPostgreSqlServices()))
        {
        }
        
        public override string ExceptedConcatSql => "INSERT INTO destination_entities (\"string_field\") VALUES (CONCAT(NEW.string_field, 'abc'));";

        public override string ExceptedStringLowerSql => "INSERT INTO destination_entities (\"string_field\") VALUES (LOWER(NEW.string_field));";

        public override string ExceptedStringUpperSql => "INSERT INTO destination_entities (\"string_field\") VALUES (UPPER(NEW.string_field));";

        public override string ExceptedStringTrimSql => "INSERT INTO destination_entities (\"string_field\") VALUES (BTRIM(NEW.string_field));";

        public override string ExceptedContainsSql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (STRPOS(NEW.string_field, 'abc') > 0);";

        public override string ExceptedEndsWithSql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (NEW.string_field LIKE ('%' || 'abc'));";

        public override string ExceptedIsNullOrEmptySql => "INSERT INTO destination_entities (\"boolean_value\") VALUES (NEW.string_field IS NULL OR NEW.string_field = '');";
    }
}
