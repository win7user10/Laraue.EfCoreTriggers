namespace Laraue.EfCoreTriggers.Common.v2;

public interface ITriggerSqlGenerator
{
    string GenerateCreateTriggerSql();
    string GenerateDeleteTriggerSql();
}