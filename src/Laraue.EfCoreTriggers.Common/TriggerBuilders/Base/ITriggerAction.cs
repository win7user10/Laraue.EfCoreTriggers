using System;
using System.Collections.Generic;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.Base
{
    public interface ITriggerAction
    {
        
    }
    
    public interface ITrigger
    {
        List<ITriggerAction> Actions { get; }
        List<ITriggerAction> Conditions { get; }
        
        TriggerEvent TriggerEvent { get; }

        TriggerTime TriggerTime { get; }

        Type TriggerEntityType { get; }

        string Name { get; }
    }
}
