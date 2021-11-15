﻿using Laraue.EfCoreTriggers.MySql;
using Laraue.EfCoreTriggers.Tests;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.MySqlTests.Unit
{
    [Collection(CollectionNames.MySql)]
    public class MySqlUnitMathFunctionsTests : UnitMathFunctionsTests
    {
        public MySqlUnitMathFunctionsTests() : base(new MySqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        
        public override string ExceptedAbsSql => "INSERT INTO destination_entities (`decimal_value`) VALUES (ABS(NEW.decimal_value));";

        public override string ExceptedAcosSql => "INSERT INTO destination_entities (`double_value`) VALUES (ACOS(NEW.double_value));";

        public override string ExceptedAsinSql => "INSERT INTO destination_entities (`double_value`) VALUES (ASIN(NEW.double_value));";

        public override string ExceptedAtanSql => "INSERT INTO destination_entities (`double_value`) VALUES (ATAN(NEW.double_value));";

        public override string ExceptedAtan2Sql => "INSERT INTO destination_entities (`double_value`) VALUES (ATAN2(NEW.double_value, NEW.double_value));";

        public override string ExceptedCeilingSql => "INSERT INTO destination_entities (`double_value`) VALUES (CEILING(NEW.double_value));";
        
        public override string ExceptedCosSql => "INSERT INTO destination_entities (`double_value`) VALUES (COS(NEW.double_value));";

        public override string ExceptedExpSql => "INSERT INTO destination_entities (`double_value`) VALUES (EXP(NEW.double_value));";
      
        public override string ExceptedFloorSql => "INSERT INTO destination_entities (`double_value`) VALUES (FLOOR(NEW.double_value));";

    }
}
