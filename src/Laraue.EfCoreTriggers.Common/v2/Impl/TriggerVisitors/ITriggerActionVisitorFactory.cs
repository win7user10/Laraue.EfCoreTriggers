﻿using Laraue.EfCoreTriggers.Common.SqlGeneration;
using Laraue.EfCoreTriggers.Common.TriggerBuilders.Base;

namespace Laraue.EfCoreTriggers.Common.v2.Impl.TriggerVisitors;

public interface ITriggerActionVisitorFactory
{
    SqlBuilder Visit(ITriggerAction triggerAction, VisitedMembers visitedMembers);
}