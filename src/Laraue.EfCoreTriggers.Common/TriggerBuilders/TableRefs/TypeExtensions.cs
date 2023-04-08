using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Laraue.EfCoreTriggers.Common.TriggerBuilders.TableRefs;

public static class TypeExtensions
{
    public static bool TryGetOldTableRef(this MemberInfo memberInfo, [NotNullWhen(true)] out Type? refType)
    {
        return TryGetTableRef(memberInfo, typeof(IOldTableRef<>), out refType);
    }

    public static bool TryGetNewTableRef(this MemberInfo memberInfo, [NotNullWhen(true)] out Type? refType)
    {
        return TryGetTableRef(memberInfo, typeof(INewTableRef<>), out refType);
    }
    
    private static bool TryGetTableRef(this MemberInfo memberInfo, Type tableRefType, [NotNullWhen(true)] out Type? refType)
    {
        refType = null;
        if (memberInfo.ReflectedType.GenericTypeArguments.Length != 1)
        {
            return false;
        }

        var genericArgType = memberInfo.ReflectedType.GenericTypeArguments[0];
        if (!genericArgType.IsClass)
        {
            return false;
        }

        if (!typeof(ITableRef).IsAssignableFrom(memberInfo.ReflectedType))
        {
            return false;
        }

        var tableRefProperty = tableRefType.GetProperties().Single();
        if (memberInfo.Name != tableRefProperty.Name)
        {
            return false;
        }
        
        refType = genericArgType;
        return true;
    }
}