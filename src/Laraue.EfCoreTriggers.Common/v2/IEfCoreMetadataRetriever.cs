using System;
using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.v2;

public interface IEfCoreMetadataRetriever
{
    string GetColumnName(MemberInfo memberInfo);
    
    string GetTableName(Type entity);
}