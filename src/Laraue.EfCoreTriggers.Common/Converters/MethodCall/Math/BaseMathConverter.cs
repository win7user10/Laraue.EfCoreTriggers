using System;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.Math
{
    public abstract class BaseMathConverter : MethodCallConverter
    {
        protected override Type ReflectedType => typeof(System.Math);
    }
}
