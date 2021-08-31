using Laraue.EfCoreTriggers.PostgreSql;
using Laraue.EfCoreTriggers.Tests.Tests.Unit;
using Xunit.Categories;

namespace Laraue.EfCoreTriggers.PostgreSqlTests
{
    [UnitTest]
    public class PostgreSqlUnitMathFunctionsTests : UnitMathFunctionsTests
    {
        public PostgreSqlUnitMathFunctionsTests() : base(new PostgreSqlProvider(new ContextFactory().CreateDbContext().Model))
        {
        }
        public override string ExceptedAbsSql => "INSERT INTO transactions_mirror (value) VALUES (ABS(NEW.value));";

        public override string ExceptedAcosSql => "INSERT INTO transactions_mirror (double_value) VALUES (ACOS(NEW.double_value));";

        public override string ExceptedAsinSql => "INSERT INTO transactions_mirror (double_value) VALUES (ASIN(NEW.double_value));";
       
        public override string ExceptedAtanSql => "INSERT INTO transactions_mirror (double_value) VALUES (ATAN(NEW.double_value));";
        
        public override string ExceptedAtan2Sql => "INSERT INTO transactions_mirror (double_value) VALUES (ATAN2(NEW.double_value, NEW.double_value));"; 

        public override string ExceptedCeilingSql => "INSERT INTO transactions_mirror (double_value) VALUES (CEILING(NEW.double_value));"; 

        public override string ExceptedCosSql => "INSERT INTO transactions_mirror (double_value) VALUES (COS(NEW.double_value));"; 

        public override string ExceptedExpSql => "INSERT INTO transactions_mirror (double_value) VALUES (EXP(NEW.double_value));"; 
        
        public override string ExceptedFloorSql => "INSERT INTO transactions_mirror (double_value) VALUES (FLOOR(NEW.double_value));"; 
    }
}
