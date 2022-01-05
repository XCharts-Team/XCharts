using System;

namespace XCharts
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ListFor : Attribute
    {
        public readonly Type type;

        public ListFor(Type type)
        {
            this.type = type;
        }
    }
}