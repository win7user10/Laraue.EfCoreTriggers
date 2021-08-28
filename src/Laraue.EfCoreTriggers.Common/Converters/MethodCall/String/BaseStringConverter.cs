using System;

namespace Laraue.EfCoreTriggers.Common.Converters.MethodCall.String
{
    public abstract class BaseStringConverter : MethodCallConverter
    {
        public override Type ReflectedType => typeof(string);
    }
}