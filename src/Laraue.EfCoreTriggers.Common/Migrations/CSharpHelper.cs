using Laraue.EfCoreTriggers.Common.Services.Impl.TriggerVisitors;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Laraue.EfCoreTriggers.Common.Migrations;

public class CSharpHelper : Microsoft.EntityFrameworkCore.Design.Internal.CSharpHelper
{
    private readonly ITriggerVisitor _triggerVisitor;
    
    public CSharpHelper(IRelationalTypeMappingSource relationalTypeMappingSource, ITriggerVisitor triggerVisitor) 
        : base(relationalTypeMappingSource)
    {
        _triggerVisitor = triggerVisitor;
    }

    public override string UnknownLiteral(object value)
    {
        if (value is ITrigger trigger)
        {
            return _triggerVisitor.GenerateCreateTriggerSql(trigger);
        }

        return base.UnknownLiteral(value);
    }
}